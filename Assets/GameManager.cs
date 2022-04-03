using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int ScorePerSecond = 10;
    public int Score { get; private set; }
    public Text ScoreUI;
    public PipeGenerator generator;

    public GameObject StartGameUI;
    public GameObject GameUI;
    public GameObject EndGameUI;

    public bool IsGameStarted;
    public bool IsGameOver;

    //half assed singleton
    public static GameManager instance;

    private void OnEnable()
    {
        instance = this;

        RestartGame();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsGameStarted)
        {
            SetScore(Score + Mathf.RoundToInt(Time.fixedDeltaTime * ScorePerSecond));
        }
    }

    public void RestartGame()
    {
        IsGameStarted = false;
        IsGameOver = false;
        ShowStartGame();
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

    private void SetScore(int value)
    {
        Score = value;
        ScoreUI.text = value.ToString();
    }

    private void ShowGameOver()
    {
        EndGameUI.SetActive(true);
        GameUI.SetActive(true);
        StartGameUI.SetActive(false);
    }

    private void ShowStartGame()
    {
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
}
