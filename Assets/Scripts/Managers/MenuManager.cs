using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        
        private const string GAME_SCENE_NAME = "Game";
        private int _scoreHits;
        private int _scoreShots;
        
        private void Start()
        {
            _scoreHits = PlayerPrefs.GetInt(PrefsKeys.SCORE_HITS, 0);
            _scoreShots = PlayerPrefs.GetInt(PrefsKeys.SCORE_SHOTS, 0);
            _scoreText.text = $"Score: {_scoreHits} hits in {_scoreShots} shots";
        }

        public void StartGame() => SceneManager.LoadScene(GAME_SCENE_NAME);
        public void QuitGame()
        {
            PlayerPrefs.Save();
            Debug.Log("[SAVE] Data saved on application quit.");
            Application.Quit();
        }
    }
}