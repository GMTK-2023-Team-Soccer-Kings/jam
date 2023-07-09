using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSwap : MonoBehaviour
{

    public GameObject Bmail;
    public GameObject Brblls;

    void Toggle()
    {
        Bmail.SetActive(!Bmail.activeInHierarchy);
        Brblls.SetActive(!Brblls.activeInHierarchy);
    }

}
