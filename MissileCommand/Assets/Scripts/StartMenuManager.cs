using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class StartMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject highScorePanel;
    [SerializeField] private TextMeshProUGUI[] highScoreTextFields;
    private List<HighScoreEntry> highScoreEntryList;

    public static StartMenuManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        /*
        highScoreEntryList = new List<HighScoreEntry>()
      {
          new HighScoreEntry { score = 10000, name = "AAA"},
          new HighScoreEntry { score = 20000, name = "AAB"},
          new HighScoreEntry { score = 200, name = "AAC"},
          new HighScoreEntry { score = 23410000, name = "AAD"},
          new HighScoreEntry { score = 10, name = "AAE"}
      };
        
        SaveScores();
        */

    }


    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("MissileCommandGame_01");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu");

    }

    public void ShowHighScores()
    {
        highScorePanel.SetActive(true);
        DisplayHighScore();
        //Debug.Log("Clicked high scores button");
    }

    public void SaveScores()
    {

        // Sort by highest to lowest
        for (int i = 0; i < highScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScoreEntryList.Count; j++)
            {
                if (highScoreEntryList[j].score > highScoreEntryList[i].score)
                {
                    //swap values
                    HighScoreEntry tmpEntry = highScoreEntryList[i];
                    highScoreEntryList[i] = highScoreEntryList[j];
                    highScoreEntryList[j] = tmpEntry;


                }
            }
        }

        SaveLoadManager.SaveScores(highScoreEntryList);
    }

    public bool IsThisANewHighScore(int score)
    {
        highScoreEntryList = SaveLoadManager.LoadScores();

        for (int i = 0; i < highScoreEntryList.Count; i++)
        {
            if (score > highScoreEntryList[i].score)
            {
                return true;
            }
        }
        return false;
    }

    public void AddNewHighScore(HighScoreEntry newScoreEntry)
    {
        highScoreEntryList.Add(newScoreEntry);

        SaveScores();

        highScoreEntryList.RemoveAt(5);
    }

    public void DisplayHighScore()
    {
        highScoreEntryList = SaveLoadManager.LoadScores();

        // Display high score on the UI
        for (int i = 0; i < highScoreTextFields.Length; i++)
        {
            if (i < highScoreEntryList.Count)
            {
                highScoreTextFields[i].text = "         " + highScoreEntryList[i].name + "                                                 " + highScoreEntryList[i].score;
            }
            else
            {
                highScoreTextFields[i].text = "         ---                                                 ---";
            }
        }
    }
}

[Serializable]
public class HighScoreEntry
{
    public int score;
    public string name;
}