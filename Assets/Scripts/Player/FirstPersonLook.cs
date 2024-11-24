using System;
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

    void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameMode.FirstPersonLook = this;
        _playerControl = new PlayerControl();
        _playerControl.UI.PauseMenu.started += context => ShowMainMenu();
        // ������ ����� � ����������������� ����
        _mouseSensitivity = _mouseMaxSensitivity;
        //ChangeSettings();
    }

    // ��������� �������, ���� ��� UI
    private void ShowMainMenu()
    {
        GameMode.PlayerUI.Pause();
        // ���������� ����������� ���������
        // LocalizationManager.SafeCSV();
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

        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }*/
    }

    public void ChangeSettings() 
    {
        AudioListener.volume = float.Parse(Settings.GetParam("volume"));
        _mouseSensitivity = _mouseMaxSensitivity * float.Parse(Settings.GetParam("sensitivity"));
        Debug.Log(_mouseSensitivity);
    }
}