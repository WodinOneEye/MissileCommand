using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMissile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject explosionEnemyMissilePrefab;
    [SerializeField] private GameObject enemyMissilePrefab;
    [SerializeField] private float stationaryTimeThreshold = 2f; // Time in seconds to check if the missile is stationary

    private GameController myGameController;
    private GameObject[] defenders;
    private Vector3 target;

    private float randomTimer;
    private Vector3 lastPosition;
    private float stationaryTimer;

    void Start()
    {
        myGameController = GameObject.FindObjectOfType<GameController>();
        defenders = GameObject.FindGameObjectsWithTag("Defenders");
        target = defenders[Random.Range(0, defenders.Length)].transform.position;
        speed = myGameController.enemyMissileSpeed;

        randomTimer = Random.Range(0.1f, 35);
        randomTimer = randomTimer / myGameController.enemyMissileSpeed;
        Invoke("SplitMissile", randomTimer);

        lastPosition = transform.position;
        stationaryTimer = 0f;
    }

    void Update()
    {
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Subtract 90 degrees to correct for the default sprite orientation
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        CheckIfStationary();
    }

    private void CheckIfStationary()
    {
        if (transform.position == lastPosition)
        {
            stationaryTimer += Time.deltaTime;
            if (stationaryTimer >= stationaryTimeThreshold)
            {
                Debug.Log("Missile is stationary for too long. Destroying missile.");
                Destroy(gameObject);
            }
        }
        else
        {
            stationaryTimer = 0f;
        }

        lastPosition = transform.position;
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
        //myAudio.Play();
        Destroy(explosion, 1f);
        Destroy(gameObject);
    }

    private void SplitMissile()
    {
        float yValue = Camera.main.ViewportToWorldPoint(new Vector3(0, .40f, 0)).y;

        if (transform.position.y >= yValue)
        {
            myGameController.enemyMissilesRemainingInRound++;
            Instantiate(enemyMissilePrefab, transform.position, Quaternion.identity);
        }
    }
}
