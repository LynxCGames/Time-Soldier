using BTD_Mod_Helper.Api;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;

namespace SpaceMarine;

public class RapidEquiped : BloonsTD6Mod
{

    public static void Level(ModifierTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
        {
            if (weapon.WeaponName == mod.weapon)
            {
                towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
            }
        }
        foreach (var weapon in ModContent.GetContent<ComboTemplate>())
        {
            if (weapon.WeaponName == mod.weapon)
            {
                towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
            }
        }
        towerModel.GetAttackModel().weapons[0].rate /= (modifier.bonus / 100 + 1);

        tower.UpdateRootModel(towerModel);
    }
}