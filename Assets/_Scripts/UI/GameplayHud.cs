using _Scripts.Interfaces;
using _Scripts.Interfaces.View;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public sealed class GameplayHud : MonoBehaviour, IGameplayHud
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _inputModeText;
        
        private ILoggerService _logger;
        private bool _isInitialized;
        
        public void Init(ILoggerService logger)
        {
            _logger = logger;
            if (_isInitialized)
            {
                _logger.Warn("GameplayHud is already initialized");
                return;
            }

            if (!_scoreText || !_hpText)
            {
                _logger.Error("Score or Hp text not set");
            }
        }

        public void SetHp(int current, int max)
        {
            _hpText.SetText($"Health: {current}/{max}");
        }

        public void SetScore(int score)
        {
            _scoreText.SetText($"Score: {score}");
        }

        public void SetInputMode(string mode)
        {
            _inputModeText.SetText($"InputMode: {mode}");
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