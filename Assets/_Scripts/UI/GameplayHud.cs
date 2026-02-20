using System.Collections.Generic;
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
        [SerializeField] private TMP_Text _modifiersText;
        
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
            
            Hide();
            _isInitialized = true;
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
        
        public void SetModifiers(IReadOnlyList<string> modifiers)
        {
            if (modifiers == null || modifiers.Count == 0)
            {
                _modifiersText.SetText("Modifiers: None");
                return;
            }

            var result = "Modifiers:\n";
            foreach (var mod in modifiers)
                result += $"• {mod}\n";

            _modifiersText.SetText(result);
        }
    }
}