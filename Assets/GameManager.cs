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

    public CollidableObject[] homeBase = new CollidableObject[5]; //Reference to the homebases in the game.
    public Transform[] extraLifePos;

    [Header("Gameplay Loop")]
    public bool isGameRunning = false; //Is the gameplay part of the game current active?
    
    public float totalGameTime; //The maximum amount of time or the total time avilable to the player.
    public float timeWarning; //The point in time when the player is alerted that they are almost out of time.
    public float gameTimeRemaining; //The current elapsed time.

    public float baseSpawnTimer = 5f;
    public float baseSpawnedTimer = 2f;

    public float extraLifeSpawnTimer = 10f;
    public float extraLifeSpawnedTimer = 3f;

    public Image timer;
    private Vector3 startTimerScale;
    public bool timerColor = false;
    public AudioClip timeWarningSound;

    [Header("Health System")]
    public Image[] lives; //Reference to the image in the canvas used to display lives.
    public Sprite fullLife; //Sprite to be shown when a life needs to be visible
    public Sprite noLife; //Sprite to show after loss of life
    public Player myPlayer; //Reference to the Player in the scene.

    // Start is called before the first frame update
    void Start()
    {
        startTimerScale = timer.GetComponent<RectTransform>().localScale;
        
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        GameReset();

        HideWin();

        StartCoroutine(BaseChanger());
        
        StartCoroutine(ExtraLifeSpawn());

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
                AudioSource.PlayClipAtPoint(timeWarningSound, transform.position);
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

    public void UpdatePlayerScore (int score) //Keeps track of player score and converts to High Score when approriate
    {
        currentScore = int.Parse(scoreText.text);
        currentScore += score;
        scoreText.text = currentScore.ToString();

        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            highScoreText.text = currentScore.ToString();
        }
    } 

    public void ResetPlayerScore () //Reset the players score upon starting a new game
    {
        scoreText.text = "0";
    }
   
    public void StartGame ()//Allows the game to start
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

    public void GameReset()//Reset game after a win or game over screen
    {

        timer.enabled = false;

        myPlayer.GetComponent<SpriteRenderer>().enabled = false;

        myPlayer.playerCanMove = false;

        isGameRunning = false;

        ResetTimer();

        startGameText.enabled = true;

        gameOverText.enabled = false;

    }

    public void ShowWin()//Show win text
    {
        winText.enabled = true;
        winSubText.enabled = true;
    }

    public void HideWin()//Hide win text
    {
        winText.enabled = false;
        winSubText.enabled = false;
    }

    public void ShowGameOver() //Show game over text.
    {
        gameOverText.enabled = true;
        gameOverSubText.enabled = true;
    }

    public void HideGameOver() //Hide game over text
    {
        gameOverText.enabled = false;
        gameOverSubText.enabled = false;
    }

    IEnumerator BaseChanger()
    {
        while (true)
        {
            yield return new WaitForSeconds(baseSpawnTimer);

            int randomIndex = Random.Range(0, homeBase.Length);

            yield return homeBase[randomIndex];

            float baseModifier = Random.value;

            if (!homeBase[randomIndex].hasTrophy)
            {
                if (baseModifier < 0.5f)
                {
                    homeBase[randomIndex].isCrocBase = true;

                    homeBase[randomIndex].isSafe = false;

                    homeBase[randomIndex].isHomeBase = false;

                    homeBase[randomIndex].GetComponent<SpriteRenderer>().sprite = homeBase[randomIndex].crocBaseSprite;

                    yield return new WaitForSeconds(baseSpawnedTimer);

                    homeBase[randomIndex].isCrocBase = false;

                    homeBase[randomIndex].isSafe = true;

                    homeBase[randomIndex].isHomeBase = true;

                    homeBase[randomIndex].GetComponent<SpriteRenderer>().sprite = homeBase[randomIndex].homeBaseSprite;

                }

                else if (baseModifier > 0.5f)
                {
                    homeBase[randomIndex].isFlyBase = true;

                    homeBase[randomIndex].isHomeBase = false;

                    homeBase[randomIndex].GetComponent<SpriteRenderer>().sprite = homeBase[randomIndex].flyBaseSprite;

                    yield return new WaitForSeconds(baseSpawnedTimer);

                    if (homeBase[randomIndex].hasTrophy == true)
                    {
                        homeBase[randomIndex].isFlyBase = false;

                        homeBase[randomIndex].isHomeBase = true;

                        homeBase[randomIndex].GetComponent<SpriteRenderer>().sprite = homeBase[randomIndex].trophyBaseSprite;
                    }

                    else
                    {
                        homeBase[randomIndex].isFlyBase = false;

                        homeBase[randomIndex].isHomeBase = true;

                        homeBase[randomIndex].GetComponent<SpriteRenderer>().sprite = homeBase[randomIndex].homeBaseSprite;
                    }                    
                } 
            }

        } 
        
    } //spawns either a fly or a croc inside of the homebase every x amount of time for only a short duration

    IEnumerator ExtraLifeSpawn() //spawns extra lives at random spawn points every 10 seconds for a period of 4 seconds (time can be changed)
    {
        while (true)
        {
            yield return new WaitForSeconds(extraLifeSpawnTimer);

            int randomIndex = Random.Range(0, extraLifePos.Length);

            GameObject extraLifeClone = (GameObject)Instantiate(Resources.Load("extraLife", typeof(GameObject)), extraLifePos[randomIndex].position, Quaternion.identity);

            yield return new WaitForSeconds(extraLifeSpawnedTimer);

            Destroy(extraLifeClone);

        }
    }
}
