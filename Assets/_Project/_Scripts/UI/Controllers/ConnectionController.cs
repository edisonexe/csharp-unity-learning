using System;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Network;
using Mirror;

namespace _Project._Scripts.UI.Controllers
{
    public class ConnectionController : IDisposable
    {
        private readonly IConnectionBarView _connectionBarView;
        private readonly ILobbyView _lobbyView;
        private readonly RoomNetworkManager _networkManager;

        private bool _disposed;

        public ConnectionController(
            IConnectionBarView connectionBarView,
            ILobbyView lobbyView,
            RoomNetworkManager networkManager)
        {
            _connectionBarView = connectionBarView ?? throw new ArgumentNullException(nameof(connectionBarView));
            _lobbyView = lobbyView ?? throw new ArgumentNullException(nameof(lobbyView));
            _networkManager = networkManager ?? throw new ArgumentNullException(nameof(networkManager));

            Subscribe();
            InitializeView();
        }

        private void InitializeView()
        {
            _connectionBarView.Show();
            _lobbyView.Hide();

            ApplyIdleState();
            _connectionBarView.ClearError();
            _connectionBarView.ShowStatus("Disconnected");
        }

        private void Subscribe()
        {
            _connectionBarView.HostClicked += OnHostClicked;
            _connectionBarView.ClientClicked += OnClientClicked;
            _connectionBarView.StopClicked += OnStopClicked;

            _networkManager.StatusChanged += OnStatusChanged;
            _networkManager.ErrorOccurred += OnErrorOccurred;
        }

        private void ApplyIdleState()
        {
            _connectionBarView.SetAddressInteractable(true);
            _connectionBarView.SetConnectButtonsInteractable(true);
            _connectionBarView.SetStopButtonVisible(false);
            _connectionBarView.SetStopButtonInteractable(false);
        }

        private void ApplyConnectingState()
        {
            _connectionBarView.SetAddressInteractable(false);
            _connectionBarView.SetConnectButtonsInteractable(false);
            _connectionBarView.SetStopButtonVisible(true);
            _connectionBarView.SetStopButtonInteractable(true);
        }

        private void OnHostClicked()
        {
            _connectionBarView.ClearError();
            ApplyConnectingState();

            _networkManager.networkAddress = _connectionBarView.Address;
            _networkManager.StartHost();
        }

        private void OnClientClicked()
        {
            _connectionBarView.ClearError();
            ApplyConnectingState();

            _networkManager.networkAddress = _connectionBarView.Address;
            _networkManager.StartClient();
        }

        private void OnStopClicked()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
                _networkManager.StopHost();
            else if (NetworkClient.isConnected)
                _networkManager.StopClient();
            else if (NetworkServer.active)
                _networkManager.StopServer();
        }

        private void OnStatusChanged(string status)
        {
            _connectionBarView.ShowStatus(status);

            if (status == "Connected")
            {
                ApplyConnectingState();
                _connectionBarView.ClearError();
                _lobbyView.Show();
            }
            else if (status == "Disconnected")
            {
                ApplyIdleState();
                _lobbyView.Hide();
            }
        }

        private void OnErrorOccurred(string error)
        {
            ApplyIdleState();
            _lobbyView.Hide();

            _connectionBarView.ShowError(error);
            _connectionBarView.ShowStatus("Disconnected");
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _connectionBarView.HostClicked -= OnHostClicked;
            _connectionBarView.ClientClicked -= OnClientClicked;
            _connectionBarView.StopClicked -= OnStopClicked;

            if (_networkManager != null)
            {
                _networkManager.StatusChanged -= OnStatusChanged;
                _networkManager.ErrorOccurred -= OnErrorOccurred;
            }
        }
    }
}