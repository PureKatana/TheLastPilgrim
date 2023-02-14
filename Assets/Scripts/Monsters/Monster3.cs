using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster3 : EnemyAI
{
    private Monster3Spawn m3Spawn;
    [SerializeField] private AudioSource monsterAudio;
    [SerializeField] private AudioClip monsterAttack;
    [SerializeField] private AudioClip monsterDead;
    // Start is called before the first frame update
    void Start()
    {
        m3Spawn = GameObject.Find("M3Spawn").GetComponent<Monster3Spawn>();
        GameManager.instance.enemyList.Add(this);
        monsterAudio = GetComponent<AudioSource>();
        if (Player.instance.Level != 1)
        {
            this.Health = Health + (35 * Player.instance.Level);
            this.Damage = Damage + (12 * Player.instance.Level);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (playerInSightRange && !playerInAttackRange && !playerIsDead && !attacking
                || isBeingAttacked && !playerInAttackRange && !playerIsDead && !attacking)
        {
            ChasePlayer();
            combat = true;
        }
        if (base.isDead)
        {
            Stop(transform.position);
        }
    }

    public override void Stop(Vector3 value)
    {
        base.agent.SetDestination(value);
        transform.LookAt(value);
    }


    public override IEnumerator ResetAttack(float time)
    {
        GameManager.instance.PlaySoundEffect(monsterAudio, monsterAttack, 0.3f, 1.1f);
        return base.ResetAttack(time);
    }

    public override void MonsterDeath()
    {
        base.MonsterDeath();
        GameManager.instance.PlaySoundEffect(monsterAudio, monsterDead, 0.3f, 1f);
        m3Spawn.enemyCount--;
        m3Spawn.hasReachedMax = false;
        GameManager.instance.enemyList.Remove(this);
    }
}
