using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserBrbll : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _content;
    [SerializeField] TextMeshProUGUI _likes;
    [SerializeField] Image _image;
    public void Setup(string content, int likes, Sprite gameImage)
    {
        _content.text = content;
        _likes.text = likes.ToString();
        _image.sprite = gameImage;
    }
}
