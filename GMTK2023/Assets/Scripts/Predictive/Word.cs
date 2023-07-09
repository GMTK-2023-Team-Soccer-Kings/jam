using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WordType
{
    Noun,
    Adjective,
    Verb,
    Adverb,
    Conjunction,
    Punctuation,
    Preposition,
    Comma
}

public class Word
{
    public Word(string contents, WordType type, Tag tag)
    {
        Contents = contents;
        Type = type;
        Tag = tag;  
    }

    public string Contents { get; private set; }
    public WordType Type { get; private set; }

    public Tag Tag { get; private set; }
}
