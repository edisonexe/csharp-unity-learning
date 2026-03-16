using _Project._Scripts.Network.Player;

namespace _Project._Scripts.Interfaces.Gameplay.Combat
{
    public interface IDamageable
    {
        void TakeDamage(int damage, GamePlayer player);
    }
}