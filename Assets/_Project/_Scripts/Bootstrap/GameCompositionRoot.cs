using _Project._Scripts.Network;
using _Project._Scripts.Network.Player;
using _Project._Scripts.UI.Controllers;
using _Project._Scripts.UI.Views;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Bootstrap
{
    public sealed class GameCompositionRoot : MonoBehaviour
    {
        [SerializeField] private GameHudView _gameHudView;

        private GameHudController _gameHudController;

        private void OnEnable()
        {
            GamePlayer.LocalPlayerSpawned += OnLocalPlayerSpawned;
        }

        private void OnDisable()
        {
            GamePlayer.LocalPlayerSpawned -= OnLocalPlayerSpawned;
        }

        private void Start()
        {
            if (!_gameHudView || !NetworkManager.singleton)
                return;

            var networkManager = NetworkManager.singleton as RoomNetworkManager;
            if (!networkManager)
                return;

            _gameHudController = new GameHudController(_gameHudView, networkManager);
        }

        private void OnLocalPlayerSpawned(GamePlayer player)
        {
            if (player == null)
                return;

            if (_gameHudView == null)
            {
                Debug.LogWarning("[GameCompositionRoot] GameHudView is missing.");
                return;
            }

            player.ConstructLocal(_gameHudView);
        }

        private void Update()
        {
            _gameHudController?.Tick();
        }

        private void OnDestroy()
        {
            _gameHudController?.Dispose();
        }
    }
}