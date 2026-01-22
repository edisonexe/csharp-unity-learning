using System.Diagnostics;
using _Scripts.GameFSM;
using Events;
using Services;
using UnityEngine;

namespace GameFSM.States
{
    public class PausedState : IGameState
    {
        public GameState Type => GameState.Paused;
        public void Enter(GameStateService ctx) => Time.timeScale = 0f;

        public void Update(GameStateService ctx, GameEventType type, int value)
        {
            switch (type)
            {
                case GameEventType.GamePaused:
                    ctx.ChangeState(GameState.Playing);
                    break;
                case GameEventType.RestartRequested:
                    ctx.ChangeState(GameState.Init);
                    break;
            }
        }

        public void Exit(GameStateService ctx) => Time.timeScale = 1f;
    }
}