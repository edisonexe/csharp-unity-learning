using System;

public record GameEvent
{
    public GameEventType Type { get; }
    public DateTime Time { get; }
    public string Description { get; }

    public GameEvent(GameEventType type, DateTime time, string description)
    {
        Type = type;
        Time = time;
        Description = description;
    }
}
