using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using Assets;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using UnityEngine;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace SpaceMarine;

public class Railgun : ComboTemplate
{
    public override string WeaponName => "Railgun";
    public override string Icon => VanillaSprites.RayOfDoomUpgradeIcon;
    public override string[] comboWeapons => ["Crossbow", "Laser"];
    public override string Bonus => "High Energy Beam - Fires a single beam that damages Bloons that pass through it";
    public override float FontSize => 60;
    public override float[] StartingValues => [999, 2f, 2];
    public override string Range => "Long-Range";
    public override int[] PierceValue => [0];
    public override string SpecialMods =>
        "Fires a single beam that persists for a short while. Bloons that pass through the beam take damage every second.\n\n" +
        " - Long-Range weapon\n" +
        " - Burst Fire decreases the damage tick delay\n" +
        " - Sharpshooter increases the lifespan of the beam\n" +
        " - Piercing Shot upgraded projectile deals triple damage\n" +
        " - Refraction effect is reduced to 2 instead of 3 but has a smaller cone";
}

public class RailgunSelect : ComboSelect
{
    public override string WeaponName => "Railgun";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var railgun = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        var railgunProj = Game.instance.model.GetTowerFromId("TackShooter-030").GetAttackModel().weapons[0].projectile.Duplicate();
        railgun.weapons[0].projectile = railgunProj;
        railgun.weapons[0].emission = new ParallelEmissionModel("", 10, 160, 0, true, null);
        railgun.weapons[0].ejectY = 100;
        railgun.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed = 0;
        railgun.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 1f;
        railgun.weapons[0].projectile.AddBehavior(new RefreshPierceModel("", 0.15f, false));
        railgun.weapons[0].projectile.AddBehavior(new ClearHitBloonsModel("", 1f));
        railgun.weapons[0].projectile.ApplyDisplay<RailgunProj>();
        railgun.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;

        // Stat Setter
        railgun.weapons[0].projectile.pierce = weapon.pierce;
        railgun.weapons[0].rate = weapon.speed;
        railgun.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        railgun.range = 40 + (SpaceMarine.mod.rangeLvl * 12);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 12);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            railgun.weapons[0].rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            railgun.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            railgun.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        towerModel.AddBehavior(railgun);

        tower.UpdateRootModel(towerModel);
    }
}

public class RailgunLevel : ComboLevel
{
    public override string WeaponName => "Railgun";
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
                        var speed1 = (1 - weapon1.speed) / 4 + 1;
                        var speed2 = (1 - weapon2.speed) / 4 + 1;

                        combo.speed = Mathf.Round((2f / speed1 / speed2) * 100) / 100;
                        combo.damage = 2 + (int)Mathf.Round((weapon1.damage + weapon2.damage) / 3);
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
                        var speed1 = (1 - weapon1.speed) / 4 + 1;
                        var speed2 = (1 - weapon2.speed) / 4 + 1;

                        combo.speed = Mathf.Round((2f / speed2 / speed1) * 100) / 100;
                        combo.damage = 2 + (int)Mathf.Round((weapon2.damage + weapon1.damage) / 3);
                    }
                }
            }
        }
    }
}

public class RailgunEquip : ComboEquiped
{
    public override string WeaponName => "Railgun";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

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

        if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot" || SpaceMarine.mod.modifier3 == "Piercing Shot")
        {
            PiercingShotMod.PiercingShot(towerModel);
        }

        tower.UpdateRootModel(towerModel);
    }
}