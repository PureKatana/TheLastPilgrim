using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFiring : MonoBehaviour
{
    public bool isFiring = false;
    [SerializeField] private ParticleSystem muzzle;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem hitEffectOnMonster;
    [SerializeField] private Transform raycastSpawn;
    [SerializeField] private Transform raycastEnd;
    [SerializeField] private TrailRenderer bulletEffect;
    [SerializeField] private LayerMask hitLayerMask;
    private Player player;

    Ray ray;
    RaycastHit hit;

    public Transform RaycastSpawn { get => raycastSpawn; set => raycastSpawn = value; }
    public Transform RaycastEnd { get => raycastEnd; set => raycastEnd = value; }

    private void Start()
    {
        player = Player.instance;

    }
    public void StartFiring()
    {
        isFiring = true;
        muzzle.Emit(1);//Play once;

        ray.origin = raycastSpawn.position;
        ray.direction = raycastEnd.position - raycastSpawn.position;

        TrailRenderer bullet = Instantiate(bulletEffect, ray.origin, Quaternion.identity);
        bullet.AddPosition(ray.origin);

        if(Physics.Raycast(ray, out hit, 200f, hitLayerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 1.0f);
            
            //EnemyAI.TakeDamage(player.Damage);
            if(hit.collider.gameObject.CompareTag("Boss"))
            {
                hit.collider.gameObject.GetComponentInParent<BossMonster>().TakeDamage(player.Damage);
                HitEffect();
            }
            else if(hit.collider.gameObject.CompareTag("Monster1"))
            {
                hit.collider.gameObject.GetComponentInParent<Monster1>().TakeDamage(player.Damage);
                HitEffect();
            }
            else if(hit.collider.gameObject.CompareTag("Monster2"))
            {
                hit.collider.gameObject.GetComponentInParent<Monster2>().TakeDamage(player.Damage);
                HitEffect();
            }
            else if(hit.collider.gameObject.CompareTag("Monster3"))
            {
                hit.collider.gameObject.GetComponentInParent<Monster3>().TakeDamage(player.Damage);
                HitEffect();
            }
            else
            {
                //Normal hit effect
                hitEffect.transform.position = hit.point;
                hitEffect.transform.forward = hit.normal;
                hitEffect.Emit(2);
            }

            bullet.transform.position = hit.point;
        }
    }

    private void HitEffect()
    {
        hitEffectOnMonster.transform.position = hit.point;
        hitEffectOnMonster.transform.forward = hit.normal;
        hitEffectOnMonster.Emit(2);
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
