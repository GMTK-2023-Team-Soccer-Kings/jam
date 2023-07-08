using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PredictiveChoicesController : MonoBehaviour
{
    PredictiveText _predictive;

    [SerializeField] Tag _currentValidTags;

    bool _keywordsActive = false;

    [SerializeField] TextMeshProUGUI[] _choices;

    [SerializeField] TextAsset _sentenceStructuresFile;

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

    private void Awake()
    {
        _predictive = GetComponent<PredictiveText>();
        ReadSentenceStructuresFile();
    }

    private void Start()
    {
        ChooseNewSentenceStructure();
        GoToNextWord();
    }

    private void GoToNextWord()
    {
        //TODO Check for completed sentence

        _currentWordType = _selectedStructure.Dequeue();
        GenerateOptionsFor(_currentWordType);

        DisplayOptions();
    }

    public void GenerateOptionsFor(WordType wordType)
    {
        _generatedNormalWords = _predictive.GetOptionsFor(wordType, Tag.None);
        _generatedKeywords = _predictive.GetOptionsFor(wordType, _currentValidTags);
    }

    private void ChooseNewSentenceStructure()
    {
        if (_longSentence && _shortSentence)
        {
            Debug.LogError("Brrbl is ready to be sent, but code attempted new sentence structure.");
            return;
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
            _selectedStructure = _longSentenceStructures[index];
        }
        else
        {
            int index = Random.Range(0, _shortSentenceStructures.Count);
            _selectedStructure = _shortSentenceStructures[index];
        }
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
            _choices[i].text = word.Contents;
            i++;
        }
    }

    public void ToggleKeywords()
    {
        _keywordsActive = !_keywordsActive;
        DisplayOptions();
    }

    public void ChooseWordOption(int optionIndex) //activated by the buttons
    {
        Word chosenWord = _keywordsActive ? _generatedKeywords[optionIndex] : _generatedNormalWords[optionIndex];

        _completedBrrbl.Add(chosenWord);

        _outputBox.text += chosenWord.Contents + " ";
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
    };
}
