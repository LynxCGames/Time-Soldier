using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api;
using System;

namespace SpaceMarine;

public class WSelect : WeaponSelect
{
    public override string WeaponName => "";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Creating Attack Model


        // Stat Setter


        // Basic Stat Adjusters



        tower.UpdateRootModel(towerModel);
    }
}

public class WLevel : WeaponLevel
{
    public override string WeaponName => "";
    public override void Level(WeaponTemplate weapon)
    {
        weapon.level++;

        var pierceModifier = 1;
        var speedModifier = 1f;
        var damageModifier = 1;

        if (weapon.level < 10)
        {
            weapon.pierce = (int)weapon.StartingValues[0];
            weapon.speed = weapon.StartingValues[1];
            weapon.damage = (int)weapon.StartingValues[2];

            for (int i = 2; i <= weapon.level; i++)
            {

            }
        }
        else
        {

        }
    }
}

public class WEquip : WeaponEquiped
{
    public override string WeaponName => "";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();



        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            towerModel.GetAttackModel().weapons[0].rate /= 1.06f;
        }

        foreach (var modifier in GetContent<ModifierTemplate>())
        {
            if (modifier.ModName == "Rapid Fire")
            {
                if (SpaceMarine.mod.modifier1 == "Rapid Fire" || SpaceMarine.mod.modifier2 == "Rapid Fire" || SpaceMarine.mod.modifier3 == "Rapid Fire")
                {
                    towerModel.GetAttackModel().weapons[0].rate /= (modifier.bonus / 100 + 1);
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}