using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using UnityEngine;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppSystem.Linq;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;

namespace SpaceMarine;

public class ClusterBombSelect : SpecialSelect
{
    public override string SpecialName => "Cluster Bomb";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Missile" || SpaceMarine.mod.weapon == "Hydra Rockets")
            {
                var explosion = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
                explosion.GetDamageModel().immuneBloonProperties = BloonProperties.Black;
                explosion.GetDamageModel().damage = towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage;

                if (SpaceMarine.mod.camoActive == true)
                {
                    explosion.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    explosion.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
                var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().Duplicate();
                var bombEffect = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();
                effect.effectModel = bombEffect;

                var bomb = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                bomb.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                bomb.GetDamageModel().damage = 0;
                bomb.pierce = 999;
                bomb.GetBehavior<TravelStraitModel>().Speed /= 2f;
                bomb.GetBehavior<TravelStraitModel>().Lifespan = 0.2f;
                bomb.display = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.display;
                bomb.scale /= 1.5f;

                bomb.AddBehavior(new CreateProjectileOnExpireModel("Explosion", explosion, new SingleEmissionModel("", null), false));
                bomb.AddBehavior(effect);
                bomb.AddBehavior(sound);

                if (SpaceMarine.mod.weapon == "Hydra Rockets")
                {
                    towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("ClusterBomb", bomb, new ArcEmissionModel("", (int)Mathf.Round(modifier.bonus / 2), 0, 360, null, true, false), true, true, false));
                }
                else
                {
                    towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("ClusterBomb", bomb, new ArcEmissionModel("", (int)modifier.bonus, 0, 360, null, true, false), true, true, false));
                }
            }

            if (SpaceMarine.mod.weapon == "Icy Barrage")
            {
                var explosion = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
                explosion.GetDamageModel().immuneBloonProperties = BloonProperties.Black;
                explosion.GetDamageModel().damage = 2 * modifier.level - 1;
                explosion.collisionPasses = new int[] { 0, -1 };
                explosion.AddBehavior(new FreezeModel("", 0, 1.5f, "CryoIce:Regular:Freeze", 3, "Ice", true, new GrowBlockModel(""), null, 0, false, false, false));

                if (SpaceMarine.mod.camoActive == true)
                {
                    explosion.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    explosion.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
                var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().Duplicate();
                var bombEffect = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();
                effect.effectModel = bombEffect;

                var bomb = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                bomb.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                bomb.GetDamageModel().damage = 0;
                bomb.pierce = 999;
                bomb.GetBehavior<TravelStraitModel>().Speed /= 2f;
                bomb.GetBehavior<TravelStraitModel>().Lifespan = 0.2f;
                bomb.display = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.display;
                bomb.scale /= 1.5f;

                bomb.AddBehavior(new CreateProjectileOnExpireModel("Explosion", explosion, new SingleEmissionModel("", null), false));
                bomb.AddBehavior(effect);
                bomb.AddBehavior(sound);

                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.
                    AddBehavior(new CreateProjectileOnExhaustFractionModel("ClusterBomb", bomb, new ArcEmissionModel("", (int)Mathf.Round(modifier.bonus / 2), 0, 360, null, true, false), 1, -1, false, false, false));
            }

            if (SpaceMarine.mod.weapon == "Eruption")
            {
                var burn = Game.instance.model.GetTowerFromId("MortarMonkey-002").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
                burn.lifespan = 5;
                burn.GetBehavior<DamageOverTimeModel>().damage = 1;
                burn.GetBehavior<DamageOverTimeModel>().interval = 0.5f;

                var explosion = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
                explosion.GetDamageModel().immuneBloonProperties = BloonProperties.Black & BloonProperties.Purple;
                explosion.GetDamageModel().damage = 2 * modifier.level - 1;
                explosion.collisionPasses = new[] { -1, 0, 1 };
                explosion.AddBehavior(burn);

                if (SpaceMarine.mod.camoActive == true)
                {
                    explosion.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    explosion.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
                var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().Duplicate();
                var bombEffect = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();
                effect.effectModel = bombEffect;

                var bomb = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                bomb.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                bomb.GetDamageModel().damage = 0;
                bomb.pierce = 999;
                bomb.GetBehavior<TravelStraitModel>().Speed /= 2f;
                bomb.GetBehavior<TravelStraitModel>().Lifespan = 0.25f;
                bomb.display = Game.instance.model.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.display;
                bomb.scale /= 1.3f;

                bomb.AddBehavior(new CreateProjectileOnExpireModel("Explosion", explosion, new SingleEmissionModel("", null), false));
                bomb.AddBehavior(effect);
                bomb.AddBehavior(sound);

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("ClusterBomb", bomb, new ArcEmissionModel("", (int)modifier.bonus, 0, 360, null, true, false), true, true, false));
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class ClusterBombLevel : SpecialLevel
{
    public override string SpecialName => "Cluster Bomb";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 2;
        }
    }
}

public class ClusterBombEquip : SpecialEquiped
{
    public override string SpecialName => "Cluster Bomb";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Missile")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("ClusterBomb"))
                {
                    behavior.GetDescendant<ArcEmissionModel>().count = (int)modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Hydra Rockets")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("ClusterBomb"))
                {
                    behavior.GetDescendant<ArcEmissionModel>().count = (int)Mathf.Round(modifier.bonus / 2);
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Icy Barrage")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnExhaustFractionModel>().ToArray())
            {
                if (behavior.name.Contains("ClusterBomb"))
                {
                    behavior.GetDescendant<ArcEmissionModel>().count = (int)Mathf.Round(modifier.bonus / 2);
                    behavior.projectile.GetDamageModel().damage = 2 * modifier.level - 1;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Eruption")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("ClusterBomb"))
                {
                    behavior.GetDescendant<ArcEmissionModel>().count = (int)modifier.bonus;
                    behavior.projectile.GetDamageModel().damage = 2 * modifier.level - 1;
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}