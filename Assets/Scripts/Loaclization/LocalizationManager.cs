using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public static class LocalizationManager
{  
    private static char _fieldSeperator = ';';
    private static char _lineSeperater = '|';
    private static TextAsset _csvAsset;
    private static string[,] _localization;
    private static string[] _languages = {"rus","eng"}; 
    private static int _currentLenguage = 1;

    public static event Action OnChangeLanguage;

    
    public static void SetCSV(TextAsset csvAsset) 
    {
        _csvAsset = csvAsset;
        ResetCSV();
        SetLanguageSetting();
        OnChangeLanguage?.Invoke();
    }

    public static void ResetCSV() 
    {
        string[] records = _csvAsset.text.Split(_lineSeperater);
        _localization = new string[records.Length, records[0].Split(_fieldSeperator).Length];
        for (int i = 0; i < records.Length - 1; i++)
        {
            string[] fields = records[i].Split(_fieldSeperator);
            if (fields[0] == "") continue;
            for (int j = 0; j < fields.Length; j++)
            {
                if (j == 0) fields[j] = fields[j].Replace("\n", string.Empty);
                _localization[i, j] = fields[j];
            }
        }
    }

    private static void SetLanguageSetting() 
    {
        _currentLenguage =int.Parse(Settings.GetParam("language"));
    }

    public static string GetTextForTag(string tag) 
    {
        Debug.Log("� ����� ��������� ���" + tag);
        if (_localization != null)
        {
            //���������� ����
            // ���� ���� ����� ������, ���� � ��� ������� ������ � CSV � _languages ���������
            int numLeng = 0;
            for (int i = 1; i < _localization.GetLength(1); i++)
            {
                if (_localization[0, i] == _languages[_currentLenguage])
                {
                    numLeng = i;
                    break;
                }
            }
            //���������� �������� �� ����
            if (numLeng > 0)
            {
                for (int i = 1; i < _localization.GetLength(0); i++)
                {
                    if (_localization[i, 0].Contains(tag))
                    {
                        return _localization[i, numLeng].TrimStart('"').TrimEnd('"');
                    }
                }
            }           
        }
        return tag;
    }

    public static void Change() 
    {
        _currentLenguage++;
        if (_currentLenguage== _languages.Length) {_currentLenguage = 0;}
        Settings.SetParam("language", _currentLenguage.ToString());
        OnChangeLanguage?.Invoke();
    }

    /// <summary>
    /// ��������� ���� �����, ������� ������������ (equal=true) ��� �� ������������ (equal=false) avail
    /// </summary>
    /// <param name="avail"> ��� ����������� - f - �� ��������, t - ��������, n - �����</param>
    /// <param name="equal"> ����������� ��������� �������� avail ��� �� ���������</param>
    /// <returns></returns>
    public static List<string> GetTagList(string avail, bool equal)
    {
        List<string> list = new List<string>();
       // if(_localization !=null)
        for (int i = 1; i < _localization.GetLength(0); i++)
        {
            if (_localization[i, 1] != "" && (_localization[i, 1] == avail) == equal) list.Add(_localization[i, 0]);
        }
        return list;
    }


    /// <summary>
    /// �������� �� ����������� ���� (���� �� �����, �� ����� �����������)
    /// </summary>
    /// <param name="tag"> ���</param>
    /// <param name="avail"> ��� ����������� - f - �� ��������, t - ��������, n - �����</param>
    /// <param name="equal"> ����������� ��������� �������� avail ��� �� ���������</param>
    /// <returns></returns>
    public static bool CheackAvail(string tag,string avail, bool equal) 
    {
        for (int i = 1; i < _localization.GetLength(0); i++)
            {
                if (_localization[i, 0].Contains(tag) && (_localization[i, 1]==avail) == equal && _localization[i, 1]!="") return true;   
            }
        return false;
    }

    public static void WriteAvailForTag(string tag, string avail) 
    {
        for (int i = 1; i < _localization.GetLength(0); i++)
        {
            if (_localization[i, 0].Contains(tag)) { _localization[i, 1] = avail; return; }
        }
    }

    public static void SafeCSV() 
    {
        string line = "";
        for (int i = 0; i < _localization.GetLength(0); i++)
        {
            for (int j = 0; j < _localization.GetLength(1); j++)            
            {
                if (j == _localization.GetLength(1) - 1) line = line + _localization[i, j];
                else line = line + _localization[i, j] + _fieldSeperator;
            }
            if (i< _localization.GetLength(0)-1) line = line + _lineSeperater;
        }
        File.WriteAllText(Application.dataPath + "/Resources/Dictonary.csv", line);
        Debug.Log("���� �����������!");
    }
}

