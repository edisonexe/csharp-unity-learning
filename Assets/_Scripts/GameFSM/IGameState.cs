using _Scripts.GameFSM;
using Events;
using Services;

namespace GameFSM
{
    public interface IGameState
    {
        GameState Type { get; }
        void Enter(GameStateService ctx);
        void Update(GameStateService ctx, GameEventType type, int value);
        void Exit(GameStateService ctx);
    }
}