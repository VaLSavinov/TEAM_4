using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb : MonoBehaviour, IInteractable
{
    private Light _light;

    private void Start()
    {
        _light = GetComponent<Light>();
    }

    public void Interact()
    {
        if (_light != null)
        {
            _light.enabled = !_light.enabled;
            Debug.Log("Лампочка " + (_light.enabled ? "включена" : "выключена"));
        }
    }
}
