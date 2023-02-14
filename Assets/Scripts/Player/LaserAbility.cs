using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GAP_LaserSystem;

[CreateAssetMenu]
public class LaserAbility : Ability
{
    public override void TriggerAbility()
    {
        GameObject.Find("Player").GetComponentInChildren<AbilityHolder>().laser.SetActive(true);

        GameObject.Find("Player").GetComponentInChildren<AbilityHolder>().laser.GetComponent<LaserScript>().ShootLaser(activeTime);
    }
}
