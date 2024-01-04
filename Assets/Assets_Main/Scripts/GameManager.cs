using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    //Creating a singleton
    public static GameManager Instance { get; private set; }

    [Header ("References")]
    public TMP_Text lossText;
    public TMP_Text currentScoreText;
    public TMP_Text highestScoreText;
    public GameObject draftChoices;
    public PlayerController playerController;
    public GameObject projectilePrefab;
    public GameObject collectiblePrefab;

    [Header("Art")]
    public GameObject fx_enemyDeathPrefab;

    [Header ("Game Design Player")]
    public float playAgainTime; //after losing, reset the scene after X seconds
    

    [Header("Collectibles")]
    public float levelUpTime; //level up every X seconds
    public int XpSmallCollectibleAmount;//Amount of xp provided by small xp collectibles
    public int XpBigCollectibleAmount;//Amount of xp provided by big xp collectibles
    public float XpSmallOdds;//Amount of xp provided by small xp collectibles
    public float XpBigOdds;//Amount of xp provided by small xp collectibles
    public float startingSpeed;//Initial speed of collectible
    public float scalingSpeed;//Scaling speed

    [Header("Game Design Enemy")]
    public float enemyScalingRate; //increase the scaling every X seconds

    [Header("To Save")]
    public int currentScore;

    // Start is called before the first frame update
    void Awake()
    {
        //Static instantiation
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //set values
        highestScoreText.text = "Highest Score: " + StaticData.Instance.highestScore;

        //Disable UI elements
        lossText.enabled = false;
        HideChoices();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerLost()
    {
        //Display a text
        lossText.enabled = true;

        //Resets the scene within seconds
        Invoke("ResetScene", playAgainTime);
    }

    void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        //Set values
        currentScore = 0;

        ResumeGame();
    }

    public void OnLevelUp()
    {
        PauseGame();
        DisplayChoices();
        GetComponentInChildren<EnemySpawner>().IncreaseEnemySpawnRate(enemyScalingRate);
    }

    public void PlayerMakesChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                playerController.IncreaseFireRate();
                break;
            case 2:
                playerController.IncreaseProjectileNumber();
                break;
            case 3:
                playerController.IncreaseProjectilePiercing();
                break;
            default:
                break;
        }
        playerController.RefreshUI();
        HideChoices();

        ResumeGame();
        playerController.AfterLevelUp();
    }
    private void HideChoices()
    {
        draftChoices.SetActive(false);
    }

    private void DisplayChoices()
    {
        draftChoices.SetActive(true);
    }

    public void AddScore(int score)
    {
        //recalcule le score
        currentScore += score;

        //reaffiche le score
        currentScoreText.text = "Score: "+currentScore;

        if (StaticData.Instance.highestScore < currentScore)
        {
            StaticData.Instance.highestScore = currentScore;
        }
    }



    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

}
