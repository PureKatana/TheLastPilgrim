using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public override void TriggerAbility()
    {
        GameObject.Find("Player").GetComponentInChildren<AbilityHolder>().dash = true;
        GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>()._controller.Move(GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>().moveDirection.normalized * 30f * Time.deltaTime);

        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySoundEffect(
                            GameObject.Find("GameManager").GetComponent<GameManager>().GameAudio,
                            GameObject.Find("Player").GetComponentInChildren<AbilityHolder>().abilitySounds[2], 0.4f, 1f);
    }
    
}
