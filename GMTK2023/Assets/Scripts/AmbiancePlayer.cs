using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiancePlayer : MonoBehaviour
{
    AudioClip[] audioClips;

    AudioSource audioSource;

    (int, int) intervalRange = (15, 70);

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

            int i = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[i];

            audioSource.Play();
        }
    }

    private void Update()
    {
        audioSource.panStereo += 0.1f * Time.deltaTime;
    }

}
