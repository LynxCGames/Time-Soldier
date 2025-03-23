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
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;

namespace SpaceMarine;

public class ForestSpirit : ComboTemplate
{
    public override string WeaponName => "Forest Spirit";
    public override string Icon => VanillaSprites.SpiritoftheForestUpgradeIcon;
    public override string[] comboWeapons => ["Crossbow", "Thorn"];
    public override string Bonus => "Nature's Domain - Creates thorns nearby on the track that continuously pop Bloons that pass over them";
    public override float FontSize => 54;
    public override float[] StartingValues => [2, 1.4f, 1];
    public override string Range => "Mid-Range";
    public override int[] PierceValue => [1];
    public override string SpecialMods =>
        "Fires 5 thorns at a time. Creates a domain of thorns on the track nearby that deals constant damage to Bloons.\n\n" +
        " - Mid-Range weapon\n" +
        " - Burst Fire increases the thorn domain's damage\n" +
        " - Burst Fire decreases the damage tick delay of the track thorns\n" +
        " - Sharpshooter causes thorns to crit\n" +
        " - Vine Growth attacks significantly slower but can grab small MOABs\n" +
        " - Vine Growth shoots out 8 thorns from the grabbed target\n" +
        " - Rage increases attack speed while attacking Bloons";
}

public class ForestSpiritSelect : ComboSelect
{
    public override string WeaponName => "Forest Spirit";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var vines = Game.instance.model.GetTowerFromId("Druid-050").GetBehavior<SpiritOfTheForestModel>().Duplicate();
        vines.name = "ForestSpiritTSMod";
        vines.damageOverTimeZoneModelFar.GetDescendant<DamageOverTimeCustomModel>().interval = 0.5f;
        vines.damageOverTimeZoneModelFar.GetDescendant<DamageOverTimeCustomModel>().additive = 0;
        vines.middleRange = 0;
        vines.closeRange = 0;
        vines.circleRadius = 70;
        vines.time = 10;

        var thorns = Game.instance.model.GetTowerFromId("Druid").GetAttackModel().Duplicate();

        // Stat Setter
        thorns.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        thorns.weapons[0].rate = weapon.speed;
        thorns.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        vines.damageOverTimeZoneModelFar.GetDescendant<DamageOverTimeCustomModel>().damage = weapon.level + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        thorns.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            thorns.weapons[0].rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            thorns.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            thorns.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            vines.GetDescendant<DamageOverTimeCustomModel>().immuneBloonProperties = BloonProperties.None;
        }

        towerModel.AddBehavior(thorns);
        towerModel.AddBehavior(vines);

        tower.UpdateRootModel(towerModel);
    }
}

public class ForestSpiritLevel : ComboLevel
{
    public override string WeaponName => "Forest Spirit";
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

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round(weapon1.pierce / 2) + (int)Mathf.Round(weapon2.pierce / 2);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round(weapon1.damage / 3) + (int)Mathf.Round(weapon2.damage / 2);
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

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round(weapon2.pierce / 2) + (int)Mathf.Round(weapon1.pierce / 2);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round(weapon2.damage / 3) + (int)Mathf.Round(weapon1.damage / 2);
                    }
                }
            }
        }
    }
}

public class ForestSpiritEquip : ComboEquiped
{
    public override string WeaponName => "Forest Spirit";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        towerModel.GetBehavior<SpiritOfTheForestModel>().damageOverTimeZoneModelFar.GetDescendant<DamageOverTimeCustomModel>().damage = weapon.level + SpaceMarine.mod.damageLvl;

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

        if (SpaceMarine.mod.modifier1 == "Burst Fire" || SpaceMarine.mod.modifier2 == "Burst Fire" || SpaceMarine.mod.modifier3 == "Burst Fire")
        {
            foreach (var modifier in GetContent<SpecialTemplate>())
            {
                if (modifier.ModName == "Burst Fire")
                {
                    towerModel.GetBehavior<SpiritOfTheForestModel>().damageOverTimeZoneModelFar.GetDescendant<DamageOverTimeCustomModel>().damage += modifier.level;
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}