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
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public GameObject gameOverScreen;

    public GameObject victoryScreen;
    public TextMeshProUGUI livesScreen;

    public int playerLives = 3;
    private Boolean gameOver = false;
    private Boolean stageClear = false;

    private void Start()
    {
        livesScreen.SetText("Lives : "+playerLives.ToString());
        gameOverScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }

    public void OnBallDeath(){
        
        if(playerLives <= 0){
            gameOver = true;
            ShowGameOverScreen();
        }else{
            playerLives --;
            livesScreen.SetText("Lives : "+playerLives.ToString());
        }
    }

    public void OnBrickEmpty(){
        if(BricksManager.Instance.RemainingBricks.Count <= 0){
            stageClear = true;
            ShowVictoryScreen();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Boolean IsGameOver(){
        return gameOver;
    }

    public Boolean IsStageClear(){
        return stageClear;
    }

    internal void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    internal void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

}