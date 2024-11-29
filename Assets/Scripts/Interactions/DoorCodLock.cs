using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCodLock : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorControl _doorControl;
    [SerializeField] private AudioClip _positivOpen;
    [SerializeField] private AudioClip _negativeOpen;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void ImpossibleOpen() 
    {
        _audioSource.clip = _negativeOpen;
        _audioSource.Play();
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
        else
        {
            _audioSource.clip = _positivOpen;
            _audioSource.Play();
            _doorControl.Interact();
        }
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
            _audioSource.clip = _positivOpen;
            _audioSource.Play();
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


