using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShieldAbility : Ability
{
    public override void TriggerAbility()
    {
        GameObject.Find("Player").GetComponentInChildren<AbilityHolder>().shield.SetActive(true);
        GameObject.Find("Player").GetComponentInChildren<AbilityHolder>().shield.GetComponent<Shield>().TriggerShield();
        GameObject.Find("Player").GetComponentInChildren<Player>().Burning.SetActive(false);
    }

    
}
