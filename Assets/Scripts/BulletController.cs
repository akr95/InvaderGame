using System;
using System.Collections;
using System.Collections.Generic;
using InVaderGame.Utils;
using UnityEngine;

namespace InVaderGame.Main
{

    public class BulletController : MonoBehaviour
    {

        private Transform _bullet;
        private float _selfDestroyHeight ;
        [SerializeField] private float _speed;

        public Action<int> scoreIncrement;


        // Start is called before the first frame update
        void Start()
        {
            _bullet = GetComponent<Transform>();
            _selfDestroyHeight = Camera.main.orthographicSize;
        }

        /// <summary>
        /// move the bullet
        /// </summary>
        void FixedUpdate()
        {
            _bullet.transform.Translate(Vector2.up * _speed, Space.World);

            if (_bullet.position.y > _selfDestroyHeight)
                Destroy(_bullet.gameObject);
        }

        
    }
}
