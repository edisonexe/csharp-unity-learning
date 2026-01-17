using Events;

namespace Services
{
    public class GameStateService
    {
        public GameState State { get; private set; } = GameState.Init;
        public int Score { get; private set; }

        private readonly int _targetScore;

        public GameStateService(int targetScore)
        {
            _targetScore = targetScore;
        }

        public void Enable() => EventBus.Subscribe(HandleGameEvent);

        public void Disable() => EventBus.Unsubscribe(HandleGameEvent);

        public void StartGame()
        {
            if (State != GameState.Init) return;
            State = GameState.Playing;
        }
        
        private void HandleGameEvent(GameEventType type, int value)
        {
            if (State != GameState.Playing) return;
            if (type != GameEventType.ItemPicked) return;
            
            var add = value <= 0 ? 1 : value;
            Score += add;

            EventBus.Raise(GameEventType.ScoreChanged, Score);

            if (Score >= _targetScore)
                State = GameState.Win;
        }
    }
}