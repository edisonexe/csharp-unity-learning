using _Scripts.Interfaces;

namespace _Scripts.GameFlow
{
    public class GameResultModel : IGameResult
    {
        public GameResult Current { get; private set; }
        
        public void Set(GameResult result) => Current = result;
    }
}