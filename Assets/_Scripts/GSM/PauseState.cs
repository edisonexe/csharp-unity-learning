using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;
using _Scripts.Interfaces.View;

namespace _Scripts.GSM
{
    public sealed class PauseState : IGameState
    {
        private readonly ILoggerService _logger;
        private readonly IPauseView _view;
        private readonly ITimeService _time;
        private readonly GameStateMachine _gsm;
        private readonly IInputToggle _inputToggle;
        private readonly IGameSession _gameSession;
        
        public PauseState(ILoggerService logger, IPauseView view, GameStateMachine gsm, ITimeService time, 
            IInputToggle inputToggle, IGameSession gameSession)
        {
            _view = view;
            _time = time;
            _gsm = gsm;
            _logger = logger;
            _inputToggle = inputToggle;
            _gameSession = gameSession;
        }

        public void Enter()
        {
            _inputToggle.Disable();
            _time.Pause();
            _view.Show();
            _view.OnResume += ResumeHandler;
            _view.OnToMenu += OnToMenuHandler;
            _logger.Log("[PauseState]: Enter");
        }

        public void Exit()
        {
            _time.Resume();
            _view.Hide();
            _view.OnResume -= ResumeHandler;
            _view.OnToMenu -= OnToMenuHandler;
            _logger.Log("[PauseState]: Exit");
        }

        private void ResumeHandler()
        {
            _gsm.ChangeState<GameplayState>();
        }

        private void OnToMenuHandler()
        {
            _gameSession.Restart();
            _gsm.ChangeState<MainMenuState>();
        }
    }
}