using System;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Network
{
    public class RoomPlayer : NetworkRoomPlayer
    {
        [SyncVar(hook = nameof(OnNicknameChanged))]
        private string _nickname = "Player";

        [SyncVar(hook = nameof(OnColorChanged))]
        private Color _color = Color.white;

        public string Nickname => _nickname;
        public Color PlayerColor => _color;
        public bool IsReady => readyToBegin;

        public event Action DataChanged;

        public override void OnStartClient()
        {
            base.OnStartClient();
            RaiseChanged();
            NotifyLobbyPlayersChanged();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            NotifyLobbyPlayersChanged();
        }

        public override void OnClientEnterRoom()
        {
            base.OnClientEnterRoom();
            RaiseChanged();
            NotifyLobbyPlayersChanged();
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            base.ReadyStateChanged(oldReadyState, newReadyState);
            RaiseChanged();
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            base.IndexChanged(oldIndex, newIndex);
            RaiseChanged();
            NotifyLobbyPlayersChanged();
        }

        [Command]
        public void CmdSetNickname(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname))
                return;

            nickname = nickname.Trim();

            if (nickname.Length > 16)
                nickname = nickname.Substring(0, 16);

            _nickname = nickname;
        }

        [Command]
        public void CmdSetColor(Color color)
        {
            _color = color;
        }

        private void OnNicknameChanged(string oldValue, string newValue)
        {
            RaiseChanged();
        }

        private void OnColorChanged(Color oldValue, Color newValue)
        {
            RaiseChanged();
        }

        private void RaiseChanged()
        {
            DataChanged?.Invoke();
        }

        private void NotifyLobbyPlayersChanged()
        {
            if (NetworkManager.singleton is RoomNetworkManager manager)
                manager.NotifyPlayersChanged();
        }
    }
}