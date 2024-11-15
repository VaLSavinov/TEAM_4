using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoute : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints;
    [SerializeField] private int _countMaxEnemyInRoom;

    public List<Transform> GetWayPoints() { return _wayPoints; }

    public int CountMaxEnemyInRoom { get { return _countMaxEnemyInRoom; } }
}
