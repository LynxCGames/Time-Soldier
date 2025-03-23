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
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppSystem.Linq;

namespace SpaceMarine;

public class Fireworks : ComboTemplate
{
    public override string WeaponName => "Fireworks";
    public override string Icon => VanillaSprites.BurnyStuffUpgradeIcon;
    public override string[] comboWeapons => ["Crossbow", "Fire"];
    public override string Bonus => "Celebration - Periodically fires out 2 homing firwork rockets that create an explosion of wonder";
    public override float FontSize => 50;
    public override float[] StartingValues => [8, 0.9f, 1];
    public override string Range => "Mid-Range";
    public override int[] PierceValue => [0, 1, 0, 3 ,0];
    public override string SpecialMods =>
        "Main weapon shoots out firecrackers that explode in a small area on contact and set Bloons on fire. " +
        "Periodically will fire out 2 additional firework rockets that have a much bigger explosion and send blazing shrapnel in all directions.\n\n" +
        " - Mid-Range weapon\n" +
        " - Firework rocket stats are double the weapon's stats\n" +
        " - Burst Fire causes firecrackers to fire in bursts\n" +
        " - Sharpshooter allows firecrackers and fireworks to crit\n" +
        " - Firecrackers crit every 8th shot\n" +
        " - Fireworks crit every 3rd shot\n" +
        " - Flame Spreader increases the amount of blazing shrapnel shot out from firework explosions\n" +
        " - Scorcher decreases the burn tick delay";
}

public class FireworksSelect : ComboSelect
{
    public override string WeaponName => "Fireworks";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
        seeking.distance = 999;
        seeking.constantlyAquireNewTarget = true;
        seeking.turnRate *= 2f;

        var burn = Game.instance.model.GetTowerFromId("MortarMonkey-002").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
        burn.lifespan = 5;
        burn.GetBehavior<DamageOverTimeModel>().damage = 1;
        burn.GetBehavior<DamageOverTimeModel>().interval = 1;

        var firecracker = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().Duplicate();
        firecracker.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.display;
        firecracker.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new[] { -1, 0, 1 };
        firecracker.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(burn);

        var shrapnel = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
        shrapnel.display = Game.instance.model.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.display;
        shrapnel.pierce = 3;
        shrapnel.maxPierce = 3;
        shrapnel.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;

        var firework = Game.instance.model.GetTowerFromId("BombShooter-120").GetAttackModel().weapons[0].Duplicate();
        firework.ejectX = 0;
        firework.ejectY = 0;
        firework.emission = new ArcEmissionModel("", 2, 0, 180, null, false, false);
        firework.projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-040").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.display;
        firework.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new[] { -1, 0, 1 };
        firework.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(burn);
        firework.projectile.AddBehavior(seeking);

        // Stat Setter
        firecracker.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        firecracker.weapons[0].rate = weapon.speed;
        firecracker.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        firework.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = (weapon.pierce + SpaceMarine.mod.pierceLvl) * 3;
        firework.rate = weapon.speed * 3;
        firework.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = (weapon.damage * 2) + SpaceMarine.mod.damageLvl;

        shrapnel.GetDamageModel().damage = weapon.level + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        firecracker.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            firecracker.weapons[0].rate /= 1.06f;
            firework.rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            firecracker.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            firework.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            shrapnel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            firecracker.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            firework.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            shrapnel.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        firework.projectile.AddBehavior(new CreateProjectileOnContactModel("Shrapnel", shrapnel, new ArcEmissionModel("", 8, 0, 360, null, false, false), false, false, false));
        firecracker.AddWeapon(firework);
        towerModel.AddBehavior(firecracker);

        tower.UpdateRootModel(towerModel);
    }
}

public class FireworksLevel : ComboLevel
{
    public override string WeaponName => "Fireworks";
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
                        var speed2 = (1 - weapon2.speed) / 2 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round((weapon1.pierce + weapon2.pierce) / 2.5f);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round((weapon1.damage + weapon2.damage) / 3.5f);
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
                        var speed2 = (1 - weapon1.speed) / 2 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round((weapon2.pierce + weapon1.pierce) / 2.5f);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round((weapon2.damage + weapon1.damage) / 3.5f);
                    }
                }
            }
        }
    }
}

public class FireworksEquip : ComboEquiped
{
    public override string WeaponName => "Fireworks";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        towerModel.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.pierce * 3;
        towerModel.GetAttackModel().weapons[1].rate = weapon.speed * 3;
        towerModel.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = (weapon.damage * 2) + SpaceMarine.mod.damageLvl;

        foreach (var behavior in towerModel.GetAttackModel().weapons[1].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
        {
            if (behavior.name.Contains("Shrapnel"))
            {
                behavior.projectile.GetDamageModel().damage = weapon.level + SpaceMarine.mod.damageLvl;
            }
        }

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