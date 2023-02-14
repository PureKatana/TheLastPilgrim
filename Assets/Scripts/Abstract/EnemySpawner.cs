using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float randomXStart;
    public float randomXEnd;
    public float randomZStart;
    public float randomZEnd;
    protected float xPos;
    protected float zPos;
    protected float yPos;
    public int enemyCount;
    public int maxNumberOfEnemy;
    public float timeForRespawn;
    public bool hasReachedMax;
    

    public virtual IEnumerator EnemySpawn()
    {
        while (enemyCount < maxNumberOfEnemy)
        {
            xPos = Random.Range(randomXStart, randomXEnd); //120,170
            zPos = Random.Range(randomZStart, randomZEnd);//105,150
            Instantiate(enemy, new Vector3(xPos, 1.5f, zPos), Quaternion.identity);
            yield return new WaitForSeconds(1f);
            enemyCount++;
        }
    }
    
}
