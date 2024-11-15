using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField, Tooltip("���������� ������.")] private int _countEnemy;
    [SerializeField] private GameObject _enemyPref;
    private List<List<Transform>> _itinerarys; // ������ ���������
    private List<EnemyAI> _enemys;

    private Dictionary<RoomAccessControl,List<SEnemyWayPoint>> _waypoints = new Dictionary<RoomAccessControl, List<SEnemyWayPoint>>();
    // Vector2 - ��������� ������, ��� x - ����. ���-��, y - ������� ���-��
    private Dictionary<RoomAccessControl, Vector2> _roomData = new();


    private void Awake()
    {
        _enemys = new List<EnemyAI>();
    }

    /// <summary>
    /// �������� �� ����������� ������ ������ �����
    /// </summary>
    /// <returns></returns>
    private bool CheackPossibilityPlacement(RoomAccessControl room, List<SEnemyWayPoint> points)
    {
        if (_roomData[room].x >= _roomData[room].y) return false;
        foreach (SEnemyWayPoint point in points)
            if (point.IsAvail) return true;
        return false;
    }


    /// <summary>
    /// ������� ����� � ��������� �� ��������
    /// </summary>
    public void CreateEnemy() 
    {
        int indexPatch;
        List<SEnemyWayPoint> sEnemyWayPoints = new List<SEnemyWayPoint>();
        SEnemyWayPoint sEnemyWayPoint = new SEnemyWayPoint();
        for (int i = 0; i < _countEnemy; i++) 
        {
            foreach (KeyValuePair<RoomAccessControl, List<SEnemyWayPoint>> room in _waypoints)
            {
                // �������� �� ����������� ������
                if (!CheackPossibilityPlacement(room.Key, room.Value)) continue;
                // ����������� ����, ��� ���� ������������ � ���� �������
                if (UnityEngine.Random.Range(0, 100) < 50) continue;
                sEnemyWayPoints = room.Value;
                while (true)
                {
                    indexPatch = UnityEngine.Random.Range(0, sEnemyWayPoints.Count);
                    if (sEnemyWayPoints[indexPatch].IsAvail) break;                    
                }
                GameObject newEnmy = Instantiate(_enemyPref, sEnemyWayPoints[indexPatch].Point);
                EnemyAI enemyAi = newEnmy.GetComponent<EnemyAI>();
                enemyAi.SetStartParameters(room.Key,indexPatch,this);
                sEnemyWayPoint = sEnemyWayPoints[indexPatch];
                sEnemyWayPoint.IsAvail = false;
                sEnemyWayPoints[indexPatch] = sEnemyWayPoint;
                _enemys.Add(enemyAi);
                break;
            }
            /*
            int indexRoom = UnityEngine.Random.Range(0, _itinerarys.Count);
            int indexPatch = UnityEngine.Random.Range(0, _itinerarys[indexRoom].Count);
            GameObject newEnmy = Instantiate(_enemyPref, _itinerarys[indexRoom][indexPatch]);
            EnemyAI enemyAi = newEnmy.GetComponent<EnemyAI>();
            enemyAi.SetPatch(_itinerarys[indexRoom]);
            enemyAi.SetPlayer(_player);
            //���� �������� �����, ������� ������ ����� ������ �� EnemyManager
            _enemys.Add(enemyAi);*/
        }
    }

    /// <summary>
    /// ���������� �������� �� �������
    /// </summary>
    /// <param name="room"></param>
    /// <param name="maxCount"></param>
    /// <param name="patch"></param>
    public void AddWaypoints(RoomAccessControl room, int maxCount, List<Transform> patch) 
    {
        if (patch==null || patch.Count == 0) return; 
        List<SEnemyWayPoint> sEnemyWayPoints = new List<SEnemyWayPoint>();
        SEnemyWayPoint sEnemyWayPoint = new SEnemyWayPoint();
        // ���� ������������ ���-�� ������ �� ������, ����� �� ���-�� ����� (-1 ����� ��������� �������������)
        if (maxCount == 0) maxCount = patch.Count - 1;
        _roomData.Add(room, new Vector2(maxCount, 0));
        foreach (Transform transform in patch) 
        {
            sEnemyWayPoint.Point = transform;
            sEnemyWayPoint.IsAvail = true;
            sEnemyWayPoints.Add(sEnemyWayPoint);
        }
        _waypoints.Add(room, sEnemyWayPoints);        
    }
}
