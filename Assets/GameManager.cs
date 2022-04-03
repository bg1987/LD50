using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public int ScorePerSecond = 10;
    public bool IsGameStarted { get; private set; }
    public bool IsGameOver { get; private set; }
    
    public int Score { get; private set; }

    public Text ScoreUI;

    public GameObject StartGameUI;
    public GameObject GameUI;
    public GameObject EndGameUI;
    
    public PipeGenerator generator;

    public int secondsToEnd = 120;
    public AnimationCurve difficultyCurve;


    //half assed singleton
    public static GameManager instance;

    private void OnEnable()
    {
        instance = this;
        RestartGame();
    }

    public float DifficultyModifier => Mathf.Clamp01(difficultyCurve.Evaluate((Score*1f / ScorePerSecond*1f) / secondsToEnd *1f));

    void FixedUpdate()
    {
        if (IsGameStarted)
        {
            SetScore(Score + Mathf.RoundToInt(Time.fixedDeltaTime * ScorePerSecond));
        }
    }

    #region Game Flow Control
    public void RestartGame(float delay = 0f)
    {
        IsGameStarted = false;
        IsGameOver = false;
        ShowStartGame(delay);
    }

    public void StartGame()
    {
        SetScore(0);
        generator.Generate();
        IsGameStarted = true;
        ShowGameUI();
    }

    public void GameOver()
    {
        IsGameStarted = false;
        IsGameOver = true;
        ShowGameOver();
    }

    private void ShowGameOver()
    {
        EndGameUI.SetActive(true);
        GameUI.SetActive(true);
        StartGameUI.SetActive(false);
    }

    private void ShowStartGame(float delay = 0f)
    {
        StartCoroutine(ShowStartGameCoro(delay));
    }

    IEnumerator ShowStartGameCoro(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndGameUI.SetActive(false);
        GameUI.SetActive(false);
        StartGameUI.SetActive(true);
    }

    private void ShowGameUI()
    {
        EndGameUI.SetActive(false);
        GameUI.SetActive(true);
        StartGameUI.SetActive(false);
    }
    #endregion
    
    private void SetScore(int value)
    {
        Score = value;
        ScoreUI.text = value.ToString();
    }
    
    
}
