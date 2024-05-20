using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject endOfRoundPanel;
    private EnemyMissileSpawner myEnemyMissileSpawner;
    private bool isRoundOver = false;

    public int score = 0;
    public int level = 1;

    public int cityCounter = 0;

    public float enemyMissileSpeed = 5f;
    [SerializeField] private float enemyMissileSpeedMultiplier = 0.25f;
    public int playerMissilesRemaining = 45;
    public int currentMissilesLoadedInLauncher = 0;
    private int enemyMissilesThisRound = 20;
    private int enemyMissilesRemainingInRound = 0;
    [SerializeField] private int missileEndOfRoundPoints = 5;
    [SerializeField] private int cityEndOfRoundPoints = 100;

    // Score values
    private int missileDestroyedPoints = 25;

    [SerializeField] private TextMeshProUGUI myScoreText;
    [SerializeField] private TextMeshProUGUI myLevelText;
    [SerializeField] private TextMeshProUGUI myMissilesText;
    [SerializeField] private TextMeshProUGUI MissilesLeftInLauncherText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI leftOverMissileBonusText;
    [SerializeField] private TextMeshProUGUI leftOverCityBonusText;
    [SerializeField] private TextMeshProUGUI totalBonusText;

    void Start()
    {
        // Initial setup of missiles
        currentMissilesLoadedInLauncher = 10;
        playerMissilesRemaining -= 10;

        myEnemyMissileSpawner = GameObject.FindObjectOfType<EnemyMissileSpawner>();
        cityCounter = GameObject.FindObjectsOfType<CityScript>().Length;

        // Update UI texts
        UpdateScoreText();
        UpdateLevelText();
        UpdateMissilesRemainingText();
        UpdateMissilesInLauncherText();

        StartRound();
    }

    void Update()
    {
        // Check if the round is over
        if (enemyMissilesRemainingInRound <= 0 && !isRoundOver)
        {
            Debug.Log("Round is over");
            isRoundOver = true;
            StartCoroutine(EndOfRound());
        }

        if (cityCounter <=0)
        {
            SceneManager.LoadScene("TheEnd");
        }
    }

    // Update the remaining missiles text
    public void UpdateMissilesRemainingText()
    {
        myMissilesText.text = "Missiles in Storage: " + playerMissilesRemaining;
        UpdateMissilesInLauncherText();
    }

    // Update the missiles loaded in the launcher text
    public void UpdateMissilesInLauncherText()
    {
        MissilesLeftInLauncherText.text = "Missiles Loaded: " + currentMissilesLoadedInLauncher;
    }

    // Update the score text
    public void UpdateScoreText()
    {
        myScoreText.text = "Score: " + score;
    }

    // Update the level text
    public void UpdateLevelText()
    {
        myLevelText.text = "Level: " + level;
    }

    // Add points for destroyed missiles
    public void AddMissileDestroyedPoints()
    {
        score += missileDestroyedPoints;
        EnemyMissileDestroyed();
        UpdateScoreText();
    }

    // Decrease the count of remaining enemy missiles
    public void EnemyMissileDestroyed()
    {
        enemyMissilesRemainingInRound--;
        Debug.Log("Enemy missiles remaining in round: " + enemyMissilesRemainingInRound);
    }

    // Logic when a player fires a missile
    public void PlayerFiredMissile()
    {
        if (currentMissilesLoadedInLauncher > 0)
        {
            currentMissilesLoadedInLauncher--;
        }

        if (currentMissilesLoadedInLauncher == 0)
        {
            if (playerMissilesRemaining >= 10)
            {
                currentMissilesLoadedInLauncher = 10;
                playerMissilesRemaining -= 10;
            }
            else
            {
                currentMissilesLoadedInLauncher = playerMissilesRemaining;
                playerMissilesRemaining = 0;
            }
        }

        UpdateMissilesRemainingText();
    }

    // Logic when a missile launcher is hit
    public void MissileLauncherHit()
    {
        Debug.Log("Missile launcher hit. Before hit - Loaded: " + currentMissilesLoadedInLauncher + ", Remaining: " + playerMissilesRemaining);

        // Set loaded missiles to 0
        currentMissilesLoadedInLauncher = 0;

        // Attempt to reload the launcher with 10 missiles if available
        if (playerMissilesRemaining >= 10)
        {
            currentMissilesLoadedInLauncher = 10;
            playerMissilesRemaining -= 10;
        }
        else
        {
            currentMissilesLoadedInLauncher = playerMissilesRemaining;
            playerMissilesRemaining = 0;
        }

        // Update UI texts
        UpdateMissilesRemainingText();
        UpdateMissilesInLauncherText();

        Debug.Log("Missile launcher hit. After hit - Loaded: " + currentMissilesLoadedInLauncher + ", Remaining: " + playerMissilesRemaining);
    }

    // Start a new round of the game
    public void StartRound()
    {
        myEnemyMissileSpawner.missilesToSpawnThisRound = enemyMissilesThisRound;
        enemyMissilesRemainingInRound = enemyMissilesThisRound;
        myEnemyMissileSpawner.StartRound();
    }

    // Handle the end of a round
    public IEnumerator EndOfRound()
    {
        yield return new WaitForSeconds(0.5f);
        endOfRoundPanel.SetActive(true);

        CityScript[] cities = GameObject.FindObjectsOfType<CityScript>();
        int leftOverMissileBonus = (playerMissilesRemaining + currentMissilesLoadedInLauncher ) * missileEndOfRoundPoints;
        int leftOverCityBonus = cities.Length * cityEndOfRoundPoints;
        int totalBonus = leftOverCityBonus + leftOverMissileBonus;

        if (level >=3 &&  level < 5)
        {
            totalBonus *= 2;
        }

        else if (level >= 5 && level < 7)
        {
            totalBonus *= 3;
        }

        else if (level >= 7 && level < 9)
        {
            totalBonus *= 4;
        }

        else if (level >= 9 && level < 11)
        {
            totalBonus *= 5;
        }

        else if (level >= 11)
        {
            totalBonus *= 6;
        }

        leftOverMissileBonusText.text = "Remaining Missile Bonus: " + leftOverMissileBonus;
        leftOverCityBonusText.text = "Remaining City Bonus: " + leftOverCityBonus;
        totalBonusText.text = "Total Bonus: " + totalBonus;



        score += totalBonus;
        level += 1;
        UpdateScoreText();

        countdownText.text = "3";
        yield return new WaitForSeconds(1.5f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1.5f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1.5f);

        endOfRoundPanel.SetActive(false);
        isRoundOver = false;

        // Updating new round settings
        playerMissilesRemaining = 45;
        enemyMissileSpeed *= enemyMissileSpeedMultiplier;

        currentMissilesLoadedInLauncher = 10;
        playerMissilesRemaining -= 10;

        StartRound();
        UpdateLevelText();
        UpdateMissilesRemainingText();
    }
}
