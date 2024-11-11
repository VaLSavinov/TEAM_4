using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAI : MonoBehaviour
{
    /// Для тестирования
    [SerializeField] private PlayerCharacter _character; // Используем прямую ссылку на персонажа игрока (пока назначем в редакторе, за тем - берем из Enemymanager)
    public Transform[] waypoints; // Массив точек для перемещения
    public float waitTime = 2f; // Время ожидания на каждой точке
  //  public float detectionRadius = 10f; // Радиус обнаружения игрока
    public float returnWaitTime = 2f; // Время ожидания перед возвращением к патрулированию
    [SerializeField] private float _searchTime = 5f; // Время поиска
    // Скорость передвижени и преследования
    [SerializeField] private float _speedWalk;
    [SerializeField] private float _speedRun;
    /// Для тестирования
    [SerializeField] private Material materialPatrool;
    [SerializeField] private Material materialAlerted;
    [SerializeField] private Material materialChasing;
    [SerializeField] private Material materialSearching;

    //private Transform _player;
    private NavMeshAgent _agent;
    private int _currentWaypointIndex = 0;
   // private bool _isChasing = false;
    //private Vector3 _lastKnownPosition; // Последняя известная позиция игрока
    private EEnemyState _state = EEnemyState.Patrolling; // Для хранение текущего состояния бота - по умолчанию - патруль
    private bool _isWalk = true;
    private MeshRenderer _meshRenderer;
    private float _countdownTimeSearch; // Время отсчета для поиска игрока
    private Vector3 _lastWayPoint; // Запаминание последней целевой точки


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speedWalk;
        _meshRenderer = GetComponent<MeshRenderer>();
        StartPatrol();
       // _player = GameObject.FindWithTag("Player").transform; // Предполагается, что у игрока установлен тег "Player"
       // GoToNextWaypoint();
    }

    private void Update()
    {
        CheckingState();
        /* if (_isChasing)
         {
             ChasePlayer();
         }
         else
         {
             CheckForPlayer();
             Patrol();
         }*/
    }

    /*
    private void CheckForPlayer()
    {
        if (Vector3.Distance(transform.position, _player.position) < detectionRadius)
        {
            _isChasing = true;
            _lastKnownPosition = _player.position; // Сохраняем последнюю известную позицию игрока
        }
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);

        // Если игрок выходит за пределы радиуса обнаружения
        if (Vector3.Distance(transform.position, _player.position) > detectionRadius)
        {
            ReturnToLastKnownPosition();
        }
    }

    private void ReturnToLastKnownPosition()
    {
        _agent.SetDestination(_lastKnownPosition);

        // Ждем некоторое время на последней известной позиции
        StartCoroutine(WaitAndReturn());
    }
    
    private IEnumerator WaitAndReturn()
    {
        yield return new WaitForSeconds(returnWaitTime);
        Debug.Log("Сработало");
        _state = EEnemyState.Patrolling;
        _meshRenderer.material = materialPatrool;
        GoToNextWaypoint(); // Возвращаемся к патрулированию
    }   
    */
    /// <summary>
    /// Обход точек
    /// </summary>
    private void Patrol()
    {
        if (_agent.remainingDistance < 0.5f && _isWalk)
        {
            _isWalk = false;
            StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        yield return new WaitForSeconds(waitTime);
        GoToNextWaypoint();
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
        // Можно добавить логику для ожидания перед переходом к следующей точке
        // Например, добавив время ожидания после достижения точки
        // StartCoroutine(WaitAtWaypoint());
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
                break;
            case EEnemyState.Searching:
                Searching();
                break;
            default:
                break;
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

    /// <summary>
    /// Переходим в режим патрулирования
    /// </summary>
    private void StartPatrol()
    {        
        _state = EEnemyState.Patrolling;
        _meshRenderer.material = materialPatrool;
        GoToNextWaypoint();
    }

    // ДОРАБОТАТЬ
    public void SetPatch(List<Transform> newPutch)
    {
        waypoints = newPutch.ToArray();
    }

    public Vector3 GetCharaterCameraPosition() 
    {
        return _character.GetCameraPosition();
    }

    /// <summary>
    /// Начало преследования игрока
    /// </summary>
    public void ChasePlayer() 
    {
        _state = EEnemyState.Chasing;
        _meshRenderer.material = materialChasing;
        _agent.SetDestination(_character.transform.position);
    }

    /// <summary>
    /// Перследование закончено. Начинается поиск
    /// </summary>
    public void StartSearchingPlayer()
    {
        _state = EEnemyState.Searching;
        _meshRenderer.material = materialSearching;
        _countdownTimeSearch = Time.time;

    }

    public void StartAlerted() 
    {
        _state = EEnemyState.Alerted;
        _meshRenderer. material = materialAlerted;
        _lastWayPoint = _agent.destination;
        // Останавливаем
        _agent.SetDestination(transform.position);
    }
}