using System;
using System.Collections;
using System.Collections.Generic;
using InVaderGame.Utils;
using UnityEngine;

namespace InVaderGame.Main
{

    /// <summary> Manages the state of the level </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        //private variables
        private int Score;

        // Action which help us to inform score updated.
        public Action<int> scoreUpdated;

        private void Start()
        {
            UIController.resetGameState += UIController_resetGameState;
            EnemyController.Instance.updateScore += IncrementScore;
        }


        private void OnDestroy()
        {
            UIController.resetGameState -= UIController_resetGameState;
        }


        void Update()
        {

        }

        #region Implement delegate
        /// <summary>
        /// Implemet reset state delegate, which reset the level manager.
        /// </summary>
        /// <param name="isReset"></param>
        private void UIController_resetGameState(bool isReset)
        {
            if (isReset)
                Reset();
        }

        #endregion

        /// <summary>
        /// Update Score
        /// </summary>
        /// <param name="enemyCount"></param>
        public void IncrementScore(int enemyCount)
        {
            Debug.Log("enemy count " + enemyCount);
            if (enemyCount > 1) Score += enemyCount * GetFabonacciSeries(enemyCount+1) * 10;
            else Score += 10;

            scoreUpdated?.Invoke(Score);
        }

        public void Reset()
        {
            Debug.Log("reset score");
            Score = 0;
            // reset logic
        }

        private int GetFabonacciSeries(int count)
        {
            int n1 = 0, n2 = 1, sum = 0;
            for (int i = 2; i < count; ++i)  
            {
                sum = n1 + n2;
                n1 = n2;
                n2 = sum;
            }
            Debug.Log("sum "+sum);
            return sum;
        }

    }
}