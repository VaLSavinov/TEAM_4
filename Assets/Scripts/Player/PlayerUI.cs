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
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject _settingMenu;
    [SerializeField] private Slider _sliderVolume;
    [SerializeField] private Slider _sliderSensitiviti;
    [SerializeField] private Animation _animate;
    [SerializeField] private List<AnimationClip> _clips;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource winSound;

    private AudioSource _audioSource;
    private PlayerControl _playerControl;
    private AudioSource[] _audios;
    private List<AudioSource> _pauseAudios = new List<AudioSource>();
    private bool _lastVisible = true;
    private bool _isImpact = false;
    

    private void Awake()
    {
        GameMode.PlayerUI = this;
        _audioSource = GetComponent<AudioSource>();
        _playerControl = new PlayerControl();
        _playerControl.UI.PauseMenu.started += context => Resume();
    }

    public void Start()
    {
        _isImpact = true;
        _animate.clip = _clips[3];
        _animate.Play();
    }

    public void StopAllSound()
    {
        if (_audios == null)
        {
            _audios = FindObjectsOfType<AudioSource>();
        }
        foreach (var audio in _audios)
        {
            if (audio.isPlaying)
            {
                audio.Pause();
                _pauseAudios.Add(audio);
            }
        }
    }

    public void PlayPausedAudios()
    { 
        foreach (var audio in _pauseAudios)
            { audio.Play(); }
    }

    private void ReplaceAvail(string current, string newAvail) 
    {
        List<string> tags = LocalizationManager.Instance.GetTagList(current, true);
        foreach (var tag in tags)
        {
            LocalizationManager.Instance.WriteAvailForTag(tag, newAvail);
        }
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
        ReplaceAvail("grab","f");
        _playerControl.UI.PauseMenu.started -= context => Resume();
        _playerControl.Disable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        PlayPausedAudios();
       
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        StopAllSound();
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _playerControl.Enable();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _playerControl.Disable();
        pauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayPausedAudios();
    }

    public void Finish()
    {
        ReplaceAvail("grab", "n");
        LocalizationManager.Instance.SafeCSV();
        StopAllSound();
        finishScreen.SetActive(true);
        winScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winSound.Play();
    }

    public void GameOver()
    {
        ReplaceAvail("grab", "f");
        StopAllSound();
        finishScreen.SetActive(true);
        deathScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMode.FirstPersonLook.BlockPlayerController();
        deathSound.Play();
    }

    public void OpenSetting()
    {
        _settingMenu.SetActive(true);
        _sliderVolume.value = float.Parse(Settings.Instance.GetParam("volume"));
        _sliderSensitiviti.value = float.Parse(Settings.Instance.GetParam("sensitivity"));
    }

    public void SaveSetting()
    {
        Settings.Instance.SafeCSV();
        GameMode.FirstPersonLook.ChangeSettings();
        _settingMenu.SetActive(false);
    }

    public void ChangeValueSound()
    {
        Settings.Instance.SetParam("volume", _sliderVolume.value.ToString());
        AudioListener.volume = _sliderSensitiviti.value;
    }

    public void ChangeValueSensetiviti()
    {
        Settings.Instance.SetParam("sensitivity", _sliderSensitiviti.value.ToString());
    }

    public void ChangeVisiblePayer(bool isVisible) 
    {
        if (_lastVisible != isVisible && !_isImpact)
        {         
            if (isVisible )
                _animate.clip = _clips[0];
            else
            _animate.clip = _clips[1]; 
            _lastVisible = isVisible;
            _animate.Play();
         }
    }

    public void ImpactAnimate() 
    {
        _animate.clip = _clips[2];
        _isImpact = true;
        _animate.Play();
    }

    public void StartGame() 
    {
        _isImpact = false;
        GameMode.FirstPersonMovement.UnBlockControl();
    }
}
