using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;

namespace SpaceMarine;

public class FireSelect : WeaponSelect
{
    public override string WeaponName => "Fire";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Creating Attack Model
        var burn = Game.instance.model.GetTowerFromId("MortarMonkey-002").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
        burn.lifespan = 5;
        burn.GetBehavior<DamageOverTimeModel>().interval = 1;

        var fire = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        fire.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.display;
        fire.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.Purple;
        fire.weapons[0].projectile.collisionPasses = new[] { -1, 0, 1 };

        // Stat Setter
        fire.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        fire.weapons[0].rate = weapon.speed;
        fire.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;
        burn.GetBehavior<DamageOverTimeModel>().damage = weapon.level;

        fire.weapons[0].projectile.AddBehavior(burn);

        // Basic Stat Adjusters
        fire.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            fire.weapons[0].rate /= 1.06f;
        }
        if (SpaceMarine.mod.camoActive == true)
        {
            fire.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            fire.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = Il2Cpp.BloonProperties.None);
        }

        towerModel.AddBehavior(fire);


        tower.UpdateRootModel(towerModel);
    }
}

public class FireLevel : WeaponLevel
{
    public override string WeaponName => "Fire";
    public override void Level(WeaponTemplate weapon)
    {
        weapon.level++;

        var speedModifier = 1.08f;
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
                    weapon.pierce += 1;
                }
                if (i % 2 == 1)
                {
                    weapon.speed = MathF.Round((weapon.speed / speedModifier) * 100) / 100;
                    speedModifier += 0.02f;
                }
                if (i % 2 == 0 && i < 4)
                {
                    weapon.damage += damageModifier;
                    damageModifier++;
                }
                else if (i % 2 == 0 && i >= 4)
                {
                    weapon.damage += damageModifier;
                    damageModifier += 3;
                }
            }
        }
        else
        {
            weapon.pierce += 3;
            weapon.speed = MathF.Round((weapon.speed / 1.2f) * 100) / 100;
            weapon.damage += 9;
        }
    }
}

public class FireEquip : WeaponEquiped
{
    public override string WeaponName => "Fire";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = weapon.level;

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