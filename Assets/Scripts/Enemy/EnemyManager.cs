using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField, Tooltip(" оличество врагов.")] private int _countEnemy;
    [SerializeField] private GameObject _enemyPref;

    [Header("Ќастройки дл€ обучающего уровн€")]
    [SerializeField] private List<RoomAccessControl> _rooms;
    [SerializeField] private TextAsset _setting;
    [SerializeField] private TextAsset _dictonary;


    private List<EnemyAI> _enemys;

    private Dictionary<RoomAccessControl,List<SEnemyWayPoint>> _waypoints = new Dictionary<RoomAccessControl, List<SEnemyWayPoint>>();
    // Vector2 - двумерный массив, где x - макс. кол-во, y - текущее кол-во
    private Dictionary<RoomAccessControl, Vector2> _roomData = new();


    private void Awake()
    {
        _enemys = new List<EnemyAI>();
        GameMode.EnemyManager = this;
        // ≈сли комнаты заданы вручную, значит спавним вручную
        if (_rooms.Count > 0) 
        {
            EnemyRoute enemyRoute = null;
            foreach (RoomAccessControl room in _rooms)
            {
                if (room.TryGetComponent<EnemyRoute>(out enemyRoute))
                    AddWaypoints(room, enemyRoute.CountMaxEnemyInRoom,enemyRoute.GetWayPoints());
                Debug.Log(enemyRoute);
            }
            CreateEnemy();
        }
    }

    /// <summary>
    /// ѕровекра на возможность спавна нового врага
    /// </summary>
    /// <returns></returns>
    private bool CheackPossibilityPlacement(RoomAccessControl room)
    {
        if (room.HasPower && _roomData[room].x > _roomData[room].y)  return true;
        return false;        
    }


    private RoomAccessControl GetNewRoom(RoomAccessControl currentRoom) 
    {
        foreach (KeyValuePair<RoomAccessControl, List<SEnemyWayPoint>> room in _waypoints)
        {
            // ѕроверка на возможность спавна и на соответсвие выбранной комнаты текущей
            if (currentRoom==room.Key || !CheackPossibilityPlacement(room.Key)) continue;
            // ¬еро€тность того, что враг заспавнитьс€ в этой комнате (только дл€ основного уровн€)
            if (_rooms.Count==0 && UnityEngine.Random.Range(0, 100) < 50)  continue;
            return room.Key;
        }
        return currentRoom;
    }

    /// <summary>
    /// —оздаем ботов и назначаем им маршруты
    /// </summary>
    public void CreateEnemy() 
    {
        int indexPatch;
        RoomAccessControl room;
        Transform spawnPoint;
        for (int i = 0; i < _countEnemy; i++) 
        {
            spawnPoint = GetNewPoint(null, -1, out room, out indexPatch);
            if (spawnPoint == null) continue;
            GameObject newEnmy = Instantiate(_enemyPref, spawnPoint);
            EnemyAI enemyAi = newEnmy.GetComponent<EnemyAI>();
            enemyAi.SetStartParameters(room, indexPatch, this);
            _enemys.Add(enemyAi);            
        }
    }

    /// <summary>
    /// ƒобовление маршрута из комнаты
    /// </summary>
    /// <param name="room"></param>
    /// <param name="maxCount"></param>
    /// <param name="patch"></param>
    public void AddWaypoints(RoomAccessControl room, int maxCount, List<Transform> patch) 
    {
        if (patch==null || patch.Count == 0) return; 
        List<SEnemyWayPoint> sEnemyWayPoints = new List<SEnemyWayPoint>();
        SEnemyWayPoint sEnemyWayPoint = new SEnemyWayPoint();
        // ≈сли максимальное кол-во врагов не задано или больше, чем точек - берем от кол-ва точек (-1 чтобы оствалась вариативность)
        if (maxCount == 0 || maxCount>= patch.Count)  maxCount = patch.Count - 1;
        _roomData.Add(room, new Vector2(maxCount, 0));
        foreach (Transform transform in patch) 
        {
            sEnemyWayPoint.Point = transform;
            sEnemyWayPoint.IsAvail = true;
            sEnemyWayPoints.Add(sEnemyWayPoint);
        }
        _waypoints.Add(room, sEnemyWayPoints);        
    }

    public Transform GetNewPoint(RoomAccessControl room, int indexRout, out RoomAccessControl newRoom, out int newIndex) 
    {
        // ≈сли это запрос от бота
        if (room != null)
        {
            // Ўанс того, что бот захочет помен€ть комнату
            if (UnityEngine.Random.Range(0, 100) < 25) newRoom = GetNewRoom(room);
            else newRoom = room;
        }
        // ≈сли метод вызываетс€ при спавне ботов
        else
        {
            newRoom = GetNewRoom(room);
            if (newRoom == null)
            {
                newIndex = -1;
                return null;
            }
        }
        while (true) 
        {
            newIndex = UnityEngine.Random.Range(0, _waypoints[newRoom].Count);
            if (_waypoints[newRoom][newIndex].IsAvail) break;
        }
        SEnemyWayPoint editWaypoint;
        // —начала освободим занимаемое место, если оно было
        if (room != null)
        {
            editWaypoint = _waypoints[room][indexRout];
            editWaypoint.IsAvail = true;
            _waypoints[room][indexRout] = editWaypoint;
        }
        // ≈сли бот помен€л комнату
        if (room != newRoom)
        {
            if (room!=null) _roomData[room] = new Vector2(_roomData[room].x, _roomData[room].y-1);
            _roomData[newRoom] = new Vector2(_roomData[newRoom].x, _roomData[newRoom].y + 1);
        }
        editWaypoint = _waypoints[newRoom][newIndex];
        editWaypoint.IsAvail = false;
        _waypoints[newRoom][newIndex] = editWaypoint;
        return _waypoints[newRoom][newIndex].Point;
    }

    /// <summary>
    /// Ќапарвл€ем ботов к источнику шума
    /// </summary>
    /// <param name="pointAlaem"></param>
    /// <param name="distance"></param>
    public void AlarmAtDistance(Vector3 pointAlaem, float distance) 
    {
        foreach (EnemyAI enemyAI in _enemys)
        {
            if (Vector3.Distance(enemyAI.transform.position, pointAlaem) <= distance) enemyAI.StartAlerted(pointAlaem);
        }
    }

    public EnemyAI GetEnemyForGameObject(GameObject enemyObgect) 
    {
        foreach (EnemyAI enemy in _enemys) 
        {
            if(enemy.gameObject == enemyObgect) return enemy;
        }
        return null;
    }
}
