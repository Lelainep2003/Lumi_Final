using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameOverScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the text object in the scene

    void Start()
    {
        // Set score text from Player_Script's static variable
        scoreText.text = "Score: " + Player_Script.finalScore;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MainScene"); // Change to your main scene name
        }
    }
}