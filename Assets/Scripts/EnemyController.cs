using System;
using System.Collections;
using System.Collections.Generic;
using InVaderGame.Utils;
using UnityEngine;

namespace InVaderGame.Main
{

    public class EnemyController : Singleton<EnemyController>
    {

        [SerializeField] private float _speed;
        [SerializeField] private float _speedOffSet = 0.01f;
        [SerializeField] private float _gridDownOffset = 0.3f;
        [SerializeField] private int _maxBulletFire;
        [SerializeField] private Transform _enemyHolder;
        [SerializeField] private GameObject _enemyBullet;
        [SerializeField] private float _fireRate = 0.995f;
        [SerializeField] private float _movementSpeed = 0.12f;

        [Header("Set Enemy movement limit in x direction")]
        [SerializeField] private float _minXValue;  //  Minimium x value of the screen.
        [SerializeField] private float _maxXValue;  //  Maximum x value of the screen.

        //Action
        public Action<int> updateScore;

        private float _selfDestroyHeight;
        private int _destroyedEnemyByBullet;

        // Start is called before the first frame update
        void Start()
        {
            _selfDestroyHeight = -Camera.main.orthographicSize;
            CancelInvoke();
            StopAllCoroutines();
            InvokeRepeating("EnemyMovement", 0.1f, _movementSpeed);
            EnemyInfo.destroyedEnemyinfo += BulletHitEnemy;
            UIController.resetGameState += UIController_resetGameState;

        }

        private void OnDestroy()
        {
            EnemyInfo.destroyedEnemyinfo -= BulletHitEnemy;
            UIController.resetGameState -= UIController_resetGameState;
        }


        #region Implement delegate
        /// <summary>
        /// Implemet reset state delegate, which reset the level manager.
        /// </summary>
        /// <param name="isReset"></param>
        private void UIController_resetGameState(bool isReset)
        {
            if (isReset)
            {
                
                Start();

            }
        }

        #endregion


        /// <summary>
        /// Move enemey left and right.
        /// </summary>
        void EnemyMovement()
        {
            _enemyHolder.position += Vector3.right * _speed;

            int fireCountInThisLoop = UnityEngine.Random.Range(0, _maxBulletFire);
            int fireCounter = 0;
            foreach (Transform enemy in _enemyHolder)
            {
                if (enemy.position.x < _minXValue || enemy.position.x > _maxXValue)
                {
                    _speed = -_speed;
                    _enemyHolder.position += Vector3.down * _gridDownOffset;
                    return;
                }

                // bullet instiantate
                var enemyInfos = EnemyInfos();
                if (enemyInfos.Count > 0 && fireCounter <= fireCountInThisLoop)
                {
                    int count = UnityEngine.Random.Range(0, enemyInfos.Count);
                    if (UnityEngine.Random.value > _fireRate)
                    {
                        Instantiate(_enemyBullet, enemyInfos[count].transform.position, enemyInfos[count].transform.rotation);
                        fireCounter++;
                    }
                }
                enemyInfos.Clear();

                Debug.Log("enemey pos " +(enemy.position.y < _selfDestroyHeight/1.8f));
                if (enemy.position.y < _selfDestroyHeight / 1.8f)
                    MainCharacter.Instance.gameOver?.Invoke();

            }

        }


        private void BulletHitEnemy(Vector2 enemyPosition)
        {
            _destroyedEnemyByBullet = 0;
            StartCoroutine(EnemyBulletController(enemyPosition));
        }



