namespace Events
{
    public static class EventBus
    {
        public delegate void GameEvent(GameEventType type, int value);
        public static event GameEvent OnGameEvent;

        public static void Raise(GameEventType type, int value = 0)
        {
            OnGameEvent?.Invoke(type, value);
        }
        
        public static void Subscribe(GameEvent handler) => OnGameEvent += handler;
        public static void Unsubscribe(GameEvent handler) => OnGameEvent -= handler;
    }
}