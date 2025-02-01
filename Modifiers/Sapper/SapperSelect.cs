using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;

namespace SpaceMarine;

public class SapperSelect : BloonsTD6Mod
{
    public static void Select(ModifierTemplate modifier, Tower tower)
    {
        if (mod.modifier1 == modifier.ModName || mod.modifier2 == modifier.ModName || mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            var damageModifier = new DamageModifierForTagModel("FortModifier", "Ceramic", modifier.bonus, 0, false, false);

            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.hasDamageModifiers = true);
            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(damageModifier));

            if (mod.modifier1 == "Piercing Shot" || mod.modifier2 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}