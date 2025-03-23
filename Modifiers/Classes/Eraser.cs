using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using System;
using Il2CppSystem.Linq;

namespace SpaceMarine;

public class EraserSelect : ModifierSelect
{
    public override string ModName => "Eraser";
    public override void EditTower(ModifierTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            var cleanse = new RemoveBloonModifiersModel("Eraser", true, false, false, false, false, Array.Empty<string>(), Array.Empty<string>());

            if (modifier.level >= 2)
            {
                cleanse.cleanseCamo = true;
            }
            if (modifier.level >= 3)
            {
                cleanse.cleanseFortified = true;
            }

            if (SpaceMarine.mod.weapon == "Graviton")
            {
                towerModel.GetDescendant<CreateTowerModel>().tower.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.collisionPasses = new int[] { 0, -1 });
                towerModel.GetDescendant<CreateTowerModel>().tower.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(cleanse));
            }
            else if (SpaceMarine.mod.weapon == "Necromancer")
            {
                towerModel.GetAttackModel(0).GetDescendants<ProjectileModel>().ForEach(model => model.collisionPasses = new int[] { 0, -1 });
                towerModel.GetAttackModel(0).GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(cleanse));

                towerModel.GetAttackModel(1).GetDescendants<ProjectileModel>().ForEach(model => model.collisionPasses = new int[] { 0, -1 });
                towerModel.GetAttackModel(1).GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(cleanse));
            }
            else
            {
                towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.collisionPasses = new int[] { 0, -1 });
                towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ForEach(model => model.AddBehavior(cleanse));
            }

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class EraserLevel : ModifierLevel
{
    public override string ModName => "Eraser";
    public override void Level(ModifierTemplate modifier, Tower tower)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 1;
        }
    }
}

public class EraserEquiped : ModifierEquiped
{
    public override string ModName => "Eraser";
    public override void EditTower(ModifierTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Graviton")
        {
            foreach (var behavior in towerModel.GetDescendant<CreateTowerModel>().tower.GetAttackModel().GetDescendants<RemoveBloonModifiersModel>().ToArray())
            {
                if (modifier.level >= 2)
                {
                    behavior.cleanseCamo = true;
                }
                if (modifier.level >= 3)
                {
                    behavior.cleanseFortified = true;
                }
            }
        }
        else if (SpaceMarine.mod.weapon == "Necromancer")
        {
            foreach (var behavior in towerModel.GetAttackModel(0).GetDescendants<RemoveBloonModifiersModel>().ToArray())
            {
                if (modifier.level >= 2)
                {
                    behavior.cleanseCamo = true;
                }
                if (modifier.level >= 3)
                {
                    behavior.cleanseFortified = true;
                }
            }

            foreach (var behavior in towerModel.GetAttackModel(1).GetDescendants<RemoveBloonModifiersModel>().ToArray())
            {
                if (modifier.level >= 2)
                {
                    behavior.cleanseCamo = true;
                }
                if (modifier.level >= 3)
                {
                    behavior.cleanseFortified = true;
                }
            }
        }
        else
        {
            foreach (var behavior in towerModel.GetAttackModel().GetDescendants<RemoveBloonModifiersModel>().ToArray())
            {
                if (modifier.level >= 2)
                {
                    behavior.cleanseCamo = true;
                }
                if (modifier.level >= 3)
                {
                    behavior.cleanseFortified = true;
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