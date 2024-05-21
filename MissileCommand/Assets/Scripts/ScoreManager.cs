using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private List<HighScoreEntry> highScoreEntryList;

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
          new HighScoreEntry { score = 0, name = "JBL"},
          new HighScoreEntry { score = 0, name = "JBL"},
          new HighScoreEntry { score = 0, name = "JBL"},
          new HighScoreEntry { score = 0, name = "JBL"},
          new HighScoreEntry { score = 0, name = "JBL"}
      };

        SaveScores(); 
        */
    }

    
    void Update()
    {
        
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
}

[Serializable]
public class HighScoreEntry
{
    public int score;
    public string name;
}
