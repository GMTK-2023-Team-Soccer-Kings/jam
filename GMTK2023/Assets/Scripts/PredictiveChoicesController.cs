using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PredictiveChoicesController : MonoBehaviour
{
    PredictiveText _predictive;

    WordType _selectedWordType;
    [SerializeField] Tag _currentValidTags;

    bool _keywordsActive = false;

    [SerializeField] TextMeshProUGUI[] _choices;

    private void Awake()
    {
        _predictive = GetComponent<PredictiveText>();
    }

    private void SetupSentence()
    {

    }


    public void ShowOptionsFor(WordType wordType)
    {
        _selectedWordType = wordType;
        Tag tag = _currentValidTags;
        if (!_keywordsActive) tag = Tag.None;

        int i = 0;
        foreach (Word word in _predictive.GetOptionsFor(wordType, tag))
        {
            _choices[i].text = word.Contents;
            i++;
        }
    }
}
