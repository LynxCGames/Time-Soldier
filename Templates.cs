using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace SpaceMarine
{
    // Weapon Templates
    public abstract class WeaponTemplate : ModContent
    {
        public override void Register() { }
        public abstract string WeaponName { get; }
        public abstract string Icon { get; }
        public abstract string Bonus { get; }
        public abstract string[] Combos { get; }
        public abstract float FontSize { get; }
        public abstract float NameSize { get; }
        public abstract float[] StartingValues { get; }
        public abstract string Range { get; }
        public abstract int[] PierceValue { get; }
        public int pierce = 1;
        public float speed = 1;
        public int damage = 1;
        public int level = 0;
        public float cost = 10;
        public bool isUnlocked = false;
    }

    public abstract class WeaponSelect : ModContent
    {
        public override void Register() { }
        public abstract string WeaponName { get; }
        public abstract void EditTower(WeaponTemplate weapon, Tower tower);
    }

    public abstract class WeaponLevel : ModContent
    {
        public override void Register() { }
        public abstract string WeaponName { get; }
        public abstract void Level(WeaponTemplate weapon);
    }

    public abstract class WeaponEquiped : ModContent
    {
        public override void Register() { }
        public abstract string WeaponName { get; }
        public abstract void EditTower(WeaponTemplate weapon, Tower tower);
    }

    // Combo Weapon Templates
    public abstract class ComboTemplate : ModContent
    {
        public override void Register() { }
        public abstract string WeaponName { get; }
        public abstract string[] comboWeapons { get; }
        public abstract string Icon { get; }
        public abstract string Bonus { get; }
        public abstract float FontSize { get; }
        public abstract string SpecialMods { get; }
        public abstract float[] StartingValues { get; }
        public abstract string Range { get; }
        public abstract int[] PierceValue { get; }
        public int pierce = 1;
        public float speed = 1;
        public int damage = 1;
        public int level = 0;
        public bool isUnlocked = false;
        public bool discovered = true;
    }

    public abstract class ComboSelect : ModContent
    {
        public override void Register() { }
        public abstract string WeaponName { get; }
        public abstract void EditTower(ComboTemplate weapon, Tower tower);
    }

    public abstract class ComboLevel : ModContent
    {
        public override void Register() { }
        public abstract string WeaponName { get; }
        public abstract void Level(WeaponTemplate weapon1, ComboTemplate combo);
    }

    public abstract class ComboEquiped : ModContent
    {
        public override void Register() { }
        public abstract string WeaponName { get; }
        public abstract void EditTower(ComboTemplate weapon, Tower tower);
    }

    // Modifier Templates
    public abstract class ModifierTemplate : ModContent
    {
        public override void Register() { }
        public abstract string ModName { get; }
        public abstract string Icon { get; }
        public abstract string Effect { get; }
        public abstract int MaxLevel { get; }
        public abstract float CostMultiplier { get; }
        public abstract float StartingCost { get; }
        public abstract float FontSize { get; }
        public abstract float NameSize { get; }
        public abstract float StartingValue { get; }
        public virtual string[] EraserProperties { get; }
        public virtual PrefabReference[] CharmSprite { get; }
        public abstract string Stat { get; }
        public float bonus = 1;
        public int level = 0;
        public float cost = 12;
        public bool isUnlocked = false;
        public string icon = "";
    }

    public abstract class ScavengerTemplate : ModContent
    {
        public override void Register() { }
        public abstract int MaxLevel { get; }
        public abstract string[] CostValues { get; }
        public abstract float[] Cost { get; }
        public abstract float StartingValue { get; }
        public float bonus = 1;
        public int level = 0;
        public bool isUnlocked = false;
    }

    public abstract class ModifierSelect : ModContent
    {
        public override void Register() { }
        public abstract string ModName { get; }
        public abstract void EditTower(ModifierTemplate modifier, Tower tower);
    }

    public abstract class ModifierLevel : ModContent
    {
        public override void Register() { }
        public abstract string ModName { get; }
        public abstract void Level(ModifierTemplate modifier, Tower tower);
    }

    public abstract class ScavengerLevel : ModContent
    {
        public override void Register() { }
        public abstract string ModName { get; }
        public abstract void Level(ScavengerTemplate modifier, Tower tower);
    }

    public abstract class ModifierEquiped : ModContent
    {
        public override void Register() { }
        public abstract string ModName { get; }
        public abstract void EditTower(ModifierTemplate modifier, Tower tower);
    }

    // Special Modifier Templates
    public abstract class SpecialTemplate : ModContent
    {
        public override void Register() { }
        public abstract string ModName { get; }
        public abstract string Icon { get; }
        public abstract string Weapon { get; }
        public virtual string[] LaserWeapons { get; }
        public abstract string WeaponIcon { get; }
        public abstract string Effect { get; }
        public abstract int MaxLevel { get; }
        public abstract float CostMultiplier { get; }
        public abstract float StartingCost { get; }
        public abstract float FontSize { get; }
        public abstract float NameSize { get; }
        public abstract float StartingValue { get; }
        public abstract string Stat { get; }
        public float bonus = 1;
        public int level = 0;
        public float cost = 15;
        public bool isUnlocked = false;
    }

    public abstract class SpecialSelect : ModContent
    {
        public override void Register() { }
        public abstract string SpecialName { get; }
        public abstract void EditTower(SpecialTemplate modifier, Tower tower);
    }

    public abstract class SpecialLevel : ModContent
    {
        public override void Register() { }
        public abstract string SpecialName { get; }
        public abstract void Level(SpecialTemplate modifier);
    }

    public abstract class SpecialEquiped : ModContent
    {
        public override void Register() { }
        public abstract string SpecialName { get; }
        public abstract void EditTower(SpecialTemplate modifier, Tower tower);
    }
}