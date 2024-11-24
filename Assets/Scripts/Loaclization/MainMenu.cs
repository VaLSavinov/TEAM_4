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
    [SerializeField] private Image topBackground;
    [SerializeField] private Image bottomBackground;
    [SerializeField] private Text flashingText;

    private Color _topImageColor;
    private bool _isFlashing = true;
    
    private void Start()
    {
        Settings.SetCSV(_setting);
        LocalizationManager.SetCSV(_textAsset);
        _topImageColor = topBackground.color;
        StartCoroutine(FlashText());
    }
    
    private void Update()
    {
        if (Input.anyKeyDown && _isFlashing)
        {
            StopAllCoroutines(); // Îñòàíàâëèâàåì ìèãàíèå òåêñòà
            StartCoroutine(FadeOutTopImage());
            flashingText.gameObject.SetActive(false);
            _isFlashing = false; // Âûêëþ÷àåì ÷òîáû êîðóòèíà íå ïîâòîðÿëà àíèìàöèþ
        }
    }

    private IEnumerator FlashText()
    {
        while (_isFlashing)
        {
            flashingText.color = Color.gray; // Äåëàåì öâåò òåêñòà ñåðûì
            yield return new WaitForSeconds(0.6f); // Æäåì 0.6 ñåêóíä
            flashingText.color = Color.white; // Âîçâðàùàåì öâåò òåêñòà ê áåëîìó
            yield return new WaitForSeconds(0.6f);
        }
    }

    private IEnumerator FadeOutTopImage()
    {
        float duration = 2f; // Äëèòåëüíîñòü àíèìàöèè
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration); // Âû÷èñëÿåì íîâóþ ïðîçðà÷íîñòü
            _topImageColor.a = alpha; // Óñòàíàâëèâàåì íîâóþ ïðîçðà÷íîñòü äëÿ âåðõíåãî èçîáðàæåíèÿ
            topBackground.color = _topImageColor; // Ïðèìåíÿåì öâåò ê èçîáðàæåíèþ

            yield return null; // Æäåì ñëåäóþùåãî êàäðà
        }

        // ïðîâåðêà ÷òî ïðîçðà÷íîñòü óñòàíîâëåíà â 0 ïîñëå çàâåðøåíèÿ
        _topImageColor.a = 0f;
        topBackground.color = _topImageColor;
        topBackground.gameObject.SetActive(false); // Âûêëþ÷àåì (íî âðîäå íå îáÿçàòåëüíî)
    }

    // Âåøàåì íà êíîïêó, ïî êîòîðîé ìåíÿåì ÿçûê
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
