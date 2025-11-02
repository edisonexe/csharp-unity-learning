using Task1Autobattler.Characters;
using Task1Autobattler.Interfaces;

namespace Task1Autobattler.Items;

public abstract class Weapon : Item, IEquippable
{
    public int DamageBonus { get; }
    protected Weapon(string name, int price, int damageBonus) : base(name, price)
    {
        DamageBonus = damageBonus;
    }
    public override void Use(Player player) => player.EquipWeapon(this);

    public virtual void Equip(Player player)
    {
        Console.WriteLine($"Герой экипировал {Name} (+{DamageBonus} к урону)");
    }
    public virtual void Unequip(Player player)
    {
        Console.WriteLine($"Герой снял {Name} (-{DamageBonus} к урону)");
    }
}