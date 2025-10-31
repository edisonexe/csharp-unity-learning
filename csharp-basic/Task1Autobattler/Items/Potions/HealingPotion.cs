using Task1Autobattler.Characters;

namespace Task1Autobattler.Items.Potions;

public class HealingPotion : Potion
{
    public int HealAmount { get; }
    public HealingPotion() : base("Зелье лечения", 15) => HealAmount = 25;

    public override void Consume(Player player)
    {
        int healed = player.Heal(HealAmount);
        Console.WriteLine($"Герой использовал {Name}, здоровье восстановлено на {healed}");
    }
}