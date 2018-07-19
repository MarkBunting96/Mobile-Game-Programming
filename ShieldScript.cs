//Title : ShieldScript.cs
//Author : L.A.Steed & M.K.Bunting
//Date : 27/03/2017
//Last Modified : 27/03/2017

//Purpose: Contains the shields health, detects collision and deactivates the shield and explodes when health is depleted


//Default Unity Includes:
using UnityEngine;
using System.Collections;

//My Includes:
// N/A


//public class that contains start, fixed update and ontriggerenter functions
public class ShieldScript : MonoBehaviour
{

    public GameObject explosion;                        //explosion game object, instantiated when the shield's health has been depleted
    private Transform collisionPosition;                //private transform, used for the explosion position
    private float shieldHealth;                         //private float containing the shield health

    //A start functions which runs when the shield is enabled, it sets the shield health to 3, and adds itself to the list of shields
    //in the game manager
    void Start()
    {
        shieldHealth = 3;
        GameManager.instance.AddShieldToList(this);

    }

    //Fixed Update which controls the destruction of the shield 
    void FixedUpdate()
    {   
        //if statement that checks if the shield health has been depleted, deactivates the shield, sets the collision position 
        //and instantiates the explosion using this position
        if (shieldHealth <= 0)
        {
            this.gameObject.SetActive(false);
            collisionPosition = transform;
            Instantiate(explosion, collisionPosition.position, collisionPosition.rotation);
        }

        //if statement that checks if the number of enemies (stored in game manager) has been depleted then deactivates the shield
        //(used to respawn the shields after a wave is complete)
        if (GameManager.instance.GetNumEnemies() <= 0)
        {   
            this.gameObject.SetActive(false);
        }

    }

    //ontriggerenter2d function which runs when the collider has collided with another collider
    void OnTriggerEnter2D(Collider2D other)
    {
        //if statement that checks if the other object colliding with this is a laser and decrements the health
        if (other.gameObject.CompareTag("Laser") || other.gameObject.CompareTag("EnemyLaser"))
        {
            shieldHealth--;
            //if statement which checks if the shield health has been depleted, deactivates the shield, sets the collision position,
            //instantiates the explosion using this position, then removes a shield from the game manager
            if (shieldHealth <= 0)
            {
                this.gameObject.SetActive(false);
                collisionPosition = transform;
                Instantiate(explosion, collisionPosition.position, collisionPosition.rotation);
                GameManager.instance.RemoveShields();
            }
        }

    }
}
