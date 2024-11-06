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


    // Вешаем на кнопку, по которой меняем язык
    public void ChangeLang() 
    {
        LocalizationManager.Change();
    }
}
