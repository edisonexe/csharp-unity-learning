using Task1Autobattler.Characters;

namespace Task1Autobattler.Items.Potions;

public class HealingPotion : Potion
{
    private int _healAmount;
    public HealingPotion() : base("Зелье лечения", 15) => _healAmount = 25;

    public override void Consume(Player player)
    {
        var healed = player.Heal(_healAmount);
        Console.WriteLine($"Герой использовал {Name}, здоровье восстановлено на {healed}");
    }
}