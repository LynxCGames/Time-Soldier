using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;

namespace SpaceMarine;

public class WeaponMethods : BloonsTD6Mod
{
    public static void WeaponSelect(WeaponTemplate weapon, Tower tower)
    {
        foreach (var select in ModContent.GetContent<WeaponSelect>())
        {
            if (select.WeaponName == mod.weapon)
            {
                select.EditTower(weapon, tower);
            }
        }
    }

    public static void WeaponLevels(WeaponTemplate weapon, Tower tower)
    {
        foreach (var level in ModContent.GetContent<WeaponLevel>())
        {
            if (level.WeaponName == weapon.WeaponName)
            {
                level.Level(weapon);
            }
        }
    }

    public static void LevelEquipedWeapon(WeaponTemplate weapon, Tower tower)
    {
        foreach (var equip in ModContent.GetContent<WeaponEquiped>())
        {
            if (equip.WeaponName == mod.weapon)
            {
                equip.EditTower(weapon, tower);
            }
        }
    }
}
