using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictiveText : MonoBehaviour
{
    [SerializeField] TextAsset _sentenceStructuresFile;

    [Header("Words Data")]
    [SerializeField] TextAsset _nounsFile;
    [SerializeField] TextAsset _verbsFile;
    [SerializeField] TextAsset _adjectivesFile;
    [SerializeField] TextAsset _adverbsFile;
    [SerializeField] TextAsset _conjunctionsFile;

    Dictionary<WordType, Dictionary<Tag, List<Word>>> _wordsByType = new Dictionary<WordType, Dictionary<Tag, List<Word>>>();

    private void Awake()
    {
        AddWordsFromFile(_nounsFile, WordType.Noun);
        AddWordsFromFile(_verbsFile, WordType.Verb);
        AddWordsFromFile(_adjectivesFile, WordType.Adjective);
        AddWordsFromFile(_adverbsFile, WordType.Adverb);
        AddWordsFromFile(_conjunctionsFile, WordType.Conjunction);
    }

    private void AddWordsFromFile(TextAsset _text, WordType type)
    {
        if (_text == null)
        {
            Debug.LogWarning("Warning: File missing for " + type.ToString());
            return;
        }
        Dictionary<Tag, List<Word>> wordsByTag = new Dictionary<Tag, List<Word>>();

        using (CSV csv = new CSV(_text))
        {
            foreach (List<string> row in csv.Data)
            {
                if (row[0] == "") continue;

                Tag tag = Tag.None;
                for (int i = 1; i < row.Count; i++)
                {
                    if (row[i] == "") continue;

                    tag |= _stringToTag[row[i].ToLower()];

                }

                Word word = new Word(row[0], type, tag);

                if (tag.Equals(Tag.None))
                {
                    if (wordsByTag.TryGetValue(Tag.None, out List<Word> words))
                    {
                        wordsByTag[Tag.None].Add(word);
                    }
                    else
                    {
                        wordsByTag.Add(Tag.None, new List<Word>() { word });
                    }
                }

                for (int i = 1; i < (int)Tag.End; i*=2 )
                {
                    if (tag.HasFlag((Tag)i))
                    {
                        if (wordsByTag.TryGetValue((Tag)i, out List<Word> words))
                        {
                            wordsByTag[(Tag)i].Add(word);
                        }
                        else
                        {
                            wordsByTag.Add((Tag)i, new List<Word>() { word });
                        }
                    }
                }
            }
        }

        _wordsByType.Add(type, wordsByTag);

    }

    public void GenerateSentence()
    {

    }


    public Word[] GetOptionsFor(WordType type, Tag tag)
    {
        List<Word> validWords = _wordsByType[type][tag];

        const int OPTIONS_COUNT = 3;

        Word[] options = new Word[OPTIONS_COUNT];
        for (int i = 0; i < OPTIONS_COUNT; i++)
        {
            int index = Random.Range(0, validWords.Count-1-i);
            options[i] = validWords[index];

            Word temp = validWords[validWords.Count - 1 - i];
            validWords[validWords.Count-1-i] = validWords[index];
            validWords[index] = temp;


        }

        return options;
    }

    Dictionary<string, Tag> _stringToTag = new Dictionary<string, Tag>()
    {
        { "none",        Tag.None         },
        { "adventure",   Tag.Adventure    },
        { "storyRich",   Tag.StoryRich    },
        { "difficult",   Tag.Difficult    },
        { "platformer",  Tag.Platformer   },
        { "horror",      Tag.Horror       },
        { "charming",    Tag.Charming     },
        { "gore",        Tag.Gore         },
        { "roguelike",   Tag.Roguelike    },
        { "puzzle",      Tag.Puzzle       },
        { "simulator",   Tag.Simulator    },
        { "survival",    Tag.Survival     },
        { "cardGame",    Tag.CardGame     },
        { "rpg",         Tag.RPG          },
    };
}
