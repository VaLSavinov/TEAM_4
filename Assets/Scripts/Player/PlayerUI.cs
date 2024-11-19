using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private LoaclizationText _text;

    private void Awake()
    {
        GameMode.PlayerUI = this;
    }

    /// <summary>
    /// Показать текст в центре экрана
    /// </summary>
    /// <param name="text"> Показываемый текст</param>
    /// <param name="isRewrite"> Перезаписть текст, если в данный момент выводиться другой</param>
    public void ShowText(string text, bool isRewrite) 
    {
        if (!isRewrite && _panel.activeSelf) return;
        _text.SetTag(text);
        _panel.SetActive(true);
    }

    public void DeactivatePanel() 
    {
        _panel.SetActive(false);
    }
}
