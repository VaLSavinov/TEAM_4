using UnityEngine;

public class InputManager : MonoBehaviour
{
    // �������� InputManager, ����������� ����� �������� ������ � ���������� �� ������ ����� � ����.
    public static InputManager Instance { get; private set; }

    // �������� ��� ������� � ��������� ���� ��������.
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    // �������� ��� ������� � ��������� �������� ����.
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }

    // �������� ��� ��������, ���� �� ������ ������ ������ � ������� �����.
    public bool JumpPressed { get; private set; }

    private void Awake()
    {
        // ������������� ��������, ����������, ��� ���������� ������ ���� ��������� InputManager.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ������ ��� �������� ����� �����.
        }
        else
        {
            Destroy(gameObject); // ���������� ��������, ���� �� ����������.
        }
    }

    void Update()
    {
        // �������� �������� ���� �������� � ���� �� ����� ������������.
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        // ���������, ���� �� ������ ������ ������ � ���� �����.
        JumpPressed = Input.GetButtonDown("Jump");
    }
}