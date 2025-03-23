using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppSystem.Linq;

namespace SpaceMarine;

public class IciclesSelect : SpecialSelect
{
    public override string SpecialName => "Icicles";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Ice")
            {
                var icicles = Game.instance.model.GetTowerFromId("TackShooter-002").GetAttackModel().Duplicate();
                icicles.name = "IcicileMod";
                icicles.range = 25;
                icicles.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
                icicles.weapons[0].projectile.scale /= 2;
                icicles.weapons[0].rate = 0.9f;
                icicles.weapons[0].projectile.pierce = 2;
                icicles.weapons[0].projectile.maxPierce = 2;
                icicles.weapons[0].projectile.GetDamageModel().damage = modifier.bonus;
                icicles.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;
                icicles.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.6f;

                if (SpaceMarine.mod.camoActive == true)
                {
                    icicles.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    icicles.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.AddBehavior(icicles);
            }

            if (SpaceMarine.mod.weapon == "Icicle Impale")
            {
                var icicles = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().weapons[0].projectile.Duplicate();
                icicles.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
                icicles.scale /= 2;
                icicles.pierce = 2;
                icicles.maxPierce = 2;
                icicles.GetDamageModel().damage = modifier.bonus;
                icicles.GetDamageModel().immuneBloonProperties = BloonProperties.White;
                icicles.GetBehavior<TravelStraitModel>().Lifespan *= 1.6f;

                if (SpaceMarine.mod.camoActive == true)
                {
                    icicles.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    icicles.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("IcicileMod", icicles, new ArcEmissionModel("", 8, 0, 360, null, false, false), true, false, false));
            }

            if (SpaceMarine.mod.weapon == "Icy Barrage")
            {
                var icicles = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().weapons[0].projectile.Duplicate();
                icicles.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
                icicles.scale /= 2;
                icicles.pierce = 2;
                icicles.maxPierce = 2;
                icicles.GetDamageModel().damage = modifier.bonus;
                icicles.GetDamageModel().immuneBloonProperties = BloonProperties.White;
                icicles.GetBehavior<TravelStraitModel>().Lifespan *= 1.6f;

                if (SpaceMarine.mod.camoActive == true)
                {
                    icicles.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    icicles.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.
                    AddBehavior(new CreateProjectileOnExhaustFractionModel("IcicileMod", icicles, new ArcEmissionModel("", 6, 0, 360, null, true, false), 1, -1, false, false, false));
            }

            if (SpaceMarine.mod.weapon == "Precision Laser")
            {
                var icicles = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().weapons[0].projectile.Duplicate();
                icicles.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
                icicles.scale /= 2;
                icicles.pierce = 2;
                icicles.maxPierce = 2;
                icicles.GetDamageModel().damage = modifier.bonus;
                icicles.GetDamageModel().immuneBloonProperties = BloonProperties.White;
                icicles.GetBehavior<TravelStraitModel>().Lifespan *= 1.6f;

                if (SpaceMarine.mod.camoActive == true)
                {
                    icicles.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    icicles.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("IcicileMod", icicles, new ArcEmissionModel("", 6, 0, 360, null, false, false), true, false, false));

                if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
                {
                    PiercingShotMod.PiercingShot(towerModel);
                }
            }

            if (SpaceMarine.mod.weapon == "Blizzard")
            {
                towerModel.GetAttackModel().weapons[0].GetDescendant<RandomEmissionModel>().count = modifier.level + 5;

                var icicles = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().weapons[0].projectile.Duplicate();
                icicles.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
                icicles.scale /= 2;
                icicles.pierce = 2;
                icicles.maxPierce = 2;
                icicles.GetDamageModel().damage = modifier.bonus;
                icicles.GetDamageModel().immuneBloonProperties = BloonProperties.White;
                icicles.GetBehavior<TravelStraitModel>().Lifespan *= 1.6f;

                if (SpaceMarine.mod.camoActive == true)
                {
                    icicles.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    icicles.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[1].projectile.AddBehavior(new CreateProjectileOnContactModel("IcicileMod", icicles, new ArcEmissionModel("", 8, 0, 360, null, false, false), true, false, false));
            }

            if (SpaceMarine.mod.weapon == "Arctic Knight")
            {
                var icicle = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                icicle.GetDamageModel().damage = modifier.bonus;
                icicle.GetDamageModel().immuneBloonProperties = BloonProperties.White;
                icicle.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
                icicle.scale /= 2;
                icicle.pierce = 2;
                icicle.maxPierce = 2;

                if (SpaceMarine.mod.camoActive == true)
                {
                    icicle.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    icicle.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("IcicleMod", icicle, new ArcEmissionModel("", 3, 0, 25, null, true, false), false, false, false));
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class IciclesLevel : SpecialLevel
{
    public override string SpecialName => "Icicles";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            if (modifier.level < 4)
            {
                modifier.bonus += 1;
            }
            else if (modifier.level >= 4)
            {
                modifier.bonus += 2;
            }
        }
    }
}

public class IciclesEquip : SpecialEquiped
{
    public override string SpecialName => "Icicles";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Ice")
        {
            foreach (var attack in towerModel.GetAttackModels())
            {
                if (attack.name.Contains("IcicileMod"))
                {
                    attack.weapons[0].projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Icicle Impale")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("IcicileMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Icy Barrage")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnExhaustFractionModel>().ToArray())
            {
                if (behavior.name.Contains("IcicileMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Precision Laser")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("IcicileMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot" || SpaceMarine.mod.modifier3 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }
        }

        if (SpaceMarine.mod.weapon == "Blizzard")
        {
            towerModel.GetAttackModel().weapons[0].GetDescendant<RandomEmissionModel>().count = modifier.level + 5;

            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("IcicileMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Arctic Knight")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("IcicleMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}