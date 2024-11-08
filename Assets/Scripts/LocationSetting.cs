using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LocationSetting : MonoBehaviour
{
    [SerializeField] private RoomSetting _startRoom;
    [SerializeField] private int _maxCountRoom;
    [SerializeField] private int _maxInRooms;
    [SerializeField] private List<SODataRoom> _roomsSO;
    [SerializeField] private GameObject _doorPref;

    private List<RoomSetting> _rooms = new List<RoomSetting>();
    // Список колайдеров для исключения пересечений комнат
    private List<Collider> _roomColliders = new List<Collider>();

    private NavMeshSurface _navMeshSurface;

    private void Start()
    {    
        _startRoom.SetStartPorameters(this);
        AddColliderInList(_startRoom.GetComponent<Collider>());
        _startRoom.CreationChildrenRooms(1, _maxInRooms);
        _navMeshSurface = GetComponent<NavMeshSurface>();
        _navMeshSurface.BuildNavMesh();

    }

    private SODataRoom SelectRoomBySize(int size)
    {
        List<int> indexRoomSO = new List<int>();
        for (int i =0; i < _roomsSO.Count; i++) 
        {
            if (_roomsSO[i].roomSize<size) indexRoomSO.Add(i);
        }
        if (indexRoomSO.Count > 0)
        {
            int i = indexRoomSO[UnityEngine.Random.Range(0, indexRoomSO.Count)];
            return _roomsSO[i];
        }
        else  return null;
    }

    public void AddRoomInList(RoomSetting room) 
    {
        _rooms.Add(room);
    }

    public void AddColliderInList(Collider roomColider)
    {
        _roomColliders.Add(roomColider);
    }

    public bool CheackIntersectColliders(Collider roomCollider)
    {
        foreach (Collider collider in _roomColliders)
            if(roomCollider.bounds.Intersects(collider.bounds)) return true;
        return false;
    }

    public bool IsCanCreate() 
    {
        if (_rooms.Count < _maxCountRoom) return true;
        return false;
    }  

    public SODataRoom GetRandomRoomSO(int size)
    {
        // Если размер - 0, то считаем, что размер комнаты не важен
        if (size == 0)
        {
            // Исключаем создание единственной комнаты без выходов
            if (_rooms.Count == 0)
            {
                SODataRoom roomSO;
                while (true)
                {
                    roomSO = _roomsSO[Random.Range(0, _roomsSO.Count)];
                    if (!roomSO.isEndRoom) break;
                }
                return roomSO;
            }
            else return _roomsSO[Random.Range(0, _roomsSO.Count)];
            
        } 
        else return SelectRoomBySize(size);
    }
    

    public GameObject GetDoorObject() => _doorPref;

      

}
