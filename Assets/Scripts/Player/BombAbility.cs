using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BombAbility : Ability
{
    [SerializeField] private Transform bombProjectile;
    [SerializeField] private Transform muzzle;
    
    public override void TriggerAbility()
    {
        //For the projectile
        Vector3 pos = GameObject.Find("Player").GetComponentInChildren<RaycastFiring>().RaycastSpawn.position;
        Vector3 end = GameObject.Find("Player").GetComponentInChildren<RaycastFiring>().RaycastEnd.position;
        Vector3 dir = (end - pos).normalized;
        Instantiate(bombProjectile, pos, Quaternion.LookRotation(dir, Vector3.up));

        //Sound effect
        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySoundEffect(
                        GameObject.Find("GameManager").GetComponent<GameManager>().GameAudio,
                        GameObject.Find("Player").GetComponentInChildren<AbilityHolder>().abilitySounds[0], 0.3f, 1f);
        //For the muzzle
        Instantiate(muzzle, pos, Quaternion.LookRotation(dir, Vector3.up));
    }
}
