using System.Text;
using _Scripts.GameFSM;
using Events;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private GameObject _winText;
        [SerializeField] private GameObject _loseText;
        
        [SerializeField] private TMP_Text _stateText;
        [SerializeField] private TMP_Text _pauseButtonLabel;
        [SerializeField] private TMP_Text _hpText;
        
        [SerializeField][Min(1)] private int _historyLimit = 20;
        
        private GameState _currentState = GameState.Init;

        private void Awake()
        {
            if (_winText) _winText.SetActive(false);
            if (_loseText) _loseText.SetActive(false);

            SetState(_currentState);
        }

        private void Start()
        {
            if (!_scoreText) Debug.LogError("ScoreText не задан!");
            if (!_winText) Debug.LogError("WinText не задан!");
            if (!_loseText) Debug.LogWarning("[UIManager]: LoseText не задан!");
            if (!_stateText) Debug.LogWarning("[UIManager]: StateText не задан!");
        }

        private void OnEnable() => EventBus.Subscribe(HandleGameEvent);

        private void OnDisable() => EventBus.Unsubscribe(HandleGameEvent);

        public void PrintHistory()
        {
            if (EventBus.History.Count == 0)
            {
                Debug.Log("=== ИСТОРИЯ СОБЫТИЙ: ПУСТО ===");
                return;
            }

            var sb = new StringBuilder(256);
            sb.AppendLine("===== ИСТОРИЯ СОБЫТИЙ =====");

            var history = EventBus.History;
            int n = Mathf.Clamp(_historyLimit, 1, history.Count);
            int startIndex = history.Count - n;

            for (var i = startIndex; i < history.Count; i++)
            {
                var e = history[i];
                var time = FormatTime(e.Time);
                var entry = FormatEntry(time, e.Type, e.Value);
                sb.AppendLine(entry);
            }
            
            sb.AppendLine("=========================");

            Debug.Log(sb.ToString());

        }
        
        public void OnPauseContinueClicked()
        {
            if (_currentState is GameState.Win or GameState.Lose)
                return;
            
            EventBus.Raise(GameEventType.GamePaused, 1);
        }

        public void OnRestartClicked() => EventBus.Raise(GameEventType.RestartRequested, 1);
        
        private void HandleGameEvent(GameEventType type, int value)
        {
            if (type == GameEventType.ScoreChanged && _scoreText)
                _scoreText.text = $"Счёт: {value}";

            if (type == GameEventType.GameStateChanged)
            {
                _currentState = (GameState)value;
                SetState(_currentState);
            }
            
            if (type == GameEventType.PlayerHpChanged && _hpText)
            {
                _hpText.text = $"Здоровье: {value}";
            }

            if (type == GameEventType.Win)
            {
                if (_winText) _winText.SetActive(true);
                if (_loseText) _loseText.SetActive(false);
            }

            if (type == GameEventType.Lose)
            {
                if (_loseText) _loseText.SetActive(true);
                if (_winText) _winText.SetActive(false);
            }
        }
        
        private static string FormatEntry(string time, GameEventType type, int value)
        {
            return value != 0
                ? $"[{time}] {type} ({value})"
                : $"[{time}] {type}";
        }

        private static string FormatTime(float timeSeconds)
        {
            int minutes = Mathf.FloorToInt(timeSeconds / 60f);
            float seconds = timeSeconds % 60f;
            return $"{minutes:00}:{seconds:00.00}";
        }

        private void SetState(GameState state)
        {
            if (_stateText)
            {
                _stateText.text = state switch
                {
                    GameState.Init => "Init",
                    GameState.Playing => "Игра идёт",
                    GameState.Paused => "Пауза",
                    GameState.Win => "Победа!",
                    GameState.Lose => "Поражение",
                    _ => state.ToString()
                };
            }

            if (_pauseButtonLabel)
            {
                _pauseButtonLabel.text = state == GameState.Paused ? "Продолжить" : "Пауза";
            }
        }
    }   
}