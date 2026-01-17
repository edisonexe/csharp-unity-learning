using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        private readonly List<string> _history = new();

        private void OnEnable() => EventBus.Subscribe(HandleGameEvent);

        private void OnDisable() => EventBus.Unsubscribe(HandleGameEvent);

        private void HandleGameEvent(GameEventType type, int value)
        {
            _history.Add($"{Time.time:F1}сек. | {type} | знач={value}");

            if (type == GameEventType.ScoreChanged)
                scoreText.text = $"Рекорд: {value}";
        }

        public void PrintHistory()
        {
            Debug.Log("=== ИСТОРИЯ СОБЫТИЙ ===");
            foreach (var line in _history)
                Debug.Log(line);
        }
    }   
}