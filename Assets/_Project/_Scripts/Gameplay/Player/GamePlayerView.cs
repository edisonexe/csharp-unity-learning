using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Views;
using TMPro;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Player
{
    public sealed class GamePlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private UnityEngine.Camera _playerCamera;
        [SerializeField] private GameObject _localVisualRoot;
        [SerializeField] private GameObject _remoteVisualRoot;
        [SerializeField] private Transform _weaponRoot;

        private MaterialPropertyBlock _propertyBlock;
        private bool _isLocal;
        private bool _isAlive = true;

        public UnityEngine.Camera PlayerCamera => _playerCamera;

        private void Awake()
        {
            if (!_renderer)
            {
                Debug.LogError("[GamePlayerView] Renderer is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_nicknameText)
            {
                Debug.LogError("[GamePlayerView] NicknameText is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_localVisualRoot)
            {
                Debug.LogError("[GamePlayerView] LocalVisualRoot is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_remoteVisualRoot)
            {
                Debug.LogError("[GamePlayerView] RemoteVisualRoot is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_weaponRoot)
            {
                Debug.LogError("[GamePlayerView] WeaponRoot is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_playerCamera)
            {
                Debug.LogError("[GamePlayerView] PlayerCamera is not assigned.", this);
                enabled = false;
                return;
            }
        }
        
        public void SetNickname(string nickname)
        {
            if (_nicknameText)
                _nicknameText.text = nickname;
        }

        public void SetColor(Color color)
        {
            if (!_renderer)
                return;

            _propertyBlock ??= new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor("_BaseColor", color);
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        public void SetLocalState(bool isLocal)
        {
            _isLocal = isLocal;
            ApplyVisualState();
        }

        public void SetAliveState(bool alive)
        {
            _isAlive = alive;
            ApplyVisualState();
        }

        public void SetWeaponPitch(float pitch)
        {
            if (!_weaponRoot)
                return;

            Vector3 euler = _weaponRoot.localEulerAngles;
            euler.x = NormalizePitch(pitch);
            euler.y = 0f;
            euler.z = 0f;
            _weaponRoot.localEulerAngles = euler;
        }

        private void ApplyVisualState()
        {
            bool localVisible = _isAlive && _isLocal;
            bool remoteVisible = _isAlive && !_isLocal;

            if (_playerCamera)
                _playerCamera.gameObject.SetActive(localVisible);

            if (_localVisualRoot)
                _localVisualRoot.SetActive(localVisible);

            if (_remoteVisualRoot)
                _remoteVisualRoot.SetActive(remoteVisible);

            if (_weaponRoot)
                _weaponRoot.gameObject.SetActive(_isAlive);
        }

        private static float NormalizePitch(float pitch)
        {
            return pitch > 180f ? pitch - 360f : pitch;
        }
    }
}