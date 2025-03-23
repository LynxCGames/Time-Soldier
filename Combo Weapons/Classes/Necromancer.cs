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
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;

namespace SpaceMarine;

public class Mystery : ComboTemplate
{
    public override string WeaponName => "???";
    public override string Icon => VanillaSprites.SettingsIcon;
    public override string[] comboWeapons => ["??", "??"];
    public override string Bonus => "???";
    public override float FontSize => 80;
    public override float[] StartingValues => [0, 0f, 0];
    public override string Range => "??";
    public override int[] PierceValue => [0];
    public override string SpecialMods =>
        "Discover combo to unlock almanac entry";
}

public class Necromancer : ComboTemplate
{
    public override string WeaponName => "Necromancer";
    public override string Icon => VanillaSprites.NecromancerUnpoppedArmyUpgradeIcon;
    public override string[] comboWeapons => ["Magic", "Charm"];
    public override string Bonus => "Unpopped Army - Summons undead Bloons along the track";
    public override float FontSize => 55;
    public override float[] StartingValues => [5, 1f, 2];
    public override string Range => "Mid-Range";
    public override int[] PierceValue => [1];
    public override string SpecialMods =>
        "Fires a more powerful magic bolt. Constantly summons undead Bloons that travel along the track and damage any Bloons they run into. Occassionally summons undead MOABs that deal even more damage.\n\n" +
        " - Mid-Range weapon\n" +
        " - Hex causes main attack to inflict damage over time\n" +
        " - Hex causes undead Bloons to create a magic explosion when they hit a Bloon\n" +
        " - Arcane Spike makes main attack strick Bloons with lightning\n" +
        " - Arcane Spike makes undead Bloons travel faster along the track";
}

public class NecromancerSelect : ComboSelect
{
    public override string WeaponName => "Necromancer";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model
        var magic = Game.instance.model.GetTowerFromId("WizardMonkey-100").GetAttackModel().Duplicate();

        var agemodel = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.GetBehavior<AgeModel>().Duplicate();
        agemodel.lifespanFrames = 0;
        agemodel.lifespan = 12f;
        agemodel.rounds = 9999;

        var Moab = Game.instance.model.GetTowerFromId("WizardMonkey-005").GetAttackModel(2).weapons[1].projectile.Duplicate();
        Moab.GetBehavior<TravelAlongPathModel>().lifespanFrames = 0;
        Moab.GetBehavior<TravelAlongPathModel>().lifespan = 25f;
        Moab.GetBehavior<TravelAlongPathModel>().speed = 20;
        Moab.AddBehavior(agemodel);

        var targetSelect = Game.instance.model.GetTowerFromId("EngineerMonkey-025").GetAttackModel(1).GetBehavior<TargetSelectedPointModel>().Duplicate();
        var summon = Game.instance.model.GetTowerFromId("WizardMonkey-004").GetAttackModel(2).Duplicate();
        summon.name = "NecromancerMod";
        summon.range = towerModel.range;
        summon.weapons[0].emission = new NecromancerEmissionModel("BaseDeploy_", 3, 3, 1, 3, 1, 1, 0, null, null, null, 1, 1, 1, 1, 2);
        summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames = 0;
        summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespan = 15f;
        summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed = 35;
        summon.weapons[0].projectile.AddBehavior(agemodel);
        summon.AddBehavior(targetSelect);

        // Stat Setter
        magic.weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        magic.weapons[0].rate = weapon.speed;
        magic.weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        summon.weapons[0].projectile.pierce = weapon.pierce * 2;
        summon.weapons[0].rate = weapon.speed * 1.5f;
        summon.weapons[0].projectile.GetDamageModel().damage = weapon.damage;

        Moab.pierce = weapon.pierce * 4;
        Moab.GetDamageModel().damage = weapon.damage * 5;

        // Basic Stat Adjusters
        magic.range = 40 + (SpaceMarine.mod.rangeLvl * 8);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 8);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            magic.weapons[0].rate /= 1.06f;
            summon.weapons[0].rate /= 1.06f;
        }
        if (SpaceMarine.mod.camoActive == true)
        {
            magic.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            magic.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        summon.weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("NecromancerMod", summon.weapons[0].projectile, Moab, 15, 6, 5, null, 0, 0, 0));
        towerModel.AddBehavior(magic);
        towerModel.AddBehavior(summon);

        tower.UpdateRootModel(towerModel);
    }
}

public class NecromancerLevel : ComboLevel
{
    public override string WeaponName => "Necromancer";
    public override void Level(WeaponTemplate magic, ComboTemplate combo)
    {
        foreach (var charm in GetContent<ModifierTemplate>())
        {
            if (magic.WeaponName == combo.comboWeapons[0] && charm.ModName == combo.comboWeapons[1])
            {
                if (magic.isUnlocked == true && charm.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((magic.level + charm.level) / 2) + 1;

                    if (combo.level > 1)
                    {
                        var speed1 = (1 - magic.speed) / 2 + 1;
                        var speed2 = charm.bonus / 20 + 1;

                        combo.pierce = (int)combo.StartingValues[0] + (int)Mathf.Round((magic.pierce + charm.bonus) / 2);
                        combo.speed = Mathf.Round(combo.StartingValues[1] / speed1 / speed2 * 100) / 100;
                        combo.damage = (int)combo.StartingValues[2] + (int)Mathf.Round((magic.damage + charm.bonus) / 2);
                    }
                }
            }
        }
    }
}

public class NecromancerEquip : ComboEquiped
{
    public override string WeaponName => "Necromancer";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel(0).weapons[0].projectile.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel(0).weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel(0).weapons[0].projectile.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        towerModel.GetAttackModel(1).weapons[0].projectile.pierce = weapon.pierce * 2;
        towerModel.GetAttackModel(1).weapons[0].rate = weapon.speed * 1.5f;
        towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().damage = weapon.damage;

        towerModel.GetAttackModel(1).weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.pierce = weapon.pierce * 4;
        towerModel.GetAttackModel(1).weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetDamageModel().damage = weapon.damage * 5;

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