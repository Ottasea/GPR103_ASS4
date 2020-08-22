using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeObject : MonoBehaviour
{
    //The following code used the idea from the Vehicle script but also incorporates ideas from the Weekly Coder to help with the seamless transition when the log enters / leaves the screen

    public float moveSpeed = 5.0f; // The speed at which the log travels
    public bool moveRight = true; //Is the log moving towards the right?

    private readonly float playAreaWidth = 19.0f;

    // Update is called once per frame
    void Update()
    {
        MoveSafeObject(); //Has the safe object reached the determined width? If so, move it back to its start position
    }
    
    void MoveSafeObject()
    {
        Vector2 pos = transform.localPosition;

        if (moveRight)
        {
            pos.x += moveSpeed * Time.deltaTime;

            if (pos.x >= ((playAreaWidth / 2) - 1) + (playAreaWidth - 1) - GetComponent<SpriteRenderer>().size.x / 2) //if the log is moving right and has reached the determined space off screen then swap it back to its starting position
            {
                pos.x = -playAreaWidth / 2 - GetComponent<SpriteRenderer>().size.x / 2;
            }
        }
        else
        {
            pos.x -= moveSpeed * Time.deltaTime;

            if (pos.x <= ((-playAreaWidth / 2) + 1) - (playAreaWidth - 1) + GetComponent<SpriteRenderer>().size.x / 2)//if the log is moving left and has reached the determined space off screen then swap it back to its starting position
            {
                pos.x = playAreaWidth / 2 + GetComponent<SpriteRenderer>().size.x / 2;
            }
        }

        transform.localPosition = pos;
    }
    
}

