using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InVaderGame.Main
{
    [CreateAssetMenu(fileName ="PlayerProperties",menuName ="InvaderTest/PlayerProSo",order =1)]
    public class PlayerProperties : ScriptableObject
    {
        public int PlayerLives = 3;
        public float PlayerSpeed=0.1f;
        public float PlayerBulletSpeed = 0.1f;
 
    }

    /*
     * Reasons behind to create separte So class for Enemy is : Might be in future
     * if you waana so properties which may not be common for both.
     */

    [CreateAssetMenu(fileName = "EnemyProperties", menuName = "InvaderTest/EnemyProSo", order = 1)]
    public class EnemyProperties : ScriptableObject
    {
        public int EnemyLives = 3;
        public float EnemySpeed = 0.1f;
        public float EnemyBulletSpeed = 0.1f;

    }
}