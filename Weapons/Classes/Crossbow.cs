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

public class CrossbowSelect : WeaponSelect
{
    public override string WeaponName => "Crossbow";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Creating Attack Model
        var crossbow = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        crossbow.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartMonkey-003").GetAttackModel().weapons[0].projectile.display;
        crossbow.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.1f;

        // Stat Setter
        crossbow.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        crossbow.weapons[0].rate = weapon.speed;
        crossbow.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        crossbow.range = 40 + (SpaceMarine.mod.rangeLvl * 12);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 12);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            crossbow.weapons[0].rate /= 1.06f;
        }
        if (SpaceMarine.mod.camoActive == true)
        {
            crossbow.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            crossbow.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = Il2Cpp.BloonProperties.None);
        }

        towerModel.AddBehavior(crossbow);

        tower.UpdateRootModel(towerModel);
    }
}

public class CrossbowLevel : WeaponLevel
{
    public override string WeaponName => "Crossbow";
    public override void Level(WeaponTemplate weapon)
    {
        weapon.level++;

        var pierceModifier = 1;
        var speedModifier = 1.08f;
        var damageModifier = 1;

        if (weapon.level < 10)
        {
            weapon.pierce = (int)weapon.StartingValues[0];
            weapon.speed = weapon.StartingValues[1];
            weapon.damage = (int)weapon.StartingValues[2];

            for (int i = 2; i <= weapon.level; i++)
            {
                if (i % 3 == 2)
                {
                    weapon.pierce += pierceModifier;
                    pierceModifier++;
                }
                if (i % 2 == 0)
                {
                    weapon.speed = MathF.Round((weapon.speed / speedModifier) * 100) / 100;
                    speedModifier += 0.02f;
                }
                if (i % 2 == 1)
                {
                    weapon.damage += damageModifier;
                    damageModifier *= 2;
                }
            }
        }
        else
        {
            weapon.pierce += 4;
            weapon.speed = MathF.Round((weapon.speed / 1.16f) * 100) / 100;
            weapon.damage += 16;
        }
    }
}

public class CrossbowEquip : WeaponEquiped
{
    public override string WeaponName => "Crossbow";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

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