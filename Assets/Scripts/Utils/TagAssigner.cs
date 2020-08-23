using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InVaderGame.Utils
{

    public class TagAssigner : MonoBehaviour
    {
        public enum Tag
        {
            None,
            Player,
            Enemy,
            EnemyBullet,
            PlayerBullet,
            Protector
        }

        [SerializeField] private Tag _selectTag;

        public Tag AssignedTag { get { return _selectTag; } }
    }
}
