using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project._Scripts.Bootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private string _lobbySceneName = "Lobby";

        private void Start()
        {
            SceneManager.LoadScene(_lobbySceneName);
        }
    }
}