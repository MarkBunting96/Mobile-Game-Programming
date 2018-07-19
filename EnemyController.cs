//Title	 		: EnemyController.cs
//Author 		: M.K.Bunting
//Date	 		: 25/03/2017
//Last Modified	: 25/03/2017

//Purpose: Controls the collision and the game management of the individual enemies

//Default Unity includes:
using UnityEngine;
using System.Collections;

//My includes:
using UnityEngine.SceneManagement;              //Scene manager required for scene loading.
                            
//public class enemy controller which contains a start function, load by index function and an 
//on trigger 2d function.
public class EnemyController : MonoBehaviour
{

    public GameObject explosion;                //explosion gameobject to be instantiated
    private Transform collisionPosition;        //position of where the collision took place

	//Start function which is called when the gameobject is enabled
	void Start ()
    {
        //Adds the enemy to the list of enemies stored in game manager
        GameManager.instance.AddEnemyToList(this);
	}

    //Public function that loads a scene using a scene index
    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    //on trigger enter 2d function which runs when collision occus
    void OnTriggerEnter2D(Collider2D other)
    {
        collisionPosition = other.transform;        //collision position is set to the other objects transform

        //if statement which checks if the other object was a shield, creates an explosion, deactivates the shields then removes
        //them from the gamemanager
        if (other.gameObject.CompareTag("Shield"))
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            other.gameObject.SetActive(false);
            Destroy(other);
            GameManager.instance.RemoveShields();
        }
        //else if statement which checks if the other object was a player, creates an explosion, deactivates the player and
        //loads scene 2 (gameover scene)
        else if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            other.gameObject.SetActive(false);
            Destroy(other);
            LoadByIndex(2);
        }
        //else if statement which checks if the other object is a laser, creates an explosion, deactivates both objects, 
        //removes enemy from the list in game manager, and updates the score in game manager
		else if(other.gameObject.CompareTag("Laser"))
		{
			Instantiate(explosion,collisionPosition.position, collisionPosition.rotation);
            this.gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            Destroy(this);
            Destroy(other);
            GameManager.instance.RemoveEnemy();
            GameManager.instance.addScore();
        }
    }
}
