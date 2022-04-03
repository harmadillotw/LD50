using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenController : MonoBehaviour
{
    public Text scoreText;
    public Text highScoretext;

    private int highScore;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Science: " + StateSettings.score;

        highScore = PlayerPrefs.GetInt("HighScore");
        if (StateSettings.score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", StateSettings.score);
            highScoretext.text = "New Highest Science: " + StateSettings.score;
        }
        else
        {       
            highScoretext.text = "Highest Science: " + highScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
