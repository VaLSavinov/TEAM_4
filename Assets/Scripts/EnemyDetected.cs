using System.Numerics;
using UnityEngine;

public class EnemyDetected : MonoBehaviour
{
    [SerializeField] private Transform _viewPoint;
    [SerializeField] private EnemyMove _enemyMove;
    private PlayerCharacter _character;

    private bool _isDetected = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
            _character = other.GetComponent<PlayerCharacter>();
    }

    private void FixedUpdate()
    {
        if (_character != null)
        {
            Ray ray = new Ray(_viewPoint.position,NormolizateVector(_viewPoint.position,_character.GetCameraPosition()));
            //Проверка
            Debug.Log("Обнаружили");
            Debug.DrawRay(ray.origin, ray.direction * 200,Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,20))
            {
                if (hit.collider.tag == "Player")
                {
                    _enemyMove.StartPursuit(_character);
                    _isDetected = true;
                }
            }
        }
    }

     private void OnTriggerExit(Collider other)
     {
         if (!_isDetected && other.gameObject.tag == "Player")
             _character = null;
     }       

    private UnityEngine.Vector3 NormolizateVector(UnityEngine.Vector3 origin, UnityEngine.Vector3 endpoint) 
    {
        return (endpoint - origin) / UnityEngine.Vector3.Distance(endpoint, origin);
    }
}
