using _Scripts.GameFSM;
using Events;
using Services;
using UnityEngine;

namespace GameFSM.States
{
    public class PlayingState : IGameState
    {
        public GameState Type => GameState.Playing;
        
        public void Enter(GameStateService ctx) => Time.timeScale = 1f;

        public void Update(GameStateService ctx, GameEventType type, int value)
        {
            switch (type)
            {
                case GameEventType.ItemPicked:
                    ctx.AddScore(value);
                    break;
                case GameEventType.PlayerDamaged:
                    ctx.ApplyDamage(value);
                    break;
                case GameEventType.GamePaused:
                    ctx.ChangeState(GameState.Paused);
                    break;
                case GameEventType.RestartRequested:
                    ctx.ChangeState(GameState.Init);
                    break;
            }
        }

        public void Exit(GameStateService ctx) { }
    }
}