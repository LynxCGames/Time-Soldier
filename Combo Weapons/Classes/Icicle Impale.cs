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
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace SpaceMarine;

public class IcicleImpale : ComboTemplate
{
    public override string WeaponName => "Icicle Impale";
    public override string Icon => VanillaSprites.IcicleImpaleUpgradeIcon;
    public override string[] comboWeapons => ["Crossbow", "Ice"];
    public override string Bonus => "Ultra Freeze – Can freeze MOAB class Bloons";
    public override float FontSize => 55;
    public override float[] StartingValues => [10, 1.1f, 2];
    public override string Range => "Mid-Range";
    public override int[] PierceValue => [0, 2];
    public override string SpecialMods =>
        "Icicle Impale Fires a large icicle that creates an ice burst when it hits a Bloon. Icicle can hit up to 2 targets. Pierce affects the pierce of the ice burst only. Ice burst deals half of the weapon's damage.\n\n" +
        " - Mid-Range weapon\n" +
        " - Burst Fire increases main projectile pierce\n" +
        " - Sharpshooter affects the projectile and not the ice burst\n" +
        " - Icicle effect is shot out from target\n" +
        " - Icicle effect is reduced to 8 instead of 12\n" +
        " - Snowstorm creates slowing aura around the tower";
}

public class IcicleImpaleSelect : ComboSelect
{
    public override string WeaponName => "Icicle Impale";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var effect = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
        var sound = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].GetBehavior<CreateSoundOnProjectileCreatedModel>().Duplicate();
        var explosion = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
        explosion.name = "IcicleImpale";
        explosion.projectile.collisionPasses = new int[] { 0, -1 };
        explosion.projectile.AddBehavior(new FreezeModel("", 0, 1.5f, "CryoIce:Regular:Freeze", 3, "Ice", true, new Il2CppAssets.Scripts.Models.Bloons.Behaviors.GrowBlockModel(""), null, 0, false, true, false));
        explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;

        var icicle = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        icicle.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
        icicle.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;
        icicle.weapons[0].projectile.pierce = 2;
        icicle.weapons[0].projectile.maxPierce = 2;

        // Stat Setter
        explosion.projectile.pierce = weapon.pierce + (SpaceMarine.mod.pierceLvl * 2);
        icicle.weapons[0].rate = weapon.speed;
        icicle.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;
        explosion.projectile.GetDamageModel().damage = MathF.Round(weapon.damage / 2) + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        icicle.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            icicle.weapons[0].rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            icicle.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            explosion.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            icicle.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            explosion.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        icicle.weapons[0].projectile.AddBehavior(explosion);
        icicle.weapons[0].projectile.AddBehavior(sound);
        icicle.weapons[0].projectile.AddBehavior(effect);

        towerModel.AddBehavior(icicle);

        tower.UpdateRootModel(towerModel);
    }
}

public class IcicleImpaleLevel : ComboLevel
{
    public override string WeaponName => "Icicle Impale";
    public override void Level(WeaponTemplate weapon1, ComboTemplate combo)
    {
        foreach (var weapon2 in GetContent<WeaponTemplate>())
        {
            if (weapon1.WeaponName == combo.comboWeapons[0] && weapon2.WeaponName == combo.comboWeapons[1])
            {
                if (weapon1.isUnlocked == true && weapon2.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((weapon1.level + weapon2.level) / 2);

                    if (combo.level > 1)
                    {
                        var speed1 = (1 - weapon1.speed) / 2 + 1;
                        var speed2 = (2 - weapon2.speed) / 2 + 1;

                        combo.pierce = 10 + (int)Mathf.Round(weapon2.pierce / 3);
                        combo.speed = Mathf.Round((1.1f / speed1 / speed2) * 100) / 100;
                        combo.damage = 2 + (int)Mathf.Round(weapon1.damage / 1.5f) + (int)Mathf.Round(weapon2.damage / 2);
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
                        var speed1 = (1 - weapon2.speed) / 2 + 1;
                        var speed2 = (2 - weapon1.speed) / 2 + 1;

                        combo.pierce = 10 + (int)Mathf.Round(weapon1.pierce / 3);
                        combo.speed = Mathf.Round((1.1f / speed2 / speed1) * 100) / 100;
                        combo.damage = 2 + (int)Mathf.Round(weapon2.damage / 1.5f) + (int)Mathf.Round(weapon1.damage / 2);
                    }
                }
            }
        }
    }
}

public class IcicleImpaleEquip : ComboEquiped
{
    public override string WeaponName => "Icicle Impale";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = MathF.Round(weapon.damage / 2) + SpaceMarine.mod.damageLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            towerModel.GetAttackModel().GetDescendants<WeaponModel>().ForEach(model => model.rate /= 1.06f);
        }

        foreach (var modifier in GetContent<ModifierTemplate>())
        {
            if (modifier.ModName == "Rapid Fire")
            {
                if (SpaceMarine.mod.modifier1 == "Rapid Fire" || SpaceMarine.mod.modifier2 == "Rapid Fire" || SpaceMarine.mod.modifier3 == "Rapid Fire")
                {
                    towerModel.GetAttackModel().GetDescendants<WeaponModel>().ForEach(model => model.rate /= (modifier.bonus / 100) + 1);
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}