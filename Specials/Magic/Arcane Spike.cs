using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using Il2CppAssets.Scripts.Unity;
using System.Linq;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppSystem.Linq;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;

namespace SpaceMarine;

public class ArcaneSpikeSelect : SpecialSelect
{
    public override string SpecialName => "Arcane Spike";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Magic")
            {
                var lightning = Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons[1].projectile.Duplicate();
                lightning.pierce = 5;
                lightning.GetDamageModel().damage = modifier.bonus;

                if (modifier.level == 1 || modifier.level == 2)
                {
                    lightning.GetBehavior<LightningModel>().splits = 1;
                }
                else if (modifier.level == 3 || modifier.level == 4)
                {
                    lightning.GetBehavior<LightningModel>().splits = 2;
                }
                else if (modifier.level == 5 || modifier.level == 6)
                {
                    lightning.GetBehavior<LightningModel>().splits = 3;
                }

                if (SpaceMarine.mod.camoActive == true)
                {
                    lightning.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    lightning.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("ArcaneSpikeMod", lightning, new SingleEmissionModel("", null), false, false, false));
            }

            if (SpaceMarine.mod.weapon == "Graviton")
            {
                var lightning = Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().Duplicate();
                lightning.name = "ArcaneSpikeMod";
                lightning.range = 35;
                lightning.RemoveWeapon(lightning.weapons[0]);
                lightning.weapons[0].ejectX = 0;
                lightning.weapons[0].ejectY = 0;
                lightning.weapons[0].projectile.GetDamageModel().damage = 2 * modifier.bonus;
                lightning.weapons[0].rate = 1.4f;

                if (SpaceMarine.mod.camoActive == true)
                {
                    lightning.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    lightning.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetDescendant<CreateTowerModel>().tower.AddBehavior(lightning);
            }

            if (SpaceMarine.mod.weapon == "Arctic Knight")
            {
                var lightning = Game.instance.model.GetTowerFromId("Druid-400").GetAttackModel().weapons[1].projectile.Duplicate();
                lightning.scale /= 2;
                lightning.GetBehavior<TravelStraitModel>().lifespan /= 3;
                lightning.GetBehavior<CreateProjectileOnIntervalModel>().projectile.pierce = 7;
                lightning.GetBehavior<CreateProjectileOnIntervalModel>().projectile.GetDamageModel().damage = 2 * modifier.bonus;
                lightning.GetBehavior<CreateProjectileOnIntervalModel>().projectile.GetBehavior<LightningModel>().splits = 1;

                if (SpaceMarine.mod.camoActive == true)
                {
                    lightning.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    lightning.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("ArcaneSpikeMod", lightning, new ArcEmissionModel("", 2, 0, 180, null, true, false), 1, 1, false, false, false));
            }

            if (SpaceMarine.mod.weapon == "Necromancer")
            {
                var lightning = Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons[1].projectile.Duplicate();
                lightning.pierce = 5;
                lightning.GetDamageModel().damage = modifier.bonus;

                if (modifier.level == 1 || modifier.level == 2)
                {
                    lightning.GetBehavior<LightningModel>().splits = 1;
                }
                else if (modifier.level == 3 || modifier.level == 4)
                {
                    lightning.GetBehavior<LightningModel>().splits = 2;
                }
                else if (modifier.level == 5 || modifier.level == 6)
                {
                    lightning.GetBehavior<LightningModel>().splits = 3;
                }

                if (SpaceMarine.mod.camoActive == true)
                {
                    lightning.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    lightning.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("ArcaneSpikeMod", lightning, new SingleEmissionModel("", null), false, false, false));

                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed = 35 * ((modifier.bonus + 2) / 10 + 1);
                towerModel.GetAttackModel(1).weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<TravelAlongPathModel>().speed = 20 * ((modifier.bonus + 2) / 10 + 1);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class ArcaneSpikeLevel : SpecialLevel
{
    public override string SpecialName => "Arcane Spike";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus++;
        }
    }
}

public class ArcaneSpikeEquiped : SpecialEquiped
{
    public override string SpecialName => "Arcane Spike";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Magic")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("ArcaneSpikeMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;

                    if (modifier.level == 1 || modifier.level == 2)
                    {
                        behavior.projectile.GetBehavior<LightningModel>().splits = 1;
                    }
                    else if (modifier.level == 3 || modifier.level == 4)
                    {
                        behavior.projectile.GetBehavior<LightningModel>().splits = 2;
                    }
                    else if (modifier.level == 5 || modifier.level == 6)
                    {
                        behavior.projectile.GetBehavior<LightningModel>().splits = 3;
                    }
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Graviton")
        {
            var vortex = towerModel.GetDescendant<CreateTowerModel>().tower;

            foreach (var attack in vortex.GetAttackModels())
            {
                if (attack.name.Contains("ArcaneSpikeMod"))
                {
                    attack.weapons[0].projectile.GetDamageModel().damage = 2 * modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Arctic Knight")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnExhaustFractionModel>().ToArray())
            {
                if (behavior.name.Contains("ArcaneSpikeMod"))
                {
                    behavior.projectile.GetBehavior<CreateProjectileOnIntervalModel>().projectile.GetDamageModel().damage = 2 * modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Necromancer")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("ArcaneSpikeMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;

                    if (modifier.level == 1 || modifier.level == 2)
                    {
                        behavior.projectile.GetBehavior<LightningModel>().splits = 1;
                    }
                    else if (modifier.level == 3 || modifier.level == 4)
                    {
                        behavior.projectile.GetBehavior<LightningModel>().splits = 2;
                    }
                    else if (modifier.level == 5 || modifier.level == 6)
                    {
                        behavior.projectile.GetBehavior<LightningModel>().splits = 3;
                    }
                }
            }

            towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed = 35 * ((modifier.bonus + 2) / 10 + 1);
            towerModel.GetAttackModel(1).weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<TravelAlongPathModel>().speed = 20 * ((modifier.bonus + 2) / 10 + 1);
        }

        tower.UpdateRootModel(towerModel);
    }
}