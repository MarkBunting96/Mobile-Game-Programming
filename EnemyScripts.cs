//Title	 		: EnemyScripts.cs
//Author 		: M.K.Bunting
//Date	 		: 25/03/2017
//Last Modified	: 25/03/2017

//Purpose: Controls the movement of the entire enemy grid

//Default Unity includes:
using UnityEngine;
using System.Collections;

//My includes:
using UnityEngine.SceneManagement;  //Scene manager required for
                                    //scene loading.

//public enemyscripts class which contains start, loadbyindex, and fixed update functions
public class EnemyScripts : MonoBehaviour
{
    public GameObject row1;         //Gameobjects that store the 4 rows of enemies
    public GameObject row2;
    public GameObject row3;
    public GameObject row4;

    private float counter;          //a counter which is used to delay the movement of the enemies

    private bool moveLeft;          //booleans which decide wheather the enemies are going to move left, right or down
    private bool moveRight;
    private bool moveDown;

    //Start function which is called when the game object is enabled
    void Start ()
    {
        counter = 0;                //counter is set to 0.
        moveLeft = true;            //move left is set to true
        moveRight = false;          //move right is set to false
    }

    //Public function that loads a scene using a scene index
    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    //Fixed update function which controls the movement of the enemy grid
    void FixedUpdate ()
    {
        //if statement that checks if the local game manager is in the paused state, freezes the update function if paused
        if (!GameManager.instance.paused)
        { 
            //if statement that checks if the number of enemies in game manager is less than or equal to 0, then 
            //destroys the enemy grid (prevents empty grids from triggering off screen gameover).
            if (GameManager.instance.GetNumEnemies() <= 0)
            {
                this.gameObject.SetActive(false);
                Destroy(this);
            }

            //counter which increases by 1 * by the game speed set in the game manager
            counter += 1 * GameManager.instance.gameSpeed;

            //if statement that checks if the grid has went off screen, deactivates the grid tne loads scene 2 (game over)
            if (transform.position.y <= -15)
            {
                this.gameObject.SetActive(false);
                Destroy(this);
                LoadByIndex(2);
            }

            //if statement that checks if the counter has gone above 50
            if (counter >= 50)
            {
                //reset the counter to 0
                counter = 0;

                //if statement to check if the grid has reached the edge of the screen, moves the grid back on screen,
                //sets move left to false, move right to true and move down to true
                if (transform.position.x < -4)
                {
                    transform.position = new Vector2(-4, transform.position.y);

                    moveLeft = false;
                    moveRight = true;
                    moveDown = true;
                }

                //if statement to check if the grid has reached the edge of the screen, moves the grid back on screen,
                //sets move left to true, move right to false and move down to true
                if (transform.position.x > 4)
                {
                    transform.position = new Vector2(4, transform.position.y);

                    moveLeft = true;
                    moveRight = false;
                    moveDown = true;
                }

                //if statement that checks if the player is moving left and not down, then moves the player left
                if (moveLeft && !moveDown)
                {
                    transform.position = new Vector2(transform.position.x - (float)0.5, transform.position.y);
                }
                //else if statement that checks if the player is moving right and now down, then moves the player right
                else if (moveRight && !moveDown)
                {
                    transform.position = new Vector2(transform.position.x + (float)0.5, transform.position.y);
                }
                //else if statement that checks if the player is moving down then moves the player down
                else if (moveDown)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - (float)1);
                    moveDown = false;
                }
            }
        }      	
	}
}
