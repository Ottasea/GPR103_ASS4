using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    /*Following code is from The Weekly Coder youtube channel, where I also originally found the assets to use. I liked that it provided
     a nice seamless looping of the logs as opposed to what I had set up for the vehicles (which also worked for this) */

    public float moveSpeed = 5.0f; // The speed at which the log travels
    public bool moveRight = true; //Is the log moving towards the right?

    private readonly float playAreaWidth = 19.0f;

    // Update is called once per frame
    void Update()
    {
        MoveLog(); //Has the log reached the determined width? If so, move it back to its start position
    }
    
    void MoveLog()
    {
        Vector2 pos = transform.localPosition;

        if (moveRight)
        {
            pos.x += moveSpeed * Time.deltaTime;

            if (pos.x >= ((playAreaWidth / 2) - 1) + (playAreaWidth - 1) - GetComponent<SpriteRenderer>().size.x / 2)
            {
                pos.x = -playAreaWidth / 2 - GetComponent<SpriteRenderer>().size.x / 2;
            }
        }
        else
        {
            pos.x -= moveSpeed * Time.deltaTime;

            if (pos.x <= ((-playAreaWidth / 2) + 1) - (playAreaWidth - 1) + GetComponent<SpriteRenderer>().size.x / 2)
            {
                pos.x = playAreaWidth / 2 + GetComponent<SpriteRenderer>().size.x / 2;
            }
        }

        transform.localPosition = pos;
    }
    
    }

