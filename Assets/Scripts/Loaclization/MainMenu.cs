using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextAsset textAsset;

    private void Start()
    {
        LocalizationManager.SetCSV(textAsset);
    }

    // Вешаем на кнопку, по которой меняем язык
    public void ChangeLang() 
    {
        LocalizationManager.Change();
    }

    public void LoadScene(int sceneIndex)
    {
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}
