using _Scripts.GameFSM;
using Events;
using Services;
using UnityEngine;

namespace GameFSM.States
{
    public class InitState : IGameState
    {
        public GameState Type => GameState.Init;
        public void Enter(GameStateService ctx) => Time.timeScale = 1f;

        public void Update(GameStateService ctx, GameEventType type, int value) { }

        public void Exit(GameStateService ctx) { }
    }
}