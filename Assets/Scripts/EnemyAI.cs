using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAI : MonoBehaviour
{
    /// Для тестирования
    [SerializeField] private PlayerCharacter _character; // Используем прямую ссылку на персонажа игрока (пока назначем в редакторе, за тем - берем из Enemymanager)
    [SerializeField] private Transform[] waypoints; // Массив точек для перемещения
    [SerializeField] private float waitTime = 2f; // Время ожидания на каждой точке
    [SerializeField] private float returnWaitTime = 2f; // Время ожидания перед возвращением к патрулированию
    [SerializeField] private float _alertTime = 2f;
    [SerializeField] private float _searchTime = 5f; // Время поиска
    // Скорость передвижени и преследования
    [SerializeField] private float _speedPatrol;
    [SerializeField] private float _speedChase;
    [SerializeField] private float _speedAlertOrSearching;
    /// Для тестирования
    [SerializeField] private Material materialPatrool;
    [SerializeField] private Material materialAlerted;
    [SerializeField] private Material materialChasing;
    [SerializeField] private Material materialSearching;


    private NavMeshAgent _agent;
    private int _currentWaypointIndex = 0;
   
    private EEnemyState _state = EEnemyState.Patrolling; // Для хранение текущего состояния бота - по умолчанию - патруль
    private bool _isWalk = true;
    private MeshRenderer _meshRenderer;
    private float _countdownTimeSearch; // Время отсчета для поиска игрока


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _meshRenderer = GetComponent<MeshRenderer>();
        StartPatrol();
    }

    private void Update()
    {
        CheckingState();        
    }
    
    private IEnumerator WaitAtWaypoint()
    {
        yield return new WaitForSeconds(waitTime);
        if(_state == EEnemyState.Patrolling) GoToNextWaypoint();
    }

    private IEnumerator WaitAlert()
    {
        yield return new WaitForSeconds(_alertTime);
        if (_state == EEnemyState.Alerted) StartPatrol(); // Возвращаемся к патрулированию
    }   

    /// <summary>
    /// Обход точек
    /// </summary>
    /// 
    private void Patrol()
    {
        if (_agent.remainingDistance < 0.5f && _isWalk)
        {
            _isWalk = false;
            StartCoroutine(WaitAtWaypoint());
        }
    }

    /// <summary>
    /// Движение к источнику шума
    /// </summary>
    private void Alerted()
    {
        if (_agent.remainingDistance < 0.5f && _isWalk)
        {
            _isWalk = false;
            StartCoroutine(WaitAlert());
        }
    }

    /// <summary>
    /// Поиск игрока, после его побега
    /// </summary>
    private void Searching()
    {
        if (Time.time - _countdownTimeSearch >= _searchTime)
        {
            StartPatrol();
        }
        else
        {
            // Здесь возможен какой-нибудь код
        }
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;        
        // Выбор следующей точки (рандомно или последовательно)
        int oldIndexPatch = _currentWaypointIndex;
        while (oldIndexPatch == _currentWaypointIndex)
        {
            _currentWaypointIndex = Random.Range(0, waypoints.Length); // Рандомный выбор точки           
        }
        _isWalk = true;
        _agent.SetDestination(waypoints[_currentWaypointIndex].position);        
    }


    /// <summary>
    /// Действия, в зависимости от состояния бота
    /// </summary>
    private void CheckingState() 
    {
        switch (_state)
        {
            case EEnemyState.Patrolling:
                Patrol();
                break;
            case EEnemyState.Alerted:
                Alerted();
                break;                
            case EEnemyState.Searching:
                Searching();
                break;
            default:
                break;
        }
    }    

    /// <summary>
    /// Переходим в режим патрулирования
    /// </summary>
    private void StartPatrol()
    {        
        _state = EEnemyState.Patrolling;
        _meshRenderer.material = materialPatrool;
        _agent.speed = _speedPatrol;
        GoToNextWaypoint();
    }    

    /// <summary>
    /// Начало преследования игрока
    /// </summary>
    public void ChasePlayer() 
    {
        _state = EEnemyState.Chasing;
        _meshRenderer.material = materialChasing;
        _agent.SetDestination(_character.transform.position);
        _agent.speed = _speedChase;
    }

    /// <summary>
    /// Перследование закончено. Начинается поиск
    /// </summary>
    public void StartSearchingPlayer()
    {
        _state = EEnemyState.Searching;
        _meshRenderer.material = materialSearching;
        _countdownTimeSearch = Time.time;
        _agent.speed = _speedAlertOrSearching;

    }

    public void StartAlerted(Vector3 noiseSours) 
    {
        // Если бот преследует, то не отвлекается
        if (_state == EEnemyState.Chasing) return; 
        _state = EEnemyState.Alerted;
        _meshRenderer. material = materialAlerted;        
        _agent.SetDestination(noiseSours);
        _agent.speed = _speedAlertOrSearching;
        _isWalk = true;

    }

    // ДОРАБОТАТЬ
    public void SetPatch(List<Transform> newPutch)
    {
        waypoints = newPutch.ToArray();
    }

    public void SetPlayer(PlayerCharacter player)
    {
        _character = player;
    }

    public Vector3 GetCharaterCameraPosition()
    {
        return _character.GetCameraPosition();
    }

}