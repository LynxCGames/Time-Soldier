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
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;

namespace SpaceMarine;

public class Blizzard : ComboTemplate
{
    public override string WeaponName => "Blizzard";
    public override string Icon => VanillaSprites.DruidoftheStormUpgradeIcon;
    public override string[] comboWeapons => ["Ice", "Thorn"];
    public override string Bonus => "Cold Front - Tornadoes knock back Bloons along the track";
    public override float FontSize => 50;
    public override float[] StartingValues => [2, 1.1f, 1];
    public override string Range => "Mid-Range";
    public override int[] PierceValue => [1];
    public override string SpecialMods =>
        "Fires 5 ice shards at a time that freeze Bloons hit. Periodically fires out 3 tornadoes that push back small Bloons. Every 4th tornado shot is replaced with stronger tornadoes that can push back MOABs.\n\n" +
        " - Mid-Range weapon\n" +
        " - Icicles increase the number ice shards fired\n" +
        " - Icicle effect is shot from the tornadoes when they hit a Bloon\n" +
        " - Icicle effect is reduced to 8 instead of 12\n" +
        " - Snowstorm creates slowing aura around the tower\n" +
        " - Vine Growth track vines create a small frost burst when they expire\n" +
        " - Vine Growth frost burst deals damage and freezes Bloons\n" +
        " - Rage increases attack speed while attacking Bloons";
}

public class BlizzardSelect : ComboSelect
{
    public override string WeaponName => "Blizzard";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var ice = Game.instance.model.GetTowerFromId("Druid").GetAttackModel().Duplicate();
        ice.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
        ice.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White | BloonProperties.Lead;
        ice.weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
        ice.weapons[0].projectile.AddBehavior(new FreezeModel("", 0, 1.5f, "CryoIce:Regular:Freeze", 3, "Ice", true, new Il2CppAssets.Scripts.Models.Bloons.Behaviors.GrowBlockModel(""), null, 0, false, false, false));

        var wind = Game.instance.model.GetTowerFromId("BoomerangMonkey").GetAttackModel().weapons[0].Duplicate();
        wind.projectile.display = Game.instance.model.GetTowerFromId("Druid-300").GetAttackModel(1).weapons[0].projectile.display;
        wind.projectile.pierce = 24;
        wind.projectile.maxPierce = 24;
        wind.projectile.AddBehavior(new WindModel("", 100, 200, 1, false, "Ice"));
        wind.projectile.GetDamageModel().damage = 0;
        wind.projectile.GetDamageModel().maxDamage = 0;
        wind.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        wind.projectile.GetBehavior<FollowPathModel>().speed = 72;
        wind.projectile.RemoveBehavior<RotateModel>();
        wind.emission = new ArcEmissionModel("", 3, 0, 45, null, false, false);

        var tempest = wind.projectile.Duplicate();
        tempest.display = Game.instance.model.GetTowerFromId("Druid-500").GetAttackModel().weapons[2].projectile.display;
        tempest.pierce = 200;
        tempest.maxPierce = 200;
        tempest.GetBehavior<WindModel>().distanceMax = 9999;
        tempest.GetBehavior<WindModel>().affectMoab = true;
        tempest.GetBehavior<WindModel>().distanceScaleForTags = 0.5f;
        tempest.GetBehavior<WindModel>().distanceScaleForTagsTags = "Zomg";
        wind.AddBehavior(new ChangeProjectilePerEmitModel("BlizzardMod", wind.projectile, tempest, 4, 6, 5, null, 0, 0, 0));

        // Stat Setter
        ice.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        ice.weapons[0].rate = weapon.speed;
        ice.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        wind.rate = weapon.speed * 2.6f;

        // Basic Stat Adjusters
        ice.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            ice.weapons[0].rate /= 1.06f;
            wind.rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            ice.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            wind.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            ice.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            wind.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        ice.AddWeapon(wind);
        towerModel.AddBehavior(ice);

        tower.UpdateRootModel(towerModel);
    }
}

public class BlizzardLevel : ComboLevel
{
    public override string WeaponName => "Blizzard";
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
                        var speed2 = (2 - weapon2.speed) / 2 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round((weapon1.pierce + weapon2.pierce) / 6);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round((weapon1.damage + weapon2.damage) / 2);
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
                        var speed2 = (2 - weapon1.speed) / 2 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round((weapon2.pierce + weapon1.pierce) / 6);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round((weapon2.damage + weapon1.damage) / 2);
                    }
                }
            }
        }
    }
}

public class BlizzardEquip : ComboEquiped
{
    public override string WeaponName => "Blizzard";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        towerModel.GetAttackModel().weapons[1].rate = weapon.speed * 2.6f;

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