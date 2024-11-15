using UnityEngine;
using UnityEngine.UI;

public class FirstPersonLook : MonoBehaviour
{
    // Чувствительность мыши.
    [SerializeField] private float _mouseSensitivity = 100f;
    // Ссылка на тело игрока для вращения.
    [SerializeField] private Transform _playerBody;
    [SerializeField] private bool _canRotate = true;
    // Текущее вращение по оси X.
    private float _xRotation = 0f;

    void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameMode.FirstPersonLook = this;
    }

    void Update()
    {
        if (_canRotate)
        {
            float mouseX = InputManager.Instance.MouseX * _mouseSensitivity * Time.deltaTime;
            float mouseY = InputManager.Instance.MouseY * _mouseSensitivity * Time.deltaTime;
            {
                // Обычное управление поворотом через мышь
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
}