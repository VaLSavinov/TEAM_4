using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextAsset _setting;
    [SerializeField] private TextAsset _textAsset;
    [SerializeField] private Slider _sliderVolume;
    [SerializeField] private Slider _sliderSensitiviti;
    [SerializeField] private GameObject _settingMenu;

    private void Start()
    {
        Settings.SetCSV(_setting);
        LocalizationManager.SetCSV(_textAsset);
    }

    // Вешаем на кнопку, по которой меняем язык
    public void ChangeLang()
    {
        LocalizationManager.Change();
    }

    public void OpenSetting()
    {
        _settingMenu.SetActive(true);
        _sliderVolume.value = float.Parse(Settings.GetParam("volume"));
        _sliderSensitiviti.value = float.Parse(Settings.GetParam("sensitivity"));
    }

    public void LoadScene(int sceneIndex)
    {
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }


    public void SaveSetting() 
    {
        Settings.SafeCSV();
    }

    public void ChangeValueSound()
    {         
        Settings.SetParam("volume", _sliderVolume.value.ToString());
    }

    public void ChangeValueSensetiviti()
    {
        Settings.SetParam("sensitivity", _sliderSensitiviti.value.ToString());
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}
