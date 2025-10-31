using Task1Autobattler.Characters;
using Task1Autobattler.Interfaces;

namespace Task1Autobattler.Items;

public abstract class Potion : Item, IConsumable
{
    protected Potion(string name, int value) : base(name, value) { }
    public override void Use(Player player) => Consume(player);
    public abstract void Consume(Player player);
}