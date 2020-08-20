using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script must be utlised as the core component on the 'vehicle' obstacle in the frogger game.
/// </summary>
public class Vehicle : MonoBehaviour
{
    public int moveDirection = 0; //This variable is to be used to indicate the direction the vehicle is moving in.
    public float speed; //This variable is to be used to control the speed of the vehicle.
    public Vector2 startingPosition; //This variable is to be used to indicate where on the map the vehicle starts (or spawns)
    public Vector2 endPosition; //This variablle is to be used to indicate the final destination of the vehicle.
    public bool paused = true;
    public float runTime;
    float currentRunTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = startingPosition;
    }

    void Update()
    {
        currentRunTime += Time.deltaTime;
        if (currentRunTime >= runTime)
        {
            paused = false;
            transform.Translate(Vector2.right * Time.deltaTime * speed * moveDirection);
        }
        
        if ((transform.position.x * moveDirection) > (endPosition.x * moveDirection))
        {
            transform.position = startingPosition;
        }

    }
    

}

