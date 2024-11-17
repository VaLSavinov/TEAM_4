using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _batareyPreview;

    public void Interact()
    {
         GameMode.InteractGenerator();
        _batareyPreview.SetActive(false);
    }
}
