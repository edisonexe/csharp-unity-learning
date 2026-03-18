using System.Collections.Generic;
using _Project._Scripts.Gameplay.Match;
using _Project._Scripts.Interfaces.Views;
using UnityEngine;

namespace _Project._Scripts.UI.Views
{
    public sealed class MatchResultsView : MonoBehaviour, IMatchResultsView
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private GameObject _returnButton;
        [SerializeField] private Transform _contentRoot;
        [SerializeField] private LeaderboardRowView _rowPrefab;
        
        private readonly List<LeaderboardRowView> _spawnedRows = new();
        
        private void Awake()
        {
            if (!_root || !_contentRoot || !_rowPrefab || !_returnButton)
            {
                Debug.LogError("[MatchResultsView] References are not assigned.", this);
                enabled = false;
                return;
            }

            Hide();
        }

        public void Show()
        {
            if (_root)
                _root.SetActive(true);
        }

        public void Hide()
        {
            if (_root)
                _root.SetActive(false);
        }

        public void SetRows(IReadOnlyList<LeaderboardEntryData> entries)
        {
            if (!_contentRoot || !_rowPrefab)
                return;

            EnsureRowCount(entries.Count);

            for (int i = 0; i < _spawnedRows.Count; i++)
            {
                bool shouldBeVisible = i < entries.Count;
                _spawnedRows[i].gameObject.SetActive(shouldBeVisible);

                if (!shouldBeVisible)
                    continue;

                _spawnedRows[i].Bind(entries[i]);
            }
        }

        public void SetReturnButtonVisible(bool visible)
        {
            if (_returnButton)
                _returnButton.SetActive(visible);
        }

        private void EnsureRowCount(int targetCount)
        {
            while (_spawnedRows.Count < targetCount)
            {
                LeaderboardRowView rowView = Instantiate(_rowPrefab, _contentRoot);
                _spawnedRows.Add(rowView);
            }
        }
    }
}