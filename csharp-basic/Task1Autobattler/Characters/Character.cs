namespace Task1Autobattler.Characters;

public abstract class Character
{
    public string Name { get; }
    public int MaxHealth { get; }
    public int Health { get; private set; }
    public int BaseDamage { get; private set; }
    public bool IsAlive => Health > 0;
    
    protected Character(string name, int maxHealth, int baseDamage)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        BaseDamage = baseDamage;
    }

    public virtual void TakeDamage(int amount)
    {
        amount = Math.Max(0, amount);
        Health = Math.Max(0, Health - amount);
        Console.WriteLine($"{Name} получил {amount} урона. (HP: {Health}/{MaxHealth})");
    }

    public virtual int Heal(int amount)
    {
        int before = Health;
        Health = Math.Min(MaxHealth, Health + amount);
        return Health - before;
    }
    
    public abstract void Attack(Character target);
}