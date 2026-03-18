using _Project._Scripts.Gameplay.Match;
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
        [SerializeField] private MatchManager _matchManager;
        [SerializeField] private GameHudView _gameHudView;
        [SerializeField] private PlayerHudView _playerHudView;
        [SerializeField] private MatchResultsView _matchResultsView;

        private GameHudController _gameHudController;
        private MatchHudController _matchHudController;

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
            if (NetworkManager.singleton is not RoomNetworkManager networkManager)
                return;

            if (_gameHudView)
                _gameHudController = new GameHudController(_gameHudView, networkManager);

            if (_gameHudView && _matchResultsView && _matchManager)
            {
                _matchHudController = new MatchHudController(
                    _gameHudView,
                    _matchResultsView,
                    _matchManager);
            }
        }

        private void OnLocalPlayerSpawned(GamePlayer player)
        {
            if (!player || !_playerHudView)
                return;

            player.ConstructLocal(_playerHudView);
        }

        private void Update()
        {
            _gameHudController?.Tick();
        }

        private void OnDestroy()
        {
            _gameHudController?.Dispose();
            _matchHudController?.Dispose();
        }
    }
}