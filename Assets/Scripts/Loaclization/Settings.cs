using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Localization.Editor;
using UnityEngine;

public static class Settings
{
    private static char _fieldSeperator = ';';
    private static char _lineSeperater = '\n';
    private static TextAsset _csvAsset;
    private static string[,] _settings;

    public static void SetCSV(TextAsset csvAsset)
    {
        _csvAsset = csvAsset;
        string[] records = _csvAsset.text.Split(_lineSeperater);
        _settings = new string[records.Length, records[0].Split(_fieldSeperator).Length];
        for (int i = 0; i < records.Length - 1; i++)
        {
            string[] fields = records[i].Split(_fieldSeperator);
            if (fields[0] == "") continue;
            for (int j = 0; j < fields.Length; j++)
            {                
                _settings[i, j] = fields[j];
            }
        }        
    }

    public static string GetParam(string tag) 
    {
        for(int i = 0;i< _settings.GetLength(0);i++) 
        {            
            if (_settings[i, 0] == tag)
            {
                return _settings[i, 1];
            }
        }
        return null;
    }

    public static void SetParam(string tag, string value)
    {
        for (int i = 0; i < _settings.GetLength(0); i++)
        {
            if (_settings[i, 0] == tag)
            {
                _settings[i, 1] = value;
            }
        }
    }

    public static void SafeCSV()
    {
        string line = "";
        for (int i = 0; i < _settings.GetLength(0); i++)
        {
            for (int j = 0; j < _settings.GetLength(1); j++)
            {
                if (j == 1) line = line + _settings[i, j];
                else line = line + _settings[i, j] + _fieldSeperator;
            }
            if(i<_settings.GetLength(0)-1) line = line + _lineSeperater;
        }
        File.WriteAllText(Application.dataPath + "/Resources/Settings.csv", line);
        Debug.Log("Настройки сохранены!");

    }
}
