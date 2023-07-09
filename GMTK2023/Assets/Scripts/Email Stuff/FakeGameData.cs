using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGameData
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Tag Tags { get; private set; }
    public GameImage Image { get; private set; }
    public string Company { get; private set; }
    public string Subject { get; private set; }


    public FakeGameData(string name, string description, Tag tags, GameImage image, string company, string subject)
    {
        Name = name;
        Description = description;
        Tags = tags;
        Image = image;
        Company = company;
        Subject = subject;
    }
}

