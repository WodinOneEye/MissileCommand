using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMissile : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    GameObject[] defenders;

    Transform target;


    void Start()
    {
        defenders = GameObject.FindGameObjectsWithTag("Defenders");
        Debug.Log(defenders.Length);
        target = defenders[Random.Range(0, defenders.Length)].transform;
    }

    
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
