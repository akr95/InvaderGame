using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InVaderGame.Main
{

    /// <summary> Manages the state of the whole application </summary>
    public class GameManager : MonoBehaviour
    {
       
        public void LoadSceneByIndex(int index)
        {
            StartCoroutine(LoadScene(index));
        }

        private IEnumerator LoadScene(int scenIndex)
        {
            Debug.Log("Loading game!");
            yield return new WaitForSeconds(.4f);
            SceneManager.LoadSceneAsync(scenIndex).completed += GameManager_completed;
        }

        private void GameManager_completed(AsyncOperation obj)
        {
            if (obj.isDone)
            {
                // When next scene loading process done.
            }
        }
    }
}