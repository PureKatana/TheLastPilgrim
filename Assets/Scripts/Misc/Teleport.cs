using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Teleport : AbstractTeleport
{
    [SerializeField] private bool isOption;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isOption)
        {
            StartTeleport();
        }
    }

    public void TeleportOption()
    {
        StartTeleport();
    }

    void StartTeleport()
    {
        GameManager.instance.SaveData();
        StartCoroutine(Teleporting());
    }

    
}
