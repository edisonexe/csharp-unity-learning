using System;
using _Scripts.Interfaces;
using _Scripts.Interfaces.View;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public sealed class MainMenuView : MonoBehaviour, IMainMenuView
    {
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _exitBtn;
        
        public event Action OnStart;
        public event Action OnExit;

        private ILoggerService _logger;
        private bool _isInitialized;
        
        public void Init(ILoggerService logger)
        {
            _logger = logger;
            if (_isInitialized)
            {
                _logger.Warn("MainMenuView is already initialized");
                return;
            }

            if (!_startBtn || !_exitBtn)
            {
                _logger.Error("Buttons not set");
                return;
            } 
            
            _startBtn.onClick.AddListener(() => OnStart?.Invoke());
            _exitBtn.onClick.AddListener(() => OnExit?.Invoke());
            
            Hide();
            _isInitialized = true;
        }
        

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}