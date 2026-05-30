// Герої
public abstract class Hero
{
    public abstract string GetDescription();
    public abstract int GetAttackPower();
    public abstract int GetDefense();
}

public class Warrior : Hero
{
    public override string GetDescription() => "Воїн";
    public override int GetAttackPower() => 25;
    public override int GetDefense() => 20;
}

public class Mage : Hero
{
    public override string GetDescription() => "Маг";
    public override int GetAttackPower() => 30;
    public override int GetDefense() => 10;
}

public class Paladin : Hero
{
    public override string GetDescription() => "Паладин";
    public override int GetAttackPower() => 20;
    public override int GetDefense() => 25;
}

public abstract class InventoryDecorator : Hero
{
    protected Hero _hero;

    public InventoryDecorator(Hero hero)
    {
        _hero = hero;
    }

    public override string GetDescription() => _hero.GetDescription();
    public override int GetAttackPower() => _hero.GetAttackPower();
    public override int GetDefense() => _hero.GetDefense();
}


// Інвентар
public class ArmorDecorator : InventoryDecorator
{
    private readonly int _defenseBonus;

    public ArmorDecorator(Hero hero, int defenseBonus = 15) : base(hero)
    {
        _defenseBonus = defenseBonus;
    }

    public override string GetDescription() => $"{_hero.GetDescription()} + Броня (+{_defenseBonus} захисту)";

    public override int GetDefense() => _hero.GetDefense() + _defenseBonus;
}

public class WeaponDecorator : InventoryDecorator
{
    private readonly int _attackBonus;
    private readonly string _weaponName;

    public WeaponDecorator(Hero hero, int attackBonus = 20, string weaponName = "Меч") : base(hero)
    {
        _attackBonus = attackBonus;
        _weaponName = weaponName;
    }

    public override string GetDescription() => $"{_hero.GetDescription()} + {_weaponName} (+{_attackBonus} атаки)";

    public override int GetAttackPower() => _hero.GetAttackPower() + _attackBonus;
}

public class ArtifactDecorator : InventoryDecorator
{
    private readonly int _attackBonus;
    private readonly int _defenseBonus;
    private readonly string _artifactName;

    public ArtifactDecorator(Hero hero, int attackBonus = 10, int defenseBonus = 10, string artifactName = "Артефакт") : base(hero)
    {
        _attackBonus = attackBonus;
        _defenseBonus = defenseBonus;
        _artifactName = artifactName;
    }

    public override string GetDescription() => $"{_hero.GetDescription()} + {_artifactName} (+{_attackBonus} атаки, +{_defenseBonus} захисту)";

    public override int GetAttackPower() => _hero.GetAttackPower() + _attackBonus;
    public override int GetDefense() => _hero.GetDefense() + _defenseBonus;
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("1. СТВОРЕННЯ ВОЇНА");
        Hero warrior = new Warrior();
        DisplayHeroStats(warrior);

        Console.WriteLine("\nВоїн одягає броню");
        warrior = new ArmorDecorator(warrior, 15);
        DisplayHeroStats(warrior);

        Console.WriteLine("\nВоїн бере меч");
        warrior = new WeaponDecorator(warrior, 25, "Дворучний меч");
        DisplayHeroStats(warrior);

        Console.WriteLine("\nВоїн отримує артефакт");
        warrior = new ArtifactDecorator(warrior, 15, 10, "Амулет сили");
        DisplayHeroStats(warrior);

        Console.WriteLine("\n==========================================\n");

        Console.WriteLine("2. СТВОРЕННЯ МАГА");
        Hero mage = new Mage();
        DisplayHeroStats(mage);

        Console.WriteLine("\nМаг отримує чарівний посох");
        mage = new WeaponDecorator(mage, 30, "Чарівний посох");
        DisplayHeroStats(mage);

        Console.WriteLine("\nМаг вдягає магічну мантію");
        mage = new ArmorDecorator(mage, 8);
        DisplayHeroStats(mage);

        Console.WriteLine("\nМаг отримує артефакт мудрості");
        mage = new ArtifactDecorator(mage, 20, 5, "Книга заклинань");
        DisplayHeroStats(mage);

        Console.WriteLine("\n==========================================\n");

        Console.WriteLine("3. СТВОРЕННЯ ПАЛАДИНА");
        Hero paladin = new Paladin();
        DisplayHeroStats(paladin);

        Console.WriteLine("\nПаладин екіпірується повністю");
        paladin = new ArmorDecorator(paladin, 20);
        paladin = new WeaponDecorator(paladin, 30, "Святий меч");
        paladin = new ArtifactDecorator(paladin, 15, 20, "Реліквія світла");
        DisplayHeroStats(paladin);

        Console.WriteLine("\n==========================================\n");

        Console.WriteLine("4. РІЗНІ КОМБІНАЦІЇ ІНВЕНТАРЯ ДЛЯ ВОЇНА");

        Hero warrior2 = new Warrior();

        Hero warriorWithWeapon = new WeaponDecorator(new Warrior(), 35, "Сокира");
        Console.WriteLine("\nКомбінація 1 (тільки зброя):");
        DisplayHeroStats(warriorWithWeapon);

        Hero warriorWithWeaponAndArmor = new ArmorDecorator(
            new WeaponDecorator(new Warrior(), 25, "Спис"), 18);
        Console.WriteLine("\nКомбінація 2 (зброя + броня):");
        DisplayHeroStats(warriorWithWeaponAndArmor);

        Hero warriorWithTwoArtifacts = new ArtifactDecorator(
            new ArtifactDecorator(new Warrior(), 12, 8, "Перстень вогню"),
            10, 12, "Амулет захисту");
        Console.WriteLine("\nКомбінація 3 (два артефакти):");
        DisplayHeroStats(warriorWithTwoArtifacts);

        Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
        Console.ReadKey();
    }

    static void DisplayHeroStats(Hero hero)
    {
        Console.WriteLine($"Герой: {hero.GetDescription()}");
        Console.WriteLine($"Сила атаки: {hero.GetAttackPower()}");
        Console.WriteLine($"Захист: {hero.GetDefense()}");
    }
}