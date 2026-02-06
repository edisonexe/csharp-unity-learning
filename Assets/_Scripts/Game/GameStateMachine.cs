using System;
using UnityEngine;

namespace Game
{
    public class GameStateMachine
    {
        public GameState CurrentState { get; private set; } = GameState.Start;
        public event Action<GameState> OnStateChanged;

        public void StartSession()
        {
            Set(GameState.Playing);
        }

        public void Win()
        {
            if (CurrentState != GameState.Playing) return;
            Set(GameState.Win);
        }

        public void Lose()
        {
            if (CurrentState != GameState.Playing) return;
            Set(GameState.Lose);
        }
        
        private void Set(GameState next)
        {
            if (CurrentState == next) return;
            
            CurrentState = next;

            OnStateChanged?.Invoke(CurrentState);
        }
    }
}