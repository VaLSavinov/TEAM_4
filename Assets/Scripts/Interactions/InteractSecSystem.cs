using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSecSystem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemType _typeInteractObg;

    private bool _isActivate = false;

    private void Impossible() 
    {
        if (_isActivate) GameMode.PlayerUI.ShowText("UI.AlreadyActive", true);
        else GameMode.PlayerUI.ShowText("UI.Request", true);

    }

    public void Interact()
    {
        Impossible();
    }

    public bool Interact(ref GameObject interactingOject)
    {
        PickableItem item = interactingOject.GetComponent<PickableItem>();
        if (item.GetItemType() == _typeInteractObg && !_isActivate)
        {
            if (_typeInteractObg == ItemType.StunGun)
                GameMode.ChangeStateBlackOut(true);
            else
                GameMode.ChangeOpenDoor(true);
            _isActivate = true;
        }
        else Impossible();
        return false;
    }
}


