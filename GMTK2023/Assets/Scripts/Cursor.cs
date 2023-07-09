using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    private RectTransform cursorTransform;

    // Start is called before the first frame update
    void Awake()
    {
        cursorTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        cursorTransform.position = Input.mousePosition;
    }
}
