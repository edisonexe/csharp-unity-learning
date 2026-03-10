using _Project._Scripts.Controllers.Lobby;
using _Project._Scripts.Network;
using _Project._Scripts.UI;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Bootstrap
{
    public class LobbyCompositionRoot : MonoBehaviour
    {
        [SerializeField] private ConnectionBarView _connectionBarView;
        [SerializeField] private LobbyView _lobbyView;

        private ConnectionController _connectionController;
        private LobbyController _lobbyController;

        private void Start()
        {
            if (!_connectionBarView || !_lobbyView)
            {
                Debug.LogError("[LobbyCompositionRoot] Views are not assigned.");
                return;
            }

            if (!_connectionBarView.Init() || !_lobbyView.Init())
            {
                Debug.LogError("[LobbyCompositionRoot] View init failed.");
                return;
            }
            
            RoomNetworkManager networkManager = NetworkManager.singleton as RoomNetworkManager;

            if (!networkManager)
            {
                Debug.LogError("[LobbyCompositionRoot] RoomNetworkManager singleton not found.");
                return;
            }

            _connectionController = new ConnectionController(_connectionBarView, _lobbyView, networkManager);
            _lobbyController = new LobbyController(_lobbyView, networkManager);
        }

        private void OnDestroy()
        {
            _connectionController?.Dispose();
            _lobbyController?.Dispose();
        }
    }
}