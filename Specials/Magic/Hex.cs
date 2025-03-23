using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppSystem.Linq;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;

namespace SpaceMarine;

public class HexSelect : SpecialSelect
{
    public override string SpecialName => "Hex";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Magic")
            {
                var curse = Game.instance.model.GetTowerFromId("Ezili").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
                curse.lifespan = 5;
                curse.GetBehavior<DamageOverTimeModel>().interval = 1;
                curse.GetBehavior<DamageOverTimeModel>().damage = modifier.bonus;
                towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0, 1 };

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(curse);
            }

            if (SpaceMarine.mod.weapon == "Graviton")
            {
                var onDestroy = Game.instance.model.GetTowerFromId("SentryParagon").GetBehavior<CreateProjectileOnTowerDestroyModel>().Duplicate();
                onDestroy.projectileModel.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
                onDestroy.projectileModel.GetBehavior<AgeModel>().lifespan = 0.1f;

                var effect = Game.instance.model.GetTowerFromId("Ezili 4").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();
                effect.scale *= 2;

                var burst = Game.instance.model.GetTowerFromId("Ezili 4").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
                burst.pierce = 999;
                burst.radius = 35;
                burst.GetDamageModel().damage = modifier.bonus;
                burst.RemoveBehavior<AddBehaviorToBloonModel>();
                burst.RemoveBehavior<DamageModifierForTagModel>();

                onDestroy.projectileModel.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().effectModel = effect;
                onDestroy.projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile = burst;

                towerModel.GetDescendant<CreateTowerModel>().tower.AddBehavior(onDestroy);
            }

            if (SpaceMarine.mod.weapon == "Arctic Knight")
            {
                var effect = Game.instance.model.GetTowerFromId("Ezili 4").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                var burst = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                burst.name = "HexMod";
                burst.projectile.GetDamageModel().damage = modifier.bonus;
                burst.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;

                if (SpaceMarine.mod.camoActive == true)
                {
                    burst.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    burst.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(effect);
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(burst);
            }

            if (SpaceMarine.mod.weapon == "Necromancer")
            {
                var curse = Game.instance.model.GetTowerFromId("Ezili").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
                curse.lifespan = 5;
                curse.GetBehavior<DamageOverTimeModel>().interval = 1;
                curse.GetBehavior<DamageOverTimeModel>().damage = modifier.bonus;
                towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0, 1 };

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(curse);


                var effect = Game.instance.model.GetTowerFromId("Ezili 4").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                var burst = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                burst.name = "HexMod";
                burst.projectile.GetDamageModel().damage = modifier.bonus;
                burst.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;

                var bigEffect = Game.instance.model.GetTowerFromId("Ezili 4").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                var bigBurst = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                bigBurst.name = "HexMod";
                bigBurst.projectile.GetDamageModel().damage = modifier.bonus;
                bigBurst.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                bigEffect.effectModel.scale *= 2;

                if (SpaceMarine.mod.camoActive == true)
                {
                    burst.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    bigBurst.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    burst.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    bigBurst.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel(1).weapons[0].projectile.AddBehavior(effect);
                towerModel.GetAttackModel(1).weapons[0].projectile.AddBehavior(burst);
                towerModel.GetAttackModel(1).weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.AddBehavior(bigEffect);
                towerModel.GetAttackModel(1).weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel.AddBehavior(bigBurst);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class HexLevel : SpecialLevel
{
    public override string SpecialName => "Hex";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            if (modifier.level <= 2)
            {
                modifier.bonus += 1;
            }
            else if (modifier.level > 2 && modifier.level < 6)
            {
                modifier.bonus += 2;
            }
            else if (modifier.level >= 6)
            {
                modifier.bonus += 3;
            }
        }
    }
}

public class HexEquiped : SpecialEquiped
{
    public override string SpecialName => "Hex";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Magic")
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = modifier.bonus;
        }

        if (SpaceMarine.mod.weapon == "Graviton")
        {
            towerModel.GetDescendant<CreateTowerModel>().tower.GetBehavior<CreateProjectileOnTowerDestroyModel>().projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = modifier.bonus;
        }

        if (SpaceMarine.mod.weapon == "Arctic Knight")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("HexMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Necromancer")
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = modifier.bonus;

            foreach (var behavior in towerModel.GetAttackModel(1).weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("HexMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}