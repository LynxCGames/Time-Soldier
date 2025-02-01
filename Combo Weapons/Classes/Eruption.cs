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

public class Eruption : ComboTemplate
{
    public override string WeaponName => "Eruption";
    public override string Icon => VanillaSprites.BlooncinerationUpgradeIcon;
    public override string[] comboWeapons => ["Missile", "Fire"];
    public override string Bonus => "Volcanic - Launches multiple fireballs that can stun Bloons";
    public override float FontSize => 60;
    public override float[] StartingValues => [12, 1.5f, 2];
    public override string Range => "Mid-Range";
    public override string PierceType => "OnContact";
    public override int PierceValue => 2;
    public override string SpecialMods =>
        "Fires out 3 explosive fireballs that explode on contact or when they expire. Explosion sets Bloons on fire and stuns them temporarily.\n\n" +
        " - Mid-Range weapon\n" +
        " - Incendiary creates a wall of fire when it hits a Bloon\n" +
        " - Cluster Bomb shoots out mini firebombs\n" +
        " - Flame Spreader increases the number of fireballs\n" +
        " - Scorcher increases the burn damage";
}

public class EruptionSelect : ComboSelect
{
    public override string WeaponName => "Eruption";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var burn = Game.instance.model.GetTowerFromId("MortarMonkey-002").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
        burn.lifespan = 5;
        burn.GetBehavior<DamageOverTimeModel>().damage = 1;
        burn.GetBehavior<DamageOverTimeModel>().interval = 0.5f;

        var slowModel = new SlowModel("EruptionStun", 0, 1.25f, "Stun:Weak", 999, "Stun", true, false, null, false, false, false);
        var slowModifier = new SlowModifierForTagModel("EruptionStun", "Moabs", "Stun:Weak", 0, true, true, 0, false);

        var fireball = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().Duplicate();
        fireball.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("TackShooter-500").GetAttackModel(1).weapons[0].projectile.display;
        fireball.weapons[0].projectile.scale /= 1.5f;
        fireball.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan /= 1.25f;
        fireball.weapons[0].emission = new ArcEmissionModel("", 3, 0, 60, null, false, false);
        fireball.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple & BloonProperties.Black;
        fireball.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new[] { -1, 0, 1 };
        fireball.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(burn);
        fireball.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(slowModel);
        fireball.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(slowModifier);

        // Stat Setter
        fireball.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        fireball.weapons[0].rate = weapon.speed;
        fireball.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        fireball.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            fireball.weapons[0].rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            fireball.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            fireball.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        towerModel.AddBehavior(fireball);

        tower.UpdateRootModel(towerModel);
    }
}

public class EruptionLevel : ComboLevel
{
    public override string WeaponName => "Eruption";
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

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round(weapon1.pierce / 2) + (int)Mathf.Round(weapon2.pierce / 2);
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
                        var speed1 = (1 - weapon2.speed) / 2 + 1;
                        var speed2 = (1 - weapon1.speed) / 2 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round(weapon2.pierce / 2) + (int)Mathf.Round(weapon1.pierce / 2);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round(weapon2.damage / 2) + (int)Mathf.Round(weapon1.damage / 2);
                    }
                }
            }
        }
    }
}

public class EruptionEquip : ComboEquiped
{
    public override string WeaponName => "Eruption";
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

        tower.UpdateRootModel(towerModel);
    }
}