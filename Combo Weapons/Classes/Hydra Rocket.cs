using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using BTD_Mod_Helper.Api;
using UnityEngine;
using BTD_Mod_Helper.Api.Enums;

namespace SpaceMarine;

public class HydraRocket : ComboTemplate
{
    public override string WeaponName => "Hydra Rockets";
    public override string Icon => VanillaSprites.RocketStormUpgradeIcon;
    public override string[] comboWeapons => ["Crossbow", "Missile"];
    public override string Bonus => "Multi-Burst – Rockets explode multiple times";
    public override float FontSize => 60;
    public override float[] StartingValues => [8, 1.35f, 1];
    public override string Range => "Mid-Range";
    public override string PierceType => "OnContact";
    public override int PierceValue => 1;
    public override string SpecialMods =>
        "Hydra rockets can explode up to 3 times. Pierce affects the pierce of the explosions only.\n\n" +
        " - Mid-Range weapon\n" +
        " - Burst Fire increases explosion count\n" +
        " - Sharpshooter causes the explosions to crit\n" +
        " - Incendiary effect is created with every explosion\n" +
        " - Cluster Bomb effect is halved";
}

public class HydraRocketSelect : ComboSelect
{
    public override string WeaponName => "Hydra Rockets";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());
        
        // Creating Attack Model
        var effect = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
        var sound = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
        var explosion = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
        explosion.name = "HydraRocket";

        var rocket = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        rocket.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-030").GetAttackModel().weapons[0].projectile.display;
        rocket.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        rocket.weapons[0].projectile.GetDamageModel().damage = 1;
        rocket.weapons[0].projectile.GetDamageModel().maxDamage = 1;
        rocket.weapons[0].projectile.pierce = 3;
        rocket.weapons[0].projectile.maxPierce = 3;

        // Stat Setter
        explosion.projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        rocket.weapons[0].rate = weapon.speed;
        explosion.projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        rocket.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            rocket.weapons[0].rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            rocket.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            explosion.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        rocket.weapons[0].projectile.AddBehavior(explosion);
        rocket.weapons[0].projectile.AddBehavior(sound);
        rocket.weapons[0].projectile.AddBehavior(effect);

        towerModel.AddBehavior(rocket);

        tower.UpdateRootModel(towerModel);
    }
}

public class HydraRocketLevel : ComboLevel
{
    public override string WeaponName => "Hydra Rockets";
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
                        var speed1 = (1 - weapon1.speed) / 2 + 1;
                        var speed2 = (2 - weapon2.speed) / 2 + 1;

                        combo.pierce = 8 + (int)Mathf.Round(weapon1.pierce / 3) + (int)Mathf.Round(weapon2.pierce / 2);
                        combo.speed = Mathf.Round((1.5f / speed1 / speed2) * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round(weapon1.damage / 2) + (int)Mathf.Round(weapon2.damage / 4);
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

                        combo.pierce = 8 + (int)Mathf.Round(weapon2.pierce / 3) + (int)Mathf.Round(weapon1.pierce / 2);
                        combo.speed = Mathf.Round((1.5f / speed2 / speed1) * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round(weapon2.damage / 2) + (int)Mathf.Round(weapon1.damage / 4);
                    }
                }
            }
        }
    }
}

public class HydraRocketEquip : ComboEquiped
{
    public override string WeaponName => "Hydra Rockets";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

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

        tower.UpdateRootModel(towerModel);
    }
}