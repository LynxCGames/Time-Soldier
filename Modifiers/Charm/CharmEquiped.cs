using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;

namespace SpaceMarine;

public class CharmEquiped : BloonsTD6Mod
{
    public static void Level(ModifierTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        foreach (var attack in towerModel.GetAttackModels())
        {
            if (attack.name.Contains("CharmMod"))
            {
                var rateMod = 0.12f * (modifier.bonus - 1) + 1;
                attack.weapons[0].rate = 7 / rateMod;
                attack.weapons[0].projectile.GetDamageModel().damage = (2 * modifier.bonus) - 1;
                attack.weapons[0].projectile.pierce = modifier.bonus + 3;
                attack.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed = 25 + (modifier.bonus * 2);
                attack.weapons[0].projectile.display = modifier.CharmSprite[modifier.level - 1];
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}