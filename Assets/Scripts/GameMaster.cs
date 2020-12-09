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

        [SerializeField]
        private int maxLives;
        private int _currentLives;
        private int _gold;
        private int _currentLevel;
        private int _enemyKilled;

        public UnityEvent onGameStart;
        public UnityEvent onGameOver;
        public UnityEvent onGoldAmountChanged;

        [Space(6)] 
        [SerializeField]
        private Text goldText;
        [SerializeField]
        private Text livesText; 
        [SerializeField]
        private Text levelText;
        [SerializeField]
        private Text enemyKilledText;
        

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            Clear();
        }

        private void Clear()
        {
            _currentLives = maxLives;
            _gold = 0;
            _currentLevel = 1;
            _enemyKilled = 0;
            UpdateVisualData();
        }

        public void StartGame()
        {
            BoardMaster.Instance.StartGame();
            onGameStart?.Invoke();
        }

        public void RestartGame()
        {
            Clear();
            BoardMaster.Instance.RestartGame();
            onGameStart?.Invoke();
        }

        public void LevelUp()
        {
            _currentLevel++;
            UpdateVisualData();
        }

        public int GetLevel()
        {
            return _currentLevel;
        }

        public void UpdateEnemyKilled()
        {
            _enemyKilled++;
            UpdateVisualData();
        }

        public void ChangeGold(int newValue)
        {
            _gold += newValue;
            UpdateVisualData();
            onGoldAmountChanged?.Invoke();
        }

        public int GetGold()
        {
            return _gold;
        }

        public void ChangeLives(int newValue)
        {
            _currentLives += newValue;
            UpdateVisualData();
        }

        public void DecreaseLives()
        {
            ChangeLives(-1);
            UpdateVisualData();
            if (_currentLives <= 0)
                onGameOver?.Invoke();
        }

        public void UpdateVisualData()
        {
            if (goldText != null)
                goldText.text = _gold.ToString();

            if (livesText != null)
                livesText.text = _currentLives.ToString();
            
            if (levelText != null)
                levelText.text = _currentLevel.ToString();
            
            if (enemyKilledText != null)
                enemyKilledText.text = _enemyKilled.ToString();
            
            
        }
    }
}
