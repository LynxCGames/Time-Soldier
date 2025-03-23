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
using static MelonLoader.MelonLogger;

namespace SpaceMarine;

public class Almanac : BloonsTD6Mod
{
    public static void AlmanacMenu(RectTransform rect, Tower tower)
    {
        ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1300, 2500, 1850, new Vector2()), VanillaSprites.BrownInsertPanel);

        ModHelperPanel mainPanel = panel.AddPanel(new Info("Panel", 400, 0, 1650, 1800), VanillaSprites.BrownInsertPanelDark);
        ModHelperPanel namePanel = mainPanel.AddPanel(new Info("Panel", 0, 775, 1600, 200), VanillaSprites.BrownInsertPanel);
        ModHelperPanel comboPanel = mainPanel.AddPanel(new Info("Panel", -175, 550, 1250, 200), VanillaSprites.BrownInsertPanel);
        ModHelperPanel levelPanel = mainPanel.AddPanel(new Info("Panel", 650, 550, 300, 200), VanillaSprites.BrownInsertPanel);
        ModHelperPanel bonusPanel = mainPanel.AddPanel(new Info("Panel", 175, 275, 1250, 200), VanillaSprites.BrownInsertPanel);
        ModHelperPanel statPanel = mainPanel.AddPanel(new Info("Panel", -650, 275, 300), VanillaSprites.BrownInsertPanel);
        ModHelperPanel specialPanel = mainPanel.AddPanel(new Info("Panel", 0, -388, 1600, 975), VanillaSprites.BrownInsertPanel);

        ModHelperText nameText = namePanel.AddText(new Info("Text", 0, 0, 1200, 200), "", 120);
        ModHelperText comboText = comboPanel.AddText(new Info("Text", 0, 0, 1200, 200), "", 70);
        ModHelperText levelText = levelPanel.AddText(new Info("Text", 0, 0, 250, 200), "", 80);
        ModHelperText bonusText = bonusPanel.AddText(new Info("Text", 0, 0, 1200, 200), "", 50);
        ModHelperText statText = statPanel.AddText(new Info("Text", 0, 0, 300), "", 45);
        ModHelperText specialText = specialPanel.AddText(new Info("Text", 0, 0, 1500, 950), "", 70, Il2CppTMPro.TextAlignmentOptions.TopLeft);

        ModHelperScrollPanel comboScroll = panel.AddScrollPanel(new Info("scrollPanel", -850, 0, 750, 1800), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanelDark, 15, 50);
        foreach (var weapon in ModContent.GetContent<ComboTemplate>())
        {
            if (weapon.discovered == true)
            {
                comboScroll.AddScrollContent(CreateAlmanac(weapon, nameText, comboText, levelText, bonusText, statText, specialText));
            }
        }

        ModHelperButton closeBtn = panel.AddButton(new Info("closeBtn", 0, -1000, 600, 180), VanillaSprites.RedBtnLong, new System.Action(() => {
            panel.DeleteObject();
            mod.almanacOpen = false;

            if (mod.isSelected == true)
            {
                MenuUi.CreateMenu(rect, tower);
            }
        }));
        ModHelperText closeText = closeBtn.AddText(new Info("Text", 0, 0, 600, 180), "Close", 80);
    }

    public static ModHelperButton CreateAlmanac(ComboTemplate weapon, ModHelperText comboName, ModHelperText comboWeapons, ModHelperText comboLevel, ModHelperText comboBonus, ModHelperText comboStats, ModHelperText comboSpecial)
    {
        var panel = ModHelperButton.Create(new Info("WeaponContent" + weapon.WeaponName, 0, 0, 650, 150), VanillaSprites.MainBgPanelJukebox, new System.Action(() => {
            comboName.Text.text = weapon.WeaponName;
            comboWeapons.Text.text = $"Combine {weapon.comboWeapons[0]} and {weapon.comboWeapons[1]}";
            comboLevel.Text.text = $"Level {weapon.level}";
            comboBonus.Text.text = weapon.Bonus;
            comboStats.Text.text = $"Pierce:\n{weapon.pierce}\nRate:\n{weapon.speed}\nDamage\n{weapon.damage}";
            comboSpecial.Text.text = weapon.SpecialMods;
            comboSpecial.Text.fontSize = weapon.FontSize;
        }));
        ModHelperText name = panel.AddText(new Info("name", -70, 0, 470, 130), weapon.WeaponName, 50);
        ModHelperImage image = panel.AddImage(new Info("image", 240, 0, 130), weapon.Icon);

        return panel;
    }
}