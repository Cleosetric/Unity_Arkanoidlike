using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
       if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #endregion

    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public GameObject livesScreen;
    public GameObject scoreScreen;

    public int playerLives = 3;
    private int levelScore = 0;
    private TextMeshProUGUI livesText;
    private TextMeshProUGUI scoreText;
    private Boolean gameOver = false;
    private Boolean stageClear = false;

    private void Start()
    {
        AudioManager.instance.Play("Theme");
        CheckEmptyUI();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("On load Check!");
        CheckEmptyUI();
    }

    void CheckEmptyUI(){
        if(!livesScreen){
            livesScreen = GameObject.Find("Lives");
            livesText = livesScreen.GetComponent<TextMeshProUGUI>();
            livesText.SetText(playerLives.ToString());

        }
        
        if(!scoreScreen){
            scoreScreen = GameObject.Find("Score");
            scoreText = scoreScreen.GetComponent<TextMeshProUGUI>();
            scoreText.SetText(levelScore.ToString());
        }
        
        if(!gameOverScreen){
            gameOverScreen = GameObject.Find("GameOver");
            gameOverScreen.SetActive(false);
        }
        
        if(!victoryScreen){
            victoryScreen = GameObject.Find("Victory");
            victoryScreen.SetActive(false);
        }

        stageClear = false;
        gameOver = false;
    }

    public void OnLifeBuff(int life){
        playerLives += life;
        livesText.SetText(playerLives.ToString());
    }

    public void OnBallDeath(){
        
        if(playerLives <= 0){
            gameOver = true;
            ShowGameOverScreen();
        }else{
            playerLives --;
            livesText.SetText(playerLives.ToString());
        }
    }

    public void OnBrickDestroy(){
        scoreText.SetText(levelScore.ToString());
        if(BricksManager.Instance.RemainingBricks.Count <= 0){
            stageClear = true;
            ShowVictoryScreen();
        }
    }

    public void SetLives(int live){
        playerLives += live;
    }

    public void AddScore(int score){
        levelScore += score;
    }

    public void RestartGame()
    {
        playerLives = 3;
        levelScore = 0;
        SceneManager.LoadScene(0);
    }

    public void NextStage(){
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
        SceneManager.LoadScene(nextSceneIndex);
        }
    }


    public Boolean IsGameOver(){
        return gameOver;
    }

    public Boolean IsStageClear(){
        return stageClear;
    }

    internal void ShowGameOverScreen()
    {
        AudioManager.instance.Play("GameOver");
        gameOverScreen.SetActive(true);
    }

    internal void ShowVictoryScreen()
    {
        AudioManager.instance.Play("StageClear");
        victoryScreen.SetActive(true);
    }

}