using System;
using Characters.Player;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapon;

namespace UI
{
    public sealed class HudView : MonoBehaviour
    {
        [SerializeField] private GameObject _hud;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private Image _hpFill;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _killsText;
        [SerializeField] private Button _restartButton;

        [Header("Weapons Panel")]
        [SerializeField] private TMP_Text _ammoText;
        [SerializeField] private Image _weaponIcon;
        
        public event Action RestartRequested;
        
        private PlayerEntity _player;
        private GameContext _run;
        private GameStateMachine _gsm;
        private GameRestartController _restartController;
        
        private WeaponModel _currentWeapon;
        
        public void Init(PlayerEntity player, GameContext run, GameStateMachine gsm)
        {
            _player = player;
            _run = run;
            _gsm = gsm;
            
            _restartButton.onClick.RemoveAllListeners();
            _restartButton.onClick.AddListener(() => RestartRequested?.Invoke());

            UpdateHp(_player.CurrentHp, _player.MaxHp);
            UpdateKills(_run.Kills);
            
            _player.HpChanged += UpdateHp;
            var weapons = _player.Weapons;
            
            weapons.Changed += OnWeaponChanged;
            OnWeaponChanged(weapons.Current);
            
            _run.KillsChanged += UpdateKills;
            _gsm.OnStateChanged += HandleStateChanged;
            HandleStateChanged(_gsm.CurrentState);
        }

        private void HandleStateChanged(GameState state)
        {
            _hud.SetActive(state != GameState.Start);
            _winPanel.SetActive(state == GameState.Win);
            _losePanel.SetActive(state == GameState.Lose);
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
        
        private void OnWeaponChanged(WeaponModel wm)
        {
            if (_currentWeapon != null) _currentWeapon.AmmoChanged -= OnAmmoChanged;
            _currentWeapon = wm;
            _currentWeapon.AmmoChanged += OnAmmoChanged;

            _weaponIcon.sprite = _currentWeapon.Config.Icon;

            OnAmmoChanged(_currentWeapon.Ammo, _currentWeapon.Config.MaxAmmo);
        }

        private void OnAmmoChanged(int ammo, int max)
        {
            _ammoText.text = $"{ammo}/{max}";
        }
    }
}