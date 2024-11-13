using UnityEngine;
using UnityEngine.UI;

public class FirstPersonLook : MonoBehaviour
{
    // ���������������� ����.
    public float mouseSensitivity = 100f;
    // ������ �� ���� ������ ��� ��������.
    public Transform playerBody;
    // ������� �������� �� ��� X.
    private float xRotation = 0f;
    public bool canRotate = true;
    void Start()
    {
        // ���������� ������ � ������ ������ � ������ ��� ��������� ��� ������.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // �������������� ������� ������� ��������� ���������������� ����
    }

    void Update()
    {
        if (canRotate)
        {
            float mouseX = InputManager.Instance.MouseX * mouseSensitivity * Time.deltaTime;
            float mouseY = InputManager.Instance.MouseY * mouseSensitivity * Time.deltaTime;

            {
                // ������� ���������� ��������� ����� ����
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
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
        }
    }
}