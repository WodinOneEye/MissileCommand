using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileSpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyMissilePrefab;
    [SerializeField] private float yPadding = 0.5f;

    private float minX, maxX;



    void Start()
    {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).x;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).x;

        float randomX = Random.Range(minX, maxX);
        float yValue = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        Instantiate(enemyMissilePrefab, new Vector3(randomX, yValue + yPadding, 0), Quaternion.identity);   



       // Instantiate(enemyMissilePrefab, minX, Quaternion.identity);
       // Instantiate(enemyMissilePrefab, maxX, Quaternion.identity);
    }

    
    void Update()
    {
        
    }
}
