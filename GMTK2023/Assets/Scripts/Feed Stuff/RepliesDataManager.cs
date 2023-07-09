using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RepliesDataManager : MonoBehaviour
{
    [SerializeField] TextAsset _replies;

    struct Reply
    {
        public string Content;
        public float Score;
        public Tag Tag;

        public Reply(string content, float score, Tag tag)
        {
            Content = content;
            Score = score;
            Tag = tag;
        }
    }

    List<Reply> _repliesData = new List<Reply>();

    public string[] GetRepliesFor(int score, Tag tag)
    {
        string[] replies = new string[2] { "", "" };

        float[] searchingForScores = new float[2];
        switch (score)
        {
            case 0:
                searchingForScores[0] = 0;
                searchingForScores[1] = 0;
                break;
            case 1:
                searchingForScores[0] = 0;
                searchingForScores[1] = 0.5f;
                break;
            case 2:
                searchingForScores[0] = 1;
                searchingForScores[1] = 0.5f;
                break;
            case 3:
                searchingForScores[0] = 1;
                searchingForScores[1] = 1;
                break;
        }

        for (int i = 0; replies[1] == ""; i++)
        {
            int index = Random.Range(0, _repliesData.Count - i);

            float randScore = _repliesData[index].Score;
            Tag randTag = _repliesData[index].Tag;

            if ((randScore == searchingForScores[0] || randScore == searchingForScores[1]) && (tag.HasFlag(randTag) || randTag.Equals(Tag.None)))
            {
                if (replies[0] == "")
                {
                    replies[0] = _repliesData[index].Content;
                    for (int j = 0; j < searchingForScores.Length; j++)
                    {
                        if (searchingForScores[j] == randScore)
                        {
                            searchingForScores[j] = 1000;
                            break;
                        }
                    }
                }
                else
                {
                    replies[1] = _repliesData[index].Content;
                }
               
            }

            Reply temp = _repliesData[_repliesData.Count - 1 - i];
            _repliesData[_repliesData.Count - 1 - i] = _repliesData[index];
            _repliesData[index] = temp;

        }

        return replies;
    }



    private void Awake()
    {
        LoadReplyData();
    }

    private void LoadReplyData()
    {
        using (CSV csv = new CSV(_replies))
        {
            bool skippedFirst = false;
            foreach (List<string> row in csv.Data)
            {
                if (!skippedFirst) { skippedFirst = true; continue; }
                if (row[0] == "") continue;

                string content = row[0];

                float score = 0;
                switch (row[1])
                {
                    case "good":
                        score = 1;
                        break;
                    case "meh":
                        score = 0.5f;
                        break;
                    case "bad":
                        score = 0;
                        break;
                }

                Tag tag = TagData.StringToTag[row[2]];

                _repliesData.Add(new Reply(content, score, tag));
            }
        }
    }
}
