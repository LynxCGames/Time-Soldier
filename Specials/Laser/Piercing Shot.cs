using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using UnityEngine;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets;
using System.Linq;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;

namespace SpaceMarine;

public class PiercingShotSelect : SpecialSelect
{
    public override string SpecialName => "Piercing Shot";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Laser")
            {
                var charge = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();
                charge.ApplyDisplay<PiercingLaser>();
                charge.GetDamageModel().damage *= 3;
                charge.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                charge.pierce = Mathf.Round(charge.pierce * 1.5f);
                towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("PiercingMod", towerModel.GetAttackModel().weapons[0].projectile, charge, (int)modifier.bonus, 6, 5, null, 0, 0, 0));
            }

            if (SpaceMarine.mod.weapon == "Railgun")
            {
                var charge = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();
                charge.ApplyDisplay<PiercingRailgunProj>();
                charge.GetDamageModel().damage *= 3;
                charge.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("PiercingMod", towerModel.GetAttackModel().weapons[0].projectile, charge, (int)modifier.bonus, 6, 5, null, 0, 0, 0));
            }

            if (SpaceMarine.mod.weapon == "Precision Laser")
            {
                var charge = towerModel.GetAttackModel(0).weapons[0].projectile.Duplicate();
                charge.maxPierce = 6;
                charge.pierce = 6;
                charge.GetDescendants<DamageModel>().ForEach(model => model.damage *= 2);
                charge.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("PiercingMod", towerModel.GetAttackModel().weapons[0].projectile, charge, (int)modifier.bonus, 6, 5, null, 0, 0, 0));
            }

            if (SpaceMarine.mod.weapon == "Elite Laser")
            {
                var charge = towerModel.GetAttackModel(0).weapons[0].projectile.Duplicate();
                charge.ApplyDisplay<PiercingLaser>();
                charge.GetDescendants<DamageModel>().ForEach(model => model.damage *= 2);
                charge.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                charge.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().interval = 0.2f;
                towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("PiercingMod", towerModel.GetAttackModel().weapons[0].projectile, charge, (int)modifier.bonus, 6, 5, null, 0, 0, 0));
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class PiercingShotLevel : SpecialLevel
{
    public override string SpecialName => "Piercing Shot";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus -= 1;
        }
    }
}

public class PiercingShotEquip : SpecialEquiped
{
    public override string SpecialName => "Piercing Shot";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        for (int i = 0; i < modifier.Weapons.Count(); i++)
        {
            if (SpaceMarine.mod.weapon == modifier.Weapons[i])
            {
                towerModel.GetAttackModel().GetDescendant<ChangeProjectilePerEmitModel>().forProjectileCount = (int)modifier.bonus;
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}