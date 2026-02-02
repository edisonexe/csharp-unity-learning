using System;

namespace Characters.Player
{
    public sealed class PlayerEntity
    {
        public int CurrentHp { get; private set; }
        public int MaxHp { get; }
        public int Damage { get; }
        public float MoveSpeed { get; }
        public bool IsAlive => CurrentHp > 0;

        public event Action<int, int> HpChanged;
        public event Action Died;

        public PlayerEntity(int maxHp,  int damage, float moveSpeed)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;
            Damage = damage;
            MoveSpeed = moveSpeed;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || CurrentHp <= 0) return;

            CurrentHp = Math.Max(0, CurrentHp - amount);
            HpChanged?.Invoke(CurrentHp, MaxHp);

            if (CurrentHp == 0)
            {
                Died?.Invoke();
            }
        }
    }

}