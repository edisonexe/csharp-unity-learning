using Task1Autobattler.Characters;

namespace Task1Autobattler.Interfaces;

public interface IEquippable
{
    public void Equip(Player player);
    public void Unequip(Player player);
}
