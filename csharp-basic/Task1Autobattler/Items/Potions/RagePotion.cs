using Task1Autobattler.Characters;

namespace Task1Autobattler.Items.Potions;

public class RagePotion : Potion
{
    public int DamageBuff { get; }
    public int Duration { get; }
    public RagePotion() : base("Зелье ярости", 25)
    {
        DamageBuff = 3;
        Duration = 3;
    }
    public override void Consume(Player player)
    {
        player.ApplyDamageBuff(DamageBuff, Duration);
        Console.WriteLine($"Герой выпил {Name}: урон +{DamageBuff} на {Duration} ходов");
    }
}