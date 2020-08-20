using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
    {
    
    //Following code is from The Weekly Coder youtube channel, was unable to get OnCollisionEnter to do what I wanted for hopping on the logs
    //and this piece of code seemed to be a good alternative without destroying the code I had already put in place
    
    Rect playerRect;
    Vector2 playerSize; //Size of the player sprite
    Vector2 playerPosition; //Position of the player sprite

    Rect collidableObjectRect;
    Vector2 collidableObjectSize; //Size of the collidable object
    Vector2 collidableObjectPosition; //Position of the collidable object

    public bool isSafe;
    
    public bool isSafeObject;
    
    public bool isHomeBase;
    public bool hasTrophy = false;
    public Sprite trophyBase;
    
    public bool IsColliding (GameObject playerGameObject)
    {
        playerSize = playerGameObject.transform.GetComponent<SpriteRenderer>().size; //Accesses player sprite to assign size collider
        playerPosition = playerGameObject.transform.position; //Assigns the collider to the position of the player

        collidableObjectSize = GetComponent<SpriteRenderer>().size; //The size of the sprite the script is attached too
        collidableObjectPosition = transform.position; //The position of the collidable object

        playerRect = new Rect(playerPosition.x - playerSize.x / 2, playerPosition.y - playerSize.y / 2, playerSize.x, playerSize.y); //Creates the rect which acts as the collider for the player
        collidableObjectRect = new Rect(collidableObjectPosition.x - collidableObjectSize.x / 2, collidableObjectPosition.y - collidableObjectSize.y / 2, collidableObjectSize.x, collidableObjectSize.y); // Creates the rect which act as the collider for the collidable object this is attached too

        if (collidableObjectRect.Overlaps(playerRect, true)) //Checks to see if the collider of this object and the player are overlapping
        {
            return true;
        }

        return false;
    }
}
