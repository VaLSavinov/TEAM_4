using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    
    private IInteractable _intractableObject;

    private void Awake() 
    {
        _intractableObject = _object.GetComponent<IInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.GetComponent<PersonHand>().SetInteractObject(_intractableObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.GetComponent<PersonHand>().SetNullInteract();
    }
}
