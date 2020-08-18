using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

/// <summary>
/// This script must be used as the core Player script for managing the player character in the game.
/// </summary>
public class Player : MonoBehaviour
{
    public string playerName = ""; //The players name for the purpose of storing the high score

    public int playerTotalLives; //Players total possible lives.
    public int playerLivesRemaining; //Players actual lives remaining.
    public bool playerIsAlive = true; //Is the player currently alive?
    public bool playerCanMove = false; //Can the player currently move?
    public Sprite playerUp, playerDown, playerLeft, playerRight; //Select how the player looks based on the direction its travelling.

    private GameManager myGameManager; //A reference to the GameManager in the scene.
    public GameObject gameManager; //Allows you to determine the GameManager that oversees the Player
    private Vector2 startPos; //Where is the player on game start?

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        myGameManager = gameManager.GetComponent<GameManager>();
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
        }
        else if (Input.GetKeyDown(KeyCode.S) && playerCanMove == true)
        {
            GetComponent<SpriteRenderer>().sprite = playerDown;

            if (playerPos.y > myGameManager.levelConstraintBottom)
            {
                playerPos += Vector2.down;
            }


        }
        else if (Input.GetKeyDown(KeyCode.A) && playerCanMove == true)
        {
            GetComponent<SpriteRenderer>().sprite = playerLeft;

            if (playerPos.x > myGameManager.levelConstraintLeft)
            {
                playerPos += Vector2.left;
            }

        }
        else if (Input.GetKeyDown(KeyCode.D) && playerCanMove == true)
        {
            GetComponent<SpriteRenderer>().sprite = playerRight;
            if (playerPos.x < myGameManager.levelConstraintRight)
            {
                playerPos += Vector2.right;
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

                    Debug.Log("SAFE");

                    if (collidableObject.isLog)
                    {
                        Vector2 playerPos = transform.position;

                        if (collidableObject.GetComponent<Log>().moveRight)
                        {
                            playerPos.x += collidableObject.GetComponent<Log>().moveSpeed * Time.deltaTime;

                            if (transform.position.x >= 9.5)
                            {
                                playerPos.x = transform.position.x - 18f;
                            }
                        }
                        else
                        {
                            playerPos.x -= collidableObject.GetComponent<Log>().moveSpeed * Time.deltaTime;

                            if (transform.position.x <= -9.5)
                            {
                                playerPos.x = transform.position.x + 18f;
                            }
                        }

                        transform.position = playerPos;

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
            transform.position = startPos;
            transform.GetComponent<SpriteRenderer>().sprite = playerUp;
        }



    }
}
