using System;
using System.Collections;
using System.Numerics;
using UnityEngine;

public class EnemyDetected : MonoBehaviour
{
    [SerializeField, Tooltip("����� ������.")] private Transform _viewPoint;
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField, Tooltip("����� ������������ ����� ������ �� ����")] private float _waitToSearch = 2f;
    [SerializeField] private LayerMask _layerMask;
   
    private bool _isDetected = false;
    private bool _isInVievZone = false;

    // �����, ������ ������� 
    private float _timeLoss;


    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.tag=="Player")
            _isInVievZone = true;         */  
    }

    private void Update()
    {

        if (!_isInVievZone && _timeLoss == 0)  return;
        
        Ray ray = new Ray(_viewPoint.position,NormolizateVector(_viewPoint.position, GameMode.FirstPersonLook.transform.position));
        //��������        
        Debug.DrawRay(ray.origin, ray.direction * 200,Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,20, _layerMask))
        {
            if (hit.collider.tag == "Player")
            {
                _isDetected = true;
                _enemyAI.ChasePlayer();
            }
            else
            if (_isDetected)
            {
                // _isInVievZone = false; //��� �����, ������� �����
                _isDetected = false;
                _timeLoss = Time.time;
            }           
               
        }
        if (!_isDetected && _timeLoss > 0)
            WaitToSearch();


    }

    /// <summary>
    /// ��������� �������� ����� ���, ��� ��������� �������� � ������� � ������
    /// </summary>
    private void WaitToSearch()
    {
        if (Time.time - _timeLoss >= _waitToSearch)
        {
            _enemyAI.StartSearchingPlayer();
            _timeLoss = 0;
        }
        else
        {
            _enemyAI.ChasePlayer();
        }
    }

    private void OnTriggerExit(Collider other)
     {
         if (!_isDetected && other.gameObject.tag == "Player")
            _isInVievZone = false;
     }       

    private UnityEngine.Vector3 NormolizateVector(UnityEngine.Vector3 origin, UnityEngine.Vector3 endpoint) 
    {
        return (endpoint - origin) / UnityEngine.Vector3.Distance(endpoint, origin);
    }

 }
