using UnityEngine;

public enum OccupancyType
{
    None,
    Room,
    MainPath,
    SecondaryPath,
    TertiaryPath 
}

public class GridCell : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public bool isOccupied = false; // ���� ��������� ������
    public GameObject occupyingRoom; // �������, ������� �������� ������
    public bool isDoorway = false; // ����, ������������, �������� �� ������ ������� �������
    public OccupancyType occupancyType = OccupancyType.None; // ��� ��������� ������
    public bool isConnectedToMainPath = false; // ����, ������������, ��������� �� ������ � �������� �����

    public RoomAccessControl roomAccessControl; // ������ �� ������ RoomAccessControl, ���� ������ ����������� �������

    // ��� ��������, ���������� �������� ������� � ����������
    public string OccupyingRoomName => occupyingRoom != null ? occupyingRoom.name : "None";
}
