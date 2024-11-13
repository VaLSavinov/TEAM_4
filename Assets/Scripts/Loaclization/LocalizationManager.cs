using System;
using UnityEditor.Localization.Editor;
using UnityEngine;

public static class LocalizationManager
{  
    private static char _fieldSeperator = ';';
    private static char _lineSeperater = '\n';
    private static TextAsset _csvAsset;
    private static string[,] _localization;
    private static string[] _languages = {"rus","eng"}; 
    private static int _currentLenguage = 1;

    public static event Action OnChangeLanguage;

    
    public static void SetCSV(TextAsset csvAsset) 
    {
        _csvAsset = csvAsset;
        string[] records = _csvAsset.text.Split(_lineSeperater);
        _localization = new string[records.Length, records[0].Split(_fieldSeperator).Length];
        for (int i = 0; i < records.Length; i++)
        {
            string[] fields = records[i].Split(_fieldSeperator);
            for (int j = 0;j<fields.Length;j++)
            {
                _localization[i,j] = fields[j];
            }
        }
        OnChangeLanguage?.Invoke();
    }

    public static string GetTextForTag(string tag) 
    {
        if (_localization != null)
        {
            //Определяем язык
            // Этот цикл можно убрать, если у нас порядок языков в CSV и _languages совпадает
            int numLeng = 0;
            for (int i = 1; i < _localization.GetLength(1); i++)
            {
                if (_localization[0, i] == _languages[_currentLenguage])
                {
                    numLeng = i;
                    break;
                }
            }
            //Возвращаем значенеи по тегу
            if (numLeng > 0)
            {
                for (int i = 1; i < _localization.GetLength(0); i++)
                {
                    if (_localization[i, 0] == tag) return _localization[i, numLeng];
                }
            }           
        }
        return tag;
    }

    public static void Change() 
    {
        _currentLenguage++;
        if (_currentLenguage== _languages.Length) {_currentLenguage = 0;}        
        OnChangeLanguage?.Invoke();
    }
}

