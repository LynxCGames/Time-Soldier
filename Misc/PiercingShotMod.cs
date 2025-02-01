using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2Cpp;
using Assets;
using static SpaceMarine.SpaceMarine;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;

namespace SpaceMarine;

public class PiercingShotMod : BloonsTD6Mod
{
    public static void PiercingShot(TowerModel towerModel)
    {
        if (mod.weapon == "Laser")
        {
            var charge = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();
            charge.ApplyDisplay<PiercingLaser>();
            charge.GetDamageModel().damage *= 3;
            charge.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            charge.pierce = Mathf.Round(charge.pierce * 1.5f);
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel = charge;
        }
        if (mod.weapon == "Railgun")
        {
            var charge = towerModel.GetAttackModel(0).weapons[0].projectile.Duplicate();
            charge.ApplyDisplay<PiercingRailgunProj>();
            charge.GetDamageModel().damage *= 3;
            charge.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel = charge;
        }
        if (mod.weapon == "PrecisionLaser")
        {
            var charge = towerModel.GetAttackModel(0).weapons[0].projectile.Duplicate();
            charge.maxPierce = 6;
            charge.pierce = 6;
            charge.GetDescendants<DamageModel>().ForEach(model => model.damage *= 2);
            charge.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel = charge;
        }
        if (mod.weapon == "Elite Laser")
        {
            var charge = towerModel.GetAttackModel(0).weapons[0].projectile.Duplicate();
            charge.ApplyDisplay<PiercingLaser>();
            charge.GetDescendants<DamageModel>().ForEach(model => model.damage *= 2);
            charge.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
            charge.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().interval = 0.2f;
            towerModel.GetAttackModel().weapons[0].GetBehavior<ChangeProjectilePerEmitModel>().changedProjectileModel = charge;
        }
    }
}