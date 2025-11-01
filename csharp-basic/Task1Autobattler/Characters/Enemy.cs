namespace Task1Autobattler.Characters;

public class Enemy : Character
{
    private static readonly Random Rng = new Random();
    public int RewardGold { get; }

    public Enemy(string name, int hp, int dmg, int reward) : 
        base(name, hp, dmg) => RewardGold = reward;

    public static Enemy CreateRandom(int level)
    {
        level = int.Max(1, level);
        int hp = 25 + level * 5 + Rng.Next(-5, 6);
        int dmg = 4 + level + Rng.Next(0, 3);
        int reward = 15 + level * 5 + Rng.Next(0, 6);
        string[] names = { "Гоблин", "Разбойник", "Скелет"};
        string name = names[Rng.Next(names.Length)];
        return new Enemy(name, hp, dmg, reward);
    }

    public override void Attack(Character target)
    {
        Console.WriteLine($"{Name} бьёт героя на {BaseDamage} урона!");
        target.TakeDamage(BaseDamage);
    }
}