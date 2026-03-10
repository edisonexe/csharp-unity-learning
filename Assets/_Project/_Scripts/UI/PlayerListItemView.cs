using System;
using _Project._Scripts.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Scripts.UI
{
    public class PlayerListItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private TMP_Text _hostText;
        [SerializeField] private TMP_Text _readyText;
        [SerializeField] private Image _colorIndicator;

        [SerializeField] private Color _readyColor = Color.green;
        [SerializeField] private Color _notReadyColor = Color.red;

        private RoomPlayer _player;
        private Func<RoomPlayer, bool> _isHost;
        private bool _isBound;
        
        public void Bind(RoomPlayer player, Func<RoomPlayer, bool> isHost)
        {
            if (!player)
            {
                Debug.LogError("[PlayerListItemView] Bind called with null player.");
                return;
            }
            
            Unbind();

            _player = player;
            _isHost = isHost;

            if (_player)
                _player.DataChanged += Refresh;

            _isBound = true;
            Refresh();
        }

        public void SetNickname(string nickname)
        {
            if (_nicknameText)
                _nicknameText.text = nickname;
        }

        public void SetColor(Color color)
        {
            if (_colorIndicator)
                _colorIndicator.color = color;
        }

        public void SetReady(bool ready)
        {
            if (!_readyText)
                return;

            _readyText.text = ready ? "Ready" : "Not Ready";
            _readyText.color = ready ? _readyColor : _notReadyColor;
        }

        public void SetHost(bool host)
        {
            if (_hostText)
                _hostText.text = host ? "Host" : "";
        }

        private void Refresh()
        {
            if (!_player)
                return;

            SetNickname(_player.Nickname);
            SetColor(_player.PlayerColor);
            SetReady(_player.IsReady);
            SetHost(_isHost != null && _isHost(_player));
        }

        private void Unbind()
        {
            if (_player)
                _player.DataChanged -= Refresh;

            _player = null;
            _isHost = null;
            _isBound = false;
        }

        private void OnDestroy()
        {
            if (_isBound)
                Unbind();
        }
    }
}