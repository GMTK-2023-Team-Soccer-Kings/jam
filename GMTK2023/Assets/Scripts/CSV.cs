using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSV : IDisposable
{
    public List<List<string>> Data { get; private set; }
    public CSV(TextAsset textAsset)
    {
        Data = new List<List<string>>();
        Data.Add(new List<string>());

        int row = 0;
        int column = 0;
        foreach (char c in textAsset.text)
        {
            if (c == ',')
            {
                column++;
                Data[row].Add("");
            }
            else if (c == '\n')
            {
                row++;
                Data.Add(new List<string>());
            }
            else
            {
                Data[row][column] += c;
            }
        }
    }

    public void Dispose()
    {
        Data.Clear();
    }
}
