using _Scripts.GameFSM;
using Events;
using Services;

namespace GameFSM.States
{
    public class WinState : IGameState
    {
        public GameState Type => GameState.Win;
        public void Enter(GameStateService ctx)
        {
            UnityEngine.Time.timeScale = 1f;
            EventBus.Raise(GameEventType.Win);
        }

        public void Update(GameStateService ctx, GameEventType type, int value)
        {
            if (type == GameEventType.RestartRequested)
                    ctx.ChangeState(GameState.Init);
        }

        public void Exit(GameStateService ctx) { }
    }
}