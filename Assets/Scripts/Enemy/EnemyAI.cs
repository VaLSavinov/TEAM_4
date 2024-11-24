using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    [SerializeField, Tooltip("����� �������� �� ������.")] private float _waitTime = 2f;
    [SerializeField, Tooltip("����� ��������� �������.")] private float _alertTime = 2f;
    [SerializeField, Tooltip("����� ������.")] private float _searchTime = 5f;

    [SerializeField, Tooltip("�������� �� ����� �������.")] private float _speedPatrol;
    [SerializeField, Tooltip("�������� �� ����� ������.")] private float _speedChase;
    [SerializeField, Tooltip("�������� �� ����� ������� ��� ������.")] private float _speedAlertOrSearching;
    [SerializeField, Tooltip("�������� ��������.")] private float _speedRotate;

    [SerializeField] private Animator _animator;
    /// ��� ������������
    [SerializeField] private Material materialPatrool;
    [SerializeField] private Material materialAlerted;
    [SerializeField] private Material materialChasing;
    [SerializeField] private Material materialSearching;


    private NavMeshAgent _agent;
    private RoomAccessControl _room;
    private int _currentWaypointIndex = 0;
    private EnemyManager _enemyManager;

    private bool _isRotation = false;
    private Vector3 _targetPoint;
    private float _timeToRotate = 0.3f;

    private float _currentTime;
   
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
        if (_isRotation) Rotate();
        else CheckingState();
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

    private IEnumerator WaitFromAlert()
    {
        yield return new WaitForSeconds(4f);
        GoToPoint();
    }


    /// <summary>
    ///  ������� � ���� � �������� ������� �����
    /// </summary>
    /// <param name="targetPoint"></param>
    /// <returns></returns>
    private void Rotate()
    {
        // ������ ������� ����
        if (Time.time - _currentTime < _timeToRotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_targetPoint - transform.position,Vector3.up), _speedRotate * Time.deltaTime);
        }
        else
        {
            _agent.SetDestination(_targetPoint);
            _animator.SetInteger("State", 1);
            _isRotation = false;
            _isWalk = true;
        }
    }


    /// <summary>
    /// ����� �����
    /// </summary>
    /// 
    private void Patrol()
    {
        if (_agent.remainingDistance < 0.5f && _isWalk)
        {
            _animator.SetInteger("State",0);
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
            _animator.SetInteger("State", 3);
            StartCoroutine(WaitAlert());
        }       
    }

    /// <summary>
    /// ����� ������, ����� ��� ������
    /// </summary>
    private void Searching()
    {
        if (Time.time - _countdownTimeSearch >= _searchTime)
            StartPatrol();        
    }

    private void GoToNextWaypoint()
    {
        Vector3 target = _enemyManager.GetNewPoint(_room, _currentWaypointIndex, out _room, out _currentWaypointIndex).position;
        _isRotation = true;
        _targetPoint = target;
        _currentTime = Time.time;
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
        _animator.SetInteger("State", 1);
        GoToNextWaypoint();
    }    

    /// <summary>
    /// ������ ������������� ������
    /// </summary>
    public void ChasePlayer() 
    {
        _agent.SetDestination(GameMode.PersonHand.transform.position);
        if (_state != EEnemyState.Chasing)
        {
            _agent.speed = _speedChase;
            _animator.SetInteger("State", 2);
            _state = EEnemyState.Chasing;
            _meshRenderer.material = materialChasing;
        }
 
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
        _agent.SetDestination(transform.position);
        _animator.SetInteger("State", 3);

    }

    public void StartAlerted(Vector3 noiseSours) 
    {
        // ���� ��� ����������, �� �� �����������
        if (_state == EEnemyState.Chasing) return;
        _state = EEnemyState.WaitAlert;
        _meshRenderer. material = materialAlerted;
        _agent.speed = _speedAlertOrSearching;
        _agent.SetDestination(transform.position);
        _animator.SetInteger("State", 4);
        _targetPoint = noiseSours;
        transform.LookAt(noiseSours);
        StartCoroutine(WaitFromAlert());
    }  

    public void SetStartParameters(RoomAccessControl room, int waypointIndex, EnemyManager enemyManager) 
    {
        _room = room;
        _currentWaypointIndex = waypointIndex;
        _enemyManager = enemyManager;
        // ��������� ����� ����� �� �����, ����� ������ ����������� ������ ����� ����, ��� ������������ ��� ����
        _animator.SetInteger("State", 0);
        StartCoroutine(WaitAtWaypoint());
    }

    public void GoToPoint()
    {
        _state = EEnemyState.Alerted;
        _agent.SetDestination(_targetPoint);
        _isWalk = true;
    }
}