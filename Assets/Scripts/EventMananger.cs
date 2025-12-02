using System;
using System.Collections.Generic;
using System.Linq;

public static class EventMananger
{
    public delegate void GameEventHandler(GameEvent e);
    public static event GameEventHandler OnGameEvent;

    private static readonly List<GameEvent> _eventHistory = new();
    public static IReadOnlyList<GameEvent> EventHistory => _eventHistory;

    public static void TriggerEvent(GameEvent gameEvent)
    {
        if (gameEvent == null)
            throw new ArgumentNullException(nameof(gameEvent));
        
        _eventHistory.Add(gameEvent);
        OnGameEvent?.Invoke(gameEvent);
    }

    public static IEnumerable<GameEvent> GetEventsByType(GameEventType eventType)
    {
        return _eventHistory.Where(e => e.Type == eventType); 
    }

    public static int GetEventCountByType(GameEventType eventType)
    {
        return _eventHistory.Count(e => e.Type == eventType);
    }

    public static IEnumerable<GameEvent> GetLastEventsByCount(int count)
    {
        return _eventHistory.OrderByDescending(e => e.Time).Take(count);
    }

    public static IEnumerable<(GameEventType, int Count)> GetMostFrequentEventsByCount(int topCount = 3)
    {
        return _eventHistory
            .GroupBy(g => g.Type)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => (g.Key, g.Count()));
    }
    
}