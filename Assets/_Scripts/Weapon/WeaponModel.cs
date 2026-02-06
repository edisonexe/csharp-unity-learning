using Configs;

namespace Weapon
{
    using System;

    public sealed class WeaponModel
    {
        public event Action<int, int> AmmoChanged;

        private readonly WeaponConfig _config;
        private float _nextFireTime;
        private int _ammo;
        private readonly float _range;
        private float _reloadEndTime;
        public WeaponConfig Config => _config;
        public int Ammo => _ammo;
        public float Range => _range;

        public WeaponModel(WeaponConfig config)
        {
            _config = config;
            _ammo = config.MaxAmmo;
            _range = config.Range;
        }

        public void Refill()
        {
            _ammo = _config.MaxAmmo;
            AmmoChanged?.Invoke(_ammo, _config.MaxAmmo);
        }

        private bool CanFire(float time) => _ammo > 0 && time >= _nextFireTime;

        public bool TryConsumeShot(float time)
        {
            if (!CanFire(time)) return false;

            _ammo--;
            _nextFireTime = time + _config.FireRate;

            AmmoChanged?.Invoke(_ammo, _config.MaxAmmo);
            return true;
        }
    }

}