//Title	 		: LoadSceneOnClick.cs
//Author 		: L.A.Steed
//Date	 		: 25/03/2017
//Last Modified	: 25/03/2017

//Purpose: Controls which enemy fires using random numbers

//Default Unity includes:
using UnityEngine;
using System.Collections;

//public class which contains start and fixed update
public class RowScripts : MonoBehaviour
{
    public GameObject enemy1;               //all 8 enemies in that row, assigned in the inspector
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy5;
    public GameObject enemy6;
    public GameObject enemy7;
    public GameObject enemy8;

    public GameObject enemyLaser;           //enemy laser to instantiate

    public float fireRate;                  //enemies rate of fire

    private float nextShot;                 //when the enemy can next shoot

    public bool frontRow;                   //bool to see if it is the front row or not
    private float toFire;                   //float for which enemy is to fire

    // Use this for initialization
    void Start ()
    {
        frontRow = true;                    //frontrow is set to true
	}

    //Fixed update used for enemy firing
    void FixedUpdate()
    {
        //if statement that checks if the local game manager is in the paused state, freezes the update function if paused
        if (!GameManager.instance.paused)
        {

            toFire = Random.value;          //toFire is assigned a random number between 0-10 (0.0, 1.0, but mult by 10)
            toFire *= 10;

            //if statement to check if it is the front row
            if (frontRow)
            {
                //if statement to check if next shot can be taken, then next shot is assigned using fire rate, and a switch
                //statement is used to pick an enemy out of the rows to shoot.
                if (Time.time > nextShot)
                {
                    nextShot = Time.time + fireRate;
                    switch ((int)toFire)
                    {
                        case 0:
                            if (enemy1.activeSelf)
                                Instantiate(enemyLaser, enemy1.transform.position, enemy1.transform.rotation);
                            break;
                        case 1:
                            if (enemy2.activeSelf)
                                Instantiate(enemyLaser, enemy2.transform.position, enemy2.transform.rotation);
                            break;
                        case 2:
                            if (enemy3.activeSelf)
                                Instantiate(enemyLaser, enemy3.transform.position, enemy3.transform.rotation);
                            break;
                        case 3:
                            if (enemy4.activeSelf)
                                Instantiate(enemyLaser, enemy4.transform.position, enemy4.transform.rotation);
                            break;
                        case 4:
                            if (enemy5.activeSelf)
                                Instantiate(enemyLaser, enemy5.transform.position, enemy5.transform.rotation);
                            break;
                        case 5:
                            if (enemy6.activeSelf)
                                Instantiate(enemyLaser, enemy6.transform.position, enemy6.transform.rotation);
                            break;
                        case 6:
                            if (enemy7.activeSelf)
                                Instantiate(enemyLaser, enemy7.transform.position, enemy7.transform.rotation);
                            break;
                        case 7:
                            if (enemy8.activeSelf)
                                Instantiate(enemyLaser, enemy8.transform.position, enemy8.transform.rotation);
                            break;

                    }
                }
            }
        }
	}
}
