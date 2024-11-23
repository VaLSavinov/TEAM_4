using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextAsset textAsset;
    [SerializeField] private Image topBackground;
    [SerializeField] private Image bottomBackground;
    [SerializeField] private Text flashingText;

    private Color _topImageColor;
    private bool _isFlashing = true;

    private void Start()
    {
        LocalizationManager.SetCSV(textAsset);

        _topImageColor = topBackground.color;
        StartCoroutine(FlashText());
    }
    private void Update()
    {
        if (Input.anyKeyDown && _isFlashing)
        {
            StopAllCoroutines(); // Останавливаем мигание текста
            StartCoroutine(FadeOutTopImage());
            flashingText.gameObject.SetActive(false);
            _isFlashing = false; // Выключаем чтобы корутина не повторяла анимацию
        }
    }

    private IEnumerator FlashText()
    {
        while (_isFlashing)
        {
            flashingText.color = Color.gray; // Делаем цвет текста серым
            yield return new WaitForSeconds(0.6f); // Ждем 0.6 секунд
            flashingText.color = Color.white; // Возвращаем цвет текста к белому
            yield return new WaitForSeconds(0.6f);
        }
    }

    private IEnumerator FadeOutTopImage()
    {
        float duration = 2f; // Длительность анимации
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration); // Вычисляем новую прозрачность
            _topImageColor.a = alpha; // Устанавливаем новую прозрачность для верхнего изображения
            topBackground.color = _topImageColor; // Применяем цвет к изображению

            yield return null; // Ждем следующего кадра
        }

        // проверка что прозрачность установлена в 0 после завершения
        _topImageColor.a = 0f;
        topBackground.color = _topImageColor;
        topBackground.gameObject.SetActive(false); // Выключаем (но вроде не обязательно)
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
