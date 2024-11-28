using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorControl : MonoBehaviour, IInteractable
{
    [SerializeField] private Animation _animate;
    [SerializeField] private RoomAccessControl _roomAccessControl;
    [SerializeField] private GameObject _door1;
    [SerializeField] private GameObject _door2;
    [SerializeField] private LoaclizationText _roomName1;
    [SerializeField] private LoaclizationText _roomName2;

    // ������ ����, ��� ��������������� � ������
    private List<GameObject> _interactors = new List<GameObject>();
    private bool _isOpen = false;
    private bool _isLock = true;
    private bool _isAlwaysOpen = false;

    private void Awake()
    {
        _roomAccessControl.NoPower += BlockDoors;
        GameMode.Events.OnInteractGenerator += AnBlockDoors;
        GameMode.Events.OnOpenDoor += GloabalOpenDoor;
        if (_roomAccessControl.GetTagNameRoom() != "")
        {
            _roomName1.SetTag(_roomAccessControl.GetTagNameRoom());
            _roomName2.SetTag(_roomAccessControl.GetTagNameRoom());
        }
    }

    private void GloabalOpenDoor(bool obj)
    {
        _isAlwaysOpen = true;
        if (!_isOpen && _roomAccessControl.HasPower)
        { 
            PlayClip("OpenDoor");
            _isOpen = true;
        }
        StartCoroutine(OpenDoor(30f));

    }

    private IEnumerator OpenDoor(float second)
    {
        yield return new WaitForSeconds(second);
        _isAlwaysOpen = false;
        if (_isOpen && _interactors.Count == 0)
        {
            PlayClip("CloseDoor");
            _isOpen = false;
        }
    }

    private void BlockDoors()
    {
        // ������������� ����, ����������� ������
        _door1.layer = 0;
        _door2.layer = 0;
        _roomName1.SetTag("UI.NoPower");
        _roomName2.SetTag("UI.NoPower");
        _roomName1.GetComponent<TextMeshPro>().color = Color.red;
        _roomName2.GetComponent<TextMeshPro>().color = Color.red;
    }

    private void AnBlockDoors()
    {
        _door1.layer = 3;
        _door2.layer = 3;
        _roomName1.SetTag(_roomAccessControl.GetTagNameRoom());
        _roomName2.SetTag(_roomAccessControl.GetTagNameRoom());
        _roomName1.GetComponent<TextMeshPro>().color = Color.white;
        _roomName2.GetComponent<TextMeshPro>().color = Color.white;
    }

    /// <summary>
    /// �������� ������ ��� �����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    { 
        if(_isLock)
            if(_roomAccessControl != null && _roomAccessControl.GetCardColor() == AccessCardColor.None || _roomAccessControl == null)
                _isLock = false;
        if (((_roomAccessControl == null || _roomAccessControl.HasPower)) && other.gameObject.tag == "Enemy" && !_isAlwaysOpen)
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
            if (_interactors.Count == 0 && !_isAlwaysOpen) _isOpen = false;
            if (!_isAlwaysOpen)
                PlayClip("CloseDoor");
        }
    }

    /// <summary>
    /// ������� ��������
    /// </summary>
    /// <param name="other"></param>
    private void PlayClip(string name)
    {
        if (_interactors.Count > 0 && !_isAlwaysOpen) return;
        _animate.clip = _animate.GetClip(name);
        _animate.Play();
    }

  

    /// <summary>
    ///  �������� ����� ����� ������
    /// </summary>
    public void Interact()
    {
        if (!_isLock && !_isOpen) 
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
