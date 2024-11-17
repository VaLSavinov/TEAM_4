using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacmentPoint : MonoBehaviour
{
    [SerializeField] private GameObject _interactObject;
    [SerializeField] private ItemType _itemType;

    private IInteractable _interactable;

    private void Awake()
    {
        _interactable = _interactObject.GetComponent<IInteractable>();
    }

    public bool PlaceObject(GameObject obj) 
    {
        PickableItem pickable;
        if (obj.TryGetComponent<PickableItem>(out pickable) && _itemType == pickable.GetItemType())
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.transform.tag = "Untagged";
            _interactable.Interact();
            return true;
        }        
        return false;
    }
}
