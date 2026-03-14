namespace _Project._Scripts.Interfaces
{
    public interface IGameHudView
    {
        void SetMode(string mode);
        void SetPlayersCount(int count);
        void SetPing(int pingMs);
        void SetHp(int cur, int max);
    }
}