using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace SpaceMarine;

public class ModifierMethods : BloonsTD6Mod
{
    public static void ModifierSelect(ModifierTemplate modifier, Tower tower)
    {
        foreach (var select in ModContent.GetContent<ModifierSelect>())
        {
            if (select.ModName == modifier.ModName)
            {
                select.EditTower(modifier, tower);
            }
        }
    }

    public static void ModifierLevels(ModifierTemplate modifier, Tower tower)
    {
        foreach (var level in ModContent.GetContent<ModifierLevel>())
        {
            if (level.ModName == modifier.ModName)
            {
                level.Level(modifier, tower);
            }
        }
    }

    public static void ScavengerLevels(ScavengerTemplate modifier, Tower tower)
    {
        foreach (var level in ModContent.GetContent<ScavengerLevel>())
        {
            level.Level(modifier, tower);
        }
    }

    public static void LevelEquipedModifier(ModifierTemplate modifier, Tower tower)
    {
        foreach (var equip in ModContent.GetContent<ModifierEquiped>())
        {
            if (equip.ModName == modifier.ModName)
            {
                equip.EditTower(modifier, tower);
            }
        }
    }
}