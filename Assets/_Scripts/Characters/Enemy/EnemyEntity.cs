using UnityEngine;

namespace Characters.Enemy
{
    public sealed class EnemyEntity
    {
        public int CurrentHp { get; private set; }
        public int MaxHp { get; }
        public bool IsDead => CurrentHp <= 0;
        public float MoveSpeed { get; }
        public int Damage { get; }
        public float AttackCooldown { get; }

        
        private float _nextAttackTime;

        public EnemyEntity(int maxHp, float moveSpeed, int damage, float attackCooldown)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;
            MoveSpeed = moveSpeed;
            Damage = damage;
            AttackCooldown = attackCooldown;
            _nextAttackTime = 0f;
        }

        public bool CanAttack(float time)
        {
            return time >= _nextAttackTime;
        }

        public void MarkAttack(float time)
        {
            _nextAttackTime = time + AttackCooldown;
        }

        public void TakeDamage(int amount)
        {
            if (CurrentHp < 0 || amount <= 0) return;
            CurrentHp = Mathf.Max(0, CurrentHp - amount);
        }

    }
}