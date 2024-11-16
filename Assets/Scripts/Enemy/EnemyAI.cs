using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAI : MonoBehaviour
{
    [SerializeField, Tooltip("����� ����������� �����.")] private Transform[] _waypoints;
    [SerializeField, Tooltip("����� �������� �� ������.")] private float _waitTime = 2f;
    [SerializeField, Tooltip("����� ��������� �������.")] private float _alertTime = 2f;
    [SerializeField, Tooltip("����� ������.")] private float _searchTime = 5f;

    [SerializeField, Tooltip("�������� �� ����� �������.")] private float _speedPatrol;
    [SerializeField, Tooltip("�������� �� ����� ������.")] private float _speedChase;
    [SerializeField, Tooltip("�������� �� ����� ������� ��� ������.")] private float _speedAlertOrSearching;
    /// ��� ������������
    [SerializeField] private Material materialPatrool;
    [SerializeField] private Material materialAlerted;
    [SerializeField] private Material materialChasing;
    [SerializeField] private Material materialSearching;


    private NavMeshAgent _agent;
    private RoomAccessControl _room;
    private int _currentWaypointIndex = 0;
    private EnemyManager _enemyManager;
   
    private EEnemyState _state = EEnemyState.Patrolling; // ��� �������� �������� ��������� ���� - �� ��������� - �������
    private bool _isWalk = true;
    private MeshRenderer _meshRenderer;
    private float _countdownTimeSearch; // ����� ������� ��� ������ ������


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
        yield return new WaitForSeconds(_waitTime);
        if(_state == EEnemyState.Patrolling) GoToNextWaypoint();
    }

    private IEnumerator WaitAlert()
    {
        yield return new WaitForSeconds(_alertTime);
        if (_state == EEnemyState.Alerted) StartPatrol(); // ������������ � ��������������
    }   

    /// <summary>
    /// ����� �����
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
    /// �������� � ��������� ����
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
    /// ����� ������, ����� ��� ������
    /// </summary>
    private void Searching()
    {
        if (Time.time - _countdownTimeSearch >= _searchTime)
        {
            StartPatrol();
        }
        else
        {
            // ����� �������� �����-������ ���
        }
    }

    private void GoToNextWaypoint()
    {
        _isWalk = true;
        _agent.SetDestination(_enemyManager.GetNewPoint(_room, _currentWaypointIndex,out _room, out _currentWaypointIndex).position);
    }


    /// <summary>
    /// ��������, � ����������� �� ��������� ����
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
    /// ��������� � ����� ��������������
    /// </summary>
    private void StartPatrol()
    {        
        _state = EEnemyState.Patrolling;
        _meshRenderer.material = materialPatrool;
        _agent.speed = _speedPatrol;
        GoToNextWaypoint();
    }    

    /// <summary>
    /// ������ ������������� ������
    /// </summary>
    public void ChasePlayer() 
    {
        _state = EEnemyState.Chasing;
        _meshRenderer.material = materialChasing;
        _agent.SetDestination(GameMode.PersonHand.transform.position);
        _agent.speed = _speedChase;
    }

    /// <summary>
    /// ������������� ���������. ���������� �����
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
        // ���� ��� ����������, �� �� �����������
        if (_state == EEnemyState.Chasing) return; 
        _state = EEnemyState.Alerted;
        _meshRenderer. material = materialAlerted;        
        _agent.SetDestination(noiseSours);
        _agent.speed = _speedAlertOrSearching;
        _isWalk = true;

    }  

    public void SetStartParameters(RoomAccessControl room, int waypointIndex, EnemyManager enemyManager) 
    {
        _room = room;
        _currentWaypointIndex = waypointIndex;
        _enemyManager = enemyManager;
        // ��������� ����� ����� �� �����, ����� ������ ����������� ������ ����� ����, ��� ������������ ��� ����
        StartCoroutine(WaitAtWaypoint());
    }
}