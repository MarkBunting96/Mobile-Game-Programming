//Title	 		: GameManager.cs
//Author 		: M.K.Bunting
//Date	 		: 02/03/2017
//Last Modified	: 02/03/2017

//Purpose: Public singleton game object used to control the entire game, loads the player, enemies, shields
//variables for delays, game speed, UI game objects, score, wave number and a large range of functions called 
//from other scripts for game management.

//Default Unity includes:
using UnityEngine;
using System.Collections;

//My includes:
using System.Collections.Generic;                           //Allows us to use Lists. 
using UnityEngine.SceneManagement;                          //Scene manager required for scene loading.
using UnityEngine.UI;                                       //Allows the use of UI objects

//public game manager class which contains an awake function, add enemy to list function, remove enemy from list function, 
//add shields to list function, remove shields function, initialise game function, load enemies function, setup finished function,
//load shields function, load player function, get num enemies function, increase game speed function, add score function, 
//enable text function, disable text function and an update function.
public class GameManager : MonoBehaviour
{
    public float waveStartDelay = (float)2;                 //A delay used to spawn the enemy grid after 2 seconds
    public float gameSpeed = 1;                             //initial game speed set to 1
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private int wave = 0;                                   //Current wave number
    private int numEnemies;                                 //number of enemies 
    private int numShields;                                 //number of shields
    private bool gameSetup = false;                         //boolean to see if the game has been set up, initialised to false
    private bool sceneLoaded = false;                       //boolean to see if a scene has been loaded, initialised to false
    private int score = 0;                                  //initial score set to 0
    private Text waveText;                                  //text which will contain the wave and the wave number                               
    private Text scoreText;                                 //text which will contain the score and score number

    //hidden in inspector so that it cannot be modified in inspector, but accessible by other functions
    [HideInInspector]                                       
    public bool paused = false;                             //boolean to see if the game is paused or not



    private List<EnemyController> enemies;                  //list of enemy controller scripts called enemies
    private List<ShieldScript> shields;                     //list of shield scripts called shields

    public GameObject enemyGrid;                            //enemy grid game object to instantiate
    public GameObject player;                               //player game object to instantiate
    public GameObject shieldGrid;                           //shield grid game object to instantiate

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    //Add enemyToList function, takes a script parameter, called when an enemy is enabled and stores 
    //the enemy on the list
    public void AddEnemyToList(EnemyController script)
    {
        enemies.Add(script);
    }

    //Removeenemy function, decrements num of enemies, and calls the increase speed function
    public void RemoveEnemy()
    {
        numEnemies--;
        increaseSpeed();
    }

    //Add shield to list function, takes a script parameter, called when a shield is enembled and stores
    //the shield on the list
    public void AddShieldToList(ShieldScript script)
    {
        shields.Add(script);
    }

    //Removeshields function, called when a shield is destroyed, decrements the num of shields
    public void RemoveShields()
    {
        numShields--;
    }

    //initialise game, called when the scene is loaded
    public void InitGame()
    {
        enemies = new List<EnemyController>();                              //initialises space for the enemy list
        shields = new List<ShieldScript>();                                 //initialises space for shield list
            
        numEnemies = 32;                                                    //num enemies initialised to 32
        numShields = 3;                                                     //num shields initialised to 3

        score = 0;                                                          //score initialised to 0

        wave = 0;                                                           //wave initialised to 0

        gameSpeed = 1;                                                      //gameSpeed initialised to 1

        gameSetup = false;                                                  //gameSetup set to false

        waveText = GameObject.Find("Wave Text").GetComponent<Text>();       //wave text assigned to wave text game object in the scene
        scoreText = GameObject.Find("Score Text").GetComponent<Text>();     //score text assigned to score text game object in the scene

        LoadEnemies();                                                      //LoadEnemies is called
        LoadPlayer();                                                       //LoadPlayer is called
    }

