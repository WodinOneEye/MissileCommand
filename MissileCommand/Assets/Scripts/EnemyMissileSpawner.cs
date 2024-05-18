using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileSpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyMissilePrefab;
    [SerializeField] private float yPadding = 0.5f;

    private float minX, maxX;

    public int missilesToSpawnThisRound = 10;
    public float delayBetweenMissiles = .5f;
    float yValue;



    void Start()
    {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).x;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).x;

        float randomX = Random.Range(minX, maxX);
        yValue = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        // Instantiate(enemyMissilePrefab, new Vector3(randomX, yValue + yPadding, 0), Quaternion.identity);   

        StartCoroutine(SpawnMissiles());
    }

    
    void Update()
    {
        
    }

    public IEnumerator SpawnMissiles ()
    {
        while (missilesToSpawnThisRound > 0)
        {
            float randomX = Random.Range(minX, maxX);
            //yValue = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
            Instantiate(enemyMissilePrefab, new Vector3(randomX, yValue + yPadding, 0), Quaternion.identity);

            missilesToSpawnThisRound--;

            yield return new WaitForSeconds(delayBetweenMissiles);
        }
    }



}
