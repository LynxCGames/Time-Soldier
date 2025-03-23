using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers;
using System;

namespace SpaceMarine;

public class MagicSelect : WeaponSelect
{
    public override string WeaponName => "Magic";
    public override void EditTower(WeaponTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Creating Attack Model
        var magic = Game.instance.model.GetTowerFromId("WizardMonkey-100").GetAttackModel().Duplicate();

        // Stat Setter
        magic.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        magic.weapons[0].rate = weapon.speed;
        magic.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        magic.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            magic.weapons[0].rate /= 1.06f;
        }
        if (SpaceMarine.mod.camoActive == true)
        {
            magic.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            magic.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = Il2Cpp.BloonProperties.None);
        }

        towerModel.AddBehavior(magic);

        tower.UpdateRootModel(towerModel);
    }
}

public class MagicLevel : WeaponLevel
{
    public override string WeaponName => "Magic";
    public override void Level(WeaponTemplate weapon)
    {
        weapon.level++;

        var pierceModifier = 1;
        var speedModifier = 1.09f;
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
                if (i % 2 == 1)
                {
                    weapon.speed = MathF.Round((weapon.speed / speedModifier) * 100) / 100;
                    speedModifier += 0.03f;
                }
                if (i % 2 == 0)
                {
                    weapon.damage += damageModifier;
                    damageModifier++;
                }
            }
        }
        else
        {
            weapon.pierce += 3;
            weapon.speed = MathF.Round((weapon.speed / 1.20f) * 100) / 100;
            weapon.damage += 9;
        }
    }
}

public class MagicEquip : WeaponEquiped
{
    public override string WeaponName => "Magic";
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