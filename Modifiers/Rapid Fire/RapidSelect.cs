using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace SpaceMarine;

public class RapidSelect : BloonsTD6Mod
{
    public static void Select(ModifierTemplate modifier, Tower tower)
    {
        if (mod.modifier1 == modifier.ModName || mod.modifier2 == modifier.ModName || mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            towerModel.GetAttackModel().GetDescendants<WeaponModel>().ForEach(model => model.rate /= modifier.bonus / 100 + 1);

            tower.UpdateRootModel(towerModel);
        }
    }
}