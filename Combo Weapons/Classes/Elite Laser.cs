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
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;

namespace SpaceMarine;

public class EliteLaser : ComboTemplate
{
    public override string WeaponName => "Elite Laser";
    public override string Icon => VanillaSprites.ArcaneSpikeUpgradeIcon;
    public override string[] comboWeapons => ["Laser", "Fire"];
    public override string Bonus => "Target Finder - Homes in on Bloons after a short delay";
    public override float FontSize => 55;
    public override float[] StartingValues => [3, 1.2f, 1];
    public override string Range => "Mid-Range";
    public override string PierceType => "OnExhaust";
    public override int PierceValue => 1;
    public override string SpecialMods =>
        "Fires 2 standard looking lasers. After a short delay, the lasers immediately seek out any nearby Bloons and set them on fire.\n\n" +
        " - Mid-Range weapon\n" +
        " - Piercing Shot upgraded projectile deals double damage\n" +
        " - Piercing Shot upgraded projectile burn deals damage faster\n" +
        " - Refraction splits the laser into more homing lasers\n" +
        " - Flame Spreader increases the number of lasers fired\n" +
        " - Scorcher increases the burn damage";
}

public class EliteLaserSelect : ComboSelect
{
    public override string WeaponName => "Elite Laser";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var laser = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        laser.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-300").GetAttackModel().weapons[0].projectile.display;
        laser.weapons[0].projectile.scale /= 1.25f;
        laser.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
        laser.weapons[0].projectile.GetDamageModel().damage = 1;
        laser.weapons[0].projectile.GetDamageModel().maxDamage = 1;
        laser.weapons[0].projectile.pierce = 15;
        laser.weapons[0].projectile.maxPierce = 15;
        laser.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 2;
        laser.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed /= 2.5f;
        laser.weapons[0].emission = new ArcEmissionModel("", 2, 0, 20, null, false, false);

        var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
        seeking.distance = 999;
        seeking.constantlyAquireNewTarget = true;
        seeking.turnRate *= 2.5f;

        var burn = Game.instance.model.GetTowerFromId("MortarMonkey-002").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
        burn.lifespan = 5;
        burn.GetBehavior<DamageOverTimeModel>().damage = 1;
        burn.GetBehavior<DamageOverTimeModel>().interval = 0.5f;

        var seeker = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
        seeker.display = Game.instance.model.GetTowerFromId("HeliPilot-500").GetAttackModel().weapons[2].projectile.display;
        seeker.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
        seeker.GetBehavior<TravelStraitModel>().Lifespan *= 4;
        seeker.collisionPasses = new[] { -1, 0, 1 };
        seeker.AddBehavior(seeking);
        seeker.AddBehavior(burn);

        // Stat Setter
        seeker.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        laser.weapons[0].rate = weapon.speed;
        seeker.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

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
            seeker.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            laser.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            seeker.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        laser.weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("EliteLaser", seeker, new SingleEmissionModel("", null), 1, -1, false, false, false));

        towerModel.AddBehavior(laser);

        tower.UpdateRootModel(towerModel);
    }
}

public class EliteLaserLevel : ComboLevel
{
    public override string WeaponName => "Elite Laser";
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
                        var speed2 = (1 - weapon2.speed) / 2 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round(weapon1.pierce / 3) + (int)Mathf.Round(weapon2.pierce / 2);
                        combo.speed = Mathf.Round((combo.StartingValues[1] / speed1 / speed2) * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round(weapon1.damage / 2) + (int)Mathf.Round(weapon2.damage / 3);
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

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round(weapon2.pierce / 2) + (int)Mathf.Round(weapon1.pierce / 2);
                        combo.speed = Mathf.Round((combo.StartingValues[1] / speed1 / speed2) * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round(weapon2.damage / 2) + (int)Mathf.Round(weapon1.damage / 3);
                    }
                }
            }
        }
    }
}

public class EliteLaserEquip : ComboEquiped
{
    public override string WeaponName => "Elite Laser";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

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