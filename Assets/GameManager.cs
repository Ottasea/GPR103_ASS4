using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is to be attached to a GameObject called GameManager in the scene. It is to be used to manager the settings and overarching gameplay loop.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Scoring")]
    public int currentScore = 0; //The current score in this round.
    
    public Text scoreText;
    public Text highScoreText;

    public Text winText;
    public Text winSubText;

    public Text gameOverText;
    public Text gameOverSubText;
    public Text startGameText;

   

    [Header("Playable Area")]
    public float levelConstraintTop; //The maximum positive Y value of the playable space.
    public float levelConstraintBottom; //The maximum negative Y value of the playable space.
    public float levelConstraintLeft; //The maximum negative X value of the playable space.
    public float levelConstraintRight; //The maximum positive X value of the playablle space.

    [Header("Gameplay Loop")]
    public bool isGameRunning = false; //Is the gameplay part of the game current active?
    
    public float totalGameTime; //The maximum amount of time or the total time avilable to the player.
    public float timeWarning; //The point in time when the player is alerted that they are almost out of time.
    public float gameTimeRemaining; //The current elapsed time.

    
    public Image timer;
    private Vector3 startTimerScale;
    public bool timerColor = false;

    [Header("Health System")]
    public Image[] lives; //Reference to the image in the canvas used to display lives.
    public Sprite fullLife;
    public Sprite noLife;
    public Player myPlayer; //Reference to the Player in the scene.

    // Start is called before the first frame update
    void Start()
    {
        startTimerScale = timer.GetComponent<RectTransform>().localScale;
        
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        GameReset();

        HideWin();

    }

    // Update is called once per frame
    void Update()
    {
        HealthSystem();
        GameTimer();
        StartGame();
    }

    void HealthSystem() //Displays the visual representation for the lives dictated by the player script.
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < myPlayer.playerLivesRemaining)
            {
                lives[i].sprite = fullLife;
            }
            else
            {
                lives[i].sprite = noLife;
            }
            
            if (i < myPlayer.playerTotalLives)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }

    void GameTimer() //Controls the timer which counts down from 30, player dies at 0 and then timer resets if game is still playing
    {
        
        if (isGameRunning == true)
        {
            gameTimeRemaining += Time.deltaTime;

            Vector3 scale = new Vector3(startTimerScale.x - gameTimeRemaining, startTimerScale.y, startTimerScale.z); //Scale of the image changes by subtracting the current timer from its x scale

            timer.GetComponent<RectTransform>().localScale = scale;

            if (gameTimeRemaining >= startTimerScale.x - timeWarning) //When the timer reaches the timeWarning number, change the timer bar to red
            {
                timerColor = true;
                timer.color = Color.red;
            }
            else
            {
                timerColor = false;
                timer.color = Color.white;
            }


            if (gameTimeRemaining >= totalGameTime) //Is the current timer is equal to or greater than the max time, player dies
            {
                myPlayer.GameOver();
            }
        }
        
    }

    public void ResetTimer () //Resets the timer back to zero when called
    {
        gameTimeRemaining = 0;
    } 

    public void UpdatePlayerScore (int score)
    {
        currentScore += score;
        scoreText.text = currentScore.ToString();

        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            highScoreText.text = currentScore.ToString();
        }
    } 

    public void ResetPlayerScore ()
    {
        scoreText.text = "0";
    }

    public void StartGame ()
    {
        if (isGameRunning == false)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                myPlayer.playerCanMove = true;

                isGameRunning = true;

                timer.enabled = true;

                myPlayer.GetComponent<SpriteRenderer>().enabled = true;

                ResetPlayerScore();

                startGameText.enabled = false;
            }
        }
        
       
    }

    public void GameReset()
    {

        timer.enabled = false;

        myPlayer.GetComponent<SpriteRenderer>().enabled = false;

        myPlayer.playerCanMove = false;

        isGameRunning = false;

        ResetTimer();

        startGameText.enabled = true;

        gameOverText.enabled = false;

    }

    public void ShowWin()
    {
        winText.enabled = true;
        winSubText.enabled = true;
    }

    public void HideWin()
    {
        winText.enabled = false;
        winSubText.enabled = false;
    }

    public void ShowGameOver()
    {
        gameOverText.enabled = true;
        gameOverSubText.enabled = true;
    }

    public void HideGameOver()
    {
        gameOverText.enabled = false;
        gameOverSubText.enabled = false;
    }
}
