using System;
using InVaderGame.Utils;
using UnityEngine;

namespace InVaderGame.Main
{

    public class MainCharacter : Singleton<MainCharacter>
    {
        [Tooltip("control player speed")]
        [SerializeField] private float _speed;  // control player speed

        [Header("Set character movement position in x direction")]
        [SerializeField] private float _minXValue;  //  Minimium x value of the screen.
        [SerializeField] private float _maxXValue;  //  Maximum x value of the screen.

        [Header("Bullet Components")]
        [SerializeField] private float _frameRate;
        [SerializeField] private Transform _bulletSpawnTransform;
        [SerializeField] private GameObject _bulletPrefab;

        [Header("Player SO")]
        [SerializeField] private PlayerProperties _playerProperties;  // hold player info


        //private variables
        private bool _isPlayerDead;
        private int _playerLive;


        //public variables
        public int PlayerLive { get { return _playerLive; } set { _playerLive = value; } }
        public bool IsPlayerDead { get { return _isPlayerDead; } private set { _isPlayerDead = value; } }


        // Action
        public Action gameOver;


        void Start()
        {
            UIController.resetGameState += UIController_resetGameState;
            _playerLive = _playerProperties.PlayerLives;
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
                Reset();
        }

        #endregion

        private void FixedUpdate()
        {
            #region character movement.
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Vector2 pos = transform.position + Vector3.left * _speed;
                if (transform.position.x > _minXValue)
                    transform.position = pos;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector2 pos = transform.position + Vector3.right * _speed;
                if (transform.position.x < _maxXValue)
                    transform.position = pos;
            }
            #endregion
        }

        private void Update()
        {
            // Bullet spawn 
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Instantiate(_bulletPrefab, _bulletSpawnTransform.position, _bulletSpawnTransform.rotation);
            }
        }

        /// <summary>
        /// When player hit by player 
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var enemyBullet = collision.gameObject.GetComponent<TagAssigner>()?.AssignedTag;

            if (enemyBullet != null && enemyBullet == TagAssigner.Tag.EnemyBullet)
            {
                _playerLive--;
                Handheld.Vibrate();

            }
            else if (enemyBullet != null && enemyBullet == TagAssigner.Tag.Enemy)
            {
                _playerLive = 0;
                Handheld.Vibrate();
            }

            if (_playerLive <= 0)
            {
                _isPlayerDead = true;

                // End Game
                PlayerStateController(true);
            }

        }

        // handle player die or alive state 
        private void PlayerStateController(bool isDie)
        {
            GetComponent<SpriteRenderer>().enabled = !isDie;
            GetComponent<Collider2D>().enabled = !isDie;
            gameOver?.Invoke();
        }


        /// <summary>
        /// reset the game state
        /// </summary>
        private void Reset()
        {
            Debug.Log("reset  main");
            _isPlayerDead = false;
            _playerLive = _playerProperties.PlayerLives;
            PlayerStateController(false);
        }
    }
}