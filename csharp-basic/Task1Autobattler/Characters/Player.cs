using Task1Autobattler.Items;
using Task1Autobattler.Items.Potions;
using Task1Autobattler.Items.Weapons;

namespace Task1Autobattler.Characters;

public class Player : Character
{
    private int temporaryDamageBuff;
    private int buffTurnsLeft;
    
    public int Gold { get; private set; } = 50;
    public Weapon? EquippedWeapon { get; private set; }
    public List<Item> Inventory { get; private set; } = new List<Item>();

    public Player(string name, int maxHealth, int baseDamage) : base(name, maxHealth, baseDamage)
    {
        Inventory.Add(new HealingPotion());
        Inventory.Add(new RagePotion());
        Inventory.Add(new Axe());
        Inventory.Add(new Sword());
        Inventory.Add(new Dagger());
    }

    public int CurrentDamage => BaseDamage + (EquippedWeapon?.DamageBonus ?? 0) + temporaryDamageBuff;

    public override void Attack(Character target)
    {
        int dmg = CurrentDamage;
        Console.WriteLine($"Герой атакует {target.Name} на {dmg} урона!");
        target.TakeDamage(dmg);
        TickBuff();
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (EquippedWeapon == weapon)
        {
            Console.WriteLine($"{weapon.Name} уже экипирован.");
            return;
        }
        UnequipWeapon();
        EquippedWeapon = weapon;
        weapon.Equip(this);
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon != null)
        {
            EquippedWeapon.Unequip(this);
            EquippedWeapon = null;
        }
    }

    public void Use(Item item) => item.Use(this);

    public void ApplyDamageBuff(int amount, int turns)
    {
        temporaryDamageBuff += amount;
        buffTurnsLeft = Math.Max(buffTurnsLeft, turns);
    }

    private void TickBuff()
    {
        if (buffTurnsLeft > 0)
        {
            buffTurnsLeft--;
            if (buffTurnsLeft == 0)
            {
                Console.WriteLine($"Бафф урона закончился (-{temporaryDamageBuff} урона).");
                temporaryDamageBuff = 0;
            }
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        Console.WriteLine($"Получено золото: +{amount}. Всего: {Gold}");
    }

    public void ShowStatus()
    {
        Console.WriteLine("==== Статус героя ====");
        Console.WriteLine($"Имя: {Name}");
        Console.WriteLine($"HP: {Health}/{MaxHealth}");
        Console.WriteLine($"Базовый урон: {BaseDamage}");
        Console.WriteLine($"Оружие: {(EquippedWeapon != null ? EquippedWeapon.Name + 
                                                               $" (+{EquippedWeapon.DamageBonus})" : "нет")}");
        Console.WriteLine($"Временный бонус к урону: {temporaryDamageBuff}");
        Console.WriteLine($"Текущий урон атаки: {CurrentDamage}");
        Console.WriteLine($"Золото: {Gold}");
        Console.WriteLine("Инвентарь:");
        if (Inventory.Count == 0) Console.WriteLine("  (пусто)");
        else
        {
            for (int i = 0; i < Inventory.Count; i++)
                Console.WriteLine($"  {i + 1}. {Inventory[i].Name}");
        }
        Console.WriteLine("======================");
    }
}