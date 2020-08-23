
using InVaderGame.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InVaderGame.Main
{

    public class UIController : Singleton<UIController>
    {
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


        // Start is called before the first frame update
        void Start()
        {
            Reset();
            LevelManager.Instance.scoreUpdated += Instance_ScoreUpdate;
            MainCharacter.Instance.gameOver += GameOver;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.scoreUpdated -= Instance_ScoreUpdate;
            MainCharacter.Instance.gameOver -= GameOver;
        }

        private void FixedUpdate()
        {
            if (!MainCharacter.Instance.IsPlayerDead)
            {
                SetPlayerLifeUI(MainCharacter.Instance.PlayerLive);
            }
           
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _gameStateText.text = "Game Pause";
                Time.timeScale = 0;
                GameObjectOnOff(_gameStatPopUp,true);
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

            _scroreText.text = "Score : "+obj.ToString();
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
            for (int i=0; i< _characterLife.transform.childCount; i++)
            {
                if (i<playLife)
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
        private  void Reset()
        {
            Debug.Log("reset ");
            _scroreText.text = "Score : 0";
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

        public void HomeBtn()
        {
            Time.timeScale = 1;
        }


        // GameOver PopUp call
        private void GameOver()
        {
            _gameStateText.text = "Game Over";
            Time.timeScale = 0;
            SetPlayerLifeUI(MainCharacter.Instance.PlayerLive);
            GameObjectOnOff(_gameStatPopUp, true);
            GameObjectOnOff(_restartBtn, true);
            GameObjectOnOff(_continueBtn, false);
        }

    }
}
