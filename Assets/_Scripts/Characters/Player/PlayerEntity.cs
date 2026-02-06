using System;
using System.Collections.Generic;
using Configs;
using Weapon;

namespace Characters.Player
{
    public sealed class PlayerEntity
    {
        public int CurrentHp { get; private set; }
        public int MaxHp { get; }
        public int Damage { get; }
        public float MoveSpeed { get; }
        public bool IsAlive => CurrentHp > 0;
        public WeaponSet Weapons { get; }
        public event Action<int, int> HpChanged;
        public event Action Died;

        public PlayerEntity(PlayerConfig cfg)
        {
            MaxHp = cfg.MaxHp;
            CurrentHp = cfg.MaxHp;
            Damage = cfg.Damage;
            MoveSpeed = cfg.MoveSpeed;
            Weapons = CreateWeaponSet(cfg.WeaponCfgs);
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
        
        private WeaponSet CreateWeaponSet(WeaponConfig[] cfgs)
        {
            var models = new List<WeaponModel>(cfgs.Length);
            foreach (var cfg in cfgs)
                models.Add(new WeaponModel(cfg));

            return new WeaponSet(models);
        }
    }

}