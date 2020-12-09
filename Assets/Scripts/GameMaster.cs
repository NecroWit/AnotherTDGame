using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    private int maxLives;
    private int currentLives;
    private int gold;

    private UnityEvent onGameStart;
    private UnityEvent onGameOver;

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Awake()
    {
        Instance = this;
        currentLives = maxLives;
    }

    public void StartGame()
    {
        BoardMaster.Instance.StartGame();
        onGameStart?.Invoke();
    }

    public void ChangeGold(int newValue)
    {
        gold += newValue;
        UpdateVisualData();
    }

    public void ChangeLives(int newValue)
    {
        currentLives += newValue;
        UpdateVisualData();
    }

    public void DecreaseLives()
    {
        ChangeLives(-1);
        if(currentLives <= 0)
            onGameOver?.Invoke();
    }

    public void UpdateVisualData()
    {
        //
    }
}
