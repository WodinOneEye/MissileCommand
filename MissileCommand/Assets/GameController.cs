using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    
    public int score = 0;
    public int level = 1;
    public int missilesRemaining = 30;

    [SerializeField] private TextMeshProUGUI myScoreText;
    [SerializeField] private TextMeshProUGUI myLevelText;
    [SerializeField] private TextMeshProUGUI myMissilesText;




    void Start()
    {
        myScoreText.text = "Score: " + score;   
        myLevelText.text = "Level: " + level;
        UpdateMissilesRemainingText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMissilesRemainingText()
    {
        myMissilesText.text = "Missiles: " + missilesRemaining;
    }

}
