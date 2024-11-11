using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float _speedWalk;
    [SerializeField] private float _speedRun;
    [SerializeField] private Vector2 _rangeTimeOut;
    [SerializeField] private List<Transform> _path;

    private bool _isPursuit = false;
    private bool _isWait = false;
    private NavMeshAgent _agentAI;
    private int _indexPointPath = -1;
    private float _currentTime;
    private float _timeOut;


    private void Awake()
    {
        _agentAI = GetComponent<NavMeshAgent>();
        SetNewTarget();
        _agentAI.speed = _speedWalk;
        _path = new List<Transform>();
    }

    private void Update()
    {              
        if (_isPursuit) { }
        else
        {
            Debug.Log("Работает");
            if (_isWait && Time.time - _currentTime >= _timeOut)
            {
                SetNewTarget();
                _isWait = false;
            }
            if (!_agentAI.hasPath && !_isWait) 
            {
                _isWait = true;
                _currentTime = Time.time;
            }
        }
    }

    private void SetNewTarget()
    {        
        if (_path.Count>0)
        {
            _indexPointPath++;
            if (_indexPointPath == _path.Count) _indexPointPath = 0;
            _agentAI.destination = _path[_indexPointPath].position;
            _timeOut = UnityEngine.Random.Range(_rangeTimeOut.x, _rangeTimeOut.y);
        }
    }

    public void StartPursuit(PlayerCharacter player) 
    {
        if (!_isPursuit)
        {
            _agentAI.destination = player.transform.position;
            _isPursuit = true;
            _agentAI.speed = _speedRun;
        }
        else
        {
            _agentAI.destination = player.transform.position;
        }
       
    }

    public void SetPatch(List<Transform> newPutch) 
    {
        _path = newPutch;
    }
}
