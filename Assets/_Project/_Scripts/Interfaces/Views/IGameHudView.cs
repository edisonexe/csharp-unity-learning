namespace _Project._Scripts.Interfaces.Views
{
    public interface IGameHudView
    {
        void SetMode(string mode);
        void SetPlayersCount(int count);
        void SetPing(int pingMs);
    }
}