using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        private const string GAME_SCENE_NAME = "Game";
    
        public void StartGame() => SceneManager.LoadScene(GAME_SCENE_NAME);
    }
}