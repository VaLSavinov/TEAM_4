using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private LoaclizationText _centerText;

    private AudioSource _audioSource;

    private void Awake()
    {
        GameMode.PlayerUI = this;
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// �������� ����� � ������ ������
    /// </summary>
    /// <param name="text"> ������������ �����</param>
    /// <param name="isRewrite"> ����������� �����, ���� � ������ ������ ���������� ������</param>
    public void ShowText(string text, bool isRewrite) 
    {
        if (!isRewrite && _panel.activeSelf) return;
        _centerText.SetTag(text);
        _panel.SetActive(true);
    }

    public void DeactivatePanel() 
    {
        _panel.SetActive(false);
    }

    public void PlayAudioClip(AudioClip clip) 
    {
        if (!_audioSource.isPlaying) 
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
