using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using UnityEngine;

namespace SpaceMarine;

public class ArcticKnight : ComboTemplate
{
    public override string WeaponName => "Arctic Knight";
    public override string Icon => "912b53e210adb7841a0b7c5e4767832f";
    public override string[] comboWeapons => ["Ice", "Magic"];
    public override string Bonus => "Riptide - Tridents become stronger over time";
    public override float FontSize => 55;
    public override float[] StartingValues => [10, 1.2f, 1];
    public override string Range => "Long-Range";
    public override int[] PierceValue => [1, 2];
    public override string SpecialMods =>
        "Fires magic tridents that grow in strength the longer they exist. Tridents release a strong wave when they expire. Waves deal damage based on how strong the trident got.\n\n" +
        " - Long-Range weapon\n" +
        " - Icicles cause tridents to crack 3 forward firing icicles from Bloons hit\n" +
        " - Temporary snowstorm fields are created when tridents expire\n" +
        " - Hex causes tridents to create a small magic burst when they hit a Bloon\n" +
        " - Arcane Spike causes tridents to release mini balls of lightning when they expire";
}

public class ArcticKnightSelect : ComboSelect
{
    public override string WeaponName => "Arctic Knight";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
        seeking.distance = 999;
        seeking.constantlyAquireNewTarget = true;

        var trident = Game.instance.model.GetTowerFromId("Mermonkey-040").GetAttackModel().Duplicate();
        trident.weapons[0].projectile.AddBehavior(seeking);
        trident.weapons[0].projectile.RemoveBehavior<DamageModifierForTagModel>();
        trident.weapons[0].projectile.RemoveBehavior<DamageModifierForBloonStateModel>();

        trident.weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.RemoveBehavior<DamageModifierForTagModel>();
        trident.weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.RemoveBehavior<DamageModifierForBloonStateModel>();

        // Stat Setter
        trident.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        trident.weapons[0].rate = weapon.speed;
        trident.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;
        trident.weapons[0].projectile.GetBehavior<ScaleDamageWithTimeModel>().maxDamage = weapon.level * 3;
        trident.weapons[0].projectile.GetBehavior<ScaleDamageWithTimeModel>().scalePerSecond = weapon.level * 0.75f;

        trident.weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.pierce = (weapon.pierce + SpaceMarine.mod.pierceLvl) * 2;
        trident.weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.GetDamageModel().damage = 2 * weapon.damage + SpaceMarine.mod.damageLvl;
        trident.weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.GetBehavior<ScaleDamageWithTimeModel>().maxDamage = weapon.level * 5;
        trident.weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.GetBehavior<ScaleDamageWithTimeModel>().scalePerSecond = weapon.level * 1.25f;

        // Basic Stat Adjusters
        trident.range = 40 + (SpaceMarine.mod.rangeLvl * 12);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 12);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            trident.weapons[0].rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            trident.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            trident.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        towerModel.AddBehavior(trident);

        tower.UpdateRootModel(towerModel);
    }
}

public class ArcticKnightLevel : ComboLevel
{
    public override string WeaponName => "Arctic Knight";
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
                        var speed1 = (2 - weapon1.speed) / 2 + 1;
                        var speed2 = (1 - weapon2.speed) / 2 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round((weapon1.pierce + weapon2.pierce) / 5);
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
                        var speed1 = (2 - weapon2.speed) / 2 + 1;
                        var speed2 = (1 - weapon1.speed) / 2 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round((weapon2.pierce + weapon1.pierce) / 5);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round(weapon2.damage / 2) + (int)Mathf.Round(weapon1.damage / 2);
                    }
                }
            }
        }
    }
}

public class ArcticKnightEquip : ComboEquiped
{
    public override string WeaponName => "Arctic Knight";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<ScaleDamageWithTimeModel>().maxDamage = weapon.level * 3;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<ScaleDamageWithTimeModel>().scalePerSecond = weapon.level * 0.75f;

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.pierce = (weapon.pierce + SpaceMarine.mod.pierceLvl) * 2;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.GetDamageModel().damage = 2 * weapon.damage + SpaceMarine.mod.damageLvl;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.GetBehavior<ScaleDamageWithTimeModel>().maxDamage = weapon.level * 5;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<ScaleProjectileOverTimeModel>().bonusProjectileModel.GetBehavior<ScaleDamageWithTimeModel>().scalePerSecond = weapon.level * 1.25f;

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