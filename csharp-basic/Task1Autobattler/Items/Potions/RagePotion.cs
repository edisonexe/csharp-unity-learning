using Task1Autobattler.Characters;

namespace Task1Autobattler.Items.Potions;

public class RagePotion : Potion
{
    private int _damageBuff;
    private int _duration;
    public RagePotion() : base("Зелье ярости", 25)
    {
        _damageBuff = 3;
        _duration = 3;
    }
    public override void Consume(Player player)
    {
        player.ApplyDamageBuff(_damageBuff, _duration);
        Console.WriteLine($"Герой выпил {Name}: урон +{_damageBuff} на {_duration} ходов");
    }
}