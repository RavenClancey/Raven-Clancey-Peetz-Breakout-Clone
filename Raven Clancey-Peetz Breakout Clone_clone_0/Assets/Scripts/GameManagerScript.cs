using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Text for the score on canvas

    private int playerScore = 0;                        // variable for the score of the player


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // increase the player score by the given amount
    public void AddScore(int score)
    {
        playerScore += score;
        UpdateScore();
    }

    // Update the Score text on canvas with the player current score;
    public void UpdateScore()
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }

   
}
