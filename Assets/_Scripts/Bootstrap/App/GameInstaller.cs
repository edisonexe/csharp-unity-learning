using _Scripts.Bootstrap.GameScene;
using _Scripts.GSM;
using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;
using _Scripts.Services.Input;
using _Scripts.Services.Logging;
using _Scripts.Services.Time;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Bootstrap.App
{
    public sealed class GameInstaller : MonoBehaviour
    {
        [Header("Bootstrap Settings")]
        [SerializeField] private string _gameSceneName = "Game";
        [SerializeField] private InputMode _inputMode = InputMode.Keyboard;

        private ILoggerService _logger;
        private ITimeService _time;

        private GameInputActions _inputActions;
        private IPauseInputService _pauseInput;
        private IInputModeProvider _inputModeProvider;

        private GameStateMachine _gsm;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            ComposeApp();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(_gameSceneName);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void ComposeApp()
        {
            _logger = new UnityLoggerService();
            _time = new UnityTimeService();
            _gsm = new GameStateMachine(_logger);

            _inputActions = new GameInputActions();
            _inputActions.Enable();

            _inputModeProvider = new InputModeProvider(_inputMode);

            _pauseInput = new PauseInputService(_inputActions, _logger);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != _gameSceneName)
                return;

            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.TryGetComponent<SceneBootstrapper>(out var bootstrapper))
                {
                    bootstrapper.Install(_logger, _time, _pauseInput, _inputModeProvider, _inputActions, _gsm);
                    break;
                }
            }
        }
    }
}
