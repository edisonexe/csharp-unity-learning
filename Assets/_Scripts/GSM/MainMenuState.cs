using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;
using _Scripts.Interfaces.View;
using UnityEngine;

namespace _Scripts.GSM
{
    public sealed class MainMenuState : IGameState
    {
        private readonly ILoggerService _logger;
        private readonly GameStateMachine _gsm;
        private readonly IMainMenuView _view;
        private readonly IInputToggle _inputToggle;
        
        public MainMenuState(ILoggerService logger, IMainMenuView view, GameStateMachine gsm, IInputToggle inputToggle)
        {
            _view = view;
            _gsm = gsm;
            _logger = logger;
            _inputToggle = inputToggle;
        }
        
        public void Enter()
        {
            _inputToggle.Disable();
            _view.Show();
            _view.OnStart += StartHandler;
            _view.OnExit += ExitHandler;
            _logger.Log("[MainMenuState]: Enter");
        }

        public void Exit()
        {
            _view.Hide();
            _view.OnStart -= StartHandler;
            _view.OnExit -= ExitHandler;
            _logger.Log("[MainMenuState]: Exit");
        }

        private void StartHandler()
        {
            _gsm.ChangeState<GameplayState>();
        }

        private void ExitHandler()
        {
            Application.Quit();
        }
    }
}