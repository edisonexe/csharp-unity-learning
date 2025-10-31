using Task1Autobattler.Characters;

namespace Task1Autobattler.Items;

public abstract class Item
{
    public string Name { get; }
    public int Price { get; }

    protected Item(string name, int price)
    {
        Name = name;
        Price = price;
    }

    public abstract void Use(Player player);
}