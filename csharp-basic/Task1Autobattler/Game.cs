using Task1Autobattler.Characters;
using Task1Autobattler.Items;
using Task1Autobattler.Items.Potions;
using Task1Autobattler.Items.Weapons;

namespace Task1Autobattler;

public static class Game
{
    public static void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var player = new Player("Герой", maxHealth: 100, baseDamage: 5);
        
        int level = 1;
        Enemy enemy = Enemy.CreateRandom(level);

        while (player.IsAlive)
        {
            Console.WriteLine("\n============================");
            Console.WriteLine($"Герой: {player.Name} (HP: {player.Health}/{player.MaxHealth}, " +
                              $"урон: {player.CurrentDamage})");
            Console.WriteLine($"Противник: {enemy.Name} (HP: {enemy.Health}/{enemy.MaxHealth}, " +
                              $"урон: {enemy.BaseDamage})");
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1) Атаковать");
            Console.WriteLine("2) Использовать зелье");
            Console.WriteLine("3) Выбрать оружие из инвентаря");
            Console.WriteLine("4) Использовать зелье из инвентаря");
            Console.WriteLine("5) Показать статус героя");
            Console.WriteLine("0) Выход");
            Console.Write("Твой выбор: ");

            string? input = Console.ReadLine();
            Console.WriteLine();
            if (!int.TryParse(input, out int choice)) continue;

            bool enemyGetsTurn = false;

            switch (choice)
            {
                case 1: // Attack
                    player.Attack(enemy);
                    enemyGetsTurn = true;
                    break;
                case 2: // Use potion
                    var pot = player.Inventory.OfType<Potion>().FirstOrDefault();
                    if (pot == null) Console.WriteLine("Нет зелий в инвентаре.");
                    else {
                        player.Use(pot);
                        enemyGetsTurn = true;
                    }
                    break;
                case 3: // Select weapon to equip
                    SelectWeapon(player);
                    break;
                case 4: // Select potion to use
                    SelectPotion(player, out bool used);
                    if (used) enemyGetsTurn = true;
                    break;
                case 5:
                    player.ShowStatus();
                    break;
                case 0:
                    Console.WriteLine("Выход из игры.");
                    return;
                default:
                    continue;
            }

            // Enemy turn
            if (enemyGetsTurn && enemy.IsAlive)
            {
                enemy.Attack(player);
                if (!player.IsAlive) break;
            }

            // Check enemy death
            if (!enemy.IsAlive)
            {
                Console.WriteLine($"Враг повержен! Награда: {enemy.RewardGold} золота.");
                player.AddGold(enemy.RewardGold);
                level++;
                enemy = Enemy.CreateRandom(level);
                Console.WriteLine($"На арену выходит новый враг: {enemy.Name}!\n");
            }
        }

        Console.WriteLine("Герой пал в бою. Игра окончена ;(");
    }
    private static void SelectWeapon(Player player)
    {
        var weapons = player.Inventory.OfType<Weapon>().ToList();
        if (weapons.Count == 0)
        {
            Console.WriteLine("Нет оружия в инвентаре.");
            return;
        }
        Console.WriteLine("Выберите оружие для экипировки (0 — отмена):");
        for (int i = 0; i < weapons.Count; i++)
        {
            var w = weapons[i];
            Console.WriteLine($"{i + 1}) {w.Name} (+{w.DamageBonus} урона)");
        }
        Console.Write("Ваш выбор: ");
        if (int.TryParse(Console.ReadLine(), out int idx))
        {
            if (idx == 0) return;
            if (idx >= 1 && idx <= weapons.Count)
                player.Use(weapons[idx - 1]);
        }
    }

    private static void SelectPotion(Player player, out bool used)
    {
        used = false;
        var potions = player.Inventory.OfType<Potion>().ToList();
        if (potions.Count == 0)
        {
            Console.WriteLine("Нет зелий в инвентаре.");
            return;
        }
        Console.WriteLine("Выберите зелье для использования (0 — отмена):");
        for (int i = 0; i < potions.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {potions[i].Name}");
        }
        Console.Write("Ваш выбор: ");
        if (int.TryParse(Console.ReadLine(), out int idx))
        {
            if (idx == 0) return;
            if (idx >= 1 && idx <= potions.Count)
            {
                player.Use(potions[idx - 1]);
                used = true;
            }
        }
    }
}