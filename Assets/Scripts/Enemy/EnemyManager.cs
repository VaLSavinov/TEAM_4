using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField, Tooltip("���������� ������.")] private int _countEnemy;
    [SerializeField, Tooltip("���������� ������, ���������� �� ���������")] private int _countEnemyNoRoom;
    [SerializeField] private GameObject _enemyPref;

    [Header("��������� ��� ���������� ������")]
    [SerializeField] private List<RoomAccessControl> _rooms;
    [SerializeField] private List<Transform> _wayPointsNoroom;


    private List<EnemyAI> _enemys;
    private bool _isTrainingLevel;
    private int _currnetCountInCoridors=0;

    private Dictionary<RoomAccessControl,List<SEnemyWayPoint>> _waypoints = new Dictionary<RoomAccessControl, List<SEnemyWayPoint>>();
    // Vector2 - ��������� ������, ��� x - ����. ���-��, y - ������� ���-�� �����, z - ��������� �������� 0 ��� 1 - ����� �� ���� �������� �������, ��� 0 - ���
    private Dictionary<RoomAccessControl, Vector3> _roomData = new();
    private List<SEnemyWayPoint> _corridorPoints = new List<SEnemyWayPoint>();


    private void Awake()
    {
        _enemys = new List<EnemyAI>();
        GameMode.EnemyManager = this;
        if (_rooms.Count > 0 || _wayPointsNoroom.Count > 0) _isTrainingLevel = true;
        // ���� ������� ������ �������, ������ ������� �������
        if (_isTrainingLevel)
        {
            if (_rooms.Count > 0)
            {
                EnemyRoute enemyRoute = null;
                float hasExit = 0;
                foreach (RoomAccessControl room in _rooms)
                {
                    if (room.TryGetComponent<EnemyRoute>(out enemyRoute))
                    {
                        if (enemyRoute.HasExit) hasExit=1;
                        else hasExit=0;
                        AddWaypoints(room, enemyRoute.CountMaxEnemyInRoom, enemyRoute.GetWayPoints(), hasExit);
                    }
                }

            }
            if (_wayPointsNoroom.Count > 0)
            {
                Debug.Log("��������� �����");
                SEnemyWayPoint enemyFreePoint = new SEnemyWayPoint();
                foreach (Transform freePoint in _wayPointsNoroom) 
                {                   
                    enemyFreePoint.IsAvail = true;
                    enemyFreePoint.Point = freePoint;
                    _corridorPoints.Add(enemyFreePoint);
                }
                Debug.Log("����� ���������!");
            }
            CreateEnemy();
        }
    }

    /// <summary>
    /// �������� �� ����������� ������ ������ �����
    /// </summary>
    /// <returns></returns>
    private bool CheackPossibilityPlacement(RoomAccessControl room, bool canChange)
    {
        if (room.HasPower && _roomData[room].x > _roomData[room].y && (_roomData[room].z== 1) == canChange)  return true;
        return false;        
    }


    private RoomAccessControl GetNewRoom(RoomAccessControl currentRoom, bool canChange) 
    {
        foreach (KeyValuePair<RoomAccessControl, List<SEnemyWayPoint>> room in _waypoints)
        {
            // �������� �� ����������� ������ � �� ����������� ��������� ������� �������
            if (currentRoom==room.Key || !CheackPossibilityPlacement(room.Key, canChange)) continue;
            // ����������� ����, ��� ���� ������������ � ���� ������� (������ ��� ��������� ������)
            if (!_isTrainingLevel && UnityEngine.Random.Range(0, 100) < 50)  continue;
            return room.Key;
        }
        return currentRoom;
    }

    /// <summary>
    /// ������� ����� � ��������� �� ��������
    /// </summary>
    public void CreateEnemy() 
    {
        // ��������, ��� ������� ������� � ������������ ��������, ����� � ���������, � ����� � ���������
        int indexPatch =-1;
        RoomAccessControl room = null;
        Transform spawnPoint;
        for (int i = 0; i < _countEnemy; i++) 
        {
            indexPatch = -1;
            room = null;
            // ���������� �������, � ������� ����� ���������� ����
            // ������� ������������ �o�����
            spawnPoint = GetNewPoint(ref room, ref indexPatch,true,false);
            // ����� ��������
            if (spawnPoint == null)
            {
                if (_currnetCountInCoridors >= _countEnemyNoRoom) spawnPoint = null;
                else
                {
                    _currnetCountInCoridors++;
                    spawnPoint = GetFreePoint(ref indexPatch);
                }
            }            
            // ����� ������� �������
            if (spawnPoint == null)
                spawnPoint = GetNewPoint(ref room, ref indexPatch, true, true);
            if (spawnPoint == null)  continue;
            GameObject newEnmy = Instantiate(_enemyPref, spawnPoint);
            EnemyAI enemyAi = newEnmy.GetComponent<EnemyAI>();
            enemyAi.SetStartParameters(room, indexPatch, this);
            _enemys.Add(enemyAi);        
        }
    }

    private Transform GetFreePoint(ref int indexPatch)
    {        
        SEnemyWayPoint newPoint;
        int newIndex;       
        while (true)
        {
            newIndex = UnityEngine.Random.Range(0, _corridorPoints.Count);
            if (_corridorPoints[newIndex].IsAvail) break;
        }
        newPoint = _corridorPoints[newIndex];
        newPoint.IsAvail = false;
        _corridorPoints[newIndex] = newPoint;
        if (indexPatch > -1)
        {
            newPoint = _corridorPoints[indexPatch];
            newPoint.IsAvail = true;
            _corridorPoints[indexPatch] = newPoint;
        }
        indexPatch = newIndex;
        return _corridorPoints[newIndex].Point;
    }

    /// <summary>
    /// ���������� �������� �� �������
    /// </summary>
    /// <param name="room"></param>
    /// <param name="maxCount"></param>
    /// <param name="patch"></param>
    public void AddWaypoints(RoomAccessControl room, int maxCount, List<Transform> patch, float HasChange) 
    {
        if (patch==null || patch.Count == 0) return; 
        List<SEnemyWayPoint> sEnemyWayPoints = new List<SEnemyWayPoint>();
        SEnemyWayPoint sEnemyWayPoint = new SEnemyWayPoint();
        // ���� ������������ ���-�� ������ �� ������ ��� ������, ��� ����� - ����� �� ���-�� ����� (-1 ����� ��������� �������������)
        if (maxCount == 0 || maxCount>= patch.Count)  maxCount = patch.Count - 1;
        _roomData.Add(room, new Vector3(maxCount, 0,HasChange));
        foreach (Transform transform in patch) 
        {
            sEnemyWayPoint.Point = transform;
            sEnemyWayPoint.IsAvail = true;
            sEnemyWayPoints.Add(sEnemyWayPoint);
        }
        _waypoints.Add(room, sEnemyWayPoints);        
    }

    public void AddFreePoint(Transform point) 
    {
        SEnemyWayPoint newPoint;
        newPoint.Point = point;
        newPoint.IsAvail = true;
        _corridorPoints.Add(newPoint);
    }


    public Transform GetNewPoint(ref RoomAccessControl room, ref int indexRout, bool isSpawn, bool canChange)
    {
        RoomAccessControl newRoom;
        int newIndex;
        // ���� ��� ������ �� ����
        if (!isSpawn)
        {
            Debug.Log("������ �� ����� ���� " + room);
            if (room != null)
            {
                if (_roomData[room].z==1) canChange = true;
                else canChange = false;
                // ���� ����, ��� ��� ������� �������� ������� � �� ����� �� ��������
                if (canChange && UnityEngine.Random.Range(0, 100) < 50 && canChange) newRoom = GetNewRoom(room, true);
                else newRoom = room;
            }
            else return GetFreePoint(ref indexRout);
        }
        // ���� ����� ���������� ��� ������ �����
        else
        {
            newRoom = GetNewRoom(room, canChange);
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
        // ������� ��������� ���������� �����, ���� ��� ����
        if (room != null)
        {
            editWaypoint = _waypoints[room][indexRout];
            editWaypoint.IsAvail = true;
            _waypoints[room][indexRout] = editWaypoint;
        }
        // ���� ��� ������� �������
        if (room != newRoom)
        {
            if (room!=null) _roomData[room] = new Vector3(_roomData[room].x, _roomData[room].y-1, _roomData[room].z);
            _roomData[newRoom] = new Vector3(_roomData[newRoom].x, _roomData[newRoom].y + 1, _roomData[newRoom].z);
        }
        editWaypoint = _waypoints[newRoom][newIndex];
        editWaypoint.IsAvail = false;
        _waypoints[newRoom][newIndex] = editWaypoint;
        room = newRoom;
        indexRout = newIndex;
        return _waypoints[newRoom][newIndex].Point;
    }

    /// <summary>
    /// ���������� ����� � ��������� ����
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
