using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InVaderGame.Utils;
using UnityEngine;


namespace InVaderGame.Main
{

    public class EnemyBulletController : MonoBehaviour
    {
        private Transform _bullet;
        private float _selfDestroyHeight;
        [SerializeField] private float _speed;


        // Start is called before the first frame update
        void Start()
        {
            _bullet = GetComponent<Transform>();
            _selfDestroyHeight = -Camera.main.orthographicSize;
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
                var listOfObjects = FindObjectsOfType<TagAssigner>().ToList().FindAll((obj) => obj.AssignedTag == TagAssigner.Tag.EnemyBullet);

                for (int i = 0; i < listOfObjects.Count(); i++)
                {
                    Destroy(listOfObjects[i].gameObject);
                }
            }
        }

        #endregion

        /// <summary>
        /// move the bullet
        /// </summary>
        void FixedUpdate()
        {
            _bullet.transform.Translate(Vector2.down * _speed, Space.World);

            if (_bullet.position.y < _selfDestroyHeight)
                Destroy(_bullet.gameObject);
        }


        // Delete the bullets if hit together.
        private void OnCollisionEnter2D(Collision2D collision)
        {

            var objectTag = collision.gameObject.GetComponent<TagAssigner>()?.AssignedTag;
            if (objectTag != null && objectTag == TagAssigner.Tag.PlayerBullet)
            {
                DestroyImmediate(this.gameObject);
                DestroyImmediate(collision.gameObject);
            }

        }
    }
}
