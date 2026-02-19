using _Scripts.GameFlow;

namespace _Scripts.Interfaces
{
    public interface IGameResult
    {
        GameResult Current { get; }
        void Set(GameFlow.GameResult result);
    }
}