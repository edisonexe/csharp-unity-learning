using System;
using _Scripts.Interfaces;
using _Scripts.Interfaces.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public sealed class GameOverView : MonoBehaviour, IGameOverView
    {
        [SerializeField] private Button _restartBtn;
        [SerializeField] private Button _toMenuBtn;
        [SerializeField] private TMP_Text _title;
        
        public event Action OnRestart;
        public event Action OnToMenu;
        
        private ILoggerService _logger;
        private bool _isInitialized;
        
        public void Init(ILoggerService logger)
        {
            _logger = logger;
            
            if (_isInitialized)
            {
                _logger.Warn("GameOverView is already initialized");
                return;
            }
            
            if (!_restartBtn || !_toMenuBtn)
            {
                _logger.Error("Buttons not set");
                return;
            }

            if (!_title)
            {
                _logger.Error("Title not set");
                return;
            }
            
            _restartBtn.onClick.AddListener(() => OnRestart?.Invoke());
            _toMenuBtn.onClick.AddListener(() => OnToMenu?.Invoke());
            
            _isInitialized = true;
        }

        public void SetTitle(string title)
        {
            _title.SetText($"{title}");
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