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
    

    




    void Start()
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

    

    public void DisplayHighScore()
    {
        List<HighScoreEntry> highScoreEntryList = SaveLoadManager.LoadScores();

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

