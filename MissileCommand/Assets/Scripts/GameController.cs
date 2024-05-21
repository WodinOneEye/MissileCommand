using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject endOfRoundPanel;
    [SerializeField] private GameObject newHighScorePanel;
    [SerializeField] private TMP_InputField newhighScoreInitials;
    private EnemyMissileSpawner myEnemyMissileSpawner;
    private bool isRoundOver = false;

    private StartMenuManager myStartMenuManager;
    private ScoreManager myScoreManager;

    public int score = 0;
    public int level = 1;

    public int cityCounter = 0;

    public float enemyMissileSpeed = 5f;
    [SerializeField] private float enemyMissileSpeedMultiplier = 0.25f;
    public int playerMissilesRemaining = 45;
    public int currentMissilesLoadedInLauncher = 0;
    private int enemyMissilesThisRound = 20;
    public int enemyMissilesRemainingInRound = 0;
    [SerializeField] private int missileEndOfRoundPoints = 5;
    [SerializeField] private int cityEndOfRoundPoints = 100;

    public bool isGameOver = false;

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

        myStartMenuManager = GameObject.FindObjectOfType<StartMenuManager>();
        myScoreManager = GameObject.FindObjectOfType<ScoreManager>();

        // Update UI texts
        UpdateScoreText();
        UpdateLevelText();
        UpdateMissilesRemainingText();
        UpdateMissilesInLauncherText();

        StartRound();
    }

    void Update()
    {
        if (cityCounter <= 0 && !isGameOver)
        {
            HandleGameOver();
            return;
        }

        // Check if the round is over
        if (enemyMissilesRemainingInRound <= 0 && !isRoundOver && !isGameOver)
        {
            enemyMissile[] m = GameObject.FindObjectsOfType<enemyMissile>();
            if (m.Length == 0)
            {
                isRoundOver = true;
                StartCoroutine(EndOfRound());
            }
        }
    }

    private void HandleGameOver()
    {
        isGameOver = true;
        if (myScoreManager.IsThisANewHighScore(score))
        {
            // Check if the new high score qualifies for the top 5
            if (IsScoreInTop5(score))
            {
                newHighScorePanel.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("TheEnd");
            }
        }
        else
        {
            SceneManager.LoadScene("TheEnd");
        }
    }

    // Method to check if the score is in the top 5
    private bool IsScoreInTop5(int score)
    {
        var highScores = myScoreManager.GetHighScores();

        // If there are fewer than 5 high scores, the score is automatically in the top 5
        if (highScores.Count < 5)
        {
            return true;
        }

        // Check if the score is higher than the lowest score in the top 5
        return score > highScores[4].score;
    }

    // Other methods remain the same...

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
        int leftOverMissileBonus = (playerMissilesRemaining + currentMissilesLoadedInLauncher) * missileEndOfRoundPoints;
        int leftOverCityBonus = cities.Length * cityEndOfRoundPoints;
        int totalBonus = leftOverCityBonus + leftOverMissileBonus;

        if (level >= 3 && level < 5)
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

    public void SubmitClicked()
    {
        string initials = newhighScoreInitials.text.ToUpper();
        if (initials.Length > 3)
        {
            initials = initials.Substring(0, 3);
        }
        myScoreManager.AddNewHighScore(new HighScoreEntry { score = this.score, name = initials });
        SceneManager.LoadScene("StartMenu");
    }
}
