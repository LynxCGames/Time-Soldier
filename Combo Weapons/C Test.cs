using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using BTD_Mod_Helper.Api;
using UnityEngine;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;

namespace SpaceMarine;
/*
public class Test : ComboTemplate
{
    public override string WeaponName => "";
    public override string Icon => VanillaSprites.RocketStormUpgradeIcon;
    public override string[] comboWeapons => ["", ""];
    public override string Bonus => "";
    public override float FontSize => 60;
    public override float[] StartingValues => [1, 1f, 1];
    public override string Range => "-Range";
    public override string PierceType => "";
    public override int PierceValue => 1;
    public override string SpecialMods =>
        "\n\n" +
        " - -Range weapon\n" +
        " - \n" +
        " - \n" +
        " - \n" +
        " - ";
}
*/
public class CSelect : ComboSelect
{
    public override string WeaponName => "";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
        towerModel.RemoveBehavior(towerModel.GetAttackModel());

        // Creating Attack Model


        // Stat Setter


        // Basic Stat Adjusters


        tower.UpdateRootModel(towerModel);
    }
}

public class CLevel : ComboLevel
{
    public override string WeaponName => "";
    public override void Level(WeaponTemplate weapon1, ComboTemplate combo)
    {
        foreach (var weapon2 in ModContent.GetContent<WeaponTemplate>())
        {
            if (weapon1.WeaponName == combo.comboWeapons[0] && weapon2.WeaponName == combo.comboWeapons[1])
            {
                if (weapon1.isUnlocked == true && weapon2.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((weapon1.level + weapon2.level) / 2);

                    if (combo.level > 1)
                    {
                        var speed1 = (1 - weapon1.speed) / 2 + 1;
                        var speed2 = (2 - weapon2.speed) / 2 + 1;

                        combo.pierce = 8 + (int)Mathf.Round(weapon1.pierce / 3) + (int)Mathf.Round(weapon2.pierce / 2);
                        combo.speed = Mathf.Round((1.5f / speed1 / speed2) * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round(weapon1.damage / 2) + (int)Mathf.Round(weapon2.damage / 4);
                    }
                }
            }
            if (weapon1.WeaponName == combo.comboWeapons[1] && weapon2.WeaponName == combo.comboWeapons[0])
            {
                if (weapon1.isUnlocked == true && weapon2.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((weapon1.level + weapon2.level) / 2);

                    if (combo.level > 1)
                    {
                        var speed1 = (1 - weapon2.speed) / 2 + 1;
                        var speed2 = (2 - weapon1.speed) / 2 + 1;

                        combo.pierce = 8 + (int)Mathf.Round(weapon2.pierce / 3) + (int)Mathf.Round(weapon1.pierce / 2);
                        combo.speed = Mathf.Round((1.5f / speed2 / speed1) * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round(weapon2.damage / 2) + (int)Mathf.Round(weapon1.damage / 4);
                    }
                }
            }
        }
    }
}

public class CEquip : ComboEquiped
{
    public override string WeaponName => "";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();



        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            towerModel.GetAttackModel().weapons[0].rate /= 1.06f;
        }

        foreach (var modifier in GetContent<ModifierTemplate>())
        {
            if (modifier.ModName == "Rapid Fire")
            {
                if (SpaceMarine.mod.modifier1 == "Rapid Fire" || SpaceMarine.mod.modifier2 == "Rapid Fire" || SpaceMarine.mod.modifier3 == "Rapid Fire")
                {
                    towerModel.GetAttackModel().weapons[0].rate /= (modifier.bonus / 100 + 1);
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}