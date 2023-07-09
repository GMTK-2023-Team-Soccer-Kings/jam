using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSounds : MonoBehaviour
{
    public AudioClip open;
    public AudioClip close;

    AudioSource sound;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }

    public void Close ()
    {
        sound.clip = close;
        sound.Play();
    }

    public void Open()
    {
        sound.clip = open;
        sound.Play();
    }

}
