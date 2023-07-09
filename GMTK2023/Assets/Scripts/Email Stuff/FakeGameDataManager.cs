using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeGameDataManager : MonoBehaviour
{
    [SerializeField] TextAsset _gameCSV;

    [SerializeField] List<GameImage> _gameImages;

    Dictionary<string, GameImage> _gameImagesByName = new Dictionary<string, GameImage>();

    List<FakeGameData> _gameData = new List<FakeGameData>();

    [SerializeField] string _endScene;

    public FakeGameData GetRandomGame()
    {
        if (_gameData.Count == 0)
        {
            foreach (UserBrbll brbll in FindObjectsOfType<UserBrbll>())
            {
                brbll.transform.SetParent(null);
                DontDestroyOnLoad(brbll.gameObject);
            }

            SceneManager.LoadScene(_endScene);
            return null;
        }


        int index = Random.Range(0, _gameData.Count);
        FakeGameData game = _gameData[index];

        _gameData.Remove(game);

        return game;
    }


    private void Awake()
    {
        LoadGameImages();
        LoadGameData();
    }

    private void LoadGameImages()
    {
        foreach (GameImage image in _gameImages)
        {
            _gameImagesByName[image.ParodyGameName] = image;
        }
    }

    private void LoadGameData()
    {
        using (CSV csv = new CSV(_gameCSV))
        {
            foreach (List<string> row in csv.Data)
            {
                if (row[0] == "") continue;

                string title = row[0];
                Tag tag = Tag.None;

                const int TAG_START = 2;
                const int TAG_COUNT = 3;
                for (int i = TAG_START; i < TAG_START + TAG_COUNT; i++)
                {
                    tag |= TagData.StringToTag[row[i]];
                }

                GameImage image = _gameImagesByName[title];
                string description = row[5];

                _gameData.Add(new FakeGameData(title, description, tag, image, row[6], row[7]));

            }
        }
    }

}
