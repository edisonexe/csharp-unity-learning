using System;
using System.Collections.Generic;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Views;
using _Project._Scripts.Network;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.UI.Controllers
{
    public class LobbyController : IDisposable
    {
        private readonly ILobbyView _lobbyView;
        private readonly RoomNetworkManager _networkManager;

        private RoomPlayer _localPlayer;
        private bool _disposed;
        
        public LobbyController(ILobbyView lobbyView, RoomNetworkManager networkManager)
        {
            _lobbyView = lobbyView ?? throw new ArgumentNullException(nameof(lobbyView));
            _networkManager = networkManager ?? throw new ArgumentNullException(nameof(networkManager));

            Subscribe();

            TryAttachLocalPlayer();
            RefreshLobby();
        }

        private void Subscribe()
        {
            _lobbyView.NicknameChanged += OnNicknameChanged;
            _lobbyView.ColorChanged += OnColorChanged;
            _lobbyView.ReadyClicked += OnReadyClicked;
            _lobbyView.StartGameClicked += OnStartGameClicked;

            _networkManager.PlayersChanged += OnPlayersChanged;
        }

        private void UnsubscribeLocalPlayer()
        {
            if (_localPlayer)
                _localPlayer.DataChanged -= OnLocalPlayerDataChanged;
        }

        private void SubscribeLocalPlayer(RoomPlayer player)
        {
            if (player)
                player.DataChanged += OnLocalPlayerDataChanged;
        }

        private RoomPlayer GetLocalPlayer()
        {
            if (!NetworkClient.localPlayer) return null;

            return NetworkClient.localPlayer.GetComponent<RoomPlayer>();
        }

        private void TryAttachLocalPlayer()
        {
            RoomPlayer player = GetLocalPlayer();

            if (player == _localPlayer)
                return;

            UnsubscribeLocalPlayer();
            _localPlayer = player;
            SubscribeLocalPlayer(_localPlayer);

            if (_localPlayer)
            {
                _localPlayer.CmdSetDefaultNickname();
            }
        }

        private RoomPlayer GetCachedLocalPlayer()
        {
            return _localPlayer ? _localPlayer : GetLocalPlayer();
        }
        
        private void OnNicknameChanged(string nickname)
        {
            var localPlayer = GetCachedLocalPlayer();
            if (!localPlayer)
                return;

            localPlayer.CmdSetNickname(nickname);
        }

        private void OnColorChanged(Color color)
        {
            var localPlayer = GetCachedLocalPlayer();
            if (!localPlayer)
                return;

            localPlayer.CmdSetColor(color);
        }

        private void OnReadyClicked()
        {
            var localPlayer = GetCachedLocalPlayer();
            if (!localPlayer)
                return;

            localPlayer.CmdChangeReadyState(!localPlayer.IsReady);
        }

        private void OnStartGameClicked()
        {
            var localPlayer = GetCachedLocalPlayer();
            if (!localPlayer)
                return;

            if (!_networkManager.IsHost(localPlayer))
                return;

            if (!NetworkServer.active)
                return;

            _networkManager.StartGame();
        }

        private void OnPlayersChanged()
        {
            TryAttachLocalPlayer();
            RefreshLobby();
        }

        private void OnLocalPlayerDataChanged()
        {
            RefreshLobby();
        }

        private void RefreshLobby()
        {
            List<RoomPlayer> players = _networkManager.GetRoomPlayers();
            var localPlayer = GetCachedLocalPlayer();

            _lobbyView.SetPlayers(players, _networkManager.IsHost);
            RefreshControls(players, localPlayer);

            if (!localPlayer)
                return;

            _lobbyView.SetNickname(localPlayer.Nickname);
            _lobbyView.SetSelectedColor(localPlayer.Color);
        }

        private void RefreshControls(List<RoomPlayer> players, RoomPlayer localPlayer)
        {
            var isHost = _networkManager.IsHost(localPlayer);

            _lobbyView.SetStartGameVisible(isHost);

            var canStart = false;

            if (isHost)
            {
                canStart = players.Count >= _networkManager.MinPlayersToStart;

                for (var i = 0; i < players.Count; i++)
                {
                    var player = players[i];

                    if (!player || !player.IsReady)
                    {
                        canStart = false;
                        break;
                    }
                }
            }

            _lobbyView.SetStartGameAvailable(canStart);
            _lobbyView.SetReadyButtonText(localPlayer && localPlayer.IsReady ? "Unready" : "Ready");
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            UnsubscribeLocalPlayer();

            _lobbyView.NicknameChanged -= OnNicknameChanged;
            _lobbyView.ColorChanged -= OnColorChanged;
            _lobbyView.ReadyClicked -= OnReadyClicked;
            _lobbyView.StartGameClicked -= OnStartGameClicked;

            _networkManager.PlayersChanged -= OnPlayersChanged;
        }
    }
}