using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMissile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject explosionEnemyMissilePrefab;

    private GameController myGameController;
    private GameObject[] defenders;
    private Vector3 target;

    void Start()
    {
        myGameController = GameObject.FindObjectOfType<GameController>();
        defenders = GameObject.FindGameObjectsWithTag("Defenders");
        target = defenders[Random.Range(0, defenders.Length)].transform.position;
        speed = myGameController.enemyMissileSpeed;
    }

    void Update()
    {
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Subtract 90 degrees to correct for the default sprite orientation
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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
            if (col.GetComponent<MissileLauncherScript>() != null)
            {
                myGameController.MissileLauncherHit();
                return;
            }
            Destroy(col.gameObject); // Destroy the defender
            myGameController.cityCounter--;
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

    private void MissileExplode() //spawns explosion and destroys missile
    {
        GameObject explosion = Instantiate(explosionEnemyMissilePrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        Destroy(gameObject);
    }
}
