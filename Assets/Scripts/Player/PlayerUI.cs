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
    [SerializeField] private GameObject _overText;
    [SerializeField] private GameObject _settingMenu;
    [SerializeField] private Slider _sliderVolume;
    [SerializeField] private Slider _sliderSensitiviti;
    [SerializeField] private Animation _animate;
    [SerializeField] private List<AnimationClip> _clips;

    private AudioSource _audioSource;
    private PlayerControl _playerControl;
    private AudioSource[] _audios;
    private List<AudioSource> _pauseAudios = new List<AudioSource>();
    private bool _lastVisible = true;

    // Добавляем ссылку на GridManager
    private GridManager _gridManager;

    private void Awake()
    {
        GameMode.PlayerUI = this;
        _audioSource = GetComponent<AudioSource>();
        _playerControl = new PlayerControl();
        _playerControl.UI.PauseMenu.started += context => Resume();

        // Получаем ссылку на GridManager
        _gridManager = FindObjectOfType<GridManager>();
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

    private void PlayPausedAudios()
    {
        foreach (var audio in _pauseAudios)
        {
            audio.Play();
        }
        _pauseAudios.Clear(); // Очищаем список после воспроизведения
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
    /// Показать текст в центре экрана
    /// </summary>
    /// <param name="text"> Показываемый текст</param>
    /// <param name="isRewrite"> Перезаписать текст, если в данный момент выводится другой</param>
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
        ReplaceAvail("grab", "f");
        _playerControl.UI.PauseMenu.started -= context => Resume();
        _playerControl.Disable();

        // Удаляем строку загрузки сцены
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Вызываем метод ResetLevel() у GridManager
        if (_gridManager != null)
        {
            _gridManager.ResetLevel();
        }
        else
        {
            Debug.LogError("GridManager не найден!");
        }

        // Сбрасываем время игры
        Time.timeScale = 1;

        // Возобновляем звуки
        PlayPausedAudios();

        // Закрываем экраны завершения игры, если они активны
        finishScreen.SetActive(false);
        wintext.SetActive(false);
        _overText.SetActive(false);

        // Скрываем панель паузы, если она активна
        pauseScreen.SetActive(false);

        // Скрываем курсор и блокируем его
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        wintext.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GameOver()
    {
        ReplaceAvail("grab", "f");
        StopAllSound();
        finishScreen.SetActive(true);
        _overText.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMode.FirstPersonLook.BlockPlayerController();
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
        AudioListener.volume = _sliderVolume.value; // Исправлено: используем _sliderVolume.value
    }

    public void ChangeValueSensetiviti()
    {
        Settings.Instance.SetParam("sensitivity", _sliderSensitiviti.value.ToString());
    }

    public void ChangeVisiblePayer(bool isVisible)
    {
        if (_lastVisible != isVisible)
        {
            if (isVisible)
                _animate.clip = _clips[0];
            else
                _animate.clip = _clips[1];
            _lastVisible = isVisible;
            _animate.Play();
        }
    }
}
