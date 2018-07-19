//Title : PlayerController.cs
//Author : L.A.Steed & M.K.Bunting
//Date : 25/03/2017
//Last Modified : 25/03/2017

//Purpose: Controls the player within the game. Reads input to move the player/make
//the player shoot. 


//Default Unity Includes:
using UnityEngine;
using System.Collections;

//My Includes:
// N/A


//Public class containing the start, update and fixed update functions.
public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb2d;               //The player's RigidBody2D component
        
	public float fireRate;                  //The rate of which the player can fire

	private float nextShot;                 //When the player can fire next

	public GameObject laser;                //A laser game object, instantiated when the player fires
	public Transform shotPos1;              //A transform that aligns with the left gun of the ship, as the ship fires 2 lasers at a time
	public Transform shotPos2;              //A transform on the right side
    public float speed;                     //Speed of the player's movement

	//A start functions which runs when the player is enabled, it gets the rigidbody2d component of the ship and stores it in rb2d
	void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    //Update function, called once per frame, used to track when the player fires and controls fire rate delay
	void Update()
	{
        //if statement that checks if the local game manager is in the paused state, freezes the update function if paused
        if (!GameManager.instance.paused)
        {
			Touch mTouch = Input.touches[0];

			if(mTouch.phase == TouchPhase.Began && Time.time > nextShot)
			{
				//Sets the next shot time using the fire rate variable
				nextShot = Time.time + fireRate;
				//Instantiates two lasers at the left and right shot positions
				Instantiate(laser, shotPos1.position, shotPos1.rotation);
				Instantiate(laser, shotPos2.position, shotPos2.rotation);
			}

            //if statement to check if the player has pressed the fire button and enough time has passed for them to take another shot
			/*if (Input.GetButton("Fire1") && Time.time > nextShot)
            {
                //Sets the next shot time using the fire rate variable
                nextShot = Time.time + fireRate;
                //Instantiates two lasers at the left and right shot positions
                Instantiate(laser, shotPos1.position, shotPos1.rotation);
                Instantiate(laser, shotPos2.position, shotPos2.rotation);
            }
            */
        }
	}


	//Fixed update function for player movement, as it is consistent
	void FixedUpdate ()
    {
        //if statement that checks if the local game manager is in the paused state, freezes the update function if paused
        if (!GameManager.instance.paused)
        {

            //TESTING ACCELOROMETER
			if(Input.acceleration.x > 0 && rb2d.transform.position.x <= 15.5)
            {
                rb2d.MovePosition(new Vector2(rb2d.position.x + speed * Time.deltaTime, rb2d.position.y));
            }

			if(Input.acceleration.x < 0 && rb2d.transform.position.x >= -15.5)
            {
                rb2d.MovePosition(new Vector2(rb2d.position.x - speed * Time.deltaTime, rb2d.position.y));
            }

            //if statement to check if the right key has been pressed (horizontal axis > 0) and if the player is at the edge of the screen
            if (Input.GetAxisRaw("Horizontal") > 0 && rb2d.transform.position.x <= 15.5)
            {
                //Moves the rb2d component right using the speed variable and time
                rb2d.MovePosition(new Vector2(rb2d.position.x + speed * Time.deltaTime, rb2d.position.y));
            }
            //if statement to check if the left key has been pressed (horizontal axis < 0) and if the player is at the edge of the screen
            if (Input.GetAxisRaw("Horizontal") < 0 && rb2d.transform.position.x >= -15.5)
            {
                //Moves the rb2d component left using the speed variable and time
                rb2d.MovePosition(new Vector2(rb2d.position.x - speed * Time.deltaTime, rb2d.position.y));
            }
        }
    }
}
