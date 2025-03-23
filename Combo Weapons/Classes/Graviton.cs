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
using System;
using Sentries;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

namespace SpaceMarine;

public class Graviton : ComboTemplate
{
    public override string WeaponName => "Graviton";
    public override string Icon => VanillaSprites.MADUpgradeIconAA;
    public override string[] comboWeapons => ["Missile", "Magic"];
    public override string Bonus => "Event Horizon - Missile creates a vortex that sucks Bloons in and damages them continuously";
    public override float FontSize => 55;
    public override float[] StartingValues => [999, 8f, 1];
    public override string Range => "Mid-Range";
    public override int[] PierceValue => [0];
    public override string SpecialMods =>
        "Fires a graviton missile that creates a mini blackhole that will attract nearby Bloons into it. Bloons inside the vortex will recieve constant damage.\n\n" +
        " - Mid-Range weapon\n" +
        " - Incendiary further increases damage of the vortex\n" +
        " - Cluster Bomb causes vortex to fire waves of bombs out from its center\n" +
        " - Hex causes vortex to create a magic explosion when it expires\n" +
        " - Arcane Spike causes vortex to strike Bloons with lightning";
}

public class GravitonSelect : ComboSelect
{
    public override string WeaponName => "Graviton";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
        seeking.distance = 999;
        seeking.constantlyAquireNewTarget = true;

        var graviton = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        graviton.weapons[0].projectile.maxPierce = 1;
        graviton.weapons[0].projectile.pierce = 1;
        graviton.weapons[0].projectile.GetDamageModel().damage = 0;
        graviton.weapons[0].projectile.GetDamageModel().maxDamage = 0;
        graviton.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        graviton.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.display;
        graviton.weapons[0].projectile.AddBehavior(seeking);

        var vortex = GetTowerModel<VortexTower>().Duplicate();
        vortex.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage;

        // Stat Setter
        graviton.weapons[0].rate = weapon.speed;
        vortex.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        graviton.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            graviton.weapons[0].rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            graviton.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            vortex.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        graviton.weapons[0].projectile.AddBehavior(new CreateTowerModel("", vortex, 1, false, false, false, true, false));
        towerModel.AddBehavior(graviton);

        tower.UpdateRootModel(towerModel);
    }
}

public class GravitonLevel : ComboLevel
{
    public override string WeaponName => "Graviton";
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
                        
                        combo.speed = MathF.Round(8f / speed1 / speed2 * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round((weapon1.damage + weapon2.damage) / 3);
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
                        var speed2 = (2 - weapon2.speed) / 4 + 1;

                        combo.speed = MathF.Round(8f / speed2 / speed1 * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round((weapon2.damage + weapon1.damage) / 3);
                    }
                }
            }
        }
    }
}

public class GravitonEquip : ComboEquiped
{
    public override string WeaponName => "Graviton";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            towerModel.GetAttackModel().weapons[0].rate /= 1.06f;
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
        /*
        if (mod.modifier1 == "Cluster Bomb" || mod.modifier2 == "Cluster Bomb" || mod.modifier3 == "Cluster Bomb")
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
        }*/

        tower.UpdateRootModel(towerModel);
    }
}