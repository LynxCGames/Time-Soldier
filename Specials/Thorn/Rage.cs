using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;

namespace SpaceMarine;

public class RageSelect : SpecialSelect
{
    public override string SpecialName => "Rage";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Thorn" || SpaceMarine.mod.weapon == "Blizzard" || SpaceMarine.mod.weapon == "Forest Spirit")
            {
                towerModel.AddBehavior(new DamageBasedAttackSpeedModel("RageTSMod", 10, 120, modifier.bonus / 1000, 10));
            }

            if (SpaceMarine.mod.weapon == "Thorns of Wrath")
            {
                towerModel.AddBehavior(new DamageBasedAttackSpeedModel("RageTSMod", 10, 120, modifier.bonus / 1000, 10));
                towerModel.AddBehavior(new RateSupportModel("RageTSMod", 1 - (modifier.bonus / 200), true, "Support:Rate", false, 1, null, "", ""));
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class RageLevel : SpecialLevel
{
    public override string SpecialName => "Rage";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            if (modifier.level < 4)
            {
                modifier.bonus += 15;
            }
            else if (modifier.level >= 4)
            {
                modifier.bonus += 20;
            }
        }
    }
}

public class RageEquiped : SpecialEquiped
{
    public override string SpecialName => "Rage";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Thorn" || SpaceMarine.mod.weapon == "Blizzard" || SpaceMarine.mod.weapon == "Forest Spirit")
        {
            towerModel.GetBehavior<DamageBasedAttackSpeedModel>().increasePerThreshold = modifier.bonus / 1000;
        }

        if (SpaceMarine.mod.weapon == "Thorns of Wrath")
        {
            towerModel.GetBehavior<DamageBasedAttackSpeedModel>().increasePerThreshold = modifier.bonus / 1000;
            towerModel.GetBehavior<RateSupportModel>().multiplier = 1 - (modifier.bonus / 200);
        }

        tower.UpdateRootModel(towerModel);
    }
}