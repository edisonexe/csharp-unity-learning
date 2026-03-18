using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Views;
using TMPro;
using UnityEngine;

namespace _Project._Scripts.UI.Views
{
    public sealed class GameHudView : MonoBehaviour, IGameHudView
    {
        [SerializeField] private TMP_Text _modeText;
        [SerializeField] private TMP_Text _playersText;
        [SerializeField] private TMP_Text _pingText;
        [SerializeField] private TMP_Text _timerText;
        
        private void Awake()
        {
            if (!_modeText || !_playersText || !_pingText || !_timerText)
            {
                Debug.LogError("[GameHudView] References are not assigned.", this);
                enabled = false;
            }
        }
        
        public void SetMode(string mode)
        {
            if (_modeText)
                _modeText.text = $"Mode: {mode}";
        }

        public void SetPlayersCount(int count)
        {
            if (_playersText)
                _playersText.text = $"Players: {count}";
        }

        public void SetPing(int pingMs)
        {
            if (_pingText)
                _pingText.text = $"Ping: {pingMs} ms";
        }
        
        public void SetMatchTimer(string value)
        {
            _timerText.text = $"{value}";
        }
    }
}