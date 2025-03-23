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

public class RefractionSelect : SpecialSelect
{
    public override string SpecialName => "Refraction";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Laser" || SpaceMarine.mod.weapon == "Precision Laser" || SpaceMarine.mod.weapon == "Plasma Launcher")
            {
                var laser = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                laser.GetDamageModel().damage = modifier.bonus;
                laser.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                laser.display = Game.instance.model.GetTowerFromId("DartlingGunner-300").GetAttackModel().weapons[0].projectile.display;
                laser.scale /= 2;
                laser.pierce = 2;
                laser.maxPierce = 2;

                if (SpaceMarine.mod.camoActive == true)
                {
                    laser.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    laser.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("RefractionMod", laser, new ArcEmissionModel("", 3, 0, 25, null, true, false), false, false, false));
            }

            if (SpaceMarine.mod.weapon == "Railgun")
            {
                var laser = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                laser.GetDamageModel().damage = modifier.bonus;
                laser.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                laser.display = Game.instance.model.GetTowerFromId("DartlingGunner-300").GetAttackModel().weapons[0].projectile.display;
                laser.scale /= 2;
                laser.pierce = 2;
                laser.maxPierce = 2;

                if (SpaceMarine.mod.camoActive == true)
                {
                    laser.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    laser.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("RefractionMod", laser, new ArcEmissionModel("", 2, 0, 15, null, false, false), false, false, false));
            }

            if (SpaceMarine.mod.weapon == "Elite Laser")
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().emission = new ArcEmissionModel("", modifier.level + 1, 0, modifier.level * 10, null, false, false);
            }

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class RefractionLevel : SpecialLevel
{
    public override string SpecialName => "Refraction";
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

public class RefractionEquip : SpecialEquiped
{
    public override string SpecialName => "Refraction";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Laser" || SpaceMarine.mod.weapon == "Railgun" || SpaceMarine.mod.weapon == "Precision Laser" || SpaceMarine.mod.weapon == "Plasma Launcher")
        {
            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateProjectileOnContactModel>().ToArray())
            {
                if (behavior.name.Contains("RefractionMod"))
                {
                    behavior.projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Elite Laser")
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().GetDescendant<ArcEmissionModel>().count = modifier.level + 1;
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().GetDescendant<ArcEmissionModel>().angle = modifier.level * 10;
        }

        if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot" || SpaceMarine.mod.modifier3 == "Piercing Shot")
        {
            PiercingShotMod.PiercingShot(towerModel);
        }

        tower.UpdateRootModel(towerModel);
    }
}