//Title : Shot.cs
//Author : L.A.Steed 
//Date : 28/03/2017
//Last Modified : 28/03/2017

//Purpose: Used to control the motion of the laser after it has been shot, used for both player, and enemy lasers.


//Default Unity Includes:
using UnityEngine;
using System.Collections;

//My Includes:
using UnityEngine.SceneManagement;          //Scene manager required for scene loading.

//public class which contains the start, update, loadbyindex, and ontriggerenter2d functions
public class shot : MonoBehaviour 
{
	public Vector3 m_vel;                   //Vector3 containing the velocity of the laser
	public float speed;                     //float containing the speed of the laser

    public GameObject explosion;            //gameobject that contains the explosion prefab
    public Transform collisionPosition;     //transform that will store the collision position

    //Start function that sets the rotation of the laser
    void Start () 
	{
        //if statement that checks if the object type is a laser then sets the rotation
        if(this.GetType().ToString() == "Laser")
        {
            transform.localRotation.Set(0, 0, 90, 0);
        }

	}
	
	//Update function that controls the movement of the laser
	void Update () 
	{
        //if statement that checks if the local game manager is in the paused state, freezes the update function if paused
        if (!GameManager.instance.paused)
        {
            //Moves the laser forward using the velocity and the speed variables
            transform.position += m_vel * speed;
            //if statement to check if the laser has gone off screen and deactivates it
            if (transform.position.y > 80 || transform.position.y < -10)
            {
                this.gameObject.SetActive(false);
                Destroy(this);
            }
        }
	}

    //Public function that loads a scene using a scene index
    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    //ontriggerenter2d function which runs when the collider has collided with another collider
    void OnTriggerEnter2D(Collider2D other)
    {
        //sets the collision position using the other objects transform
        collisionPosition = other.transform;

        //if statement that checks if the other object is a shield, instantiates an explosion then deactivates the laser.
        if (other.gameObject.CompareTag("Shield"))
        {
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            this.gameObject.SetActive(false);
            Destroy(this);

        }
        //else if statement that checks if if the other object is a player and that itself is an enemy laser,
        //instantiates an explosion, deactivates both objects and loads the scene with the index of 2 (game over scene).
        else if (other.gameObject.CompareTag("Player") && this.gameObject.CompareTag("EnemyLaser"))
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            this.gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            Destroy(this);
            Destroy(other);
            LoadByIndex(2);
        }
        //else if statement that checks if there is a collision between two lasers, instantiates 2 explosions, and
        //deactivates both objects.
        else if (other.gameObject.CompareTag("Laser") || other.gameObject.CompareTag("EnemyLaser"))
        {
            Instantiate(explosion, collisionPosition.position, collisionPosition.rotation);
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            this.gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            Destroy(this);
            Destroy(other);
        }
    }
}
