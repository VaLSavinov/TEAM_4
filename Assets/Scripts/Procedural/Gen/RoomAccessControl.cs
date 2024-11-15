using UnityEngine;

public class RoomAccessControl : MonoBehaviour
{
    // ������������ ��� ����� �������
    public enum AccessLevel
    {
        None,
        Red,
        Green,
        Blue
    }

    // ���� ��� ������ ����� �������
    [SerializeField]
    private AccessLevel requiredAccessLevel = AccessLevel.None;

    // ���� ��� ������� �������
    [SerializeField]
    private bool hasPower = true; // �� ��������� ������� ����� �������

    // �������� ��� ������� � �����
    public AccessLevel RequiredAccessLevel
    {
        get => requiredAccessLevel;
        set => requiredAccessLevel = value;
    }

    public bool HasPower
    {
        get => hasPower;
        set => hasPower = value;
    }

    // ����� ��� �������� �������
    public bool CanAccess(AccessLevel playerAccessLevel)
    {
        // �������� ������� ������� � ����� �������
        return hasPower && playerAccessLevel >= requiredAccessLevel;
    }
}
