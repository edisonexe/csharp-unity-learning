using UnityEngine.SceneManagement;

namespace Game
{
    public sealed class GameRestartController
    {
        private readonly GameContext _run;
        private readonly string _sceneName;

        public GameRestartController(GameContext run, string sceneName)
        {
            _run = run;
            _sceneName = sceneName;
        }

        public void Restart()
        {
            _run.Reset();
            SceneManager.LoadScene(_sceneName);
        }
    }
}