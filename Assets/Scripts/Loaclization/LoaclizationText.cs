using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoaclizationText : MonoBehaviour
{
    private Text _text;
    private string _tag;

    private void Awake()
    {
        _text = GetComponent<Text>();
        _tag = _text.text;
        LocalizationManager.OnChangeLanguage += ChangeLanguage;

    }

    private void OnEnable()
    {
        _text.text = LocalizationManager.GetTextForTag(_tag);
    }

    private void ChangeLanguage() 
    {
        _text.text = LocalizationManager.GetTextForTag(_tag);
    }

}
