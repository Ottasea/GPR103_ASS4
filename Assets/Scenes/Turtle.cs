using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    /*Idea for script to implement diving mechanic came from the way I spawned the traffic with a wait timer between initially moving, 
     * tweaks were helped by the Walking Code tutorial due to me having already used their collision script (wanted to keep watching 
     * just incase I needed to handle something that was specific to their code.) */
    
    public enum CurrentTurtle //Allows us to set up two different types of turtles to be selected from
    {
        CurrentTurtleFloating,
        CurrentTurtleDiving
    };

    public CurrentTurtle turtleType = CurrentTurtle.CurrentTurtleFloating; //Variable to determine which turtle is currently present on screen

    public Sprite diveSprite; //Sprite for diving turtle
    public Sprite floatSprite; //Sprite for floating turtle

    public bool diving = false, surfacing = true, dived = false, surfaced = false;
    private float surfaceTime = 5.0f;
    private float diveTime = 5.0f;
    private float surfaceTimer;
    private float diveTimer;
    private float transitionTime = 5.0f;
    private float transitionTimer;

    // Update is called once per frame
    void Update()
    {
        DiveMechanic();
    }

    void DiveMechanic ()
    {
        if (turtleType == CurrentTurtle.CurrentTurtleDiving)
        {
            if (surfacing == true)
            {
                transitionTimer += Time.deltaTime;

                if (transitionTimer >= transitionTime)
                {
                    surfacing = false;
                    transitionTimer = 0;
                    surfaced = true;
                    GetComponent<SpriteRenderer>().sprite = floatSprite;
                }
            }

            if (surfaced == true)
            {
                surfaceTimer += Time.deltaTime;

                if (surfaceTimer >= surfaceTime)
                {
                    surfaced = false;
                    surfaceTimer = 0;
                    GetComponent<SpriteRenderer>().sprite = diveSprite;
                    diving = true;
                }
            }

            if (diving == true)
            {
                transitionTimer += Time.deltaTime;

                if (transitionTimer >= transitionTime)
                {
                    diving = false;
                    dived = true;
                    transitionTimer = 0;
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<CollidableObject>().isSafe = false;
                    GetComponent<CollidableObject>().isSafeObject = false;
                }
            }

            if (dived == true)
            {
                diveTimer += Time.deltaTime;

                if (diveTimer >= diveTime)
                {
                    dived = false;
                    surfacing = true;
                    diveTimer = 0;
                    GetComponent<CollidableObject>().isSafe = true;
                    GetComponent<CollidableObject>().isSafeObject = true;
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().sprite = diveSprite;
                }
            }
        }
    }
}
