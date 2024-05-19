using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject endOfRoundPanel;
    
    EnemyMissileSpawner myEnemyMissileSpawner;

    bool isRoundOver = false;
    
    public int score = 0;
    public int level = 1;
    public int playerMissilesRemaining = 60;
    private int enemyMissilesThisRound = 20;
    private int enemyMissilesRemainingInRound = 0;
    [SerializeField] private int missileEndOfRoundPoints = 5;
    [SerializeField] private int cityEndOfRoundPoints = 100;

    //Score Values
    private int missileDestroyedPoints = 25;

    [SerializeField] private TextMeshProUGUI myScoreText;
    [SerializeField] private TextMeshProUGUI myLevelText;
    [SerializeField] private TextMeshProUGUI myMissilesText;
    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private TextMeshProUGUI leftOverMissileBonusText;
    [SerializeField] private TextMeshProUGUI leftOverCityBonusText;
    [SerializeField] private TextMeshProUGUI totalBonusText;




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
        
        if (enemyMissilesRemainingInRound <=  0 && !isRoundOver)
        {
            Debug.Log("Round is over");
            isRoundOver = true;
            StartCoroutine(EndOfRound());
            
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

    public IEnumerator EndOfRound()
    {
        yield return new WaitForSeconds(0.5f);
        endOfRoundPanel.SetActive(true);

        CityScript[] cities = GameObject.FindObjectsOfType<CityScript>();
        int leftOverMissileBonus = playerMissilesRemaining * missileEndOfRoundPoints;
        int leftOverCityBonus = cities.Length * cityEndOfRoundPoints;
        int totalBonus = leftOverCityBonus + leftOverMissileBonus;

        leftOverMissileBonusText.text = "Remaining Missile Bonus: " + leftOverMissileBonus;
        leftOverCityBonusText.text = "Remaining City Bonus: " + leftOverCityBonus;
        totalBonusText.text = "Total Bonus: " + totalBonus;

        score = score + totalBonus;
        UpdateScoreText();

        //Increase enemy missile count and speed for the next round here
        countdownText.text = "3";
        yield return new WaitForSeconds(1.5f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1.5f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1.5f);

        endOfRoundPanel.SetActive(false);

        StartRound();


    }

}
