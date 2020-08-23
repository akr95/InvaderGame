using System.Collections;
using System.Collections.Generic;
using InVaderGame.Utils;
using UnityEngine;

namespace InVaderGame.Main
{
    public class GridGenerator : Singleton<GridGenerator>
    {

        public class GridInfo
        {
            public Vector2 gridSize;
            public List<EnemyInfo> enemyInfos = new List<EnemyInfo>();
        }

        private Vector2 _gridPosition; 

        [Header("Grid Generator size")]
        [SerializeField] Vector2 _gridSize;
        [SerializeField] GameObject[] _enemyPrefabs;
        [SerializeField] Transform _gridTransformParent;
        [SerializeField] float _offSet = 0.7f;

        public GridInfo gridInfo;


        void Awake()
        {
            gridInfo = new GridInfo();
            gridInfo.gridSize = _gridSize;
            _gridPosition = _gridTransformParent.position;
            GridGenertorCall();
        }

        private void Start()
        {
            UIController.resetGameState += UIController_resetGameState;
        }

        private void OnDestroy()
        {
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
                for (int i=0; i< _gridTransformParent.transform.childCount;i++)
                {
                    Destroy(_gridTransformParent.transform.GetChild(i).gameObject);
                }
                _gridTransformParent.position=_gridPosition;
                GridGenertorCall();

            }
        }

        #endregion


        /// <summary>
        /// Grid Generator and attach required components, which help to get info of each object of grid
        /// </summary>
        private void GridGenertorCall()
        {
            gridInfo.enemyInfos.Clear();
            if (_enemyPrefabs.Length == 0 || _gridTransformParent == null) return;

            for (int i = 0; i < _gridSize.x; i++)
            {
                for (int j = 0; j < _gridSize.y; j++)
                {
                    int number = Random.Range(0, _enemyPrefabs.Length);
                    GameObject enemy = Instantiate(_enemyPrefabs[number]);
                    enemy.transform.position = new Vector2(_gridTransformParent.position.x + (j * _offSet), _gridTransformParent.position.y - (i * _offSet));
                    enemy.transform.SetParent(_gridTransformParent);

                    // 
                    EnemyInfo enemyInfo = enemy.GetComponent<EnemyInfo>();
                    enemyInfo.EnemyPosition = new Vector2(i, j);
                    enemyInfo.IsEnemyAlive = true;
                    enemyInfo.IsEnemyFire = false;

                    if (i == _gridSize.x - 1)
                    {
                        enemyInfo.IsEnemyFire = true;
                    }

                    gridInfo.enemyInfos.Add(enemyInfo);
                }
            }
        }

    }
}