using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour, IInteractable
{
    [SerializeField] private Animation _animate;
    [SerializeField] private RoomAccessControl _roomAccessControl;

    // ������ ����, ��� ��������������� � ������
    private List<GameObject> _interactors = new List<GameObject>();
    private bool _isOpen = false;
    private bool _isLock = false;


    /// <summary>
    /// ����������, ������� �� �������
    /// </summary>
    private void Awake()
    {
        if (_roomAccessControl != null && _roomAccessControl.GetCardColor()!=AccessCardColor.None) _isLock = true;
        else _isLock = false;
    }

    /// <summary>
    /// �������� ������ ��� �����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    { 
        if ((_roomAccessControl == null || _roomAccessControl.HasPower) && other.gameObject.tag == "Enemy")
        { 
            PlayClip("OpenDoor");
            _interactors.Add(other.gameObject);
            _isOpen = true;
        }
       
    }

    /// <summary>
    /// �������� ����� ������� � �������
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (_isOpen && 
           (_roomAccessControl == null || _roomAccessControl.HasPower) && 
           (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player"))
        {
            // ��������� �����, ������ ���� ���, ��� �����, ����� ��� ������
            if (_interactors.Count > 0) _interactors.Remove(other.gameObject);
            if (_interactors.Count == 0) _isOpen = false;
             PlayClip("CloseDoor");
        }
    }

    /// <summary>
    /// ������� ��������
    /// </summary>
    /// <param name="other"></param>
    private void PlayClip(string name)
    {
        if (_interactors.Count > 0) return;
        _animate.clip = _animate.GetClip(name);
        _animate.Play();
    }

    /// <summary>
    ///  �������� ����� ����� ������
    /// </summary>
    public void Interact()
    {
        if (!_isLock) 
        {
            _isOpen = true;
            //_interactors.Add(GameMode.PersonHand.gameObject);
            PlayClip("OpenDoor");
        }
    }

    public bool Interact(ref GameObject interactingOject)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    ///  ��������������� � �������� �����
    /// </summary>
    public void UnLockDoor() 
    {
        _isLock = false;
        Interact();
    }

    public bool IsLockDoor() => _isLock;

    public bool IsHasPower()
    {
        if (_roomAccessControl != null) 
            return _roomAccessControl.HasPower;
        else return true;
    }

    public AccessCardColor GetCardColor() => _roomAccessControl.GetCardColor();
}
