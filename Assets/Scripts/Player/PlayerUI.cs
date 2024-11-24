using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject finishScreen;
    [SerializeField] private GameObject _panel;
    [SerializeField] private LoaclizationText _centerText;
    [SerializeField] private GameObject wintext;
    [SerializeField] private GameObject _settingMenu;
    [SerializeField] private Slider _sliderVolume;
    [SerializeField] private Slider _sliderSensitiviti;

    private AudioSource _audioSource;
    private PlayerControl _playerControl;

    private void Awake()
    {
        GameMode.PlayerUI = this;
        _audioSource = GetComponent<AudioSource>();
        _playerControl = new PlayerControl();
        _playerControl.UI.PauseMenu.started += context => Resume();
    }

    /// <summary>
    /// Ïîêàçàòü òåêñò â öåíòðå ýêðàíà
    /// </summary>
    /// <param name="text"> Ïîêàçûâàåìûé òåêñò</param>
    /// <param name="isRewrite"> Ïåðåçàïèñòü òåêñò, åñëè â äàííûé ìîìåíò âûâîäèòüñÿ äðóãîé</param>
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
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _playerControl.Enable();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerControl.Disable();
    }

    public void Finish()
    {
        finishScreen.SetActive(true);
        wintext.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _playerControl.Disable();
    }

    public void OpenSetting()
    {
        _settingMenu.SetActive(true);
        _sliderVolume.value = float.Parse(Settings.GetParam("volume"));
        _sliderSensitiviti.value = float.Parse(Settings.GetParam("sensitivity"));
    }

    public void SaveSetting()
    {
        Settings.SafeCSV();
        GameMode.FirstPersonLook.ChangeSettings();
        _settingMenu.SetActive(false);
    }

    public void ChangeValueSound()
    {
        Settings.SetParam("volume", _sliderVolume.value.ToString());
    }

    public void ChangeValueSensetiviti()
    {
        Settings.SetParam("sensitivity", _sliderSensitiviti.value.ToString());
    }
}
