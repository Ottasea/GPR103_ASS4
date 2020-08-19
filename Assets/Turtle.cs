using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    public enum CurrentTurtle //Allows us to set up two different types of turtles to be selected from
    {
        CurrentTurtleFloating,
        CurrentTurtleDiving
    };

    public CurrentTurtle turtleType = CurrentTurtle.CurrentTurtleFloating; //Variable to determine which turtle is currently present on screen

    public Sprite diveSprite; //Sprite for diving turtle
    public Sprite floatSprite; //Sprite for floating turtle

    // Update is called once per frame
    void Update()
    {
        
    }

}
