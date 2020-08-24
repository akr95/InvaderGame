
using System.IO;
using InVaderGame.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InVaderGame.Main
{

    public class UIController : Singleton<UIController>
    {
        private Playerinfo _playerinfo;
        private int _playerScore = 0;
        private int _highScore = 0;
        private string _storePath ;

        [SerializeField] TextMeshProUGUI _gameLevelText;
        [SerializeField] TextMeshProUGUI _gameStatePopUpHighScoreText;
        [SerializeField] TextMeshProUGUI _gameStatePopUpScoreText;
        [SerializeField] TextMeshProUGUI _gameStateText;
        [SerializeField] TextMeshProUGUI _scroreText;
        [SerializeField] TextMeshProUGUI _LevelNameText;
        [SerializeField] GameObject _restartBtn;
        [SerializeField] GameObject _continueBtn;
        [SerializeField] GameObject _homeBtn;
        [SerializeField] GameObject _gameStatPopUp;
        [SerializeField] GameObject _characterLife;

        #region delegate

        public delegate void ResetGameState(bool isReset);
        public static event ResetGameState resetGameState;
        #endregion

        private void Awake()
        {
            _storePath = Application.persistentDataPath + "/playerInfo.dat";
        }


        // Start is called before the first frame update
        void Start()
        {
            #region load player info

            _playerinfo = new Playerinfo();
            if (File.Exists(_storePath))
            {
                _playerinfo = SaveOrLoad.Load<Playerinfo>(_storePath);
                if (_playerinfo == null)
                    _playerinfo = new Playerinfo();
            }

            #endregion

            Reset();
            LevelManager.Instance.scoreUpdated += Instance_ScoreUpdate;
            MainCharacter.Instance.gameOver += GameOver;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.scoreUpdated -= Instance_ScoreUpdate;

        }

        private void FixedUpdate()
        {
            if (!MainCharacter.Instance.IsPlayerDead)
            {
                SetPlayerLifeUI(MainCharacter.Instance.PlayerLive);
            }
            if (GridGenerator.Instance.gridInfo.enemyInfos.Count <= 0)
            {
                _playerinfo.levelNumber++;
                GameOver();
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _gameStateText.text = "Game Pause";
                Time.timeScale = 0;
                GameObjectOnOff(_gameStatPopUp, true);
                GameObjectOnOff(_restartBtn, false);
                GameObjectOnOff(_continueBtn, true);
            }
        }

        /// <summary>
        /// Implement score updated action, which update the score in UI
        /// </summary>
        /// <param name="obj"></param>
        private void Instance_ScoreUpdate(int obj)
        {
            if (_scroreText == null) return;
            _playerScore = obj;
            _scroreText.text = "Score : " + _playerScore.ToString();
        }


        private void GameObjectOnOff(GameObject currentObject, bool status)
        {
            if (currentObject != null)
            {
                currentObject.SetActive(status);
            }
        }

        /// <summary>
        ///  Set life of player in UI view
        /// </summary>
        /// <param name="playLife"></param>
        private void SetPlayerLifeUI(int playLife)
        {
            for (int i = 0; i < _characterLife.transform.childCount; i++)
            {
                if (i < playLife)
                {
                    _characterLife.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    _characterLife.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }



        // Reset the Game State
        private void Reset()
        {
            Debug.Log("reset ");
            _scroreText.text = "Score : 0";
            _playerScore = 0;
            SetPlayerLifeUI(_characterLife.transform.childCount);

        }

        // Restart button
        public void RestartBtn()
        {
            // Handle RestartButton
            Reset();
            resetGameState?.Invoke(true);
            Time.timeScale = 1;
            GameObjectOnOff(_gameStatPopUp, false);
        }


        // Restart button
        public void ContinueBtn()
        {
            Time.timeScale = 1;
            GameObjectOnOff(_gameStatPopUp, false);
        }

        // Move to home screen and change timescale
        public void HomeBtn()
        {
            Time.timeScale = 1;
        }

        // GameOver PopUp call
        private void GameOver()
        {
            _playerinfo.levelNumber = 0;
            UpdateGameBoard();
        }


        //update game board of the player
        private void UpdateGameBoard()
        {
            if (_playerinfo.score < _playerScore)
            {
                UpdatePlayerInfo();
            }
            Time.timeScale = 0;
            SetPlayerLifeUI(MainCharacter.Instance.PlayerLive);
            GameObjectOnOff(_gameStatPopUp, true);
            GameObjectOnOff(_restartBtn, true);
            GameObjectOnOff(_continueBtn, false);
            _gameStateText.text = "Game Over";
            _gameLevelText.text = "Level : " + _playerinfo.levelNumber.ToString();
            _gameStatePopUpHighScoreText.text = "High Score : " + _playerinfo.score.ToString();
            _gameStatePopUpScoreText.text = "Player Score : " + _playerScore.ToString();
        }



        // update player info
        private void UpdatePlayerInfo()
        {
            _playerinfo.score = _playerScore;

            if (!File.Exists(_storePath))
                File.Create(_storePath);
            SaveOrLoad.Save(_storePath, _playerinfo);
        }

    }
}
