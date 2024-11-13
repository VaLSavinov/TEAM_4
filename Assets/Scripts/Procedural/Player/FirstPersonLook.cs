using UnityEngine;
using UnityEngine.UI;

public class FirstPersonLook : MonoBehaviour
{
    // „увствительность мыши.
    public float mouseSensitivity = 100f;
    // —сылка на тело игрока дл€ вращени€.
    public Transform playerBody;
    // “екущее вращение по оси X.
    private float xRotation = 0f;
    public bool canRotate = true;
    void Start()
    {
        // «акрепл€ем курсор в центре экрана и делаем его невидимым при старте.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // »нициализируем слайдер текущим значением чувствительности мыши
    }

    void Update()
    {
        if (canRotate)
        {
            float mouseX = InputManager.Instance.MouseX * mouseSensitivity * Time.deltaTime;
            float mouseY = InputManager.Instance.MouseY * mouseSensitivity * Time.deltaTime;

            {
                // ќбычное управление поворотом через мышь
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