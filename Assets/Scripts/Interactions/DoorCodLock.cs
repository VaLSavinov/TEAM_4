using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCodLock : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorControl _doorControl;

    private void ImpossibleOpen() 
    {
        GameMode.PlayerUI.ShowText("UI." + _doorControl.GetCardColor().ToString(),true);
    }

    public void Interact()
    {
        if(!_doorControl.IsHasPower()) 
        {
            GameMode.PlayerUI.ShowText("UI.NoPower",true);
            return; 
        }
        if (_doorControl.IsLockDoor()) ImpossibleOpen(); 
        else _doorControl.Interact();
    }

    public bool Interact(ref GameObject interactingOject)
    {
        if (!_doorControl.IsHasPower())
        {
            GameMode.PlayerUI.ShowText("UI.NoPower", true);
            return false;
        }
        if (!_doorControl.IsLockDoor())
        {
            _doorControl.Interact();
            return true;
        }
        else
        {           
            if (interactingOject.GetComponent<PickableItem>().GetCardColor() == _doorControl.GetCardColor())
                _doorControl.UnLockDoor();
            else ImpossibleOpen();
        }
        return false;
    }
}


