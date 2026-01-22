using System.Collections.Generic;
using _Scripts.GameFSM;

namespace GameFSM
{
    public class GameStateMachine
    {
        private readonly Dictionary<GameState, HashSet<GameState>> _allowedStates = new()
        {
            { GameState.Init, new HashSet<GameState> { GameState.Playing } },
            { GameState.Playing, new HashSet<GameState> { GameState.Init, GameState.Paused, GameState.Lose, GameState.Win } },
            { GameState.Paused, new HashSet<GameState> { GameState.Init, GameState.Playing } },
            { GameState.Lose, new HashSet<GameState> { GameState.Init } },
            { GameState.Win, new HashSet<GameState> { GameState.Init } }
        };

        public bool CanTransition(GameState from, GameState to) 
            => _allowedStates.ContainsKey(from) && _allowedStates[from].Contains(to);
    }
}