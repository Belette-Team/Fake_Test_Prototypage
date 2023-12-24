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
    public TMP_Text highestScore;
    public GameObject draftChoices;
    public PlayerController playerController;

    [Header ("Game Design")]
    public float playAgainTime; //after losing, reset the scene after X seconds
    public float levelUpTime; //level up every X seconds
    public float enemyScalingRate; //increase the scaling every X seconds

    [Header("To Save")]
    public int currentScore;

    float levelUpTimeTimer; //level up every X seconds


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
        highestScore.text = "Highest Score: " + StaticData.Instance.highestScore;

        //Disable UI elements
        lossText.enabled = false;
        HideChoices();
    }

    // Update is called once per frame
    void Update()
    {
        if(levelUpTimeTimer<levelUpTime)
        {
            levelUpTimeTimer += Time.deltaTime;
        }
        else
        {
            //Level up
            OnLevelUp();
            levelUpTimeTimer = 0;
        }
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
        HideChoices();
        ResumeGame();

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
        currentScore++;

        if(StaticData.Instance.highestScore < currentScore)
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
