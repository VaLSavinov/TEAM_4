using System.Numerics;
using UnityEngine;

public class EnemyDetected : MonoBehaviour
{
    [SerializeField, Tooltip("Точка обзора.")] private Transform _viewPoint;
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private LayerMask _layerMask;
   
    private bool _isDetected = false;
    private bool _isInVievZone = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
            _isInVievZone = true;           
    }

    private void FixedUpdate()
    {
        if (!_isInVievZone) return;   
        
        Ray ray = new Ray(_viewPoint.position,NormolizateVector(_viewPoint.position, GameMode.FirstPersonLook.transform.position));
        //Проверка        
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
               // _isInVievZone = false; //Без этого, кажется лучше
                _isDetected =false;
                _enemyAI.StartSearchingPlayer();
            }
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
