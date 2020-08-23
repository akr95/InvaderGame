/*
 * This script is using for save and load the data in File.
 * 
 */

using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;


namespace InVaderGame.Utils
{

    public class SaveOrLoad
    {
        public static T Load<T>(string filename) where T : class
        {
            if (File.Exists(filename))
            {
                try
                {
                    using (Stream stream = File.OpenRead(filename))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return formatter.Deserialize(stream) as T;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
            return default(T);
        }

        public static void Save<T>(string filename, T data) where T : class
        {
            using (Stream stream = File.OpenWrite(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);

            }
        }


        public static void Delete(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

        }
    }
}