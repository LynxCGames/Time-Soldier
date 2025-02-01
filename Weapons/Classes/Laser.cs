using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using System;
using BTD_Mod_Helper.Api;

namespace SpaceMarine;

public class LaserSelect : WeaponSelect
{
    public override string WeaponName => "Laser";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Creating Attack Model
        var laser = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        laser.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-300").GetAttackModel().weapons[0].projectile.display;
        laser.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.Purple;

        // Stat Setter
        laser.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        laser.weapons[0].rate = weapon.speed;
        laser.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        laser.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            laser.weapons[0].rate /= 1.06f;
        }
        if (SpaceMarine.mod.camoActive == true)
        {
            laser.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            laser.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = Il2Cpp.BloonProperties.None);
        }

        towerModel.AddBehavior(laser);

        tower.UpdateRootModel(towerModel);
    }
}

public class LaserLevel : WeaponLevel
{
    public override string WeaponName => "Laser";
    public override void Level(WeaponTemplate weapon)
    {
        weapon.level++;

        var pierceModifier = 1;
        var speedModifier = 1.12f;

        if (weapon.level < 10)
        {
            weapon.pierce = (int)weapon.StartingValues[0];
            weapon.speed = weapon.StartingValues[1];
            weapon.damage = (int)weapon.StartingValues[2];

            for (int i = 2; i <= weapon.level; i++)
            {
                if (i % 4 == 2)
                {
                    weapon.pierce += pierceModifier;
                    pierceModifier++;
                }
                if (i % 2 == 1)
                {
                    weapon.speed = MathF.Round((weapon.speed / speedModifier) * 100) / 100;
                    speedModifier += 0.03f;
                }
                if (i < 4)
                {
                    weapon.damage += 1;
                }
                else if (i >= 4 && i < 8)
                {
                    weapon.damage += 2;
                }
                else
                {
                    weapon.damage += 3;
                }
            }
        }
        else
        {
            weapon.pierce += 3;
            weapon.speed = MathF.Round((weapon.speed / 1.25f) * 100) / 100;
            weapon.damage += 5;
        }
    }
}

public class LaserEquip : WeaponEquiped
{
    public override string WeaponName => "Laser";
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

        foreach (var modifier in ModContent.GetContent<ModifierTemplate>())
        {
            if (modifier.ModName == "Rapid Fire")
            {
                if (SpaceMarine.mod.modifier1 == "Rapid Fire" || SpaceMarine.mod.modifier2 == "Rapid Fire" || SpaceMarine.mod.modifier3 == "Rapid Fire")
                {
                    towerModel.GetAttackModel().weapons[0].rate /= (modifier.bonus / 100 + 1);
                }
            }
        }

        if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot" || SpaceMarine.mod.modifier3 == "Piercing Shot")
        {
            PiercingShotMod.PiercingShot(towerModel);
        }

        tower.UpdateRootModel(towerModel);
    }
}