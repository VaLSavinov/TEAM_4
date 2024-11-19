using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacmentPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _interactObject;
    [SerializeField] private ItemType _itemType;

    private IInteractable _interactable;

    private bool _isInteract = false;

    private void Awake()
    {
        _interactable = _interactObject.GetComponent<IInteractable>();
    }

    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(15);
        GameMode.PlayerUI.DeactivatePanel();
    }
    
    public void Interact()
    {
        if (_isInteract) { return; }
        else 
        {
            GameMode.PlayerUI.DeactivatePanel();
            GameMode.PlayerUI.ShowText("UI.Request",true);
            StartCoroutine(ShowText());
        }
    }

    public bool Interact(ref GameObject interactingOject)
    {
        PickableItem pickable;
        if (interactingOject.TryGetComponent<PickableItem>(out pickable) && _itemType == pickable.GetItemType())
        {
            interactingOject.transform.position = transform.position;
            interactingOject.transform.rotation = transform.rotation;
            interactingOject.transform.SetParent(transform);
            interactingOject.transform.tag = "Untagged";
            _interactable.Interact();
            interactingOject = null;
            _isInteract = true;
            return true;
        }
        else
        {
            GameMode.PlayerUI.DeactivatePanel();
            GameMode.PlayerUI.ShowText("UI.Request", true);
            StartCoroutine(ShowText());
        }
        return false;
    }
}
