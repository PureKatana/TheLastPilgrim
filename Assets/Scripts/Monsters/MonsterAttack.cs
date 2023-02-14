using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private Player player;
    private EnemyAI enemy;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        enemy = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!player.isDead)
            {
                player.TakeDamage(enemy.Damage);
                print("Damage taken: " + enemy.Damage);
            }

            if (player.CurrentHealth <= 0)
                enemy.playerIsDead = true;
        }
    }
}
