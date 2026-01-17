namespace _Scripts.EventBus
{
    public static class EventBus
    {
        public delegate void GameEvent(GameEventType type, int value);
        public static event GameEvent OnGameEvent;

        public static void RaiseGameEvent(GameEventType type, int value)
        {
            OnGameEvent?.Invoke(type, value);
        }
    }
}