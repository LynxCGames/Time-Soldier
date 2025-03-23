using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;

namespace SpaceMarine;

public class BurstSelect : SpecialSelect
{
    public override string SpecialName => "Burst Fire";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Crossbow" || SpaceMarine.mod.weapon == "Fireworks")
            {
                towerModel.GetAttackModel().weapons[0].emission = new ParallelEmissionModel("", (int)modifier.bonus, 25, 0, true, null);
            }

            if (SpaceMarine.mod.weapon == "Hydra Rockets")
            {
                towerModel.GetAttackModel().weapons[0].projectile.maxPierce = 2 + (int)modifier.bonus;
                towerModel.GetAttackModel().weapons[0].projectile.pierce = 2 + (int)modifier.bonus;
            }

            if (SpaceMarine.mod.weapon == "Icicle Impale")
            {
                towerModel.GetAttackModel().weapons[0].projectile.maxPierce = 1 + (int)modifier.bonus;
                towerModel.GetAttackModel().weapons[0].projectile.pierce = 1 + (int)modifier.bonus;
            }

            if (SpaceMarine.mod.weapon == "Railgun")
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<ClearHitBloonsModel>().interval = 1 - (modifier.bonus / 10);

                if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
                {
                    PiercingShotMod.PiercingShot(towerModel);
                }
            }

            if (SpaceMarine.mod.weapon == "Forest Spirit")
            {
                towerModel.GetBehavior<SpiritOfTheForestModel>().damageOverTimeZoneModelFar.GetDescendant<DamageOverTimeCustomModel>().damage += modifier.level;
                towerModel.GetBehavior<SpiritOfTheForestModel>().damageOverTimeZoneModelFar.GetDescendant<DamageOverTimeCustomModel>().interval = 0.25f;
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class BurstLevel : SpecialLevel
{
    public override string SpecialName => "Burst Fire";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 1;
        }
    }
}

public class BurstEquiped : SpecialEquiped
{
    public override string SpecialName => "Burst Fire";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Crossbow" || SpaceMarine.mod.weapon == "Fireworks")
        {
            towerModel.GetAttackModel().weapons[0].GetDescendant<ParallelEmissionModel>().count = (int)modifier.bonus;
        }

        if (SpaceMarine.mod.weapon == "Hydra Rockets")
        {
            towerModel.GetAttackModel().weapons[0].projectile.maxPierce = 2 + (int)modifier.bonus;
            towerModel.GetAttackModel().weapons[0].projectile.pierce = 2 + (int)modifier.bonus;
        }

        if (SpaceMarine.mod.weapon == "Icicle Impale")
        {
            towerModel.GetAttackModel().weapons[0].projectile.maxPierce = 1 + (int)modifier.bonus;
            towerModel.GetAttackModel().weapons[0].projectile.pierce = 1 + (int)modifier.bonus;
        }

        if (SpaceMarine.mod.weapon == "Railgun")
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<ClearHitBloonsModel>().interval = 1 - (modifier.bonus / 10);

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot" || SpaceMarine.mod.modifier3 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }
        }

        if (SpaceMarine.mod.weapon == "Forest Spirit")
        {
            towerModel.GetBehavior<SpiritOfTheForestModel>().damageOverTimeZoneModelFar.GetDescendant<DamageOverTimeCustomModel>().damage += 1;
        }

        tower.UpdateRootModel(towerModel);
    }
}