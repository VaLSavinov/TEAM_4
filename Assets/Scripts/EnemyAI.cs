using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAI : MonoBehaviour
{
    /// ��� ������������
    [SerializeField] private PlayerCharacter _character; // ���������� ������ ������ �� ��������� ������ (���� �������� � ���������, �� ��� - ����� �� Enemymanager)
    public Transform[] waypoints; // ������ ����� ��� �����������
    public float waitTime = 2f; // ����� �������� �� ������ �����
  //  public float detectionRadius = 10f; // ������ ����������� ������
    public float returnWaitTime = 2f; // ����� �������� ����� ������������ � ��������������
    [SerializeField] private float _searchTime = 5f; // ����� ������
    // �������� ����������� � �������������
    [SerializeField] private float _speedWalk;
    [SerializeField] private float _speedRun;
    /// ��� ������������
    [SerializeField] private Material materialPatrool;
    [SerializeField] private Material materialAlerted;
    [SerializeField] private Material materialChasing;
    [SerializeField] private Material materialSearching;

    //private Transform _player;
    private NavMeshAgent _agent;
    private int _currentWaypointIndex = 0;
   // private bool _isChasing = false;
    //private Vector3 _lastKnownPosition; // ��������� ��������� ������� ������
    private EEnemyState _state = EEnemyState.Patrolling; // ��� �������� �������� ��������� ���� - �� ��������� - �������
    private bool _isWalk = true;
    private MeshRenderer _meshRenderer;
    private float _countdownTimeSearch; // ����� ������� ��� ������ ������
    private Vector3 _lastWayPoint; // ����������� ��������� ������� �����


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speedWalk;
        _meshRenderer = GetComponent<MeshRenderer>();
        StartPatrol();
       // _player = GameObject.FindWithTag("Player").transform; // ��������������, ��� � ������ ���������� ��� "Player"
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
            _lastKnownPosition = _player.position; // ��������� ��������� ��������� ������� ������
        }
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);

        // ���� ����� ������� �� ������� ������� �����������
        if (Vector3.Distance(transform.position, _player.position) > detectionRadius)
        {
            ReturnToLastKnownPosition();
        }
    }

    private void ReturnToLastKnownPosition()
    {
        _agent.SetDestination(_lastKnownPosition);

        // ���� ��������� ����� �� ��������� ��������� �������
        StartCoroutine(WaitAndReturn());
    }
    
    private IEnumerator WaitAndReturn()
    {
        yield return new WaitForSeconds(returnWaitTime);
        Debug.Log("���������");
        _state = EEnemyState.Patrolling;
        _meshRenderer.material = materialPatrool;
        GoToNextWaypoint(); // ������������ � ��������������
    }   
    */
    /// <summary>
    /// ����� �����
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
        // ����� ��������� ����� (�������� ��� ���������������)
        int oldIndexPatch = _currentWaypointIndex;
        while (oldIndexPatch == _currentWaypointIndex)
        {
            _currentWaypointIndex = Random.Range(0, waypoints.Length); // ��������� ����� �����           
        }
        _isWalk = true;
        _agent.SetDestination(waypoints[_currentWaypointIndex].position);
        // ����� �������� ������ ��� �������� ����� ��������� � ��������� �����
        // ��������, ������� ����� �������� ����� ���������� �����
        // StartCoroutine(WaitAtWaypoint());
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
                break;
            case EEnemyState.Searching:
                Searching();
                break;
            default:
                break;
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

    /// <summary>
    /// ��������� � ����� ��������������
    /// </summary>
    private void StartPatrol()
    {        
        _state = EEnemyState.Patrolling;
        _meshRenderer.material = materialPatrool;
        GoToNextWaypoint();
    }

    // ����������
    public void SetPatch(List<Transform> newPutch)
    {
        waypoints = newPutch.ToArray();
    }

    public Vector3 GetCharaterCameraPosition() 
    {
        return _character.GetCameraPosition();
    }

    /// <summary>
    /// ������ ������������� ������
    /// </summary>
    public void ChasePlayer() 
    {
        _state = EEnemyState.Chasing;
        _meshRenderer.material = materialChasing;
        _agent.SetDestination(_character.transform.position);
    }

    /// <summary>
    /// ������������� ���������. ���������� �����
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
        // �������������
        _agent.SetDestination(transform.position);
    }
}