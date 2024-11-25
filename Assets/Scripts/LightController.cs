using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Light> _lamps;
    [SerializeField] private LayerMask _layersMask;

    private List<EnemyAI> _enemyAIs = new List<EnemyAI>();

    private bool _isEnabled = true;
    private bool _isBlackout = false;



    private void OnTriggerStay(Collider other)
    {
        float h, h1,r;
        Vector3 conusPos;
        if (other.tag == "Player" && _isEnabled)
        {
            foreach (Light light in _lamps)
            {
                if (!light.enabled) continue;
                // ���������� ������� �� y ��������
                h = light.transform.position.y;
                h1 = h - GameMode.FirstPersonLook.transform.position.y;
                r = (h1 / h) * light.range;
                conusPos = new Vector3(light.transform.position.x, GameMode.FirstPersonLook.transform.position.y, light.transform.position.z);
                if (Vector3.Distance(GameMode.FirstPersonLook.transform.position, conusPos) < r)
                {
                    ReleaseRayCast(light);
                }
                else GameMode.FirstPersonLook.RemoveLight(light);
            } 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") 
            _enemyAIs.Add(GameMode.EnemyManager.GetEnemyForGameObject(other.gameObject));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            foreach (Light light in _lamps)
                GameMode.FirstPersonLook.RemoveLight(light);
        else if (other.tag == "Enemy") 
                _enemyAIs.Remove(GameMode.EnemyManager.GetEnemyForGameObject(other.gameObject));
    }

    private UnityEngine.Vector3 NormolizateVector(UnityEngine.Vector3 origin, UnityEngine.Vector3 endpoint)
    {
        return (endpoint - origin) / UnityEngine.Vector3.Distance(endpoint, origin);
    }

    private void ReleaseRayCast(Light light) 
    {
        Ray ray = new Ray(light.transform.position, NormolizateVector(light.transform.position, GameMode.FirstPersonLook.transform.position));
        //��������        
        Debug.DrawRay(ray.origin, ray.direction * 200, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, _layersMask))
        {
            if (hit.collider.tag == "Player")
            {
                GameMode.FirstPersonLook.AddLight(light);
            }
            else
            {
                GameMode.FirstPersonLook.RemoveLight(light);
            }
        }       
    }

    public void Interact()
    {
        if (!_isBlackout)
        {
            _isEnabled = !_isEnabled;
            for (int i = 0; i < _lamps.Count; i++) 
            {
                _lamps[i].enabled = _isEnabled;
            }
        }
    }

    public bool Interact(ref GameObject interactingOject)
    {
        throw new System.NotImplementedException();
    }
}

