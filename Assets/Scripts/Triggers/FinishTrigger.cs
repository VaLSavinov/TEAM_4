using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")        
        {
            LocalizationManager.Instance.SafeCSV();
            GameMode.PlayerUI.Finish();
            GameMode.FirstPersonLook.BlockPlayerController();
        }
    }
}
