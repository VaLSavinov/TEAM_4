using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints; // Массив точек для перемещения
    public float waitTime = 2f; // Время ожидания на каждой точке
    public float detectionRadius = 10f; // Радиус обнаружения игрока
    public float returnWaitTime = 2f; // Время ожидания перед возвращением к патрулированию

    private NavMeshAgent _agent;
    private Transform _player;
    private int _currentWaypointIndex = 0;
    private bool _isChasing = false;
    private Vector3 _lastKnownPosition; // Последняя известная позиция игрока

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform; // Предполагается, что у игрока установлен тег "Player"
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
        _isChasing = false;
        GoToNextWaypoint(); // Возвращаемся к патрулированию
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

        // Выбор следующей точки (рандомно или последовательно)
        _currentWaypointIndex = Random.Range(0, waypoints.Length); // Рандомный выбор точки

        _agent.SetDestination(waypoints[_currentWaypointIndex].position);

        // Можно добавить логику для ожидания перед переходом к следующей точке
        // Например, добавив время ожидания после достижения точки
        StartCoroutine(WaitAtWaypoint());
    }
}