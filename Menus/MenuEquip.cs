using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using UnityEngine;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using System.Linq;

namespace SpaceMarine;

public class MenuEquip : BloonsTD6Mod
{
    public static void EquipMenu(RectTransform rect, Tower tower, ModHelperPanel mainPanel)
    {
        // Weapon Equipment
        ModHelperText weaponEquipText = mainPanel.AddText(new Info("text", -900, 710, 750, 100), "Weapons", 70);
        ModHelperScrollPanel weaponScroll = mainPanel.AddScrollPanel(new Info("scrollPanel", -900, 300, 550, 700), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanel, 15, 50);
        foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
        {
            if (weapon.isUnlocked == true && mod.weapon == "")
            {
                weaponScroll.AddScrollContent(Equipment.WeaponEquip(weapon, tower));
            }
        }

        // Bonus Weapon Equipment
        ModHelperText bonusText = mainPanel.AddText(new Info("text", -300, 710, 750, 100), "Bonus Weapons", 60);
        ModHelperScrollPanel bonusScroll = mainPanel.AddScrollPanel(new Info("scrollPanel", -300, 300, 550, 700), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanel, 15, 50);
        foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
        {
            if (weapon.isUnlocked == true && mod.weapon != "" && weapon.WeaponName != mod.weapon && mod.modifierCount < 1 && mod.comboedWeapon == false)
            {
                foreach (var weaponTest in ModContent.GetContent<WeaponTemplate>())
                {
                    if (weaponTest.WeaponName == mod.weapon)
                    {
                        for (int i = 0; i < weaponTest.Combos.Count(); i++)
                        {
                            if (weaponTest.Combos[i] == weapon.WeaponName)
                            {
                                bonusScroll.AddScrollContent(Equipment.BonusEquip(weapon, tower));
                            }
                        }
                    }
                }
            }
        }

        // Modifier Equipment
        ModHelperText modifierText = mainPanel.AddText(new Info("text", 300, 710, 750, 100), "General Modifiers", 60);
        ModHelperScrollPanel modifierScroll = mainPanel.AddScrollPanel(new Info("scrollPanel", 300, 300, 550, 700), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanel, 15, 50);
        foreach (var modifier in ModContent.GetContent<ModifierTemplate>())
        {
            if (modifier.isUnlocked == true && mod.weapon != "" && mod.modifierCount < 3)
            {
                if (modifier.ModName != mod.modifier1 && modifier.ModName != mod.modifier2 && modifier.ModName != mod.modifier3)
                {
                    modifierScroll.AddScrollContent(Equipment.ModifierEquip(modifier, tower));
                }
            }
        }

        // Special Equipment
        ModHelperText specialText = mainPanel.AddText(new Info("text", 900, 710, 750, 100), "Special Modifiers", 60);
        ModHelperScrollPanel specialScroll = mainPanel.AddScrollPanel(new Info("scrollPanel", 900, 300, 550, 700), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanel, 15, 50);
        foreach (var modifier in ModContent.GetContent<SpecialTemplate>())
        {
            if (modifier.isUnlocked == true && mod.weapon != "" && mod.modifierCount < 3)
            {
                for (int i = 0; i < modifier.Weapons.Count(); i++)
                {
                    if (modifier.Weapons[i] == mod.weapon)
                    {
                        if (modifier.ModName != mod.modifier1 && modifier.ModName != mod.modifier2 && modifier.ModName != mod.modifier3)
                        {
                            specialScroll.AddScrollContent(Equipment.SpecialEquip(modifier, tower));
                        }
                    }
                }
            }
        }

        // Weapon Panel
        ModHelperText weaponText = mainPanel.AddText(new Info("text", -945, -190, 500, 120), "Weapon", 100);
        ModHelperPanel weaponPanel = mainPanel.AddPanel(new Info("Panel", -945, -510, 500), VanillaSprites.BrownInsertPanel);
        foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
        {
            if (mod.weapon == weapon.WeaponName)
            {
                ModHelperImage image = weaponPanel.AddImage(new Info("image", 480), weapon.Icon);
            }
        }

        foreach (var weapon in ModContent.GetContent<ComboTemplate>())
        {
            if (mod.weapon == weapon.WeaponName)
            {
                ModHelperImage image = weaponPanel.AddImage(new Info("image", 480), weapon.Icon);
            }
        }

        // Reset Button
        ModHelperButton resetBtn = mainPanel.AddButton(new Info("button", -520, -380, 200), VanillaSprites.RedBtnSquareSmall, new System.Action(() => {
            if (mod.weapon != "")
            {
                mod.weapon = "";
                mod.comboedWeapon = false;
                mod.modifierCount = 0;
                mod.modifier1 = "";
                mod.modifier2 = "";
                mod.modifier3 = "";

                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
                towerModel.range = 10;

                foreach (var attack in towerModel.GetAttackModels())
                {
                    towerModel.RemoveBehavior(attack);
                }
                foreach (var behavior in towerModel.behaviors)
                {
                    if (behavior.name.Contains("SnowstormMod"))
                    {
                        towerModel.RemoveBehavior(behavior);
                    }
                }

                tower.UpdateRootModel(towerModel);

                MenuUi.instance.CloseMenu();
                MenuUi.CreateMenu(rect, tower);
            }
        }));
        ModHelperImage resetImg = resetBtn.AddImage(new Info("image", 200), VanillaSprites.RestartBtn);
        ModHelperText comboText = mainPanel.AddText(new Info("text", -520, -610, 250), "", 50);

        if (mod.comboedWeapon == true)
        {
            comboText.Text.text = "Combo Weapon";
        }

        // Modifier Panels
        ModHelperText addonText = mainPanel.AddText(new Info("text", 425, -190, 500, 120), "Modifiers", 100);
        ModHelperPanel addonPanel1 = mainPanel.AddPanel(new Info("Panel", -95, -510, 500), VanillaSprites.BrownInsertPanel);
        ModHelperPanel addonPanel2 = mainPanel.AddPanel(new Info("Panel", 425, -510, 500), VanillaSprites.BrownInsertPanel);
        ModHelperPanel addonPanel3 = mainPanel.AddPanel(new Info("Panel", 945, -510, 500), VanillaSprites.BrownInsertPanel);

        foreach (var modifier in ModContent.GetContent<ModifierTemplate>())
        {
            if (mod.modifier1 == modifier.ModName)
            {
                ModHelperImage image = addonPanel1.AddImage(new Info("image", 480), modifier.Icon);
            }
            if (mod.modifier2 == modifier.ModName)
            {
                ModHelperImage image = addonPanel2.AddImage(new Info("image", 480), modifier.Icon);
            }
            if (mod.modifier3 == modifier.ModName)
            {
                ModHelperImage image = addonPanel3.AddImage(new Info("image", 480), modifier.Icon);
            }
        }

        foreach (var modifier in ModContent.GetContent<SpecialTemplate>())
        {
            if (mod.modifier1 == modifier.ModName)
            {
                ModHelperImage image = addonPanel1.AddImage(new Info("image", 480), modifier.Icon);
            }
            if (mod.modifier2 == modifier.ModName)
            {
                ModHelperImage image = addonPanel2.AddImage(new Info("image", 480), modifier.Icon);
            }
            if (mod.modifier3 == modifier.ModName)
            {
                ModHelperImage image = addonPanel3.AddImage(new Info("image", 480), modifier.Icon);
            }
        }
    }
}