using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animation _animate;
    [SerializeField] private RoomAccessControl _roomAccessControl;

    // —писок всех, кто взаимодействует с дверью
    private List<GameObject> _interactors = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (_roomAccessControl == null || _roomAccessControl.HasPower)
            if (other.gameObject.tag == "Player" && CheackCard() || other.gameObject.tag == "Enemy")
            {
                
                if (_interactors.Count==0) PlayClip("OpenDoor");
                _interactors.Add(other.gameObject);

            }
            else return;
        else if (other.gameObject.tag == "Player" 
                 &&_roomAccessControl != null 
                 && !_roomAccessControl.HasPower)
        {
            GameMode.PlayerUI.ShowText("UI.NoPower");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_roomAccessControl == null || _roomAccessControl.HasPower)
            if (other.gameObject.tag == "Player" && CheackCard() || other.gameObject.tag == "Enemy")
            {
                // «акрываем дверь, только если все, кто может, через нее прошли
                _interactors.Remove(other.gameObject);
                if (_interactors.Count==0) PlayClip("CloseDoor");
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
