using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class LibraryMenu : MonoBehaviour
{
    [Header("����� ����������")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private TMP_Text _outText;
    [SerializeField] private Image _imageMin;
    [SerializeField] private Image _imageMax;
    [SerializeField] private GameObject _buttonPlay;

    [Header("�������� ������")]
    [SerializeField] private GameObject _prefabButton;
    [SerializeField] private Transform _buttonsGroup;
    [SerializeField] private Sprite _buttonNew;
    [SerializeField] private Sprite _buttonOld;

    [Header("����� ���������")]
    [SerializeField] private SOCollections _repotsSO;
    [SerializeField] private SOCollections _audioSO;

    [SerializeField] private List<string> _tags = new List<string>();
    private List<GameObject> _buttons = new List<GameObject>();
    private LoaclizationText _localizateText;

    private void Awake()
    {
        _localizateText = _outText.GetComponent<LoaclizationText>();
    }

    public void OpenLibrary()
    {
        GameObject newButtonObject;
        ButtonOpenCollect newButton;
        List<string> tags = new List<string>();
        _outText.gameObject.SetActive(false);
        _imageMin.gameObject.SetActive(false);
        _imageMax.gameObject.SetActive(false);
        _buttonPlay.gameObject.SetActive(false);
        _audioSource.volume = float.Parse(Settings.GetParam("volume"));
        tags = LocalizationManager.GetTagList("n", true);
        foreach (string tag in tags)
        {
            newButtonObject =  GameObject.Instantiate(_prefabButton,_buttonsGroup);
            newButton = newButtonObject.GetComponent<ButtonOpenCollect>();
            newButton.SetLibrary(this);
            newButton.SetSprits(_buttonOld,_buttonNew,true);
            newButton.SetTag(tag);
            _buttons.Add(newButtonObject);

        }
        tags = LocalizationManager.GetTagList("t", true);
        foreach (string tag in tags)
        {
            newButtonObject = GameObject.Instantiate(_prefabButton, _buttonsGroup);
            newButton = newButtonObject.GetComponent<ButtonOpenCollect>();
            newButton.SetLibrary(this);
            newButton.SetSprits(_buttonOld, _buttonNew, false);
            newButton.SetTag(tag);
            _buttons.Add(newButtonObject);
        }
    }

    public void ButtonClic(string tag) 
    {
        _audioSource.Stop();
        _localizateText.SetTag(tag);
        _outText.gameObject.SetActive(true);
        if (tag.Contains("Image"))
        {
            _imageMax.sprite = _repotsSO.GetImageForTag(tag);
            _imageMax.gameObject.SetActive(true);
            _imageMin.gameObject.SetActive(false);
            _buttonPlay.SetActive(false);
            return;
        }
        if (tag.Contains("Reports."))
        {
            _imageMin.sprite = _repotsSO.GetImageForTag(tag);
            _imageMax.gameObject.SetActive(false);
            _imageMin.gameObject.SetActive(true);
            _buttonPlay.SetActive(false);
            return;
        }
        if (tag.Contains("Audio."))
        {
            _audioSource.clip = _audioSO.GetAudioForTag(tag);
            _imageMax.gameObject.SetActive(false);
            _imageMin.gameObject.SetActive(false);
            _buttonPlay.SetActive(true);
            return;
        }


    }

    public void PlayAudio() 
    {
        if(_audioSource.isPlaying)
            _audioSource.Pause();
        else _audioSource.Play();
    }


    public void Back() 
    {
        LocalizationManager.SafeCSV();
        foreach (GameObject button in _buttons)
        {
            Destroy(button);
        }
        _audioSource.Stop();
        _buttons.Clear();    
        gameObject.SetActive(false);
    }

}
