using System;
using _Project._Scripts.Gameplay.Match;
using _Project._Scripts.Interfaces.Views;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.UI.Controllers
{
    public sealed class MatchHudController : IDisposable
    {
        private readonly IGameHudView _gameHudView;
        private readonly IMatchResultsView _resultsView;
        private readonly MatchManager _matchManager;
        private bool _disposed;

        public MatchHudController(
            IGameHudView gameHudView,
            IMatchResultsView resultsView,
            MatchManager matchManager)
        {
            _gameHudView = gameHudView ?? throw new ArgumentNullException(nameof(gameHudView));
            _resultsView = resultsView ?? throw new ArgumentNullException(nameof(resultsView));
            _matchManager = matchManager ?? throw new ArgumentNullException(nameof(matchManager));

            Debug.Log($"[MatchHudController] Created. Server={NetworkServer.active}, Client={NetworkClient.isConnected}");

            _matchManager.RemainingSecondsChanged += OnRemainingSecondsChanged;
            _matchManager.MatchStateChanged += OnMatchStateChanged;
            _matchManager.LeaderboardChanged += OnLeaderboardChanged;

            RefreshTimer(_matchManager.RemainingSeconds);
            RefreshState(_matchManager.CurrentState);
            RefreshLeaderboard();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _matchManager.RemainingSecondsChanged -= OnRemainingSecondsChanged;
            _matchManager.MatchStateChanged -= OnMatchStateChanged;
            _matchManager.LeaderboardChanged -= OnLeaderboardChanged;
        }

        private void OnRemainingSecondsChanged(int remainingSeconds)
        {
            RefreshTimer(remainingSeconds);
        }

        private void OnMatchStateChanged(MatchState state)
        {
            Debug.Log($"[MatchHudController] MatchStateChanged: {state}. Server={NetworkServer.active}, Client={NetworkClient.isConnected}");
            RefreshState(state);
        }

        private void OnLeaderboardChanged()
        {
            RefreshLeaderboard();
        }

        private void RefreshTimer(int remainingSeconds)
        {
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;
            string timerText = $"{minutes:00}:{seconds:00}";

            _gameHudView.SetMatchTimer(timerText);
        }

        private void RefreshState(MatchState state)
        {
            if (state == MatchState.Ended)
            {
                _resultsView.Show();
                _resultsView.SetReturnButtonVisible(NetworkServer.active && NetworkClient.isConnected);
                return;
            }

            _resultsView.Hide();
        }

        private void RefreshLeaderboard()
        {
            _resultsView.SetRows(_matchManager.GetSortedLeaderboard());
        }
    }
}