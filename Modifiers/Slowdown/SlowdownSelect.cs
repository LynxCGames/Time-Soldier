using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;

namespace SpaceMarine;

public class SlowdownSelect : BloonsTD6Mod
{
    public static void Select(ModifierTemplate modifier, Tower tower)
    {
        if (mod.modifier1 == modifier.ModName || mod.modifier2 == modifier.ModName || mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            var slowModel = new SlowModel("SlowdownMod", 1 - (modifier.bonus / 100), 3, "Slow:Weak", 999, null, true, false, null, false, false, false);
            var slowModifier = new SlowModifierForTagModel("SlowdownMod", "Moabs", "Slow:Weak", 1 - (modifier.bonus / 200), false, true, 0, false);

            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.collisionPasses = new int[] { 0, -1 });
            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(slowModel));
            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(slowModifier));

            if (mod.modifier1 == "Piercing Shot" || mod.modifier2 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}