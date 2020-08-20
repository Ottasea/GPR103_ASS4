using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This script must be used as the core Player script for managing the player character in the game.
/// </summary>
public class Player : MonoBehaviour
{
    public string playerName = ""; //The players name for the purpose of storing the high score

    public int playerTotalLives; //Players total possible lives.
    public int playerLivesRemaining; //Players actual lives remaining.

    public int maxY = 0, currentY = 1;

    public int frogsAtHome = 0;
    public Text youWinText;


    public bool playerCanMove = false; //Can the player currently move?
    
    public Sprite playerUp, playerDown, playerLeft, playerRight; //Select how the player looks based on the direction its travelling.

    public AudioClip hopSound;
    public AudioClip squashSound;
    public AudioClip homeBaseSound;

    public GameManager myGameManager; //A reference to the GameManager in the scene.
    private Vector2 startPos; //Where is the player on game start?

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        playerLivesRemaining = playerTotalLives;
        youWinText.enabled = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        CheckCollisions();
    }

    private void UpdatePosition()
    {
        Vector2 playerPos = transform.position;

        if (Input.GetKeyDown(KeyCode.W) && playerCanMove == true)
        {
            GetComponent<SpriteRenderer>().sprite = playerUp;

            playerPos += Vector2.up;

            if (currentY > maxY) //Will only give the player points if they have move forward passed their previous Y position (Y position resets on death and making it to a home base)
            {
                maxY = currentY;
                myGameManager.UpdatePlayerScore(10);
            }
            
            currentY++;

            PlayHopSound();
        }
        else if (Input.GetKeyDown(KeyCode.S) && playerCanMove == true)
        {
            GetComponent<SpriteRenderer>().sprite = playerDown;

            if (playerPos.y > myGameManager.levelConstraintBottom)
            {
                playerPos += Vector2.down;

                currentY--;

                PlayHopSound();
            }


        }
        else if (Input.GetKeyDown(KeyCode.A) && playerCanMove == true)
        {
            GetComponent<SpriteRenderer>().sprite = playerLeft;

            if (playerPos.x > myGameManager.levelConstraintLeft)
            {
                playerPos += Vector2.left;

                PlayHopSound();
            }

        }
        else if (Input.GetKeyDown(KeyCode.D) && playerCanMove == true)
        {
            GetComponent<SpriteRenderer>().sprite = playerRight;
            if (playerPos.x < myGameManager.levelConstraintRight)
            {
                playerPos += Vector2.right;

                PlayHopSound();
            }
        }

        transform.position = playerPos;
    }

    private void CheckCollisions()
    {
        bool isSafe = true;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("CollidableObject"); //Finds all the game objects in the scene with a certain tag and creates an array with them

        foreach (GameObject go in gameObjects)
        {
            CollidableObject collidableObject = go.GetComponent<CollidableObject>();

            if (collidableObject.IsColliding(this.gameObject))
            {
                if(collidableObject.isSafe)
                {
                    isSafe = true;

                    if (collidableObject.isSafeObject)
                    {
                        Vector2 playerPos = transform.position;

                        if (collidableObject.GetComponent<SafeObject>().moveRight)
                        {
                            playerPos.x += collidableObject.GetComponent<SafeObject>().moveSpeed * Time.deltaTime;

                            if (transform.position.x >= 9.5)
                            {
                                playerPos.x = transform.position.x - 18f;
                            }
                        }
                        else
                        {
                            playerPos.x -= collidableObject.GetComponent<SafeObject>().moveSpeed * Time.deltaTime;

                            if (transform.position.x <= -9.5)
                            {
                                playerPos.x = transform.position.x + 18f;
                            }
                        }

                        transform.position = playerPos;

                    } 

                    if (collidableObject.isHomeBase)
                    {
                        if (!collidableObject.hasTrophy)
                        {

                            collidableObject.hasTrophy = true;

                            PlayHomeBaseSound();

                            collidableObject.tag = "trophy";

                            myGameManager.UpdatePlayerScore(50);

                            collidableObject.GetComponent<SpriteRenderer>().sprite = collidableObject.trophyBase;

                            frogsAtHome++;

                            ResetPosition();
                        }

                        PlayHomeBaseSound();
                        ResetPosition();
                    }

                    
                    break;
                }
                else
                {
                    isSafe = false;
                }
            }
        }

        if (!isSafe)
        {
            if (playerLivesRemaining == 0)
            {
                GameOver();
            } 
            else
            {
                OnPlayerDeath();
            }
        }
    }


    public void OnPlayerDeath ()
    {
        PlaySquashedSound();
        LoseLife();
        ResetPosition();
    }

    public void ResetPosition ()
    {
        transform.position = startPos;
        
        transform.GetComponent<SpriteRenderer>().sprite = playerUp;
        
        maxY = 0;
        
        currentY = 1;
    }

    public void LoseLife()
    {
        playerLivesRemaining -= 1;
    }

    public void GameOver()
    {

        myGameManager.GameReset();
        
        playerLivesRemaining = playerTotalLives;

        ResetHomeBase();

        ResetPosition();

    }

    public void ResetHomeBase()
    {
        GameObject[] trophies;
        trophies = GameObject.FindGameObjectsWithTag("trophy");

        foreach (GameObject trophy in trophies)
        {
            CollidableObject trophyObject = trophy.GetComponent<CollidableObject>();

            trophyObject.GetComponent<SpriteRenderer>().sprite = trophyObject.homeBase;
        }
    }

    public void PlayerWins()
    {
        if (frogsAtHome == 5)
        {
            youWinText.enabled = true;
            
            myGameManager.isGameRunning = false;

            playerCanMove = false;
            
            int leftOverTime = (int)(myGameManager.totalGameTime - myGameManager.gameTimeRemaining);

            myGameManager.UpdatePlayerScore(leftOverTime * 20);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                myGameManager.GameReset();
            }
        }
    }

    public void PlayHopSound()
    {
        AudioSource.PlayClipAtPoint(hopSound, transform.position);
    }

    public void PlaySquashedSound()
    {
        AudioSource.PlayClipAtPoint(squashSound, transform.position);
    }

    public void PlayHomeBaseSound()
    {
        AudioSource.PlayClipAtPoint(homeBaseSound, transform.position);
    }
}
