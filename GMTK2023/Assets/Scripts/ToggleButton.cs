using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{

    public bool state;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Press()
    {
        state = !state;
        animator.SetTrigger("Swap");
    }

}
