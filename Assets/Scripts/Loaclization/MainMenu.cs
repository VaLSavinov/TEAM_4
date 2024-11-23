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
            StopAllCoroutines(); // ������������� ������� ������
            StartCoroutine(FadeOutTopImage());
            flashingText.gameObject.SetActive(false);
            _isFlashing = false; // ��������� ����� �������� �� ��������� ��������
        }
    }

    private IEnumerator FlashText()
    {
        while (_isFlashing)
        {
            flashingText.color = Color.gray; // ������ ���� ������ �����
            yield return new WaitForSeconds(0.6f); // ���� 0.6 ������
            flashingText.color = Color.white; // ���������� ���� ������ � ������
            yield return new WaitForSeconds(0.6f);
        }
    }

    private IEnumerator FadeOutTopImage()
    {
        float duration = 2f; // ������������ ��������
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration); // ��������� ����� ������������
            _topImageColor.a = alpha; // ������������� ����� ������������ ��� �������� �����������
            topBackground.color = _topImageColor; // ��������� ���� � �����������

            yield return null; // ���� ���������� �����
        }

        // �������� ��� ������������ ����������� � 0 ����� ����������
        _topImageColor.a = 0f;
        topBackground.color = _topImageColor;
        topBackground.gameObject.SetActive(false); // ��������� (�� ����� �� �����������)
    }

    // ������ �� ������, �� ������� ������ ����
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
