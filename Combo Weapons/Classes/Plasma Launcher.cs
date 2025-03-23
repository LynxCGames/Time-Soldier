using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using UnityEngine;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppSystem.Linq;

namespace SpaceMarine;

public class PlasmaLauncher : ComboTemplate
{
    public override string WeaponName => "Plasma Launcher";
    public override string Icon => VanillaSprites.TheAntiBloonUpgradeIcon;
    public override string[] comboWeapons => ["Missile", "Laser"];
    public override string Bonus => "Bloon Eradicator - Every 4th shot launches a homing mega rocket that explodes firing additional smaller rockets";
    public override float FontSize => 58;
    public override float[] StartingValues => [4, 0.8f, 1];
    public override string Range => "Long-Range";
    public override int[] PierceValue => [1, 0, 0, 4, 0, 2];
    public override string SpecialMods =>
        "Fires fast moving plasma shots. Every 4th shot is replaced by a homing mega rocket that launches 6 smaller rockets when it explodes. Mini rockets also home in on Bloons.\n\n" +
        " - Long-Range weapon\n" +
        " - Incendiary effect created by mega rocket and mini rockets\n" +
        " - Cluster Bomb effect created by mini rockets\n" +
        " - Cluster Bomb effect is halved\n" +
        " - Piercing Shot upgrades mega rockets to fire more mini rockets\n" +
        " - Refraction lasers are shot out from plasma shots";
}

public class PlasmaLauncherSelect : ComboSelect
{
    public override string WeaponName => "Plasma Launcher";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
        seeking.distance = 999;
        seeking.constantlyAquireNewTarget = true;

        var plasma = Game.instance.model.GetTowerFromId("DartMonkey-003").GetAttackModel().Duplicate();
        plasma.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("SuperMonkey-050").GetAttackModel().weapons[0].projectile.display;
        plasma.weapons[0].projectile.scale /= 1.5f;
        plasma.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;

        var rocket = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.Duplicate();
        rocket.display = Game.instance.model.GetTowerFromId("BombShooter-050").GetAttackModel().weapons[0].projectile.display;
        rocket.GetBehavior<TravelStraitModel>().speed /= 3;
        rocket.GetBehavior<TravelStraitModel>().lifespan *= 4;
        rocket.AddBehavior(seeking);
        rocket.RemoveBehavior<KnockbackModel>();

        var miniRocket = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().weapons[0].projectile.Duplicate();
        miniRocket.GetBehavior<TravelStraitModel>().lifespan *= 2;
        miniRocket.AddBehavior(seeking);

        // Stat Setter
        plasma.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        plasma.weapons[0].rate = weapon.speed;
        plasma.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        rocket.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = (weapon.pierce * 7) + (4 * SpaceMarine.mod.pierceLvl);
        rocket.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = (weapon.damage * 3) + (SpaceMarine.mod.damageLvl * 2);

        miniRocket.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = (weapon.pierce * 3) + (2 * SpaceMarine.mod.pierceLvl);
        miniRocket.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        plasma.range = 40 + (SpaceMarine.mod.rangeLvl * 12);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 12);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            plasma.weapons[0].rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            plasma.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            rocket.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            miniRocket.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            plasma.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            rocket.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            miniRocket.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        rocket.AddBehavior(new CreateProjectileOnContactModel("MiniRocketStorm", miniRocket, new ArcEmissionModel("", 6, 0, 360, null, false, false), false, true, false));
        plasma.weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("RocketMod", plasma.weapons[0].projectile, rocket, 4, 6, 5, null, 0, 0, 0));

        towerModel.AddBehavior(plasma);

        tower.UpdateRootModel(towerModel);
    }
}

public class PlasmaLauncherLevel : ComboLevel
{
    public override string WeaponName => "Plasma Launcher";
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
                        var speed1 = (2 - weapon1.speed) / 4 + 1;
                        var speed2 = (1 - weapon2.speed) / 4 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round((weapon1.pierce + weapon2.pierce) / 4);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round(weapon1.damage / 2) + (int)Mathf.Round(weapon2.damage / 2);
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
                        var speed1 = (2 - weapon2.speed) / 4 + 1;
                        var speed2 = (1 - weapon1.speed) / 4 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round((weapon2.pierce + weapon1.pierce) / 4);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round(weapon2.damage / 2) + (int)Mathf.Round(weapon1.damage / 2);
                    }
                }
            }
        }
    }
}

public class PlasmaLauncherEquip : ComboEquiped
{
    public override string WeaponName => "Plasma Launcher";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = (weapon.pierce * 7) + (4 * SpaceMarine.mod.pierceLvl);
        towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = (weapon.damage * 3) + (SpaceMarine.mod.damageLvl * 2);

        foreach (var behavior in towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetDescendants<CreateProjectileOnContactModel>().ToArray())
        {
            if (behavior.name == "MiniRocketStorm")
            {
                behavior.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = (weapon.pierce * 3) + (2 * SpaceMarine.mod.pierceLvl);
                behavior.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;
            }
        }

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