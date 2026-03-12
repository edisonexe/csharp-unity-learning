using _Project._Scripts.Network;
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

        private void Start()
        {
            if (!_gameHudView || !NetworkManager.singleton)
                return;

            var networkManager = NetworkManager.singleton as RoomNetworkManager; 
            if (!networkManager)
                return;
            
            _gameHudController = new GameHudController(_gameHudView, networkManager);
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