using System;
using System.Collections;
using System.Collections.Generic;
using InVaderGame.Utils;
using UnityEngine;

namespace InVaderGame.Main
{

    public class EnemyInfo : MonoBehaviour
    {
        // Serizable fields
        [SerializeField] EnemyType _enemyType;

        // public variable
        public static Action<Vector2> destroedEnemyinfo;
        public Vector2 EnemyPosition { get; set; }
        public bool IsEnemyAlive { get; set; }
        public bool IsEnemyFire { get; set; }
        public EnemyType EnemyTypeValue =>  _enemyType;

        /// <summary>
        /// player bullet the enemy
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("on collision enter " + collision.gameObject.name);

            var objectTag = collision.gameObject.GetComponent<TagAssigner>()?.AssignedTag;
            if (objectTag != null && objectTag == TagAssigner.Tag.PlayerBullet)
            {
                destroedEnemyinfo?.Invoke(EnemyPosition);
                IsEnemyAlive = false;

                Destroy(this.gameObject);
                Destroy(collision.gameObject);
            }

        }
    }



}
