using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppSystem.IO;
using static SpaceMarine.SpaceMarine;

namespace SpaceMarine;

public class MoabEquiped : BloonsTD6Mod
{
    public static void Level(ModifierTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        foreach (var behavior in towerModel.GetAttackModel().GetDescendants<DamageModifierForTagModel>().ToArray())
        {
            if (behavior.name.Contains("MoabModifier"))
            {
                behavior.damageMultiplier = modifier.bonus;
            }
        }

        if (mod.modifier1 == "Piercing Shot" || mod.modifier2 == "Piercing Shot")
        {
            PiercingShotMod.PiercingShot(towerModel);
        }

        tower.UpdateRootModel(towerModel);
    }
}