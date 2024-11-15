using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonHand : MonoBehaviour
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _pickUpDistance;
    [SerializeField] private float _throwForce;
    [SerializeField] private LayerMask _layerMask;

    private PlayerControl _control;
    private GameObject _hitObject;
    private Rigidbody _grabObject;
    private float _throwVerticalForce = 0.2f;
    private Dictionary<AccessCardColor, bool> _inventaryCard = new Dictionary<AccessCardColor, bool>();
    private List<AccessCardColor> _r;

    private void Awake()
    {
        _control = new PlayerControl();
        _control.Player.Take.started += context => Interaction();
        _control.Player.Throw.started += context => ThrowObject();
        _inventaryCard.Add(AccessCardColor.None,true);
        GameMode.PersonHand = this;
    }

    private void Update()
    {
        CheackInteract();
    }

    private void OnEnable()
    {
        _control.Enable();
    }

    private void OnDisable()
    {
        _control.Disable();
    }

    private void GrabObject() 
    {
        _grabObject = _hitObject.GetComponent<Rigidbody>();
        _grabObject.isKinematic = true;
        _grabObject.transform.position = _handPoint.position;
        _grabObject.transform.SetParent(_handPoint.transform);
    }

    /// <summary>
    /// Выпуск лучей и поиск интерактивных объектов
    /// </summary>
    private void CheackInteract() 
    {
        RaycastHit hit;
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        if (Physics.Raycast(ray, out hit, _pickUpDistance, _layerMask))
        {
            SetInteractObject(hit.transform.gameObject);
        }
        else 
        {
            SetNullInteract();
        }
    }

    /// <summary>
    /// Нажатие на кнопку взаимодействия
    /// </summary>
    private void Interaction() 
    {
        if (_hitObject != null)
        {
            switch (_hitObject.tag) 
            {
                case "Grab":
                case "Placed":
                    // Если есть другой объект в руках - бросаем его
                    if (_grabObject != null) DropObject();
                    GrabObject();
                    break;
                case "Interact":
                    _hitObject.transform.GetComponent<IInteractable>().Interact();
                    break;
                case "Take":
                    AddCard(_hitObject);
                    break;
                case "Place":
                    if (_grabObject != null)
                        TransferObject();
                    break;
                default:
                    break;
            }            
            return;
        }
        if (_grabObject != null) DropObject();
    }

    private void SetInteractObject(GameObject newInteractObject)
    {
        if (_hitObject == newInteractObject) return;
        _hitObject = newInteractObject;
        GameMode.PlayerUI.ShowText("UI.Interact");
    }
        
    private void DropObject() 
    {        
        if (_grabObject == null) return;
        _grabObject.transform.SetParent(null);
        _grabObject.isKinematic = false;
        _grabObject = null;
    }

    private void ThrowObject() 
    { 
        if (_grabObject == null) return;
        _grabObject.transform.SetParent(null);
        _grabObject.isKinematic = false;
        _grabObject.AddForce((_cameraTransform.forward + Vector3.up * _throwVerticalForce) * _throwForce);
        _grabObject = null;
    }

    private void SetNullInteract() 
    {
        if (_hitObject == null) return ;
        _hitObject = null;
        GameMode.PlayerUI.DeactivatePanel();
    }

    private void AddCard(GameObject card)
    {
        PickableItem cardPick = card.GetComponent<PickableItem>();
        _inventaryCard.Add(cardPick.GetCardColor(), true);        
        cardPick.Interact();
        GameMode.PlayerUI.DeactivatePanel();
    }

    private void TransferObject()
    {
        if (_hitObject.transform.GetComponent<PlacmentPoint>().PlaceObject(_grabObject.gameObject)) 
        {
            _grabObject.transform.SetParent(null);
            _grabObject.isKinematic = true;
            _grabObject = null;
        }
    }

    public bool HasCard(AccessCardColor card) 
    {
        return _inventaryCard.ContainsKey(card);
    }
}