        /// <summary>
        /// it can be called, when enemy die and need to pass shot control to last column enemy 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="enemyPosition"></param>
        private IEnumerator EnemyBulletController(Vector2 enemyPosition)
        {       
            var enemyInfo = GridGenerator.Instance.gridInfo.enemyInfos.Find((obj) => obj.EnemyPosition == enemyPosition);

            if (enemyInfo != null)
            {
                _destroyedEnemyByBullet++;
                //Find Adjacent object and delete
               yield return StartCoroutine(DeleteAdjacentEnemy(enemyInfo));

                // Remove the enemy from list which died.
                GridGenerator.Instance.gridInfo.enemyInfos.Remove(enemyInfo);

                // if enemy died from the middle of the column then no need to pass control.
                for (int i = (int)enemyPosition.x; i < GridGenerator.Instance.gridInfo.gridSize.x; i++)
                {
                    var enemy = GridGenerator.Instance.gridInfo.enemyInfos.Find((obj) => obj.EnemyPosition == new Vector2(i, enemyPosition.y));
                    if (enemy != null) break;
                }

                // Pass shot control to another enemy.
                Vector2 setFireControl = new Vector2(enemyPosition.x - 1, enemyPosition.y);

                if (setFireControl.x >= 0)
                {
                    var enemy = GridGenerator.Instance.gridInfo.enemyInfos.Find((obj) => obj.EnemyPosition == setFireControl);
                    if (enemy != null)
                    {
                        enemy.IsEnemyFire = true;
                        GridGenerator.Instance.gridInfo.enemyInfos.Remove(enemy);
                        GridGenerator.Instance.gridInfo.enemyInfos.Add(enemy);
                    }
                }

            }

        }

        /// <summary>
        /// This method retuned the list which of enemy 
        /// </summary>
        /// <returns></returns>
        private List<EnemyInfo> EnemyInfos()
        {
            var enemyList = GridGenerator.Instance.gridInfo.enemyInfos.FindAll((obj) => obj.IsEnemyFire == true);
            return enemyList;

        }

        /// <summary>
        /// Find Adjacent enemy and update score and delete
        /// </summary>
        /// <param name="enemyInfo"></param>
        private IEnumerator DeleteAdjacentEnemy(EnemyInfo enemyInfo)
        {
            yield return new WaitForEndOfFrame();
            // find Adjacent Enemy , Find in above column first
            int aboveRow = (int)enemyInfo.EnemyPosition.x - 1;
            if (aboveRow >= 0)
            {
                FindAdjcentPos(enemyInfo, aboveRow);

            }

            // find Adjacent Enemy , Find in below column first
            int belowRow = (int)enemyInfo.EnemyPosition.x + 1;
            if (belowRow < GridGenerator.Instance.gridInfo.gridSize.x)
            {
                FindAdjcentPos(enemyInfo, belowRow);
            }

            // Score Update
            updateScore?.Invoke(_destroyedEnemyByBullet);

        }

        /// <summary>
        /// Find the Adjacent object position
        /// </summary>
        /// <param name="enemyInfo"></param>
        /// <param name="rowIndex"></param>
        private void FindAdjcentPos(EnemyInfo enemyInfo, int rowIndex)
        {
            // First Adjacent
            int coloumPosFirst = (int)enemyInfo.EnemyPosition.y - 1;
            if (coloumPosFirst >= 0)
            {
                 if (MatchAdjacentObject(enemyInfo, new Vector2(rowIndex, coloumPosFirst)))
                     _destroyedEnemyByBullet++;
                   
                
            }

            // Second Adjacent
            int coloumPosSecond = (int)enemyInfo.EnemyPosition.y + 1;
            if (coloumPosSecond < GridGenerator.Instance.gridInfo.gridSize.y)
            {
                if (MatchAdjacentObject(enemyInfo, new Vector2(rowIndex, coloumPosSecond)))
                {
                    _destroyedEnemyByBullet++;
                   
                }
            }
        }


        /// <summary>
        /// Check Enemy type are same or not.
        /// </summary>
        /// <param name="enemyInfo"></param>
        /// <returns></returns>
        private bool MatchAdjacentObject(EnemyInfo enemyInfo, Vector2 vector2)
        {
            var enemy = GridGenerator.Instance.gridInfo.enemyInfos.Find((obj) => obj.EnemyPosition == vector2);
            if (enemy != null)
            {
                if (enemyInfo.EnemyTypeValue == enemy.EnemyTypeValue)
                {
                    GridGenerator.Instance.gridInfo.enemyInfos.Remove(enemy);
                    Destroy(enemy.gameObject);
                    return true;
                }
            }
            return false;
        }


    }
}
