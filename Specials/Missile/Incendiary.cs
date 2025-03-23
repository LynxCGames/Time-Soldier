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
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using static System.Reflection.Metadata.BlobBuilder;

namespace SpaceMarine;

public class IncendiarySelect : SpecialSelect
{
    public override string SpecialName => "Incendiary";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Missile" || SpaceMarine.mod.weapon == "Hydra Rockets" || SpaceMarine.mod.weapon == "Eruption")
            {
                var fireWall = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.Duplicate();
                fireWall.GetBehavior<AgeModel>().lifespan = 3;
                fireWall.GetBehavior<ClearHitBloonsModel>().interval = 0.25f;
                fireWall.AddBehavior(new RefreshPierceModel("", 0.15f, false));
                fireWall.RemoveBehavior<SetSpriteFromPierceModel>();
                fireWall.pierce = 20;
                fireWall.display = Game.instance.model.GetTowerFromId("WizardMonkey-020").GetAttackModel(2).weapons[0].projectile.display;
                fireWall.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-020").GetAttackModel(2).weapons[0].projectile.GetBehavior<CreateEffectOnExhaustedModel>().Duplicate());
                fireWall.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                fireWall.GetDamageModel().damage = modifier.bonus;

                if (SpaceMarine.mod.camoActive == true)
                {
                    fireWall.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    fireWall.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("Incendiary", fireWall, new SingleEmissionModel("", null), false, false, false));
            }

            if (SpaceMarine.mod.weapon == "Icy Barrage")
            {
                var fireWall = Game.instance.model.GetTowerFromId("WizardMonkey-020").GetAttackModel(2).weapons[0].projectile.Duplicate();
                fireWall.GetDamageModel().damage = modifier.bonus;
                fireWall.GetBehavior<AgeModel>().lifespan = 3;
                fireWall.GetBehavior<ClearHitBloonsModel>().interval = 0.25f;
                fireWall.pierce = 20;

                if (SpaceMarine.mod.camoActive == true)
                {
                    fireWall.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    fireWall.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.
                    AddBehavior(new CreateProjectileOnExhaustFractionModel("Incendiary", fireWall, new SingleEmissionModel("", null), 1, -1, false, false, false));
            }

            if (SpaceMarine.mod.weapon == "Plasma Launcher")
            {
                var fireWall = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.Duplicate();
                fireWall.GetBehavior<AgeModel>().lifespan = 3;
                fireWall.GetBehavior<ClearHitBloonsModel>().interval = 0.25f;
                fireWall.AddBehavior(new RefreshPierceModel("", 0.15f, false));
                fireWall.RemoveBehavior<SetSpriteFromPierceModel>();
                fireWall.pierce = 20;
                fireWall.display = Game.instance.model.GetTowerFromId("WizardMonkey-020").GetAttackModel(2).weapons[0].projectile.display;
                fireWall.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-020").GetAttackModel(2).weapons[0].projectile.GetBehavior<CreateEffectOnExhaustedModel>().Duplicate());
                fireWall.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                fireWall.GetDamageModel().damage = modifier.bonus;

                if (SpaceMarine.mod.camoActive == true)
                {
                    fireWall.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    fireWall.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.AddBehavior
                    (new CreateProjectileOnContactModel("Incendiary", fireWall, new SingleEmissionModel("", null), false, false, false));

                foreach (var behavior in towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetDescendants<CreateProjectileOnContactModel>().ToArray())
                {
                    if (behavior.name.Contains("MiniRocketStorm"))
                    {
                        behavior.projectile.AddBehavior(new CreateProjectileOnContactModel("Incendiary", fireWall, new SingleEmissionModel("", null), false, false, false));
                    }
                }
            }

            if (SpaceMarine.mod.weapon == "Graviton")
            {
                var fire = Game.instance.model.GetTowerFromId("TackShooter-400").GetAttackModel().Duplicate();
                fire.name = "IncendiaryMod";
                fire.range = 35;
                fire.weapons[0].rate = 0.5f;
                fire.weapons[0].projectile.GetDamageModel().damage = modifier.bonus;

                if (SpaceMarine.mod.camoActive == true)
                {
                    fire.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    fire.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetDescendant<CreateTowerModel>().tower.AddBehavior(fire);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class IncendiaryLevel : SpecialLevel
{
    public override string SpecialName => "Incendiary";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            if (modifier.level < 4)
            {
                modifier.bonus += 1;
            }
            else if (modifier.level >= 4 && modifier.level < 7)
            {
                modifier.bonus += 2;
            }
            else if (modifier.level >= 7)
            {
                modifier.bonus += 3;
            }
        }
    }
}

public class IncendiaryEquip : SpecialEquiped
{
    public override string SpecialName => "Incendiary";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Missile" || SpaceMarine.mod.weapon == "Hydra Rockets" || SpaceMarine.mod.weapon == "Eruption")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("Incendiary"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Icy Barrage")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnExhaustFractionModel>().ToArray())
            {
                if (behavior.name.Contains("Incendiary"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Plasma Launcher")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("MiniRocketStorm"))
                {
                    foreach (var fire in behavior.projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
                    {
                        if (fire.name.Contains("Incendiary"))
                        {
                            fire.projectile.GetDamageModel().damage = modifier.bonus;
                        }
                    }
                }

                if (behavior.name.Contains("Incendiary"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Graviton")
        {
            var vortex = towerModel.GetDescendant<CreateTowerModel>().tower;

            foreach (var attack in vortex.GetAttackModels())
            {
                if (attack.name.Contains("IncendiaryMod"))
                {
                    attack.weapons[0].projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}