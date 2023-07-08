using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class PredictiveText : MonoBehaviour
{
    [Header("Words Data")]
    [SerializeField] TextAsset _nounsFile;
    [SerializeField] TextAsset _verbsFile;
    [SerializeField] TextAsset _adjectivesFile;
    [SerializeField] TextAsset _conjunctionsFile;
    [SerializeField] TextAsset _punctuationsFile;


    Dictionary<WordType, Dictionary<Tag, List<Word>>> _wordsByType = new Dictionary<WordType, Dictionary<Tag, List<Word>>>();

    Dictionary<WordType, List<Word>> _untaggedWordsByType = new Dictionary<WordType, List<Word>>();

    private void Awake()
    {
        AddWordsFromFile(_nounsFile, WordType.Noun);
        AddWordsFromFile(_verbsFile, WordType.Verb);
        AddWordsFromFile(_adjectivesFile, WordType.Adjective);
        AddWordsFromFile(_conjunctionsFile, WordType.Conjunction);
        AddWordsFromFile(_punctuationsFile, WordType.Punctuation);
    }

    private void AddWordsFromFile(TextAsset _text, WordType type)
    {
        if (_text == null)
        {
            Debug.LogWarning("Warning: File missing for " + type.ToString());
            return;
        }
        Dictionary<Tag, List<Word>> wordsByTag = new Dictionary<Tag, List<Word>>();
        List<Word> untaggedWords = new List<Word>();

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

                Word word = new Word(row[0].ToLower(), type, tag);

                if (tag.Equals(Tag.None))
                {
                    untaggedWords.Add(word);
                    continue;
                }

                for (int i = 1; i < (int)Tag.End; i *= 2)
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
        _untaggedWordsByType.Add(type, untaggedWords);

    }

    public Word[] GetOptionsFor(WordType type, Tag reqTag)
    {
        const int OPTIONS_COUNT = 3;
        Word[] options = new Word[OPTIONS_COUNT];

        if (reqTag == Tag.None)
        {
            List<Word> validWords = _untaggedWordsByType[type];

            for (int i = 0; i < OPTIONS_COUNT; i++)
            {
                int index = Random.Range(0, validWords.Count - i);
                options[i] = validWords[index];

                Word temp = validWords[validWords.Count - 1 - i];
                validWords[validWords.Count - 1 - i] = validWords[index];
                validWords[index] = temp;
            }
        }
        else
        {
            if (type == WordType.Punctuation || type == WordType.Conjunction)
            {
                return new Word[0];
            }

            Word[] tempOptions = new Word[OPTIONS_COUNT]; //preshuffle
            Tag tag = Tag.Adventure;

            for (int i = 0; i < _allTags.Length; i++)
            {
                if (reqTag.HasFlag(_allTags[i]))
                {
                    tag = _allTags[i];

                    if (Random.Range(0, 2) > 0) break; //bad randomness but whatever its a jam
                }
            }

            List<Word> taggedWords = _wordsByType[type][tag];
            int index = Random.Range(0, taggedWords.Count);

            tempOptions[0] = taggedWords[index];

            for (int i = 1; i < OPTIONS_COUNT; i++)
            {
                Tag randomTag = _allTags[Random.Range(0, _allTags.Length)];
                index = Random.Range(0, _wordsByType[type][randomTag].Count);

                tempOptions[i] = _wordsByType[type][randomTag][index];
            }

            int[] indices = new int[OPTIONS_COUNT];
            for (int i = 0; i < OPTIONS_COUNT; i++)
            {
                indices[i] = i;
            }

            for (int i = 0; i < OPTIONS_COUNT; i++)
            {
                int randomIndex = Random.Range(0, OPTIONS_COUNT - i);

                options[i] = tempOptions[indices[randomIndex]];

                int temp = indices[randomIndex];
                indices[randomIndex] = indices[indices.Length - 1 - i];
                indices[indices.Length - 1 - i] = temp;
            }

        }

        return options;
    }

    Dictionary<string, Tag> _stringToTag = new Dictionary<string, Tag>()
    {
        { "none",        Tag.None         },
        { "adv",   Tag.Adventure    },
        { "stry",   Tag.StoryRich    },
        { "hrd",        Tag.Difficult    },
        { "jump",       Tag.Platformer   },
        { "hrr",      Tag.Horror       },
        { "chrm",     Tag.Charming     },
        { "gore",     Tag.Gore         },
        { "rl",      Tag.Roguelike    },
        { "pzl",      Tag.Puzzle       },
        { "sim",   Tag.Simulator    },
        { "srv",    Tag.Survival     },
        { "cg",    Tag.CardGame     },
        { "rpg",         Tag.RPG          },
    };

    Tag[] _allTags = new Tag[]
    {
        Tag.Adventure,
        Tag.StoryRich,
        Tag.Difficult,
        Tag.Platformer,
        Tag.Horror,
        Tag.Charming,
        Tag.Gore,
        Tag.Roguelike,
        Tag.Puzzle,
        Tag.Simulator,
        Tag.Survival,
        Tag.CardGame,
        Tag.RPG
    };
}
