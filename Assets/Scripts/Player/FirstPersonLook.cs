using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonLook : MonoBehaviour
{
    // ���������������� ����.
    [SerializeField] private float _mouseMaxSensitivity = 100f;
    // ������ �� ���� ������ ��� ��������.
    [SerializeField] private Transform _playerBody;
    [SerializeField] private bool _canRotate = true;
    // ������� �������� �� ��� X.
    private float _xRotation = 0f;

    private PlayerControl _playerControl;
    private float _mouseSensitivity;
    private bool _isVisibile = false;
    private List<Light> _lights;

    void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameMode.FirstPersonLook = this;
        _playerControl = new PlayerControl();
        _playerControl.UI.PauseMenu.started += context => ShowMainMenu();
        // ������ ����� � ����������������� ����
        _mouseSensitivity = _mouseMaxSensitivity;
        _lights = new List<Light>();
        //ChangeSettings();
    }

    // ��������� �������, ���� ��� UI
    private void ShowMainMenu()
    {
        GameMode.PlayerUI.Pause();
        // ���������� ����������� ���������
        // LocalizationManager.SafeCSV();
    }

    public void BlockPlayerController()
    {
        _playerControl.Disable();
    }

    private void OnEnable()
    {
        _playerControl.Enable();
    }

    private void OnDisable()
    {
        _playerControl.Disable();
    }


    void Update()
    {
        if (_canRotate)
        {
            float mouseX = InputManager.Instance.MouseX * _mouseSensitivity * Time.deltaTime;
            float mouseY = InputManager.Instance.MouseY * _mouseSensitivity * Time.deltaTime;
            {
                // ������� ���������� ��������� ����� ����
                _xRotation -= mouseY;
                _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
                _playerBody.Rotate(Vector3.up * mouseX);
            }
        }
    }

    public void ChangeSettings() 
    {
        AudioListener.volume = float.Parse(Settings.GetParam("volume"));
        _mouseSensitivity = _mouseMaxSensitivity * float.Parse(Settings.GetParam("sensitivity"));
        Debug.Log(_mouseSensitivity);
    }

    public void AddLight(Light newLight) 
    {
        if (_lights.Count == 0)
        {
            _lights.Add(newLight);
            _isVisibile = true;
            GameMode.PlayerUI.ChangeVisiblePayer(_isVisibile);
        }
        else
        {
            foreach (var light in _lights)
                if (light == newLight) return;
            _lights.Add(newLight);
        }
    }

    public void RemoveLight(Light newLight)
    {
        if (_lights.Count == 0) 
        { 
            GameMode.PlayerUI.ChangeVisiblePayer(_isVisibile); 
            return; 
        }
        _lights.Remove(newLight);
        if (_lights.Count == 0)
        {
            _isVisibile = false;
            GameMode.PlayerUI.ChangeVisiblePayer(_isVisibile);
        }
    }

    public bool GetVisability() 
    {
        return _isVisibile;
    }
}