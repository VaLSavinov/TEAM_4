using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSoundObject : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourse;
    [SerializeField] private SphereCollider _audioSphere;


    private void OnTriggerEnter(Collider other)
    {
        if (!_audioSourse.isPlaying)
        {
            _audioSourse.Play();
            /* _audioSourse.gameObject.SetActive(true);
             _audioSphere.radius = GetSizeAudioZone();*/
           Collider [] overlaping = Physics.OverlapSphere(transform.position, GetSizeAudioZone());
            foreach (Collider c in overlaping) 
            {
                if (c.tag == "Enemy") c.GetComponent<EnemyAI>().StartAlerted(transform.position);
            }
        }
    }


    private float GetSizeAudioZone() 
    {
        return _audioSourse.maxDistance / _audioSourse.volume; // Получаем радиус звука в зависимости от громкости
    }
}
