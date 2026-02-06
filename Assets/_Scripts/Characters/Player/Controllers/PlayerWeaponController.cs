using Input;
using Weapon;

namespace Characters.Player.Controllers
{
    public sealed class PlayerWeaponController
    {
        public void Update(PlayerInputReader input, WeaponSet weaponSet, WeaponModel currentWeapon)
        {
            if (!input) return;
            
            if (currentWeapon != null && input.ReloadPressedThisFrame && currentWeapon.Ammo < currentWeapon.Config.MaxAmmo)
                currentWeapon.Refill();

            if (weaponSet != null)
            {
                if (input.ScrollY > 0f) weaponSet.Next();
                else if (input.ScrollY < 0f) weaponSet.Prev();
            }
        }
    }
}