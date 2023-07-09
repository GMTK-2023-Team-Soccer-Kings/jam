using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Collections.LowLevel.Unsafe;

public class PredictiveChoicesController : MonoBehaviour
{
    PredictiveText _predictive;

    [SerializeField] Tag _currentValidTags;

    bool _keywordsActive = false;

    [SerializeField] TextMeshProUGUI[] _choices;

    [SerializeField] TextAsset _sentenceStructuresFile;
    [SerializeField] TextAsset _verbTensesFile;
    [SerializeField] TextAsset _pluralsFile;

    [SerializeField] GameObject _tenseButton;
    [SerializeField] GameObject _pluralButton;


    List<Queue<WordType>> _shortSentenceStructures = new List<Queue<WordType>>();
    List<Queue<WordType>> _longSentenceStructures = new List<Queue<WordType>>();

    Queue<WordType> _selectedStructure;
    WordType _currentWordType;

    Word[] _generatedNormalWords;
    Word[] _generatedKeywords;

    [SerializeField] TextMeshProUGUI _outputBox;

    bool _longSentence = false;
    bool _shortSentence = false;

    List<Word> _completedBrrbl = new List<Word>();

    [SerializeField] ToggleButton _keywordToggle;

    [SerializeField] GameObject _postButton;

    FakeGameData _gameData;

    BrbllCreator _brbllCreator;

    [SerializeField] TextMeshProUGUI _wordTypeTextBox;

    const int DEFAULT_KWORD_COUNT = 3;
    int _keywordCount = DEFAULT_KWORD_COUNT;

    [SerializeField] TextMeshProUGUI _keywordCountTextBox;

    int capsOption = 0;
    int currentTense = 0;
    int plurals = 0;

    Dictionary<string, List<string>> tenses = new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> pluralData = new Dictionary<string, List<string>>();


    private void Awake()
    {
        _predictive = GetComponent<PredictiveText>();
        _brbllCreator = FindObjectOfType<BrbllCreator>();

        ReadSentenceStructuresFile();
        ReadTensesFile();
        ReadPluralsFile();
    }

    private void ReadTensesFile()
    {
        using (CSV csv = new CSV(_verbTensesFile))
        {
            foreach (List<string> row in csv.Data)
            {
                if (row[0] == "") continue;

                List<string> tensesStuff = new List<string>();
                foreach (string str in row)
                {
                    tensesStuff.Add(str.ToLower());
                }

                if (!tenses.TryGetValue(tensesStuff[0], out List<string> _))
                {
                    tenses.Add(tensesStuff[0], tensesStuff);
                }
            }
        }
    }

    private void ReadPluralsFile()
    {
        using (CSV csv = new CSV(_pluralsFile))
        {
            foreach (List<string> row in csv.Data)
            {
                if (row[0] == "") continue;

                List<string> pluralStuff = new List<string>();
                foreach (string str in row)
                {
                    pluralStuff.Add(str.ToLower());
                }

                if (!pluralData.TryGetValue(pluralStuff[0], out List<string> _))
                {
                    pluralData.Add(pluralStuff[0], pluralStuff);
                }

            }
        }
    }

    public void PressPost()
    {
        int score = 0;
        foreach (Word word in _completedBrrbl)
        {
            if (word.Tag.Equals(Tag.None)) continue;

            if (_gameData.Tags.HasFlag(word.Tag))
            {
                score++;
            }
        }

        _brbllCreator.AddUserBrbll(_outputBox.text, score, _gameData);

        _outputBox.text = "";
        _gameData = null;
    }

    public void LoadNewBrrbl(FakeGameData gameData)
    {
        _longSentence = false;
        _shortSentence = false;

        _gameData = gameData;
        _currentValidTags = _gameData.Tags;

        _keywordCount = DEFAULT_KWORD_COUNT;
        ForceToggleToGeneral(false);

        ChooseNewSentenceStructure();
        GoToNextWord();
    }

    private void GoToNextWord()
    {
        _keywordCountTextBox.text = _keywordCount.ToString();

        bool endOfMessage = false;
        if (_selectedStructure.Count == 0)
        {
            endOfMessage = !ChooseNewSentenceStructure();
            if (!endOfMessage) _outputBox.text += " ";
        }

        if (endOfMessage)
        {
            _postButton.SetActive(true);
        }


        if (!endOfMessage)
        {
            _currentWordType = _selectedStructure.Dequeue();

            while (_currentWordType == WordType.Comma)
            {
                _outputBox.text += ",";
                _currentWordType = _selectedStructure.Dequeue();
            }

            _wordTypeTextBox.text = "(" + _currentWordType.ToString() + ")";


            if (_currentWordType != WordType.Punctuation)
            {
                _outputBox.text += " ";
            }

            _tenseButton.SetActive(_currentWordType == WordType.Verb);
            _pluralButton.SetActive(_currentWordType == WordType.Noun);

            if (_currentWordType == WordType.Punctuation || _currentWordType == WordType.Conjunction || _currentWordType == WordType.Preposition)
            {
                ForceToggleToGeneral(true);
            }
            else if (_keywordCount > 0)
            {
                ForceToggleToGeneral(false);
            }

            GenerateOptionsFor(_currentWordType);

            DisplayOptions();
        }
    }

