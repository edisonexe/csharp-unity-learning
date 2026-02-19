using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;
using _Scripts.Interfaces.View;

namespace _Scripts.GSM
{
    public sealed class GameOverState : IGameState
    {
        private readonly IGameOverView _view;
        private readonly GameStateMachine _gsm;
        private readonly ILoggerService _logger;
        private readonly IGameResult _gameResult;
        private readonly IInputToggle _inputToggle;
        private readonly IGameSession _gameSession;

        public GameOverState(ILoggerService logger, IGameOverView view, GameStateMachine gsm, IGameResult gameResult,
            IInputToggle inputToggle, IGameSession gameSession)
        {
            _view = view;
            _gsm = gsm;
            _logger = logger;
            _gameResult = gameResult;
            _inputToggle = inputToggle;
            _gameSession = gameSession;
        }
        
        public void Enter()
        {
            _inputToggle.Disable();
            _view.SetTitle(_gameResult.Current.ToString());
            _view.Show();
            _view.OnRestart += RestartHandler;
            _view.OnToMenu += OnToMenuHandler;
            _logger.Log("Enter GameOverState");
        }

        public void Exit()
        {
            _view.Hide();
            _view.OnRestart -= RestartHandler;
            _view.OnToMenu -= OnToMenuHandler;
            _logger.Log("Exit GameOverState");
        }

        private void RestartHandler()
        {
            _gameSession.Restart();
            _gsm.ChangeState<GameplayState>();
        }

        private void OnToMenuHandler()
        {
            _gameSession.Restart();
            _gsm.ChangeState<MainMenuState>();
        }
    }
}