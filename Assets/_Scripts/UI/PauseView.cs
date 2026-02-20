using System;
using _Scripts.Interfaces;
using _Scripts.Interfaces.View;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public sealed class PauseView : MonoBehaviour, IPauseView
    {
        [SerializeField] private Button _resumeBtn;
        [SerializeField] private Button _toMenuBtn;
        
        public event Action OnResume;
        public event Action OnToMenu;
        
        private ILoggerService _logger;
        private bool _isInitialized;
        
        public void Init(ILoggerService logger)
        {
            _logger = logger;
            if (_isInitialized)
            {
                _logger.Warn("Pause view already initialized");
                return;
            }

            if (!_resumeBtn || !_toMenuBtn)
            {
                _logger.Error("Buttons not set");
                return;
            }
            
            _resumeBtn.onClick.AddListener(() => OnResume?.Invoke());
            _toMenuBtn.onClick.AddListener(() => OnToMenu?.Invoke());
            
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