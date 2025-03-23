using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppSystem.Linq;

namespace SpaceMarine;

public class SlowdownSelect : ModifierSelect
{
    public override string ModName => "Slowdown";
    public override void EditTower(ModifierTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            var slowModel = new SlowModel("SlowdownMod", 1 - (modifier.bonus / 100), 3, "Slow:Weak", 999, null, true, false, null, false, false, false, 1);
            var slowModifier = new SlowModifierForTagModel("SlowdownMod", "Moabs", "Slow:Weak", 1 - (modifier.bonus / 200), false, false, 3, false);

            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.collisionPasses = new[] { -1, 0, 1 });
            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(slowModel));
            towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(slowModifier));

            if (SpaceMarine.mod.weapon == "Necromancer")
            {
                towerModel.GetAttackModel(1).GetDescendants<ProjectileModel>().ForEach(model => model.collisionPasses = new[] { -1, 0, 1 });
                towerModel.GetAttackModel(1).GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(slowModel));
                towerModel.GetAttackModel(1).GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(slowModifier));
            }

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class SlowdownLevel : ModifierLevel
{
    public override string ModName => "Slowdown";
    public override void Level(ModifierTemplate modifier, Tower tower)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 10;
        }
    }
}

public class SlowdownEquiped : ModifierEquiped
{
    public override string ModName => "Slowdown";
    public override void EditTower(ModifierTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        foreach (var behavior in towerModel.GetAttackModel().GetDescendants<SlowModel>().ToArray())
        {
            if (behavior.name.Contains("SlowdownMod"))
            {
                behavior.multiplier = 1 - (modifier.bonus / 100);
            }
        }
        foreach (var behavior in towerModel.GetAttackModel().GetDescendants<SlowModifierForTagModel>().ToArray())
        {
            if (behavior.name.Contains("SlowdownMod"))
            {
                behavior.slowMultiplier = 1 - (modifier.bonus / 200);
            }
        }

        if (SpaceMarine.mod.weapon == "Necromancer")
        {
            foreach (var behavior in towerModel.GetAttackModel(1).GetDescendants<SlowModel>().ToArray())
            {
                if (behavior.name.Contains("SlowdownMod"))
                {
                    behavior.multiplier = 1 - (modifier.bonus / 100);
                }
            }
            foreach (var behavior in towerModel.GetAttackModel(1).GetDescendants<SlowModifierForTagModel>().ToArray())
            {
                if (behavior.name.Contains("SlowdownMod"))
                {
                    behavior.slowMultiplier = 1 - (modifier.bonus / 200);
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