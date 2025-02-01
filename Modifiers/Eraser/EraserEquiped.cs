using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppSystem.IO;
using static SpaceMarine.SpaceMarine;

namespace SpaceMarine;

public class EraserEquiped : BloonsTD6Mod
{
    public static void Level(ModifierTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        foreach (var behavior in towerModel.GetAttackModel().GetDescendants<RemoveBloonModifiersModel>().ToArray())
        {
            if (modifier.level >= 2)
            {
                behavior.cleanseCamo = true;
            }
            if (modifier.level >= 3)
            {
                behavior.cleanseFortified = true;
            }
        }

        if (mod.modifier1 == "Piercing Shot" || mod.modifier2 == "Piercing Shot")
        {
            PiercingShotMod.PiercingShot(towerModel);
        }

        tower.UpdateRootModel(towerModel);
    }
}