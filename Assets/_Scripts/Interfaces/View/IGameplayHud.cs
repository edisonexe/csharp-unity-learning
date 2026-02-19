namespace _Scripts.Interfaces.View
{
    public interface IGameplayHud
    {
        void SetHp(int current, int max);
        void SetScore(int score);
        void SetInputMode(string mode);
        void Show();
        void Hide();
    }
}