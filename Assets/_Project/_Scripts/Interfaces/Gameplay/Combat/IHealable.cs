namespace _Project._Scripts.Interfaces.Gameplay.Combat
{
    public interface IHealable
    {
        int CurrentHp { get; }
        int MaxHp { get; }
        int Restore(int amount);
    }
}