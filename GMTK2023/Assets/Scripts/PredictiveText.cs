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
        Dictionary<Tag, List<Word>> words = new Dictionary<Tag, List<Word>>();

        using (CSV csv = new CSV(_text))
        {
            foreach (List<string> row in csv.Data)
            {
                
            }
        }

    }

    public void GenerateSentence()
    {

    }
}
