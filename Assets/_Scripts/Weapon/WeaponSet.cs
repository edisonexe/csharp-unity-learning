using System;
using System.Collections.Generic;

namespace Weapon
{
    public sealed class WeaponSet
    {
        public event Action<WeaponModel> Changed;

        private readonly List<WeaponModel> _weapons;
        private int _index;

        public WeaponModel Current => _weapons[_index];

        public WeaponSet(IEnumerable<WeaponModel> weapons)
        {
            _weapons = new List<WeaponModel>(weapons);
            if (_weapons.Count == 0)
                throw new ArgumentException("WeaponSet requires at least 1 weapon");
            _index = 0;
        }

        public void Next()
        {
            _index = (_index + 1) % _weapons.Count;
            Changed?.Invoke(Current);
        }

        public void Prev()
        {
            _index = (_index - 1 + _weapons.Count) % _weapons.Count;
            Changed?.Invoke(Current);
        }
    }

}