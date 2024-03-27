/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawner;
    [SerializeField]
    private Transform[] spawnpoint;

    public float startSpawnerInterval;
    private float spawnerInterval;

    public int numberOfEnemies;
    public int nowTheEnemies;

    private int randPoint;
    private int randEnemy;

    void Start()
    {
        spawnerInterval = startSpawnerInterval;
        
    }

    void Update()
    {
        if (spawnerInterval <= 0 && nowTheEnemies < numberOfEnemies)
        {
            randEnemy = Random.Range(0, spawner.Length);
            randPoint = Random.Range(0, spawnpoint.Length);

            Instantiate(spawner[randEnemy], spawnpoint[randPoint].transform.position,Quaternion.identity);

            spawnerInterval = startSpawnerInterval;
            nowTheEnemies++;
        }
        else
        {
            spawnerInterval -= Time.deltaTime;
        }
    }
}*/
