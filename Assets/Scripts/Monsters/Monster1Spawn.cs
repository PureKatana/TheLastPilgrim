using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Spawn : EnemySpawner
{
    private void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    void Update()
    {
        if (!hasReachedMax)
        {
            StartCoroutine(WaitForRespawn());
            hasReachedMax = true;
        }
    }

    public override IEnumerator EnemySpawn()
    {
        return base.EnemySpawn();
    }

    private IEnumerator WaitForRespawn()
    { 
        yield return new WaitForSeconds(timeForRespawn);
        StartCoroutine(EnemySpawn());
    }
}
