public static class GameEventExtensions
{
    public static string ToLogString(this GameEvent e)
    {
        if (e == null) return string.Empty;
        
        return $"<color=blue>[{e.Time:HH:mm:ss}]</color> " +
               $"<color=black><b> [{e.Type.ToString()}]</b> {e.Description} </color>";
    }
}