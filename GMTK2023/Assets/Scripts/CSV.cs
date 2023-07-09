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
        Data[0].Add("");

        int row = 0;
        int column = 0;

        bool insideQuoteBlock = false;
        foreach (char c in textAsset.text)
        {
            if (c == '"')
            {
                insideQuoteBlock = !insideQuoteBlock;

            }
            else if (c == ',' && !insideQuoteBlock)
            {
                column++;
                Data[row].Add("");
            }
            else if (c == '\n' || c == '\r')
            {
                row++;
                Data.Add(new List<string>());
                Data[row].Add("");
                column = 0;
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
