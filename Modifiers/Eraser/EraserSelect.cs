using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using System;

namespace SpaceMarine;

public class EraserSelect : BloonsTD6Mod
{
    public static void Select(ModifierTemplate modifier, Tower tower)
    {
        if (mod.modifier1 == modifier.ModName || mod.modifier2 == modifier.ModName || mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            var cleanse = new RemoveBloonModifiersModel("Eraser", true, false, false, false, false, Array.Empty<string>(), Array.Empty<string>());

            if (modifier.level >= 2)
            {
                cleanse.cleanseCamo = true;
            }
            if (modifier.level >= 3)
            {
                cleanse.cleanseFortified = true;
            }

            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.collisionPasses = new int[] { 0, -1 });
            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(cleanse));

            if (mod.modifier1 == "Piercing Shot" || mod.modifier2 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}