using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReplyBrbll : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _content;
    [SerializeField] TextMeshProUGUI _likes;
    public void Setup(string content, int likes)
    {
        _content.text = content;
        _likes.text = likes.ToString();
    }
}
