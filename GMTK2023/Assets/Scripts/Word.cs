using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WordType
{
    Noun,
    Adjective,
    Verb,
    Adverb,
    Conjunction
}

public class Word
{
    public Word(string contents, WordType type, float weight)
    {
        Contents = contents;
        Type = type;
        Weight = weight;
    }

    public string Contents { get; private set; }
    public WordType Type { get; private set; }

    public float Weight { get; private set; }

}
