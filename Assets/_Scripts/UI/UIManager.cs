using System.Collections.Generic;
using System.Text;
using Events;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private GameObject _winText;
        
        private readonly List<string> _history = new();

        private void Awake() => _winText.SetActive(false);

        private void Start()
        {
            if (!_scoreText) Debug.LogError("ScoreText не задан!");
            if (!_winText) Debug.LogError("WinText не задан!");
        }

        private void OnEnable() => EventBus.Subscribe(HandleGameEvent);

        private void OnDisable() => EventBus.Unsubscribe(HandleGameEvent);

        public void PrintHistory()
        {
            if (_history.Count == 0)
            {
                Debug.Log("=== ИСТОРИЯ СОБЫТИЙ: ПУСТО ===");
                return;
            }

            var sb = new StringBuilder(256);
            sb.AppendLine("===== ИСТОРИЯ СОБЫТИЙ =====");

            foreach (var line in _history)
                sb.AppendLine(line);

            sb.AppendLine("=========================");

            Debug.Log(sb.ToString());
        }
        
        private void HandleGameEvent(GameEventType type, int value)
        {
            string time = FormatTime(Time.time);
            string entry = FormatEntry(time, type, value);

            _history.Add(entry);

            if (type == GameEventType.ScoreChanged && _scoreText)
                _scoreText.text = $"Счёт: {value}";
            
            if(type == GameEventType.Win && _winText)
                _winText.SetActive(true);
        }
        
        private static string FormatEntry(string time, GameEventType type, int value)
        {
            return value != 0
                ? $"[{time}] {type} (+{value})"
                : $"[{time}] {type}";
        }

        private static string FormatTime(float timeSeconds)
        {
            int minutes = Mathf.FloorToInt(timeSeconds / 60f);
            float seconds = timeSeconds % 60f;
            return $"{minutes:00}:{seconds:00.00}";
        }
    }   
}