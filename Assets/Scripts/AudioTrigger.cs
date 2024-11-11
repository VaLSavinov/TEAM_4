using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag == "Enemy") 
        {
            Debug.Log("���� ������");
            other.GetComponent<EnemyAI>().StartAlerted();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
