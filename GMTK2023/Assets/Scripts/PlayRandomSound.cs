using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    [SerializeField] AudioClip[] _clips;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandom()
    {
        int i = Random.Range(0, _clips.Length);
        _audioSource.PlayOneShot(_clips[i]);
    }

}
