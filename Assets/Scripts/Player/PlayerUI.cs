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

    public void ShowText(string text) 
    {
        if (_panel.activeSelf) return;
         _text.SetTag(text);
        _panel.SetActive(true);
    }

    public void DeactivatePanel() 
    {
        _panel.SetActive(false);
    }
}
