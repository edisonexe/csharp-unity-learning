using Mirror;
using TMPro;
using UnityEngine;

namespace _Project._Scripts.Network
{
    public class GamePlayer : NetworkBehaviour
    {
        [SerializeField] private Renderer _playerRenderer;
        [SerializeField] private TMP_Text _nicknameText;

        [SyncVar(hook = nameof(OnNicknameChanged))]
        private string _nickname;

        [SyncVar(hook = nameof(OnColorChanged))]
        private Color _color = Color.white;

        private void Awake()
        {
            if (!_nicknameText)
                Debug.LogWarning("[GamePlayer] NicknameText is not assigned");

            if (!_playerRenderer)
                Debug.LogWarning("[GamePlayer] Renderer is not assigned");
        }
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            ApplyNickname();
            ApplyColor();
        }

        [Server]
        public void Initialize(string nickname, Color color)
        {
            _nickname = string.IsNullOrWhiteSpace(nickname) ? "Player" : nickname;
            _color = color;
        }

        private void OnNicknameChanged(string oldValue, string newValue)
        {
            ApplyNickname();
        }

        private void OnColorChanged(Color oldValue, Color newValue)
        {
            ApplyColor();
        }

        private void ApplyNickname()
        {
            if (_nicknameText)
                _nicknameText.text = _nickname;
        }

        private void ApplyColor()
        {
            if (_playerRenderer)
                _playerRenderer.material.color = _color;
        }
    }
}