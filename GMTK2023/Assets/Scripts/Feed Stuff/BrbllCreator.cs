using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrbllCreator : MonoBehaviour
{
    RepliesDataManager _repliesData;

    [SerializeField] UserBrbll _userBrbllPrefab;
    [SerializeField] ReplyBrbll _replyBrbllPrefab;

    [SerializeField] Transform _feedContentParent;

    (int, int)[] _likeRanges = new (int, int)[4]
    {
        (5, 75),
        (101, 400),
        (501, 1300),
        (1500, 5000)
    };

    (int, int) _replyLikeRange = (0, 300);

    private void Awake()
    {
        _repliesData = GetComponent<RepliesDataManager>();
    }

    public void AddUserBrbll(string content, int score, FakeGameData gameData)
    {
        int likes = Random.Range(_likeRanges[score].Item1, _likeRanges[score].Item2);
        string[] replies = _repliesData.GetRepliesFor(score, gameData.Tags);

        UserBrbll userBrbll = Instantiate(_userBrbllPrefab, _feedContentParent);
        userBrbll.Setup(content, likes, gameData.Image.Sprite);

        AddReplyBrbll(replies[0]);
        AddReplyBrbll(replies[1]);
    }

    public void AddReplyBrbll(string content)
    {
        int likes = Random.Range(_replyLikeRange.Item1, _replyLikeRange.Item2);

        ReplyBrbll replyBrbll = Instantiate(_replyBrbllPrefab, _feedContentParent);
        replyBrbll.Setup(content, likes);

    }
}
