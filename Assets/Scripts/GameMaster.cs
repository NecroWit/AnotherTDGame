using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace AnotherDTGame
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster Instance;

        private int maxLives;
        private int currentLives;
        private int gold;

        public UnityEvent onGameStart;
        public UnityEvent onGameOver;
        public UnityEvent onGoldAmountChanged;

        [Space(6)] 
        [SerializeField]
        private Text goldText;
        [SerializeField]
        private Text livesText;
        

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
            onGoldAmountChanged?.Invoke();
        }

        public void ChangeLives(int newValue)
        {
            currentLives += newValue;
            UpdateVisualData();
        }

        public void DecreaseLives()
        {
            ChangeLives(-1);
            UpdateVisualData();
            if (currentLives <= 0)
                onGameOver?.Invoke();
        }

        public void UpdateVisualData()
        {
            if (goldText != null)
                goldText.text = gold.ToString();

            if (livesText != null)
                livesText.text = livesText.ToString();
        }
    }
}
