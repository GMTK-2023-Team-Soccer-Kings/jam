using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    AudioSource _audioSource;

    [SerializeField] AudioClip _brbrrAudioClip;
    [SerializeField] AudioClip _bmailAudioClip;

    bool _bmailActive = true;

    [SerializeField] GameObject _notifBmail;
    [SerializeField] GameObject _notifBrbrr;

    EmailGenerator _emailGenerator;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _emailGenerator = FindObjectOfType<EmailGenerator>();
    }

    public void Toggle()
    {
        _bmailActive = !_bmailActive;

        if (_bmailActive)
        {
            _notifBmail.SetActive(false);
        }
        else
        {
            if (_notifBrbrr.activeSelf)
            {
                StartCoroutine(NewBmail());
            }

            _notifBrbrr.SetActive(false);
        }

    }

    public void AddNotif(bool isBrbrr)
    {
        PlaySound(isBrbrr);

        if ((_bmailActive && !isBrbrr) || (!_bmailActive && isBrbrr))
        {
            if (isBrbrr)
            {
                StartCoroutine(NewBmail());
            }

            return;
        }

        if (isBrbrr)
        {
            _notifBrbrr.SetActive(true);
        }
        else
        {
            _notifBmail.SetActive(true);
        }
    }

    private void PlaySound(bool isBrbrr)
    {
        if (isBrbrr)
        {
            _audioSource.volume = 1;
            _audioSource.PlayOneShot(_brbrrAudioClip);
        }
        else
        {
            _audioSource.volume = 2;
            _audioSource.PlayOneShot(_bmailAudioClip);
        }
    }

    IEnumerator NewBmail()
    {
        float time = (float)Random.Range(3, 20) / (float)10;

        yield return new WaitForSeconds(time);
        
        AddNotif(false);
        _emailGenerator.LoadNewEmail();
    }
}
