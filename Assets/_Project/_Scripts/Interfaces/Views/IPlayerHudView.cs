namespace _Project._Scripts.Interfaces.Views
{
    public interface IPlayerHudView
    {
        void SetHp(int cur, int max);
        void SetMedkitsCount(int count);
        void SetGrenadesCount(int count);

        void Show();
        void Hide();
    }
}