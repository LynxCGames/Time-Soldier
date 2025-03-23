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

namespace SpaceMarine;

public class ThornWrath : ComboTemplate
{
    public override string WeaponName => "Thorns of Wrath";
    public override string Icon => VanillaSprites.AvatarofWrathUpgradeIcon;
    public override string[] comboWeapons => ["Fire", "Thorn"];
    public override string Bonus => "Vengeance - Deals more damage the more Bloons there are";
    public override float FontSize => 54;
    public override float[] StartingValues => [3, 0.75f, 1];
    public override string Range => "Mid-Range";
    public override int[] PierceValue => [1];
    public override string SpecialMods =>
        "Fires 7 seared thorns at a time that set Bloons on fire for massive damage. The more Bloons that are out on the track, the more damage the thorns deal.\n\n" +
        " - Mid-Range weapon\n" +
        " - Flame Spreader increases the number of thorns fired\n" +
        " - Scorcher decreases the burn tick delay\n" +
        " - Vine Growth track vines slow passing Bloons by 50%\n" +
        " - Vine Growth slow is halved against MOABs\n" +
        " - Rage increases attack speed while attacking Bloons\n" +
        " - Rage give all towers in range bonus attack speed";
}

public class ThornWrathSelect : ComboSelect
{
    public override string WeaponName => "Thorns of Wrath";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var wrath = Game.instance.model.GetTowerFromId("Druid-005").GetAttackModel().weapons[0].projectile.GetBehavior<DamageModifierWrathModel>().Duplicate();

        var burn = Game.instance.model.GetTowerFromId("MortarMonkey-002").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
        burn.lifespan = 5;
        burn.GetBehavior<DamageOverTimeModel>().interval = 1;

        var thorns = Game.instance.model.GetTowerFromId("Druid").GetAttackModel().Duplicate();
        thorns.weapons[0].GetDescendant<RandomEmissionModel>().count = 7;
        thorns.weapons[0].projectile.collisionPasses = new[] { -1, 0, 1 };
        thorns.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("Druid-005").GetAttackModel().weapons[0].projectile.display;
        thorns.weapons[0].projectile.AddBehavior(wrath);

        // Stat Setter
        thorns.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        thorns.weapons[0].rate = weapon.speed;
        thorns.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        burn.GetBehavior<DamageOverTimeModel>().damage = weapon.damage + weapon.level + SpaceMarine.mod.damageLvl;
        thorns.weapons[0].projectile.AddBehavior(burn);

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
        }

        towerModel.AddBehavior(thorns);

        tower.UpdateRootModel(towerModel);
    }
}

public class ThornWrathLevel : ComboLevel
{
    public override string WeaponName => "Thorns of Wrath";
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
                        var speed2 = (2 - weapon1.speed) / 2 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round(weapon2.pierce / 2) + (int)Mathf.Round(weapon1.pierce / 2);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round(weapon2.damage / 2) + (int)Mathf.Round(weapon1.damage / 2);
                    }
                }
            }
        }
    }
}

public class ThornWrathEquip : ComboEquiped
{
    public override string WeaponName => "Thorns of Wrath";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        towerModel.GetAttackModel().weapons[0].projectile.GetDescendant<DamageOverTimeModel>().damage = weapon.damage + weapon.level + SpaceMarine.mod.damageLvl;

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