    //LoadEnemies function, called by init game, increases the wave number, sets game setup to false, adds the 
    //wave number to the wave text then invokes the setUpFinished function using the waveStartDelay variable
    public void LoadEnemies()
    {
        wave++;
        gameSetup = false;
        waveText.text = "Wave " + wave;
        Invoke("setUpFinished", waveStartDelay);    //Invoke will call the function after 2 seconds (waveStartDelay)
    }

    //setUpFinished function, called by load enemies, sets gamesetup to true, clears the enemy list, sets num enemies to 32
    //then instantiates the enemy grid. It then calls the load shields function
    void setUpFinished()
    {
        gameSetup = true;
        enemies.Clear();
        numEnemies = 32;
        Instantiate(enemyGrid);
        LoadShields();
    }

    //LoadShields function, called by setup finished, clears the shield lsit, sets num shields to 3 and instantiates the shield grid
    public void LoadShields()
    {
        shields.Clear();
        numShields = 3;
        Instantiate(shieldGrid);
    }

    //LoadPlayer function, called by init game, instantiates the player
    public void LoadPlayer()
    {
        Instantiate(player);
    }

    //GetNum enemies, returns numenemies
    public int GetNumEnemies()
    {
        return numEnemies;
    }

    //increase speed function, adds 0.5 to speed, called everytime an enemy is killed
    public void increaseSpeed()
    {
        gameSpeed += (float)0.5;
    }

    //addScore called everytime an enemy is killed, adds 25 to score and updates the score displayed
    public void addScore()
    {
        score += 25;
        scoreText.text = "Score: " + score;
    }

    //enabletext function, sets the wave and score text as active
    public void enableText()
    {
        waveText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
    }

    //disabletext function, sets the wave and score text to disabled
    public void disableText()
    {
        waveText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    //Update is called every frame.
    void Update()
    {
        //if statement that checks if num enemies is less than 0 and that the game is setup, then sets game setup to false and calls
        //load enemies
        if (numEnemies <= 0 && gameSetup)
        {
            gameSetup = false;
            LoadEnemies();
        }
        //if statement that checks if the scene index is 1 and if the scene is unloaded, then sets scene loaded to true, paused to false 
        //and calls initialise game
        if (SceneManager.GetActiveScene().buildIndex == 1 && !sceneLoaded)
        {
            sceneLoaded = true;
            paused = false;
            InitGame();
        }
        //if statement that checks if the scene index is 0 and if the scene is loaded, then it sets sceneloaded to false
        //and the enemies, shields, numenemies, numshields, score and wave to 0/null. It also sets gamespeed to 1, game setup to false, wave
        //and score text to null and cancels the invoke called in load enemies
        if (SceneManager.GetActiveScene().buildIndex == 0 && sceneLoaded)
        {
            sceneLoaded = false;

            enemies = null;
            shields = null;

            numEnemies = 0;
            numShields = 0;

            score = 0;

            wave = 0;

            gameSpeed = 1;

            gameSetup = false;

            waveText = null;
            scoreText = null;

            CancelInvoke();
        }
        //if statement that checks if the scene index is 1 and if the q button is pressed
        if (SceneManager.GetActiveScene().buildIndex == 1 && Input.GetButtonDown ("Pause"))
        {
            //if statement that checks if the game is paused, then unpauses the game
            if(paused)
            {
                paused = false;                
            }  
            //if statement that chekcks if the game is unpaused, then pauses the game
            else if(!paused)
            {
                paused = true;               
            }
        }
        //if statement that checks if the active scene index is 2, then assigns wave and score text to the 
        //corresponding game objects in the scene, and displays the players wave and score
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            waveText = GameObject.Find("Wave Text").GetComponent<Text>();
            scoreText = GameObject.Find("Score Text").GetComponent<Text>();

            waveText.text = "Wave Reached: " + wave;
            scoreText.text = "Your Score: " + score;
        }
    }
}
