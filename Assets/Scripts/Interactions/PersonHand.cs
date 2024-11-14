using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonHand : MonoBehaviour
{
    [SerializeField] private GameObject _uiMessage;
    [SerializeField] private Transform _handPoint;

    private PlayerControl _control;
    private IInteractable _interactObject;
    //private bool _isTake = false;

    private void Awake()
    {
        _control = new PlayerControl();
        _control.Player.Take.started += context => Interaction();
    }

    private void OnEnable()
    {
        _control.Enable();
    }

    private void OnDisable()
    {
        _control.Disable();
    }

    public void SetInteractObject(IInteractable newInteractObject) 
    {
        if (_interactObject==null)
            _interactObject = newInteractObject;
        _uiMessage.SetActive(true);
    }

    public void Interaction() 
    {
        if (_interactObject == null) return;

        _interactObject.Interact();

    }   

    public void DropObject() 
    { 
        if(_interactObject==null) return;
        // Для подбираемых объектов - бросаем
        _interactObject.StopInteract();
        _interactObject = null;        

    }

    public void ThrowObject() 
    { 

    }

    public void SetNullInteract() 
    {
        _interactObject = null;
        _uiMessage.SetActive(false);
    }


}
