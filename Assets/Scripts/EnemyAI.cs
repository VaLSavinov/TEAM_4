using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints; // ������ ����� ��� �����������
    public float waitTime = 2f; // ����� �������� �� ������ �����
    public float detectionRadius = 10f; // ������ ����������� ������
    public float returnWaitTime = 2f; // ����� �������� ����� ������������ � ��������������

    private NavMeshAgent _agent;
    private Transform _player;
    private int _currentWaypointIndex = 0;
    private bool _isChasing = false;
    private Vector3 _lastKnownPosition; // ��������� ��������� ������� ������

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform; // ��������������, ��� � ������ ���������� ��� "Player"
        GoToNextWaypoint();
    }

    private void Update()
    {
        if (_isChasing)
        {
            ChasePlayer();
        }
        else
        {
            CheckForPlayer();
            Patrol();
        }
    }

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
        _isChasing = false;
        GoToNextWaypoint(); // ������������ � ��������������
    }

    private void Patrol()
    {
        if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
        {
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
        _currentWaypointIndex = Random.Range(0, waypoints.Length); // ��������� ����� �����

        _agent.SetDestination(waypoints[_currentWaypointIndex].position);

        // ����� �������� ������ ��� �������� ����� ��������� � ��������� �����
        // ��������, ������� ����� �������� ����� ���������� �����
        StartCoroutine(WaitAtWaypoint());
    }
}