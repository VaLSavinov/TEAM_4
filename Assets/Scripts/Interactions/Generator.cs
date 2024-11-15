using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IInteractable
{
    public void Interact()
    {
         GameMode.InteractGenerator();
    }
}
