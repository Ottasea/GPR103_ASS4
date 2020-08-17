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
    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        myGameManager = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector2 playerPos = transform.position;

        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<SpriteRenderer>().sprite = playerUp;
            
            playerPos += Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            GetComponent<SpriteRenderer>().sprite = playerDown;
            
            if (playerPos.y > myGameManager.levelConstraintBottom)
            {
                playerPos += Vector2.down;
            }

                
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().sprite = playerLeft;

            if (playerPos.x > myGameManager.levelConstraintLeft)
            {
                playerPos += Vector2.left;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().sprite = playerRight;
            if (playerPos.x < myGameManager.levelConstraintRight)
            {
                playerPos += Vector2.right;
            }
        }

        transform.position = playerPos;
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "safe")
        {
            Debug.Log("SAFE");
        }

        else if (other.gameObject.tag == "unsafe")
        {
            Debug.Log("UNSAFE");
            transform.localPosition = startPosition;
            transform.GetComponent<SpriteRenderer>().sprite = playerUp;
        }
    }

}
