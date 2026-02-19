using _Scripts.GSM;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.GameFlow
{
    public sealed class GameController : MonoBehaviour
    {
        private GameStateMachine _gsm;
        private IPauseInputService _pauseInput;
        private ILoggerService _logger;
        private bool _isInitialized;
        
        public void Init(GameStateMachine gsm, IPauseInputService pauseInput, ILoggerService logger)
        {
            _logger = logger;
            if (_isInitialized)
            {
                _logger.Warn("GameController is already initialized");
                return;
            }

            if (pauseInput == null)
            {
                _logger.Error("PauseInput is null!");
                return;
            }
            _gsm = gsm;
            _pauseInput = pauseInput;
            
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;
            if (_pauseInput.PausePressedThisFrame)
            {
                if (_gsm.CurrentState is GameplayState)
                    _gsm.ChangeState<PauseState>();
                else if (_gsm.CurrentState is PauseState)
                    _gsm.ChangeState<GameplayState>();
            }
        }
    }
}