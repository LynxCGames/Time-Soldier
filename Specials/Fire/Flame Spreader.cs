using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;

namespace SpaceMarine;

public class FlameSpreaderSelect : SpecialSelect
{
    public override string SpecialName => "Flame Spreader";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Fire")
            {
                towerModel.GetAttackModel().weapons[0].emission = new RandomEmissionModel("FlameSpreaderMod", (int)modifier.bonus, 10 * modifier.bonus - 5, 0, null, true, 0.9f, 1.1f, 0, true);
            }

            if (SpaceMarine.mod.weapon == "Elite Laser")
            {
                towerModel.GetAttackModel().weapons[0].GetDescendant<ArcEmissionModel>().count = (int)modifier.bonus + 1;
                towerModel.GetAttackModel().weapons[0].GetDescendant<ArcEmissionModel>().angle = modifier.bonus * 10 + 10;

                if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
                {
                    PiercingShotMod.PiercingShot(towerModel);
                }
            }

            if (SpaceMarine.mod.weapon == "Eruption")
            {
                towerModel.GetAttackModel().weapons[0].GetDescendant<ArcEmissionModel>().count = (int)modifier.bonus + 2;
            }

            if (SpaceMarine.mod.weapon == "Fireworks")
            {
                towerModel.GetAttackModel().weapons[1].projectile.GetDescendant<ArcEmissionModel>().count = 8 + ((int)modifier.bonus - 1) * 2;
            }

            if (SpaceMarine.mod.weapon == "Thorns of Wrath")
            {
                towerModel.GetAttackModel().weapons[0].GetDescendant<RandomEmissionModel>().count = (int)modifier.bonus + 6;
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class FlameSpreaderLevel : SpecialLevel
{
    public override string SpecialName => "Flame Spreader";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 1;
        }
    }
}

public class FlameSpreaderEquiped : SpecialEquiped
{
    public override string SpecialName => "Flame Spreader";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Fire")
        {
            towerModel.GetAttackModel().weapons[0].GetDescendant<RandomEmissionModel>().count = (int)modifier.bonus;
            towerModel.GetAttackModel().weapons[0].GetDescendant<RandomEmissionModel>().angle = 10 * modifier.bonus - 5;
        }

        if (SpaceMarine.mod.weapon == "Elite Laser")
        {
            towerModel.GetAttackModel().weapons[0].GetDescendant<ArcEmissionModel>().count = (int)modifier.bonus + 1;
            towerModel.GetAttackModel().weapons[0].GetDescendant<ArcEmissionModel>().angle = modifier.bonus * 10 + 10;

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }
        }

        if (SpaceMarine.mod.weapon == "Eruption")
        {
            towerModel.GetAttackModel().weapons[0].GetDescendant<ArcEmissionModel>().count = (int)modifier.bonus + 2;
        }

        if (SpaceMarine.mod.weapon == "Fireworks")
        {
            towerModel.GetAttackModel().weapons[1].projectile.GetDescendant<ArcEmissionModel>().count = 8 + ((int)modifier.bonus - 1) * 2;
        }

        if (SpaceMarine.mod.weapon == "Thorns of Wrath")
        {
            towerModel.GetAttackModel().weapons[0].GetDescendant<RandomEmissionModel>().count = (int)modifier.bonus + 6;
        }

        tower.UpdateRootModel(towerModel);
    }
}