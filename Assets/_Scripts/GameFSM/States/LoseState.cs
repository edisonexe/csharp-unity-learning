using _Scripts.GameFSM;
using Events;
using Services;
using UnityEngine;

namespace GameFSM.States
{
    public class LoseState : IGameState
    {
        public GameState Type => GameState.Lose;
        public void Enter(GameStateService ctx)
        {
            Time.timeScale = 1f;
            EventBus.Raise(GameEventType.Lose);
        }

        public void Update(GameStateService ctx, GameEventType type, int value)
        {
            if (type == GameEventType.RestartRequested)
                ctx.ChangeState(GameState.Init);
        }

        public void Exit(GameStateService ctx) { }
    }
}