using System;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Network;
using _Project._Scripts.Network.Player;
using Mirror;

namespace _Project._Scripts.UI.Controllers
{
    public sealed class GameHudController : IDisposable
    {
        private readonly IGameHudView _view;
        private readonly RoomNetworkManager _networkManager;
        private bool _disposed;
        
        public GameHudController(IGameHudView view, RoomNetworkManager networkManager)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _networkManager = networkManager ?? throw new ArgumentNullException(nameof(networkManager));
    
            _networkManager.NetworkStateChanged += OnNetworkStateChanged;
            GamePlayer.PlayerCountChanged += OnPlayerCountChanged;
    
            RefreshMode();
            RefreshPlayersCount();
            RefreshPing();
        }

        public void Tick()
        {
            RefreshPing();
        }
        
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            
            GamePlayer.PlayerCountChanged -= OnPlayerCountChanged;
            _networkManager.NetworkStateChanged -= OnNetworkStateChanged;
        }

        private void OnPlayerCountChanged()
        {
            RefreshPlayersCount();
        }

        private void OnNetworkStateChanged()
        {
            RefreshMode();
        }

        private void RefreshMode()
        {
            var mode = "Offline";

            if (NetworkServer.active && NetworkClient.isConnected)
                mode = "Host";
            else if (NetworkClient.isConnected)
                mode = "Client";

            _view.SetMode(mode);
        }
        
        private void RefreshPlayersCount()
        {
            _view.SetPlayersCount(GamePlayer.PlayerCount);
        }

        private void RefreshPing()
        {
            var ping = NetworkTime.rtt > 0
                ? UnityEngine.Mathf.RoundToInt((float)(NetworkTime.rtt * 1000.0))
                : 0;

            _view.SetPing(ping);
        }
    }
}