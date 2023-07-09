using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScrollSetup : MonoBehaviour
{
    [SerializeField] Transform _content;

    // Start is called before the first frame update
    void Start()
    {
        UserBrbll[] brblls = FindObjectsOfType<UserBrbll>();


        foreach (UserBrbll brbll in brblls)
        {
            brbll.transform.SetParent(_content);
        }
    }
}
