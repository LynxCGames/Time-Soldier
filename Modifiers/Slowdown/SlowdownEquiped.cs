using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppSystem.IO;
using static SpaceMarine.SpaceMarine;

namespace SpaceMarine;

public class SlowdownEquiped : BloonsTD6Mod
{
    public static void Level(ModifierTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        foreach (var behavior in towerModel.GetAttackModel().GetDescendants<SlowModel>().ToArray())
        {
            if (behavior.name.Contains("SlowdownMod"))
            {
                behavior.multiplier = 1 - (modifier.bonus / 100);
            }
        }
        foreach (var behavior in towerModel.GetAttackModel().GetDescendants<SlowModifierForTagModel>().ToArray())
        {
            if (behavior.name.Contains("SlowdownMod"))
            {
                behavior.slowMultiplier = 1 - (modifier.bonus / 200);
            }
        }

        if (mod.modifier1 == "Piercing Shot" || mod.modifier2 == "Piercing Shot")
        {
            PiercingShotMod.PiercingShot(towerModel);
        }

        tower.UpdateRootModel(towerModel);
    }
}