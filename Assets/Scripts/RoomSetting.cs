using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomSetting: MonoBehaviour
{

    [SerializeField] private List<SRoomSpawnPoint> _roomSpawnPoint;
    private LocationSetting _locationSetting;
    /* [SerializeField] private List<GameObject> _animateObjects;
     // Это из старого
     //[SerializeField] private TriggerEvent _trigger;

     private void Awake()
     {
         SelectActivationObject();
     }  */

    /// <summary>
    /// Создание комнаты и ее настройка
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    private RoomSetting CreateRoom(Transform transform)
    {
        SODataRoom roomSO = null;
        int size = 0;
        // Примим, что если комната подходит, то метод CheckedRoom вернёт занчение -1
        while (size >= 0) 
        {
            roomSO = _locationSetting.GetRandomRoomSO(size);
            if (roomSO != null)
            {
                size = CheckedRoom(roomSO, transform);
            }
            else break;
        }
        if (roomSO == null) return null;
        else 
        {
            GameObject room = Instantiate(roomSO.prefab, transform);
            RoomSetting roomControll = room.GetComponent<RoomSetting>();
            if (roomControll != null)
            {
                roomControll.SetStartPorameters(_locationSetting);
                return roomControll;
            }
            return null;
        }
    }

    /// <summary>
    /// Проверка на возможность создания комнаты, используя CheackBox
    /// </summary>
    /// <param name="room"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    private int CheckedRoom(SODataRoom room,Transform transform)
    {
        GameObject checkedBox = Instantiate(room.checkedBox, transform);
        Collider cheackCollider = checkedBox.GetComponent<Collider>();
        if (_locationSetting.CheackIntersectColliders(cheackCollider))
        {
            Destroy(checkedBox);
            return room.roomSize;
        }
        else
        {
            _locationSetting.AddColliderInList(cheackCollider);
            return -1;
        }
    }

    private void CreateDoor(Transform transform) 
    {
        GameObject prefab = _locationSetting.GetDoorObject();
        Instantiate(prefab, transform);
    }

    /// <summary>
    /// Подпись случайно выбранного скримира на триггер
    /// </summary>
  /*  private void SelectActivationObject() 
    {
        if (_animateObjects.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, _animateObjects.Count);
            // Это из старого
          //  if (_animateObjects[index].TryGetComponent<IActivationObjects>(out IActivationObjects animateObject))            
           //     animateObject.SetActivateTrigger(_trigger);
        }
    }*/

    public void SetStartPorameters(LocationSetting location) 
    {
        _locationSetting = location;        
    }

    /// <summary>
    /// Создание следующей комнаты
    /// </summary>
    /// <returns></returns>
    public void CreationChildrenRooms(int currentIn, int maxIn)
    {
        SRoomSpawnPoint spawnPoint;
        RoomSetting room;
        for (int i = 0; i < _roomSpawnPoint.Count; i++)
        {
            if (currentIn < maxIn && _locationSetting.IsCanCreate())
            {
                spawnPoint = _roomSpawnPoint[i];
                room = CreateRoom(spawnPoint.Transform);
                if (room != null)
                {
                    _locationSetting.AddRoomInList(room);
                    spawnPoint.IsAvail = false;
                    _roomSpawnPoint[i] = spawnPoint;
                    room.CreationChildrenRooms(currentIn + 1, maxIn);
                }
            }
        }
        // Закрываем "дыры"
        for (int i = 0; i < _roomSpawnPoint.Count; i++) 
        {
            if (_roomSpawnPoint[i].IsAvail)
            {
                spawnPoint = _roomSpawnPoint[i];
                CreateDoor(spawnPoint.Transform);
                spawnPoint.IsAvail = false;
                _roomSpawnPoint[i] = spawnPoint;
            }
        }
    }

    

    
}
