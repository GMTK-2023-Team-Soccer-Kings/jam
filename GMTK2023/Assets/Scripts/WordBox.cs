using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//DEPRECATED

public class WordBox : MonoBehaviour
{
    [SerializeField] WordType _wordType;
    Action<WordType> _setupOptionsFnc;

    private void Start()
    {
        if (_setupOptionsFnc == null) // mainly just for testing purposes
        {
            GetComponent<Button>().onClick.AddListener(OnSelect);

        }
    }

    public void Setup(WordType wordType, Action<WordType> setupOptionsFnc)
    {
        _wordType = wordType;
        _setupOptionsFnc = setupOptionsFnc;
        GetComponent<Button>().onClick.AddListener(OnSelect);
    }

    private void OnSelect()
    {
        _setupOptionsFnc(_wordType);
    }
}
