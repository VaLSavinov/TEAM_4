using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoaclizationText : MonoBehaviour
{
    private Text _text;
    private TMP_Text _textMeshPro;
    private string _tag;

    private void Awake()
    {
        if (!TryGetComponent<Text>(out _text))
            _textMeshPro = GetComponent<TMP_Text>();
        if (_text != null)
            _tag = _text.text;
        else _tag = _textMeshPro.text;
        LocalizationManager.OnChangeLanguage += ChangeLanguage;

    }

    private void OnEnable()
    {
        if (_text!=null) _text.text = LocalizationManager.GetTextForTag(_tag);
        else _textMeshPro.text = LocalizationManager.GetTextForTag(_tag);
    }

    private void ChangeLanguage() 
    {
        if (_text != null)  _text.text = LocalizationManager.GetTextForTag(_tag);
        else if (_textMeshPro != null)  _textMeshPro.text = LocalizationManager.GetTextForTag(_tag);
    }

    public void SetTag(string newTag)
    {
        _tag = newTag;
        if (_text!=null) _text.text = LocalizationManager.GetTextForTag(_tag);
        else if (_textMeshPro!=null) _textMeshPro.text = LocalizationManager.GetTextForTag(_tag);
    }
}
