using Characters.Player;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public sealed class HudView : MonoBehaviour
    {
        [SerializeField] private Image _hpFill;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _killsText;
        [SerializeField] private Button _restartButton;

        private PlayerEntity _player;
        private GameContext _run;
        private const string ARENA_SCENE = "Arena";
        
        public void Init(PlayerEntity player, GameContext run)
        {
            _player = player;
            _run = run;

            UpdateHp(_player.CurrentHp, _player.MaxHp);
            UpdateKills(_run.Kills);
            
            _player.HpChanged += UpdateHp;
            _run.KillsChanged += UpdateKills;

            _restartButton.onClick.RemoveAllListeners();
            _restartButton.onClick.AddListener(RestartScene);
        }

        private void OnDestroy()
        {
            if (_player != null)
                _player.HpChanged -= UpdateHp;

            if (_run != null)
                _run.KillsChanged -= UpdateKills;
        }

        private void UpdateKills(int kills)
        {
            if (_killsText != null)
                _killsText.text = $"Kills: {kills}";
        }

        private void UpdateHp(int cur, int max)
        {
            if (_hpText)
                _hpText.text = $"{cur}/{max}";

            if (_hpFill)
            {
                float t = (max <= 0) ? 0f : (float)cur / max;
                _hpFill.fillAmount = Mathf.Clamp01(t);
            }
        }

        private void RestartScene()
        {
            _run.Reset();
            SceneManager.LoadScene(ARENA_SCENE);
        }
    }
}