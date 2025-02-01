using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using BTD_Mod_Helper.Api;
using Sentries;

namespace SpaceMarine;

public class GravitonSelect : BloonsTD6Mod
{
    public static void Select(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var graviton = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        graviton.weapons[0].projectile.maxPierce = 1;
        graviton.weapons[0].projectile.pierce = 1;
        graviton.weapons[0].projectile.GetDamageModel().damage = 0;
        graviton.weapons[0].projectile.GetDamageModel().maxDamage = 0;
        graviton.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        graviton.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-050").GetAttackModel().weapons[0].projectile.display;

        var vortex = ModContent.GetTowerModel<VortexTower>().Duplicate();
        vortex.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage;

        // Stat Setter
        graviton.weapons[0].rate = weapon.speed;
        vortex.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + mod.damageLvl;

        // Basic Stat Adjusters
        graviton.range = 40 + (mod.rangeLvl * 8);
        towerModel.range = 40 + (mod.rangeLvl * 8);

        for (int i = 0; i < mod.speedLvl; i++)
        {
            graviton.weapons[0].rate /= 1.06f;
        }

        if (mod.camoActive == true)
        {
            graviton.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            vortex.GetAttackModel().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        graviton.weapons[0].projectile.AddBehavior(new CreateTowerModel("", vortex, 1, true, false, false, true, false));

        towerModel.AddBehavior(graviton);

        tower.UpdateRootModel(towerModel);
    }
}