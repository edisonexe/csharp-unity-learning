using System;
using System.Collections.Generic;
using _Project._Scripts.Network.Player;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Network
{
    public class RoomNetworkManager : NetworkRoomManager
    {
        [Header("Lobby Settings")]
        [SerializeField, Min(1)] private int _minPlayersToStart = 2;

        [Header("Game Settings")]
        [SerializeField] private GameObject _gamePlayerPrefab;

        public int MinPlayersToStart => _minPlayersToStart;

        public event Action<string> StatusChanged;
        public event Action<string> ErrorOccurred;
        public event Action PlayersChanged;
        public event Action NetworkStateChanged;
        
        public override void OnStartHost()
        {
            base.OnStartHost();
            StatusChanged?.Invoke("Starting host");
            NotifyNetworkStateChanged();
        }

        public override void OnStopHost()
        {
            base.OnStopHost();
            NotifyNetworkStateChanged();
            NotifyPlayersChanged();
        }
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            StatusChanged?.Invoke("Connecting");
            NotifyNetworkStateChanged();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            NotifyNetworkStateChanged();
            NotifyPlayersChanged();
        }
        
        public override void OnClientConnect()
        {
            base.OnClientConnect();
            StatusChanged?.Invoke("Connected");
            NotifyNetworkStateChanged();
            NotifyPlayersChanged();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            StatusChanged?.Invoke("Disconnected");
            NotifyNetworkStateChanged();
            NotifyPlayersChanged();
        }

        public override void OnClientError(TransportError error, string reason)
        {
            base.OnClientError(error, reason);

            string message = string.IsNullOrWhiteSpace(reason)
                ? error.ToString()
                : $"{error}: {reason}";

            ErrorOccurred?.Invoke(message);
        }

        public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnRoomServerAddPlayer(conn);
        
            NotifyPlayersChanged();
            NotifyNetworkStateChanged();
        }

        public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnRoomServerDisconnect(conn);
            NotifyPlayersChanged();
            NotifyNetworkStateChanged();
        }

        public override void OnRoomServerPlayersReady()
        {
            NotifyPlayersChanged();
        }

        public override GameObject OnRoomServerCreateGamePlayer(
            NetworkConnectionToClient conn,
            GameObject roomPlayerObject)
        {
            if (!_gamePlayerPrefab)
            {
                Debug.LogError("[RoomNetworkManager] Game Player Prefab is not assigned.");
                return null;
            }

            Transform startPosition = GetStartPosition();
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            if (startPosition)
            {
                position = startPosition.position;
                rotation = startPosition.rotation;
            }

            return Instantiate(_gamePlayerPrefab, position, rotation);
        }

        public override bool OnRoomServerSceneLoadedForPlayer(
            NetworkConnectionToClient conn,
            GameObject roomPlayerObject,
            GameObject gamePlayerObject)
        {
            if (!roomPlayerObject)
            {
                Debug.LogError("[RoomNetworkManager] roomPlayerObject is null.");
                return false;
            }

            if (!gamePlayerObject)
            {
                Debug.LogError("[RoomNetworkManager] gamePlayerObject is null.");
                return false;
            }

            RoomPlayer roomPlayer = roomPlayerObject.GetComponent<RoomPlayer>();
            GamePlayer gamePlayer = gamePlayerObject.GetComponent<GamePlayer>();

            if (!roomPlayer)
            {
                Debug.LogError("[RoomNetworkManager] RoomPlayer component not found.");
                return false;
            }

            if (!gamePlayer)
            {
                Debug.LogError("[RoomNetworkManager] GamePlayer component not found.");
                return false;
            }

            gamePlayer.Initialize(roomPlayer.Nickname, roomPlayer.Color);
            return true;
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);
            NotifyNetworkStateChanged();
        }
        
        [Server]
        private bool CanStartGame()
        {
            List<RoomPlayer> players = GetRoomPlayers();

            if (players.Count < _minPlayersToStart)
                return false;

            for (var i = 0; i < players.Count; i++)
            {
                RoomPlayer player = players[i];

                if (!player || !player.IsReady)
                    return false;
            }

            return true;
        }

        [Server]
        public void StartGame()
        {
            if (!CanStartGame())
                return;

            ServerChangeScene(GameplayScene);
        }

        public List<RoomPlayer> GetRoomPlayers()
        {
            List<RoomPlayer> result = new List<RoomPlayer>();

            foreach (NetworkRoomPlayer slot in roomSlots)
            {
                if (slot is RoomPlayer player)
                    result.Add(player);
            }

            return result;
        }

        public bool IsHost(RoomPlayer player)
        {
            return player && player.index == 0;
        }

        public void NotifyPlayersChanged()
        {
            PlayersChanged?.Invoke();
        }
        
        private void NotifyNetworkStateChanged()
        {
            NetworkStateChanged?.Invoke();
        }
        
        
    }
}