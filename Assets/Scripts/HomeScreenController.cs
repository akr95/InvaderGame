using System.Collections;
using System.Collections.Generic;
using System.IO;
using InVaderGame.Main;
using InVaderGame.Utils;
using TMPro;
using UnityEngine;

namespace InVaderGame.Main
{

    public class HomeScreenController : MonoBehaviour
    {
        private string _storePath;
        private Playerinfo _playerinfo;
        [SerializeField] TextMeshProUGUI _gameStatePopUpHighScoreText;


        private void Awake()
        {
            _storePath = Application.persistentDataPath + "/playerInfo.dat";
        }

        // Start is called before the first frame update
        void Start()
        {
            if (File.Exists(_storePath))
            {
                _playerinfo = SaveOrLoad.Load<Playerinfo>(_storePath);
                if (_playerinfo != null)
                    _gameStatePopUpHighScoreText.text ="Highscore : "+ _playerinfo.score.ToString();

            }
        }

       
    }
}
