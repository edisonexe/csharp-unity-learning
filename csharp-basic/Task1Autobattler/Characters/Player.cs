using Task1Autobattler.Items;
using Task1Autobattler.Items.Potions;
using Task1Autobattler.Items.Weapons;

namespace Task1Autobattler.Characters;

public class Player : Character
{
    private int _temporaryDamageBuff;
    private int _buffTurnsLeft;
    
    private int _gold;
    private Weapon? _equippedWeapon;
    
    private readonly List<Item> _inventory = new ();
    public IReadOnlyList<Item> Inventory => _inventory.AsReadOnly();

    public Player(string name, int maxHealth, int baseDamage) : base(name, maxHealth, baseDamage)
    {
        AddItem(new HealingPotion());
        AddItem(new RagePotion());
        AddItem(new Axe());
        AddItem(new Sword());
        AddItem(new Dagger());
    }

    public int CurrentDamage => BaseDamage + (_equippedWeapon?.DamageBonus ?? 0) + _temporaryDamageBuff;

    public override void Attack(Character target)
    {
        int dmg = CurrentDamage;
        Console.WriteLine($"Герой атакует {target.Name} на {dmg} урона!");
        target.TakeDamage(dmg);
        TickBuff();
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (_equippedWeapon == weapon)
        {
            Console.WriteLine($"{weapon.Name} уже экипирован.");
            return;
        }
        UnequipWeapon();
        _equippedWeapon = weapon;
        weapon.Equip(this);
    }

    public void UnequipWeapon()
    {
        if (_equippedWeapon != null)
        {
            _equippedWeapon.Unequip(this);
            _equippedWeapon = null;
        }
    }

    public void Use(Item item) => item.Use(this);

    public void ApplyDamageBuff(int amount, int turns)
    {
        _temporaryDamageBuff += amount;
        _buffTurnsLeft = Math.Max(_buffTurnsLeft, turns);
    }

    private void TickBuff()
    {
        if (_buffTurnsLeft > 0)
        {
            _buffTurnsLeft--;
            if (_buffTurnsLeft == 0)
            {
                Console.WriteLine($"Бафф урона закончился (-{_temporaryDamageBuff} урона).");
                _temporaryDamageBuff = 0;
            }
        }
    }

    public void AddGold(int amount)
    {
        _gold += amount;
        Console.WriteLine($"Получено золото: +{amount}. Всего: {_gold}");
    }

    public void AddItem(Item item) => _inventory.Add(item);
    public bool RemoveItem(Item item) => _inventory.Remove(item);
    
    public void ShowStatus()
    {
        Console.WriteLine("==== Статус героя ====");
        Console.WriteLine($"Имя: {Name}");
        Console.WriteLine($"HP: {Health}/{MaxHealth}");
        Console.WriteLine($"Базовый урон: {BaseDamage}");
        Console.WriteLine($"Оружие: {(_equippedWeapon != null ? _equippedWeapon.Name + 
                                                               $" (+{_equippedWeapon.DamageBonus})" : "нет")}");
        Console.WriteLine($"Временный бонус к урону: {_temporaryDamageBuff}");
        Console.WriteLine($"Текущий урон атаки: {CurrentDamage}");
        Console.WriteLine($"Золото: {_gold}");
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