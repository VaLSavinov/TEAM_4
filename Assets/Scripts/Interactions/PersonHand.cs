using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonHand : MonoBehaviour
{
    [SerializeField] private GameObject _uiMessage;
    [SerializeField] private Transform _handPoint;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _pickUpDistance;
    [SerializeField] private float _throwForce;
    [SerializeField] private LayerMask _layerMask;

    private PlayerControl _control;
    private IInteractable _interactObject;
    private GameObject _takeObject;
    private float _throwVerticalForce = 0.2f;

    private void Awake()
    {
        _control = new PlayerControl();
        _control.Player.Take.started += context => Interaction();
        _control.Player.Throw.started += context => ThrowObject();
    }

    private void OnEnable()
    {
        _control.Enable();
    }

    private void OnDisable()
    {
        _control.Disable();
    }

    private void TakeObject(GameObject litleObject) 
    {
        litleObject.GetComponent<Rigidbody>().isKinematic = true;
        litleObject.transform.position = _handPoint.position;
        litleObject.transform.SetParent(_handPoint.transform);
        _takeObject = litleObject;
    }

    private void Interaction() 
    {
        if (_interactObject != null)
        {
            _interactObject.Interact();
            return;
        }
        if (_takeObject != null) DropObject();
        else
        {
            // Можно использовать, вместо триггера и для интерактивных объектов
            RaycastHit hit;
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            if (Physics.Raycast(ray, out hit, _pickUpDistance, _layerMask))
            {
                TakeObject(hit.collider.gameObject);
            }
        }
        

    }

    public void SetInteractObject(IInteractable newInteractObject)
    {
        if (_interactObject == null)
            _interactObject = newInteractObject;
        _uiMessage.SetActive(true);
    }
        
    public void DropObject() 
    {
        if (_takeObject == null) return;
        _takeObject.transform.SetParent(null);
        _takeObject.GetComponent<Rigidbody>().isKinematic = false;
        _takeObject = null;
    }

    public void ThrowObject() 
    { 
        if ( _takeObject == null) return;
        _takeObject.transform.SetParent(null);
        Rigidbody rigidbody = _takeObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.AddForce((_cameraTransform.forward + Vector3.up * _throwVerticalForce) * _throwForce);
        _takeObject = null;
    }

    public void SetNullInteract() 
    {
        _interactObject = null;
        _uiMessage.SetActive(false);
    }


}
