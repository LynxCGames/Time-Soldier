using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using System;

namespace SpaceMarine;

public class ScorcherSelect : SpecialSelect
{
    public override string SpecialName => "Scorcher";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Fire" || SpaceMarine.mod.weapon == "Fireworks" || SpaceMarine.mod.weapon == "Thorns of Wrath")
            {
                towerModel.GetAttackModel().GetDescendants<DamageOverTimeModel>().ForEach(model => model.interval = modifier.bonus);
            }

            if (SpaceMarine.mod.weapon == "Elite Laser")
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().
                    projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 20 - (modifier.bonus * 20);

                if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
                {
                    PiercingShotMod.PiercingShot(towerModel);
                }
            }

            if (SpaceMarine.mod.weapon == "Eruption")
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().
                    projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 20 - (modifier.bonus * 20);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class ScorcherLevel : SpecialLevel
{
    public override string SpecialName => "Scorcher";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus = MathF.Round(modifier.bonus * 10 - 1) / 10;
        }
    }
}

public class ScorcherEquiped : SpecialEquiped
{
    public override string SpecialName => "Scorcher";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Fire" || SpaceMarine.mod.weapon == "Fireworks" || SpaceMarine.mod.weapon == "Thorns of Wrath")
        {
            towerModel.GetAttackModel().GetDescendants<DamageOverTimeModel>().ForEach(model => model.interval = modifier.bonus);
        }

        if (SpaceMarine.mod.weapon == "Elite Laser")
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().
                projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 20 - (modifier.bonus * 20);

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }
        }

        if (SpaceMarine.mod.weapon == "Eruption")
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().
                projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 20 - (modifier.bonus * 20);
        }

        tower.UpdateRootModel(towerModel);
    }
}