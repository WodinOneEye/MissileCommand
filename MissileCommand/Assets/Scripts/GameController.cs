using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    
    EnemyMissileSpawner myEnemyMissileSpawner;
    
    public int score = 0;
    public int level = 1;
    public int playerMissilesRemaining = 60;
    private int enemyMissilesThisRound = 20;
    private int enemyMissilesRemainingInRound = 0;

    //Score Values
    private int missileDestroyedPoints = 25;

    [SerializeField] private TextMeshProUGUI myScoreText;
    [SerializeField] private TextMeshProUGUI myLevelText;
    [SerializeField] private TextMeshProUGUI myMissilesText;

    


    void Start()
    {
        myEnemyMissileSpawner = GameObject.FindObjectOfType<EnemyMissileSpawner>();
        
        UpdateScoreText();
        UpdateLevelText();
        UpdateMissilesRemainingText();

        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (enemyMissilesRemainingInRound <=  0)
        {
            Debug.Log("Round is over");
        }
    }

    public void UpdateMissilesRemainingText()
    {
        myMissilesText.text = "Missiles: " + playerMissilesRemaining;
    }

    public void UpdateScoreText ()
    {
        myScoreText.text = "Score: " + score;
    }

    public void UpdateLevelText()
    {
       myLevelText.text = "Level: " + level;
    }

    public void AddMissileDestroyedPoints()
    {
        score += missileDestroyedPoints;
        EnemyMissileDestroyed();
        UpdateScoreText();
    }

    public void EnemyMissileDestroyed()
    {
        enemyMissilesRemainingInRound--;
        Debug.Log(enemyMissilesRemainingInRound);
    }

    public void StartRound()
    {
        myEnemyMissileSpawner.missilesToSpawnThisRound = enemyMissilesThisRound;
        enemyMissilesRemainingInRound = enemyMissilesThisRound;
        myEnemyMissileSpawner.StartRound();
    }

}
