using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace InVaderGame.Utils
{

    public abstract class Singleton<T> : MonoBehaviour where T: Component
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] results = FindObjectsOfType<T>();

                    if (results.Length==0||results.Length>1)
                    {
                        return null;
                    }
                    _instance = results[0];
                }
                return _instance;

            }
        }

    }
}
