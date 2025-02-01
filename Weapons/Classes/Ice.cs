using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using BTD_Mod_Helper.Api;
using System;

namespace SpaceMarine;

public class IceSelect : WeaponSelect
{
    public override string WeaponName => "Ice";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Creating Attack Model
        var ice = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        var iceWeapon = Game.instance.model.GetTowerFromId("IceMonkey").GetAttackModel().weapons[0].Duplicate();
        iceWeapon.GetDescendants<FilterOutTagModel>().ForEach(model => model.tag = "");
        ice.weapons[0] = iceWeapon;

        // Stat Setter
        ice.weapons[0].projectile.pierce = weapon.pierce + (SpaceMarine.mod.pierceLvl * 3);
        ice.weapons[0].rate = weapon.speed;
        ice.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        ice.range = 24 + (SpaceMarine.mod.rangeLvl * 5);
        towerModel.range = 24 + (SpaceMarine.mod.rangeLvl * 5);
        ice.weapons[0].projectile.radius = 24 + (SpaceMarine.mod.rangeLvl * 5);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            ice.weapons[0].rate /= 1.06f;
        }
        if (SpaceMarine.mod.camoActive == true)
        {
            ice.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            ice.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = Il2Cpp.BloonProperties.None);
        }

        towerModel.AddBehavior(ice);

        tower.UpdateRootModel(towerModel);
    }
}

public class IceLevel : WeaponLevel
{
    public override string WeaponName => "Ice";
    public override void Level(WeaponTemplate weapon)
    {
        weapon.level++;

        var pierceModifier = 8;
        var speedModifier = 1.07f;
        var damageModifier = 1;

        if (weapon.level < 10)
        {
            weapon.pierce = (int)weapon.StartingValues[0];
            weapon.speed = weapon.StartingValues[1];
            weapon.damage = (int)weapon.StartingValues[2];

            for (int i = 2; i <= weapon.level; i++)
            {
                if (i % 2 == 1)
                {
                    weapon.pierce += pierceModifier;
                }
                weapon.speed = MathF.Round((weapon.speed / speedModifier) * 100) / 100;
                if (i % 2 == 0)
                {
                    weapon.damage += damageModifier;
                    damageModifier++;
                }
            }
        }
        else
        {
            weapon.pierce += 8;
            weapon.speed = MathF.Round((weapon.speed / 1.12f) * 100) / 100;
            weapon.damage += 5;
        }
    }
}

public class IceEquip : WeaponEquiped
{
    public override string WeaponName => "Ice";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.pierce = weapon.pierce + (SpaceMarine.mod.pierceLvl * 3);
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

        tower.UpdateRootModel(towerModel);
    }
}