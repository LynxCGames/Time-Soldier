using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;

namespace SpaceMarine;

public class Reseter : BloonsTD6Mod
{
    public static void SpecialReset()
    {
        foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
        {
            if (weapon.WeaponName == "Crossbow")
            {
                weapon.level = 1;
                weapon.cost = 15;
                weapon.isUnlocked = true;
                weapon.pierce = (int)weapon.StartingValues[0];
                weapon.speed = weapon.StartingValues[1];
                weapon.damage = (int)weapon.StartingValues[2];
            }
            else
            {
                weapon.level = 0;
                weapon.cost = 10;
                weapon.isUnlocked = false;
                weapon.pierce = (int)weapon.StartingValues[0];
                weapon.speed = weapon.StartingValues[1];
                weapon.damage = (int)weapon.StartingValues[2];
            }
        }

        foreach (var weapon in ModContent.GetContent<ComboTemplate>())
        {
            weapon.level = 0;
            weapon.isUnlocked = false;
            weapon.pierce = (int)weapon.StartingValues[0];
            weapon.speed = weapon.StartingValues[1];
            weapon.damage = (int)weapon.StartingValues[2];

            if (weapon.WeaponName == "???")
            {
                weapon.discovered = true;
            }
            if (weapon.WeaponName == "Necromancer")
            {
                weapon.discovered = false;
                weapon.level = 1;
            }
        }

        foreach (var modifier in ModContent.GetContent<ModifierTemplate>())
        {
            modifier.level = 0;
            modifier.cost = 12;
            modifier.isUnlocked = false;
            modifier.bonus = modifier.StartingValue;
            modifier.icon = modifier.Icon;
        }

        foreach (var modifier in ModContent.GetContent<ScavengerTemplate>())
        {
            modifier.level = 0;
            modifier.isUnlocked = false;
            modifier.bonus = modifier.StartingValue;
        }

        foreach (var modifier in ModContent.GetContent<SpecialTemplate>())
        {
            modifier.level = 0;
            modifier.cost = 15;
            modifier.isUnlocked = false;
            modifier.bonus = modifier.StartingValue;
        }
    }
}
