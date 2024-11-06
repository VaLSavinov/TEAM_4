using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inicializate : MonoBehaviour
{
    [SerializeField] private TextAsset textAsset;

    private void Start()
    {
        LocalizationManager.SetCSV(textAsset);
    }


    // ������ �� ������, �� ������� ������ ����
    public void ChangeLang() 
    {
        LocalizationManager.Change();
    }
}
