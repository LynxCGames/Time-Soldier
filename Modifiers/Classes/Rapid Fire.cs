using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace SpaceMarine;

public class RapidSelect : ModifierSelect
{
    public override string ModName => "Rapid Fire";
    public override void EditTower(ModifierTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            towerModel.GetAttackModel().GetDescendants<WeaponModel>().ForEach(model => model.rate /= modifier.bonus / 100 + 1);

            if (SpaceMarine.mod.weapon == "Necromancer")
            {
                towerModel.GetAttackModel(1).GetDescendants<WeaponModel>().ForEach(model => model.rate /= modifier.bonus / 100 + 1);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class RapidLevel : ModifierLevel
{
    public override string ModName => "Rapid Fire";
    public override void Level(ModifierTemplate modifier, Tower tower)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 6.25f;
        }
    }
}

public class RapidEquiped : ModifierEquiped
{
    public override string ModName => "Rapid Fire";
    public override void EditTower(ModifierTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        foreach (var weapon in GetContent<WeaponTemplate>())
        {
            if (weapon.WeaponName == SpaceMarine.mod.weapon)
            {
                towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
            }
        }
        foreach (var weapon in GetContent<ComboTemplate>())
        {
            if (weapon.WeaponName == SpaceMarine.mod.weapon)
            {
                towerModel.GetAttackModel().weapons[0].rate = weapon.speed;

                if (SpaceMarine.mod.weapon == "Necromancer")
                {
                    towerModel.GetAttackModel(1).weapons[0].rate = weapon.speed;
                }
            }
        }
        towerModel.GetAttackModel().weapons[0].rate /= (modifier.bonus / 100 + 1);

        if (SpaceMarine.mod.weapon == "Necromancer")
        {
            towerModel.GetAttackModel(1).weapons[0].rate /= (modifier.bonus / 100 + 1);
        }

        tower.UpdateRootModel(towerModel);
    }
}