    private void ForceToggleToGeneral(bool force)
    {
        if (force)
        {
            if (_keywordsActive)
            {
                _keywordToggle.Press();
                _keywordsActive = false;
            }
            _keywordToggle.GetComponent<Button>().interactable = false;
        }
        else
        {
            _keywordToggle.GetComponent<Button>().interactable = true;
        }
    }

    public void GenerateOptionsFor(WordType wordType)
    {
        _generatedNormalWords = _predictive.GetOptionsFor(wordType, Tag.None);
        _generatedKeywords = _predictive.GetOptionsFor(wordType, _currentValidTags);

        if (wordType == WordType.Punctuation)
        {
            int randEmoticon = Random.Range(0, 7);
            //if (randEmoticon == 6)
            //{
                _generatedNormalWords = _predictive.GetOptionsFor(wordType, Tag.Emoticon);
            //}
        }
    }

    public void ToggleCaps()
    {
        if (capsOption == 2)
        {
            capsOption = 0;
        }
        else
        {
            capsOption++;
        }

        DisplayOptions();
    }

    public void ToggleTense()
    {
        if (currentTense == 2)
        {
            currentTense= 0;
        }
        else
        {
            currentTense++;
        }

        DisplayOptions();
    }

    public void TogglePlural()
    {
        if (currentTense == 1)
        {
            currentTense = 0;
        }
        else
        {
            currentTense++;
        }

        DisplayOptions();
    }

    private bool ChooseNewSentenceStructure()
    {
        if (_longSentence && _shortSentence)
        {
            return false;
        }

        bool newSentenceIsLong = false;

        if (!_longSentence && !_shortSentence)
        {
            newSentenceIsLong = Random.Range(0, 2) == 0;
        }
        else 
        {
            newSentenceIsLong = !_longSentence;
        }

        if (newSentenceIsLong)
        {
            int index = Random.Range(0, _longSentenceStructures.Count);
            _selectedStructure = new Queue<WordType>(_longSentenceStructures[index]);
            _longSentence = true;
        }
        else
        {
            int index = Random.Range(0, _shortSentenceStructures.Count);
            _selectedStructure = new Queue<WordType>(_shortSentenceStructures[index]);
            _shortSentence = true;
        }

        return true;
    }

    private void ReadSentenceStructuresFile()
    {
        int shortIndex = 0;
        int longIndex = 0;

        using (CSV csv = new CSV(_sentenceStructuresFile))
        {
            foreach (List<string> row in csv.Data)
            {
                if (row.Count == 0 || row[0] == "") continue;

                switch (row[0])
                {
                    case "short":

                        _shortSentenceStructures.Add(new Queue<WordType>());
                        for (int i = 1; i < row.Count; i++)
                        {
                            if (row[i] == "") break;
                            _shortSentenceStructures[shortIndex].Enqueue(_stringToWordType[row[i]]);
                        }
                        shortIndex++;
                        break;

                    case "long":

                        _longSentenceStructures.Add(new Queue<WordType>());
                        for (int i = 1; i < row.Count; i++)
                        {
                            if (row[i] == "") break;

                            _longSentenceStructures[longIndex].Enqueue(_stringToWordType[row[i]]);
                        }
                        longIndex++;
                        break;

                    default:
                        Debug.LogError("Incorrect format in sentence structures file... " + row[0]);
                        break;
                }


            }
        }
    }



    private void DisplayOptions()
    {
        Word[] wordOptions = _generatedNormalWords;
        if (_keywordsActive) wordOptions = _generatedKeywords;

        int i = 0;
        foreach (Word word in wordOptions)
        {



            _choices[i].text = GetCorrectedWord(word);
            i++;
        }
    }

    private string GetCorrectedWord(Word word)
    {
        string wordText = word.Contents;

        if (word.Type == WordType.Verb)
        {
            wordText = tenses[word.Contents.ToLower()][currentTense];
        }

        if (word.Type == WordType.Noun)
        {
            wordText = tenses[word.Contents.ToLower()][plurals];
        }

        switch (capsOption)
        {
            case 0:
                wordText = wordText.ToLower();
                break;
            case 1:
                string temp = wordText[0].ToString().ToUpper();
                wordText = wordText.ToLower();
                wordText = temp + wordText.Remove(0, 1);

                break;
            case 2:
                wordText = wordText.ToUpper();
                break;
        }

        return wordText;
    }

    public void ToggleKeywords()
    {
        _keywordsActive = !_keywordsActive;
        DisplayOptions();
    }

    public void ChooseWordOption(int optionIndex) //activated by the buttons
    {
        if (_gameData == null) return;

        Word chosenWord = _keywordsActive ? _generatedKeywords[optionIndex] : _generatedNormalWords[optionIndex];

        _completedBrrbl.Add(chosenWord);


        _outputBox.text += GetCorrectedWord(chosenWord);



        if (_keywordsActive) _keywordCount--;

        if (_keywordCount <= 0)
        {
            ForceToggleToGeneral(true);
        }

        GoToNextWord();
    }


    Dictionary<string, WordType> _stringToWordType = new Dictionary<string, WordType>()
    {
        { "noun",        WordType.Noun          },
        { "adjective",   WordType.Adjective     },
        { "adverb",   WordType.Adverb        },
        { "conjunction",   WordType.Conjunction   },
        { "verb",  WordType.Verb          },
        { "punctuation",      WordType.Punctuation   },
        { "preposition", WordType.Preposition },
        { "comma", WordType.Comma }
    };
}
