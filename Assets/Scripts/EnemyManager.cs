using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int _countEnemy;
    [SerializeField] private GameObject _enemyPref; // Префаб с ботами
    private List<List<Transform>> _itinerarys; // Список маршрутов
    private List<EnemyAI> _enemys;
    private PlayerCharacter _player;  // Ссылка на монстра игрока


    private void Awake()
    {
        _itinerarys = new List<List<Transform>>();
        _enemys = new List<EnemyAI>();
        // Находим главного героя через тег
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
    }


    /// <summary>
    /// Создаем ботов и назначаем им маршруты
    /// </summary>
    public void CreateEnemy() 
    {
        for (int i = 0; i < _countEnemy; i++) 
        {
            int indexRoom = UnityEngine.Random.Range(0, _itinerarys.Count);
            int indexPatch = UnityEngine.Random.Range(0, _itinerarys[indexRoom].Count);
            GameObject newEnmy = Instantiate(_enemyPref, _itinerarys[indexRoom][indexPatch]);
            EnemyAI enemyAi = newEnmy.GetComponent<EnemyAI>();
            enemyAi.SetPatch(_itinerarys[indexRoom]);
            enemyAi.SetPlayer(_player);
            //Надо добавить метод, который вернет ботам ссылку на EnemyManager
            _enemys.Add(enemyAi);
        }
    }

    public void AddItinerary(List<Transform> patch) 
    {
        if (patch.Count>0) _itinerarys.Add(patch);
    }

    /// <summary>
    /// Будем возвращать ссылку на героя по запросу от ботов
    /// </summary>
    /// <returns></returns>
    public PlayerCharacter GetPlayerCharacter() { return _player; }
}
