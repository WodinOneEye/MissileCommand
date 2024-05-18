using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMissile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject explosionEnemyMissilePrefab;

    private GameController myGameController;

    GameObject[] defenders;

    Vector3 target;

    void Start()
    {
        defenders = GameObject.FindGameObjectsWithTag("Defenders");
        Debug.Log(defenders.Length);
        target = defenders[Random.Range(0, defenders.Length)].transform.position;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Defenders"))
        {
            FindObjectOfType<GameController>().EnemyMissileDestroyed();
            // Instantiate explosion and destroy it after 1 second
            GameObject explosion = Instantiate(explosionEnemyMissilePrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 1f); // Destroy explosion after 1 second
            Destroy(gameObject); // Destroy the missile
            Destroy(col.gameObject); // Destroy the defender
        }
        else if (col.CompareTag("Ground"))
        {
            FindObjectOfType<GameController>().EnemyMissileDestroyed();
            MissileExplode();
        }
        else if (col.CompareTag("Explosions"))
        {
            FindObjectOfType<GameController>().AddMissileDestroyedPoints();
            MissileExplode();
        }
    }

    private void MissileExplode () //spawns explosion and destroys missile
    {
        GameObject explosion = Instantiate(explosionEnemyMissilePrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        Destroy(gameObject);
       
    }





}
