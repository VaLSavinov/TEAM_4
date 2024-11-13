using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour, IInteractable
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (_audioSource != null)
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
                Debug.Log("���� ��������");
            }
            else
            {
                _audioSource.Play();
                Debug.Log("���� �������");
            }
        }
    }
}
