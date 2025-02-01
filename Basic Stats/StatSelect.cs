using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

namespace SpaceMarine;

public class SelectingStat : BloonsTD6Mod
{
    public static void WeaponPierceSelect(Tower tower, WeaponTemplate weapon)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Normal Projectile Pierce
        if (weapon.PierceType == "Normal" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().weapons[0].projectile.pierce += weapon.PierceValue;
        }

        // Create Projectile On Contact Pierce
        if (weapon.PierceType == "OnContact" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce += weapon.PierceValue;
        }

        tower.UpdateRootModel(towerModel);
    }

    public static void ComboPierceSelect(Tower tower, ComboTemplate weapon)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Normal Projectile Pierce
        if (weapon.PierceType == "Normal" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().weapons[0].projectile.pierce += weapon.PierceValue;
        }

        // Create Projectile On Contact Pierce
        if (weapon.PierceType == "OnContact" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce += weapon.PierceValue;
        }

        // Create Projectile On Exhaust Pierce
        if (weapon.PierceType == "OnExhaust" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.pierce += weapon.PierceValue;
        }

        // Create Projectile In Area Pierce
        if (weapon.PierceType == "InArea" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.pierce += weapon.PierceValue;
        }

        tower.UpdateRootModel(towerModel);
    }

    public static void WeaponRangeSelect(Tower tower, WeaponTemplate weapon)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Short Range
        if (weapon.Range == "Short-Range" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().range += 5;
            towerModel.range += 5;
            towerModel.GetAttackModel().weapons[0].projectile.radius += 5;

            tower.UpdateRootModel(towerModel);
        }

        // Medium Range
        if (weapon.Range == "Mid-Range" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().range += 8;
            towerModel.range += 8;

            tower.UpdateRootModel(towerModel);
        }

        // Long Range
        if (weapon.Range == "Long-Range" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().range += 12;
            towerModel.range += 12;

            tower.UpdateRootModel(towerModel);
        }
    }

    public static void ComboRangeSelect(Tower tower, ComboTemplate weapon)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Short Range


        // Medium Range
        if (weapon.Range == "Mid-Range" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().range += 8;
            towerModel.range += 8;

            tower.UpdateRootModel(towerModel);
        }

        // Long Range
        if (weapon.Range == "Long-Range" && weapon.WeaponName == mod.weapon)
        {
            towerModel.GetAttackModel().range += 12;
            towerModel.range += 12;

            tower.UpdateRootModel(towerModel);
        }
    }
}