using System.Collections;
using System.Collections.Generic;
using InVaderGame.Utils;
using UnityEngine;

namespace InVaderGame.Main
{

    public class ProtectorController : MonoBehaviour
    {
        [SerializeField] private int _health = 5;

        private int _healthStore;
        private bool _isProtectorDead;

        // Start is called before the first frame update
        void Start()
        {
            _healthStore = _health;
            _isProtectorDead = false;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = !_isProtectorDead;
            this.gameObject.GetComponent<Collider2D>().enabled = !_isProtectorDead;
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
                Reset();

            }
        }

        #endregion


        // Health 
        public void HeathDecrease(int score)
        {
           
            if (score <= 0)
            {
                _isProtectorDead = true;
                this.gameObject.GetComponent<SpriteRenderer>().enabled = !_isProtectorDead;
                this.gameObject.GetComponent<Collider2D>().enabled = !_isProtectorDead;
            }
        }

        // Enemy bullet hit the protector
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isProtectorDead) return;

            var assignedTag = collision.gameObject.GetComponent<TagAssigner>()?.AssignedTag;

            if (assignedTag != null && assignedTag == TagAssigner.Tag.Enemy)
            {
                _healthStore=0;
                HeathDecrease(_healthStore);

                // Game Over
            }
            else if(assignedTag != null && assignedTag == TagAssigner.Tag.EnemyBullet)
            {
                _healthStore--;
                HeathDecrease(_healthStore);
                Destroy(collision.gameObject);
            }
           
        }

        /// <summary>
        /// Reset the game state 
        /// </summary>
        public void Reset()
        {
            Debug.Log("reset pro ");
            Start();
        }
    }
}
