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
    public bool isOccupied = false; // ‘лаг зан€тости клетки
    public GameObject occupyingRoom; //  омната, котора€ занимает клетку
    public bool isDoorway = false; // ‘лаг, показывающий, €вл€етс€ ли клетка дверным проемом
    public OccupancyType occupancyType = OccupancyType.None; // “ип зан€тости клетки
    public bool isConnectedToMainPath = false; // ‘лаг, показывающий, соединена ли клетка с основным путем

    // ƒл€ удобства, отображаем название комнаты в инспекторе
    public string OccupyingRoomName => occupyingRoom != null ? occupyingRoom.name : "None";
}
