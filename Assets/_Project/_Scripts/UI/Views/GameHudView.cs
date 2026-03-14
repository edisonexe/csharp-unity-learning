using _Project._Scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Scripts.UI.Views
{
    public sealed class GameHudView : MonoBehaviour, IGameHudView
    {
        [SerializeField] private TMP_Text _modeText;
        [SerializeField] private TMP_Text _playersText;
        [SerializeField] private TMP_Text _pingText;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private Image _hpFill;


        private void Awake()
        {
            if (!_modeText)
            {
                Debug.LogError("[GameHudView] ModeText is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_playersText)
            {
                Debug.LogError("[GameHudView] PlayersText is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_pingText)
            {
                Debug.LogError("[GameHudView] PingText is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_hpText)
            {
                Debug.LogError("[GameHudView] HpText is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_hpFill)
            {
                Debug.LogError("[GameHudView] HpFill is not assigned.", this);
                enabled = false;
                return;
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

        public void SetHp(int cur, int max)
        {
            float ratio = (float)cur / max;

            if (_hpFill) 
                _hpFill.fillAmount = ratio;

            if (_hpText)
                _hpText.text = $"{cur} / {max}";
        }
    }
}