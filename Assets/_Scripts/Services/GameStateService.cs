using Events;
using UnityEngine;

namespace Services
{
    public class GameStateService
    {
        public GameState State { get; private set; } = GameState.Init;
        public int Score { get; private set; }
        public int PlayerHp { get; private set; }

        private readonly int _targetScore;
        private readonly int _maxHp;

        public GameStateService(int targetScore, int maxHp = 3)
        {
            _targetScore = targetScore;
            _maxHp = Mathf.Max(1, maxHp);
            PlayerHp = _maxHp;
        }

        public void Enable() => EventBus.Subscribe(HandleGameEvent);
        public void Disable() => EventBus.Unsubscribe(HandleGameEvent);

        public void StartGame()
        {
            if (State != GameState.Init) return;
            EventBus.ClearHistory();
            Score = 0;
            PlayerHp = _maxHp;
            ChangeState(GameState.Playing);
            EventBus.Raise(GameEventType.ScoreChanged, Score);
            EventBus.Raise(GameEventType.PlayerHpChanged, PlayerHp);
        }

        private void HandleGameEvent(GameEventType type, int value)
        {
            switch (type)
            {
                case GameEventType.ItemPicked:
                    HandleItemPicked(value);
                    break;

                case GameEventType.GamePaused:
                    TogglePause();
                    break;

                case GameEventType.PlayerDamaged:
                    HandlePlayerDamaged(value);
                    break;

                case GameEventType.RestartRequested:
                    ChangeState(GameState.Init);
                    Time.timeScale = 1f;
                    break;
            }
        }
        
        private void HandleItemPicked(int value)
        {
            if (State != GameState.Playing) return;
            
            var add = value <= 0 ? 1 : value;
            Score += add;

            EventBus.Raise(GameEventType.ScoreChanged, Score);

            if (Score >= _targetScore)
            {
                ChangeState(GameState.Win);
                EventBus.Raise(GameEventType.Win);
            }
        }
        
        private void HandlePlayerDamaged(int value)
        {
            if (State != GameState.Playing) return;

            int dmg = value <= 0 ? 1 : value;
            PlayerHp -= dmg;
            EventBus.Raise(GameEventType.PlayerHpChanged, PlayerHp);

            if (PlayerHp <= 0)
            {
                PlayerHp = 0;
                ChangeState(GameState.Lose);
                EventBus.Raise(GameEventType.Lose);
            }
        }

        private void TogglePause()
        {
            if (State == GameState.Playing)
            {
                ChangeState(GameState.Paused);
                Time.timeScale = 0f;
            }
            else if (State == GameState.Paused)
            {
                Time.timeScale = 1f;
                ChangeState(GameState.Playing);
            }
        }

        private void ChangeState(GameState newState)
        {
            if (State == newState) return;
            State = newState;
            EventBus.Raise(GameEventType.GameStateChanged, (int)newState);
        }
    }
}