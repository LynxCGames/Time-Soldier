using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;

namespace SpaceMarine;

public class SSelect : SpecialSelect
{
    public override string SpecialName => "";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "")
            {

            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class SLevel : SpecialLevel
{
    public override string SpecialName => "";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {

        }
    }
}

public class Equiped : SpecialEquiped
{
    public override string SpecialName => "";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "")
        {

        }

        tower.UpdateRootModel(towerModel);
    }
}