using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiancePlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;

    AudioSource audioSource;

    (int, int) intervalRange = (15, 70);

    bool mute = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(PlaySounds());
    }

    IEnumerator PlaySounds()
    {
        while (true)
        {
            float time = (float)Random.Range(intervalRange.Item1, intervalRange.Item2) / (float) 10;

            yield return new WaitForSeconds(time);

            audioSource.panStereo = (float)Random.Range(-100, 100) / (float)100;

            int i = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[i];

            audioSource.Play();
        }
    }

    public void ToggleMute()
    {
        mute = !mute;
        if (mute)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = 1;

        }
    }

}
