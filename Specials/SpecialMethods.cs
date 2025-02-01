using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace SpaceMarine;

public class SpecialMethods : BloonsTD6Mod
{
    public static void SpecialSelect(SpecialTemplate modifier, Tower tower)
    {
        foreach (var select in ModContent.GetContent<SpecialSelect>())
        {
            if (select.SpecialName == modifier.ModName)
            {
                select.EditTower(modifier, tower);
            }
        }
    }

    public static void SpecialLevels(SpecialTemplate modifier, Tower tower)
    {
        foreach (var level in ModContent.GetContent<SpecialLevel>())
        {
            if (level.SpecialName == modifier.ModName)
            {
                level.Level(modifier);
            }
        }
    }

    public static void LevelEquipedSpecial(SpecialTemplate modifier, Tower tower)
    {
        foreach (var equip in ModContent.GetContent<SpecialEquiped>())
        {
            if (equip.SpecialName == modifier.ModName)
            {
                equip.EditTower(modifier, tower);
            }
        }
    }
}