using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailGenerator : MonoBehaviour
{
    FakeGameDataManager _gameDataManager;

    [SerializeField] TextMeshProUGUI _emailBody;
    [SerializeField] TextMeshProUGUI _emailSubject;
    [SerializeField] Image _gameImage;

    PredictiveChoicesController _predictive;

    private void Awake()
    {
        _gameDataManager = GetComponent<FakeGameDataManager>();
        _predictive = FindObjectOfType<PredictiveChoicesController>();
    }

    private void Start()
    {
        LoadNewEmail();
    }

    private void LoadNewEmail()
    {
        FakeGameData gameData = _gameDataManager.GetRandomGame();

        _emailBody.text = gameData.Description;
        _emailSubject.text = gameData.Name;

        _gameImage.sprite = gameData.Image.Sprite;
        _predictive.LoadNewBrrbl(gameData.Tags);
    }

}
