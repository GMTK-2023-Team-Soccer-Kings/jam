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
    [SerializeField] TextMeshProUGUI _emailCompany;

    [SerializeField] ToggleButton _imageToggle;

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

    public void LoadNewEmail()
    {
        FakeGameData gameData = _gameDataManager.GetRandomGame();

        _emailBody.text = gameData.Description;
        _emailSubject.text = gameData.Subject;

        _gameImage.sprite = gameData.Image.Sprite;
        _predictive.LoadNewBrrbl(gameData);

        _emailCompany.text = gameData.Company;

        if (_imageToggle.state)
        {
            _imageToggle.Press();
        }
    }

}
