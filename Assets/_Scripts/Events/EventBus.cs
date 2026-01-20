using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public static class EventBus
    {
        public readonly struct EventRecord
        {
            public readonly GameEventType Type;
            public readonly int Value;
            public readonly float Time;

            public EventRecord(GameEventType type, int value, float time)
            {
                Type = type;
                Value = value;
                Time = time;
            }
        }
        
        private static readonly List<EventRecord> _history = new();
        private static int _historyCapacity = 30;

        public static IReadOnlyList<EventRecord> History => _history;

        public static int HistoryCapacity
        {
            get => _historyCapacity;
            set => _historyCapacity = Mathf.Max(1, value);
        }

        public static void ClearHistory() => _history.Clear();
        
        public delegate void GameEvent(GameEventType type, int value);
        public static event GameEvent OnGameEvent;

        public static void Raise(GameEventType type, int value = 0)
        {
            AddToHistory(type, value);
            OnGameEvent?.Invoke(type, value);
        }
        
        public static void Subscribe(GameEvent handler) => OnGameEvent += handler;
        public static void Unsubscribe(GameEvent handler) => OnGameEvent -= handler;
        private static void AddToHistory(GameEventType type, int value)
        {
            _history.Add(new EventRecord(type, value, Time.unscaledTime));

            var overflow = _history.Count - _historyCapacity;
            if (overflow > 0)
                _history.RemoveRange(0, overflow);
        }

        
    }
}