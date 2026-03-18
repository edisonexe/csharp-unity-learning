using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.Configs;
using _Project._Scripts.Network;
using _Project._Scripts.Network.Player;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Match
{
    [RequireComponent(typeof(NetworkIdentity))]
    public sealed class MatchManager : NetworkBehaviour
    {
        private const int DEFAULT_MATCH_DURATION_SECONDS = 300;
        
        [SerializeField] private MatchConfig _matchConfig;

        [SyncVar(hook = nameof(OnRemainingSecondsChanged))]
        private int _remainingSeconds;

        [SyncVar(hook = nameof(OnMatchStateChanged))]
        private MatchState _matchState = MatchState.WaitingForPlayers;

        private readonly MatchPlayerStatsDictionary _playerStats = new();
        private readonly Dictionary<uint, GamePlayer> _playersByNetId = new();

        private Coroutine _timerCoroutine;
        private int _joinOrderCounter;

        public int RemainingSeconds => _remainingSeconds;
        
        public int MatchDurationSeconds => _matchConfig 
            ? _matchConfig.MatchDurationSeconds 
            : DEFAULT_MATCH_DURATION_SECONDS;
        public MatchState CurrentState => _matchState;
        public IReadOnlyDictionary<uint, MatchPlayerStats> PlayerStats => _playerStats;

        public event Action<int> RemainingSecondsChanged;
        public event Action<MatchState> MatchStateChanged;
        public event Action LeaderboardChanged;

        public override void OnStartServer()
        {
            base.OnStartServer();

            _remainingSeconds = MatchDurationSeconds;
            _matchState = MatchState.InProgress;

            RegisterExistingPlayers();
            _timerCoroutine = StartCoroutine(ServerMatchTimerRoutine());
        }

        public override void OnStopServer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }

            _playersByNetId.Clear();
            base.OnStopServer();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            _playerStats.OnAdd += OnStatsAdded;
            _playerStats.OnSet += OnStatsSet;
            _playerStats.OnRemove += OnStatsRemoved;
            _playerStats.OnClear += OnStatsCleared;

            RemainingSecondsChanged?.Invoke(_remainingSeconds);
            MatchStateChanged?.Invoke(_matchState);
            LeaderboardChanged?.Invoke();
        }

        public override void OnStopClient()
        {
            _playerStats.OnAdd -= OnStatsAdded;
            _playerStats.OnSet -= OnStatsSet;
            _playerStats.OnRemove -= OnStatsRemoved;
            _playerStats.OnClear -= OnStatsCleared;

            base.OnStopClient();
        }

        private void OnStatsAdded(uint key) => LeaderboardChanged?.Invoke();
        private void OnStatsSet(uint key, MatchPlayerStats oldValue) => LeaderboardChanged?.Invoke();
        private void OnStatsRemoved(uint key, MatchPlayerStats value) => LeaderboardChanged?.Invoke();
        private void OnStatsCleared() => LeaderboardChanged?.Invoke();

        [Server]
        private void RegisterExistingPlayers()
        {
            _playersByNetId.Clear();
            _joinOrderCounter = 0;

            GamePlayer[] players = FindObjectsByType<GamePlayer>(FindObjectsSortMode.None);

            foreach (GamePlayer player in players)
            {
                RegisterPlayer(player);
                player.ConstructMatch(this);
            }
        }

        [Server]
        public void RegisterPlayer(GamePlayer player)
        {
            if (!player)
                return;

            uint netId = player.netId;
            _playersByNetId[netId] = player;

            if (_playerStats.ContainsKey(netId))
                return;

            MatchPlayerStats stats = new MatchPlayerStats(
                netId,
                player.Nickname,
                0,
                0,
                0,
                _joinOrderCounter++);

            _playerStats[netId] = stats;
        }

        [Server]
        public void UnregisterPlayer(GamePlayer player)
        {
            if (!player)
                return;

            _playersByNetId.Remove(player.netId);
        }

        [Server]
        public bool CanUseGameplayActions(GamePlayer player)
        {
            if (_matchState != MatchState.InProgress)
                return false;

            if (!player)
                return false;

            if (!player.IsAlive)
                return false;

            if (player.IsGameplayBlocked)
                return false;

            return true;
        }

        [Server]
        public bool CanDealDamage(GamePlayer attacker, GamePlayer target)
        {
            if (_matchState != MatchState.InProgress)
                return false;

            if (!attacker || !target)
                return false;

            if (!attacker.IsAlive || !target.IsAlive)
                return false;

            if (attacker.IsGameplayBlocked || target.IsGameplayBlocked)
                return false;

            return true;
        }

        [Server]
        public void RegisterKill(GamePlayer killer, GamePlayer victim)
        {
            if (_matchState != MatchState.InProgress)
                return;

            if (victim)
                AddDeath(victim.netId);

            if (killer && killer != victim)
                AddKillAndScore(killer.netId);
        }

        [Server]
        private void AddKillAndScore(uint netId)
        {
            if (!_playerStats.TryGetValue(netId, out MatchPlayerStats stats))
                return;

            stats.Kills += 1;
            stats.Score = CalculateScore(stats.Kills, stats.Deaths);

            _playerStats[netId] = stats;
        }

        private int CalculateScore(int kills, int deaths)
        {
            if (deaths == 0)
                return kills * 100;

            return (int)((float)kills / deaths * 100f);
        }
        
        [Server]
        private void AddDeath(uint netId)
        {
            if (!_playerStats.TryGetValue(netId, out MatchPlayerStats stats))
                return;

            stats.Deaths += 1;
            stats.Score = CalculateScore(stats.Kills, stats.Deaths);

            _playerStats[netId] = stats;
        }

        [Server]
        private IEnumerator ServerMatchTimerRoutine()
        {
            while (_remainingSeconds > 0 && _matchState == MatchState.InProgress)
            {
                yield return new WaitForSeconds(1f);
                _remainingSeconds--;
            }

            if (_matchState == MatchState.InProgress)
                EndMatch();
        }

        [Server]
        public void EndMatch()
        {
            if (_matchState == MatchState.Ended)
                return;

            _matchState = MatchState.Ended;

            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }

            foreach (GamePlayer player in _playersByNetId.Values)
            {
                if (!player)
                    continue;

                player.ServerSetGameplayBlocked(true);
                player.ServerSetAliveState(false);
            }

            RpcNotifyMatchEnded();
        }

        [ClientRpc]
        private void RpcNotifyMatchEnded()
        {
            MatchStateChanged?.Invoke(_matchState);
            LeaderboardChanged?.Invoke();
        }

        public List<LeaderboardEntryData> GetSortedLeaderboard()
        {
            List<MatchPlayerStats> orderedStats = new List<MatchPlayerStats>(_playerStats.Values);

            orderedStats.Sort((left, right) =>
            {
                int scoreCompare = right.Score.CompareTo(left.Score);
                if (scoreCompare != 0)
                    return scoreCompare;

                int killsCompare = right.Kills.CompareTo(left.Kills);
                if (killsCompare != 0)
                    return killsCompare;

                return left.JoinOrder.CompareTo(right.JoinOrder);
            });

            List<LeaderboardEntryData> result = new List<LeaderboardEntryData>(orderedStats.Count);

            for (int i = 0; i < orderedStats.Count; i++)
            {
                MatchPlayerStats stats = orderedStats[i];

                result.Add(new LeaderboardEntryData(
                    i + 1,
                    stats.Nickname,
                    stats.Kills,
                    stats.Deaths,
                    stats.Score,
                    i == 0));
            }

            return result;
        }

        [Server]
        public void ReturnToLobby()
        {
            if (NetworkManager.singleton is not RoomNetworkManager roomNetworkManager)
                return;

            roomNetworkManager.ServerChangeScene(roomNetworkManager.RoomScene);
        }

        private void OnRemainingSecondsChanged(int oldValue, int newValue)
        {
            RemainingSecondsChanged?.Invoke(newValue);
        }

        private void OnMatchStateChanged(MatchState oldValue, MatchState newValue)
        {
            MatchStateChanged?.Invoke(newValue);
        }
    }
}