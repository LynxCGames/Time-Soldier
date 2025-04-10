﻿using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppSystem.Linq;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

namespace SpaceMarine;

public class SharpshooterSelect : SpecialSelect
{
    public override string SpecialName => "Sharpshooter";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Crossbow" || SpaceMarine.mod.weapon == "Icicle Impale" || SpaceMarine.mod.weapon == "Forest Spirit")
            {
                var text = Game.instance.model.GetTowerFromId("DartMonkey-004").GetAttackModel().weapons[0].projectile.GetBehavior<ShowTextOnHitModel>().Duplicate();
                var crit = Game.instance.model.GetTowerFromId("DartMonkey-004").GetAttackModel().weapons[0].GetBehavior<CritMultiplierModel>().Duplicate();
                crit.damage = towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage * modifier.bonus;
                crit.lower = 6;
                crit.upper = 6;

                towerModel.GetAttackModel().weapons[0].AddBehavior(crit);
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(text);
            }

            if (SpaceMarine.mod.weapon == "Hydra Rockets")
            {
                var text = Game.instance.model.GetTowerFromId("DartMonkey-004").GetAttackModel().weapons[0].projectile.GetBehavior<ShowTextOnHitModel>().Duplicate();
                var crit = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();
                crit.display = Game.instance.model.GetTowerFromId("DartlingGunner-040").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.display;
                text.text = "Crit";
                crit.AddBehavior(text);

                foreach (var behavior in crit.GetDescendants<CreateProjectileOnContactModel>().ToArray())
                {
                    if (behavior.name.Contains("HydraRocket"))
                    {
                        behavior.projectile.GetDamageModel().damage *= modifier.bonus;
                    }
                }

                towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("SharpshotMod", towerModel.GetAttackModel().weapons[0].projectile, crit, 6, 6, 5, null, 0, 0, 0));
            }

            if (SpaceMarine.mod.weapon == "Railgun")
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 1 + (0.1f * modifier.bonus);

                if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
                {
                    PiercingShotMod.PiercingShot(towerModel);
                }
            }

            if (SpaceMarine.mod.weapon == "Fireworks")
            {
                var text = Game.instance.model.GetTowerFromId("DartMonkey-004").GetAttackModel().weapons[0].projectile.GetBehavior<ShowTextOnHitModel>().Duplicate();
                text.text = "Crit";

                var firecrackerCrit = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();
                var fireworkCrit = towerModel.GetAttackModel().weapons[1].projectile.Duplicate();

                firecrackerCrit.AddBehavior(text);
                fireworkCrit.AddBehavior(text);

                firecrackerCrit.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage *= modifier.bonus;
                fireworkCrit.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage *= modifier.bonus;

                towerModel.GetAttackModel().weapons[0].AddBehavior(new ChangeProjectilePerEmitModel("SharpshotMod", towerModel.GetAttackModel().weapons[0].projectile, firecrackerCrit, 8, 6, 5, null, 0, 0, 0));
                towerModel.GetAttackModel().weapons[1].AddBehavior(new ChangeProjectilePerEmitModel("SharpshotMod", towerModel.GetAttackModel().weapons[0].projectile, fireworkCrit, 3, 6, 5, null, 0, 0, 0));
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class SharpshooterLevel : SpecialLevel
{
    public override string SpecialName => "Sharpshooter";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 1;
        }
    }
}

public class SharpshooterEquip : SpecialEquiped
{
    public override string SpecialName => "Sharpshooter";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Crossbow" || SpaceMarine.mod.weapon == "Icicle Impale" || SpaceMarine.mod.weapon == "Forest Spirit")
        {
            towerModel.GetAttackModel().weapons[0].GetBehavior<CritMultiplierModel>().damage = towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage * modifier.bonus;
        }

        if (SpaceMarine.mod.weapon == "Hydra Rockets")
        {
            foreach (var behavior in towerModel.GetAttackModel().GetDescendant<ChangeProjectilePerEmitModel>().changedProjectileModel.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("HydraRocket"))
                {
                    behavior.projectile.GetDamageModel().damage = towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage * modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Railgun")
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 1 + (0.1f * modifier.bonus);

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot" || SpaceMarine.mod.modifier3 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }
        }

        if (SpaceMarine.mod.weapon == "Fireworks")
        {
            towerModel.GetAttackModel().weapons[0].GetDescendant<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage =
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage * modifier.bonus;

            towerModel.GetAttackModel().weapons[1].GetDescendant<ChangeProjectilePerEmitModel>().changedProjectileModel.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage =
                towerModel.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage * modifier.bonus;
        }

        tower.UpdateRootModel(towerModel);
    }
}