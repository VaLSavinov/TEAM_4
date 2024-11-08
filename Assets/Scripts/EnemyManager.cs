using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int _countEnemy;
    [SerializeField] private GameObject _enemyPref;
    private List<List<Transform>> _itinerarys;
    private List<EnemyMove> _enemys;

    private void Awake()
    {
        _itinerarys = new List<List<Transform>>();
        _enemys = new List<EnemyMove>();
    }

    public void CreateEnemy() 
    {
        for (int i = 0; i < _countEnemy; i++) 
        {
            int indexRoom = UnityEngine.Random.Range(0, _itinerarys.Count);
            int indexPatch = UnityEngine.Random.Range(0, _itinerarys[indexRoom].Count);
            GameObject newEnmy = Instantiate(_enemyPref, _itinerarys[indexRoom][indexPatch]);
            EnemyMove enemyMove = newEnmy.GetComponent<EnemyMove>();
            enemyMove.SetPatch(_itinerarys[indexRoom]);
            _enemys.Add(enemyMove);
        }
    }

    public void AddItinerary(List<Transform> patch) 
    {
        if (patch != null) _itinerarys.Add(patch);
    }
}
