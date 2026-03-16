using System;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Views;
using _Project._Scripts.Network;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.UI.Controllers
{
    public class ConnectionController : IDisposable
    {
        private readonly IConnectionBarView _connectionBarView;
        private readonly ILobbyView _lobbyView;
        private readonly RoomNetworkManager _networkManager;

        private bool _isConnecting;
        private bool _isStopping;
        private bool _hasConnectionError;
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

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _connectionBarView.HostClicked -= OnHostClicked;
            _connectionBarView.ClientClicked -= OnClientClicked;
            _connectionBarView.StopClicked -= OnStopClicked;

            if (_networkManager)
            {
                _networkManager.StatusChanged -= OnStatusChanged;
                _networkManager.ErrorOccurred -= OnErrorOccurred;
            }
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
            _hasConnectionError = false;
            _isConnecting = true;

            _connectionBarView.ClearError();
            ApplyConnectingState();

            _networkManager.networkAddress = _connectionBarView.Address;

            try
            {
                _networkManager.StartHost();
            }
            catch (Exception ex)
            {
                _hasConnectionError = true;
                _isConnecting = false;

                ApplyIdleState();
                _lobbyView.Hide();

                _connectionBarView.ShowStatus("Disconnected");
                _connectionBarView.ShowError(GetReadableHostStartError(ex));
            }
        }

        private void OnClientClicked()
        {
            _hasConnectionError = false;
            _isConnecting = true;

            _connectionBarView.ClearError();
            ApplyConnectingState();

            _networkManager.networkAddress = _connectionBarView.Address;

            try
            {
                _networkManager.StartClient();
            }
            catch (Exception ex)
            {
                _hasConnectionError = true;
                _isConnecting = false;

                ApplyIdleState();
                _lobbyView.Hide();

                _connectionBarView.ShowStatus("Disconnected");
                _connectionBarView.ShowError($"Failed to start client. {ex.GetBaseException().Message}");
            }
        }

        private void OnStopClicked()
        {
            _isStopping = true;
            _hasConnectionError = false;

            if (NetworkServer.active && NetworkClient.isConnected)
                _networkManager.StopHost();
            else if (NetworkClient.isConnected)
                _networkManager.StopClient();
            else if (NetworkServer.active)
                _networkManager.StopServer();
            else
                _isStopping = false;
        }

        private void OnStatusChanged(string status)
        {
            _connectionBarView.ShowStatus(status);

            if (status == "Connected")
            {
                _isStopping = false;
                _hasConnectionError = false;
                _isConnecting = true;
                
                ApplyConnectingState();
                _connectionBarView.ClearError();
                _lobbyView.Show();
                return;
            }
            if (status == "Disconnected")
            {
                ApplyIdleState();
                _lobbyView.Hide();

                if (_isStopping)
                {
                    _connectionBarView.ClearError();
                }
                else if (_isConnecting && !_hasConnectionError)
                {
                    _connectionBarView.ShowError("Failed to connect to host.");
                }

                _isStopping = false;
                _isConnecting = false;
            }
        }

        private void OnErrorOccurred(string error)
        {
            _hasConnectionError = true;
            _isConnecting = false;
            
            ApplyIdleState();
            _lobbyView.Hide();

            _connectionBarView.ShowError(error);
            _connectionBarView.ShowStatus("Disconnected");
        }

        private static string GetReadableHostStartError(Exception exception)
        {
            if (exception == null)
                return "Failed to start host.";

            Exception root = exception.GetBaseException();
            string message = root.Message;

            if (message.Contains("Only one usage of each socket address", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("обычно разрешается только одно использование адреса сокета", StringComparison.OrdinalIgnoreCase))
            {
                return "Failed to start host. The port is already in use.";
            }

            return $"Failed to start host. {message}";
        }
    }
}