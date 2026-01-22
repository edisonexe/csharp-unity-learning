using System.Collections.Generic;
using _Scripts.GameFSM;
using Events;
using GameFSM;
using GameFSM.States;
using UnityEngine;

namespace Services
{
    public class GameStateService
    {
        public int Score { get; private set; }
        public int PlayerHp { get; private set; }

        private readonly int _targetScore;
        private readonly int _maxHp;

        private GameStateMachine _gameStateMachine = new();
        private readonly Dictionary<GameState, IGameState> _states;
        private IGameState _currentState;
        
        public GameState State => _currentState.Type;
        
        public GameStateService(int targetScore, int maxHp = 3)
        {
            _targetScore = targetScore;
            _maxHp = Mathf.Max(1, maxHp);
            
            _states = new Dictionary<GameState, IGameState>
            {
                { GameState.Init, new InitState() },
                { GameState.Playing, new PlayingState() },
                { GameState.Paused, new PausedState() },
                { GameState.Win, new WinState() },
                { GameState.Lose, new LoseState() },
            };
            
            PlayerHp = _maxHp;
            _currentState = _states[GameState.Init];
            _currentState.Enter(this);
            RaiseStateChanged();
        }

        public void Enable() => EventBus.Subscribe(HandleGameEvent);
        public void Disable() => EventBus.Unsubscribe(HandleGameEvent);

        public void StartGame()
        {
            if (State != GameState.Init) return;
            EventBus.ClearHistory();
            Score = 0;
            PlayerHp = _maxHp;
            EventBus.Raise(GameEventType.ScoreChanged, Score);
            EventBus.Raise(GameEventType.PlayerHpChanged, PlayerHp);
            
            ChangeState(GameState.Playing);
        }

        public void ChangeState(GameState to)
        {
            var from = _currentState.Type;
            if (from == to) return;
            if (!_gameStateMachine.CanTransition(from, to)) return;

            _currentState.Exit(this);
            _currentState = _states[to];
            _currentState.Enter(this);

            RaiseStateChanged();
        }
        
        public void AddScore(int value)
        {
            var add = value <= 0 ? 1 : value;
            Score += add;
            EventBus.Raise(GameEventType.ScoreChanged, Score);

            if (Score >= _targetScore)
                ChangeState(GameState.Win);
        }

        public void ApplyDamage(int value)
        {
            int dmg = value <= 0 ? 1 : value;
            PlayerHp -= dmg;
            if (PlayerHp < 0) PlayerHp = 0;

            EventBus.Raise(GameEventType.PlayerHpChanged, PlayerHp);

            if (PlayerHp <= 0)
                ChangeState(GameState.Lose);
        }
        
        private void RaiseStateChanged()
            => EventBus.Raise(GameEventType.GameStateChanged, (int)_currentState.Type);
        
        private void HandleGameEvent(GameEventType type, int value)
            => _currentState.Update(this, type, value);
    }
}