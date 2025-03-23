using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppSystem.Linq;

namespace SpaceMarine;

public class MoabSelect : ModifierSelect
{
    public override string ModName => "MOAB Damage";
    public override void EditTower(ModifierTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            var damageModifier = new DamageModifierForTagModel("MoabModifier", "Moabs", modifier.bonus, 0, false, false);

            if (SpaceMarine.mod.weapon == "Graviton")
            {
                towerModel.GetDescendant<CreateTowerModel>().tower.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.hasDamageModifiers = true);
                towerModel.GetDescendant<CreateTowerModel>().tower.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(damageModifier));
            }
            else if (SpaceMarine.mod.weapon == "Necromancer")
            {
                towerModel.GetAttackModel(0).GetDescendants<ProjectileModel>().ForEach(model => model.hasDamageModifiers = true);
                towerModel.GetAttackModel(0).GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(damageModifier));

                towerModel.GetAttackModel(1).GetDescendants<ProjectileModel>().ForEach(model => model.hasDamageModifiers = true);
                towerModel.GetAttackModel(1).GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(damageModifier));
            }
            else
            {
                towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.hasDamageModifiers = true);
                towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(damageModifier));
            }

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class MoabLevel : ModifierLevel
{
    public override string ModName => "MOAB Damage";
    public override void Level(ModifierTemplate modifier, Tower tower)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 0.5f;
        }
    }
}

public class MoabEquiped : ModifierEquiped
{
    public override string ModName => "MOAB Damage";
    public override void EditTower(ModifierTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Graviton")
        {
            foreach (var behavior in towerModel.GetDescendant<CreateTowerModel>().tower.GetAttackModel().GetDescendants<DamageModifierForTagModel>().ToArray())
            {
                if (behavior.name.Contains("MoabModifier"))
                {
                    behavior.damageMultiplier = modifier.bonus;
                }
            }
        }
        else if (SpaceMarine.mod.weapon == "Necromancer")
        {
            foreach (var behavior in towerModel.GetAttackModel(0).GetDescendants<DamageModifierForTagModel>().ToArray())
            {
                if (behavior.name.Contains("MoabModifier"))
                {
                    behavior.damageMultiplier = modifier.bonus;
                }
            }

            foreach (var behavior in towerModel.GetAttackModel(1).GetDescendants<DamageModifierForTagModel>().ToArray())
            {
                if (behavior.name.Contains("MoabModifier"))
                {
                    behavior.damageMultiplier = modifier.bonus;
                }
            }
        }
        else
        {
            foreach (var behavior in towerModel.GetAttackModel().GetDescendants<DamageModifierForTagModel>().ToArray())
            {
                if (behavior.name.Contains("MoabModifier"))
                {
                    behavior.damageMultiplier = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
        {
            PiercingShotMod.PiercingShot(towerModel);
        }

        tower.UpdateRootModel(towerModel);
    }
}