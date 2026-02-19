using System;
using System.Collections.Generic;
using _Scripts.Interfaces;

namespace _Scripts.GSM
{
    public sealed class GameStateMachine
    {
        public IGameState CurrentState { get; private set; }
        private readonly Dictionary<Type, IGameState> _states = new();
        private readonly ILoggerService _logger;
        
        public GameStateMachine(ILoggerService logger)
        {
            _logger  = logger;
        }

        public void Register<TState>(TState state)  where TState : class, IGameState
        {
            var key = typeof(TState);
            if (!_states.TryAdd(key, state))
                _logger.Warn($"State {key.Name} already registered");
        }
        
        public void ChangeState<TState>() where TState : IGameState
        {
            var key = typeof(TState);
            if (!_states.TryGetValue(key, out var next))
            {
                _logger.Warn($"State {key.Name} not registered");
                return;
            }
            
            if (ReferenceEquals(CurrentState, next))
                return;
            
            var prevStateName = CurrentState?.GetType().Name ?? "<none>";
            
            CurrentState?.Exit();
            CurrentState = next;
            _logger.Log($"[GSM]: {prevStateName} -> {key.Name}");
            CurrentState.Enter();
        }

        public void Clear()
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
                CurrentState = null;
            }

            _states.Clear();
        }

    }
}