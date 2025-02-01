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

public class MissileSelect : WeaponSelect
{
    public override string WeaponName => "Missile";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Creating Attack Model
        var missile = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().Duplicate();
        missile.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().weapons[0].projectile.display;

        // Stat Setter
        missile.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.pierce + (SpaceMarine.mod.pierceLvl * 2);
        missile.weapons[0].rate = weapon.speed;
        missile.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        missile.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            missile.weapons[0].rate /= 1.06f;
        }
        if (SpaceMarine.mod.camoActive == true)
        {
            missile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            missile.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = Il2Cpp.BloonProperties.None);
        }

        if (weapon.level >= 6)
        {
            missile.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius = 27;
            missile.weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel =
                Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
        }

        towerModel.AddBehavior(missile);

        tower.UpdateRootModel(towerModel);
    }
}

public class MissileLevel : WeaponLevel
{
    public override string WeaponName => "Missile";
    public override void Level(WeaponTemplate weapon)
    {
        weapon.level++;

        var pierceModifier = 4;
        var speedModifier = 1.05f;
        var damageModifier = 1;

        if (weapon.level < 10)
        {
            weapon.pierce = (int)weapon.StartingValues[0];
            weapon.speed = weapon.StartingValues[1];
            weapon.damage = (int)weapon.StartingValues[2];

            for (int i = 2; i <= weapon.level; i++)
            {
                if (i % 2 == 0)
                {
                    weapon.pierce += pierceModifier;
                }
                weapon.speed = MathF.Round((weapon.speed / speedModifier) * 100) / 100;
                if (i % 2 == 1)
                {
                    weapon.damage += damageModifier;
                    damageModifier *= 2;
                }
            }
        }
        else
        {
            weapon.pierce += 6;
            weapon.speed = MathF.Round((weapon.speed / 1.15f) * 100) / 100;
            weapon.damage += 3;
        }
    }
}

public class MissileEquip : WeaponEquiped
{
    public override string WeaponName => "Missile";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

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

        if (SpaceMarine.mod.modifier1 == "Cluster Bomb" || SpaceMarine.mod.modifier2 == "Cluster Bomb" || SpaceMarine.mod.modifier3 == "Cluster Bomb")
        {
            foreach (var modifier in ModContent.GetContent<SpecialTemplate>())
            {
                if (modifier.ModName == "Cluster Bomb")
                {
                    foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>())
                    {
                        if (behavior.name.Contains("ClusterBomb"))
                        {
                            behavior.projectile.GetDamageModel().damage = weapon.damage;
                        }
                    }
                }
            }
        }

        if (weapon.level >= 6)
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius = 27;
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel =
                Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
        }

        tower.UpdateRootModel(towerModel);
    }
}