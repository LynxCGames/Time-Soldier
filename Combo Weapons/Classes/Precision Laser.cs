using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using System;
using UnityEngine;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;

namespace SpaceMarine;

public class PrecisionLaser : ComboTemplate
{
    public override string WeaponName => "Precision Laser";
    public override string Icon => VanillaSprites.BouncingBulletUpgradeIcon;
    public override string[] comboWeapons => ["Ice", "Laser"];
    public override string Bonus => "Ricochet – Bounces to up to 2 new targets";
    public override float FontSize => 50;
    public override float[] StartingValues => [5, 1.2f, 2];
    public override string Range => "Long-Range";
    public override string PierceType => "OnContact";
    public override int PierceValue => 1;
    public override string SpecialMods =>
        "Fires a precision laser that can't miss and can bounce up to 2 times. Laser creates an icy burst when it hits a Bloon. Pierce affects the pierce of the ice burst only. Ice burst deals half of the weapon's damage.\n\n" +
        " - Long-Range weapon\n" +
        " - Icicles are shot out from Bloons hit\n" +
        " - Icicle effect is reduced to 6 instead of 12\n" +
        " - Temporary snowstorm field is created when Laser expires\n" +
        " - Piercing Shot upgraded projectile deals double damage and bounces 3 additional times\n" +
        " - Refraction lasers are shot out from Bloons hit";
}

public class PrecisionLaserSelect : ComboSelect
{
    public override string WeaponName => "Precision Laser";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var ice = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
        ice.name = "PrecisionLaserIce";
        ice.projectile.collisionPasses = new int[] { 0, -1 };
        ice.projectile.AddBehavior(new FreezeModel("", 0, 1.5f, "CryoIce:Regular:Freeze", 3, "Ice", true, new Il2CppAssets.Scripts.Models.Bloons.Behaviors.GrowBlockModel(""), null, 0, false, false, false));
        ice.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;

        var precisionLaser = Game.instance.model.GetTowerFromId("SniperMonkey").GetAttackModel().Duplicate();
        precisionLaser.weapons[0].projectile.maxPierce = 3;
        precisionLaser.weapons[0].projectile.pierce = 3;
        precisionLaser.weapons[0].projectile.GetBehavior<AgeModel>().Lifespan = 3;
        precisionLaser.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("SniperMonkey-030").GetAttackModel().weapons[0].projectile.GetBehavior<RetargetOnContactModel>().Duplicate());
        precisionLaser.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("SniperMonkey-030").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectFromCollisionToCollisionModel>().Duplicate());
        precisionLaser.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("SniperMonkey-030").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate());
        precisionLaser.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate());
        precisionLaser.weapons[0].AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].GetBehavior<CreateSoundOnProjectileCreatedModel>().Duplicate());
        precisionLaser.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;
        precisionLaser.weapons[0].ejectY = 8;
        precisionLaser.weapons[0].ejectZ = 9;

        // Stat Setter
        ice.projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        precisionLaser.weapons[0].rate = weapon.speed;
        ice.projectile.GetDamageModel().damage = MathF.Round(weapon.damage / 2) + SpaceMarine.mod.damageLvl;
        precisionLaser.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        precisionLaser.range = 40 + (SpaceMarine.mod.rangeLvl * 12);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 12);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            precisionLaser.weapons[0].rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            ice.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            precisionLaser.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            ice.projectile.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            precisionLaser.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        precisionLaser.weapons[0].projectile.AddBehavior(ice);

        towerModel.AddBehavior(precisionLaser);

        tower.UpdateRootModel(towerModel);
    }
}

public class PrecisionLaserLevel : ComboLevel
{
    public override string WeaponName => "Precision Laser";
    public override void Level(WeaponTemplate weapon1, ComboTemplate combo)
    {
        foreach (var weapon2 in ModContent.GetContent<WeaponTemplate>())
        {
            if (weapon1.WeaponName == combo.comboWeapons[0] && weapon2.WeaponName == combo.comboWeapons[1])
            {
                if (weapon1.isUnlocked == true && weapon2.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((weapon1.level + weapon2.level) / 2);

                    if (combo.level > 1)
                    {
                        var speed1 = (2 - weapon1.speed) / 3 + 1;
                        var speed2 = (1 - weapon2.speed) / 3 + 1;

                        combo.pierce = 6 + (int)Mathf.Round((weapon1.pierce + weapon2.pierce) / 11);
                        combo.speed = Mathf.Round((1.2f / speed1 / speed2) * 100) / 100;
                        combo.damage = 2 + (int)Mathf.Round((weapon1.damage + weapon2.damage) / 2);
                    }
                }
            }
            if (weapon1.WeaponName == combo.comboWeapons[1] && weapon2.WeaponName == combo.comboWeapons[0])
            {
                if (weapon1.isUnlocked == true && weapon2.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((weapon1.level + weapon2.level) / 2);

                    if (combo.level > 1)
                    {
                        var speed1 = (1 - weapon1.speed) / 3 + 1;
                        var speed2 = (2 - weapon2.speed) / 3 + 1;

                        combo.pierce = 6 + (int)Mathf.Round((weapon2.pierce + weapon1.pierce) / 11);
                        combo.speed = Mathf.Round((1.2f / speed2 / speed1) * 100) / 100;
                        combo.damage = 2 + (int)Mathf.Round((weapon2.damage + weapon1.damage) / 2);
                    }
                }
            }
        }
    }
}

public class PrecisionLaserEquip : ComboEquiped
{
    public override string WeaponName => "Precision Laser";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>())
        {
            if (behavior.name.Contains("PrecisionLaserIce"))
            {
                behavior.projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
                behavior.projectile.GetDamageModel().damage = MathF.Round(weapon.damage / 2) + SpaceMarine.mod.damageLvl;
            }
        }

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