using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;

namespace SpaceMarine;

public class ComboMethods : BloonsTD6Mod
{
    public static void ComboSelect(ComboTemplate weapon, Tower tower)
    {
        foreach (var select in ModContent.GetContent<ComboSelect>())
        {
            if (select.WeaponName == mod.weapon && select.WeaponName == weapon.WeaponName)
            {
                select.EditTower(weapon, tower);
            }
        }
    }

    public static void ComboLevels(WeaponTemplate weapon, ComboTemplate combo)
    {
        foreach (var level in ModContent.GetContent<ComboLevel>())
        {
            if (level.WeaponName == combo.WeaponName)
            {
                level.Level(weapon, combo);
            }
        }
    }

    public static void LevelEquipedCombo(ComboTemplate weapon, Tower tower)
    {
        foreach (var equip in ModContent.GetContent<ComboEquiped>())
        {
            if (equip.WeaponName == mod.weapon)
            {
                equip.EditTower(weapon, tower);
            }
        }
    }

    public static void ComboWeaponSwitcher(WeaponTemplate weapon, ComboTemplate combo)
    {
        if ((mod.weapon == combo.comboWeapons[0] && weapon.WeaponName == combo.comboWeapons[1]) | (mod.weapon == combo.comboWeapons[1] && weapon.WeaponName == combo.comboWeapons[0]))
        {
            mod.weapon = combo.WeaponName;
        }
    }
}
