using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animation _animate;
    [SerializeField] private RoomAccessControl _roomAccessControl;

    private void OnTriggerEnter(Collider other)
    {
        if (_roomAccessControl != null) Debug.Log(_roomAccessControl.HasPower.ToString() + ' ' + _roomAccessControl.GetCardColor());
        if (_roomAccessControl == null || _roomAccessControl.HasPower)
            if (other.gameObject.tag == "Player" && CheackCard() || other.gameObject.tag == "Enemy")
            {
                PlayClip("OpenDoor");
            }
            else return;
        else if (_roomAccessControl != null &&!_roomAccessControl.HasPower)
        {
            GameMode.PlayerUI.ShowText("UI.NoPower");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_roomAccessControl == null || _roomAccessControl.HasPower)
            if (other.gameObject.tag == "Player" && CheackCard() || other.gameObject.tag == "Enemy")
            {
                PlayClip("CloseDoor");
            }
        GameMode.PlayerUI.DeactivatePanel();
    }

    private bool CheackCard()
    {
        if (_roomAccessControl == null) return true;
        if (GameMode.PersonHand.HasCard(_roomAccessControl.GetCardColor()))
            return true;
        else GameMode.PlayerUI.ShowText("UI." + _roomAccessControl.GetCardColor().ToString());
        return false;
    }

    private void PlayClip(string name)
    {
        _animate.clip = _animate.GetClip(name);
        _animate.Play();
    }
}
