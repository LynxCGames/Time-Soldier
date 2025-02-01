using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using UnityEngine;

namespace SpaceMarine;

public class WeaponUpgrades : BloonsTD6Mod
{
    public static ModHelperPanel CreateWeaponShop(WeaponTemplate weapon, Tower tower, ModHelperText text)
    {
        var panel = ModHelperPanel.Create(new Info("WeaponContent" + weapon.WeaponName, 0, 0, 650), VanillaSprites.MainBgPanelJukebox);
        ModHelperText name = panel.AddText(new Info("name", -105, 265, 400, 80), weapon.WeaponName, weapon.NameSize);
        ModHelperText level = panel.AddText(new Info("level", 205, 265, 200, 80), "Lvl " + weapon.level, 60);
        ModHelperImage image = panel.AddImage(new Info("image", -105, 55, 300), weapon.Icon);
        ModHelperText stats = panel.AddText(new Info("stats", 205, 55, 200, 300), $"Pierce:\n{weapon.pierce}\nRate:\n{weapon.speed}\nDamage:\n{weapon.damage}", 40);
        ModHelperText bonus = panel.AddText(new Info("stats", 0, -140, 600, 50), $"Bonus: {weapon.Bonus}", weapon.FontSize);
        ModHelperPanel costPanel = panel.AddPanel(new Info("Panel", -135, -230, 300, 120), VanillaSprites.GreyInsertPanel);
        ModHelperText weaponCost = costPanel.AddText(new Info("text", 55, 0, 380, 100), "", 70);
        ModHelperImage scrapIcon = costPanel.AddImage(new Info("scrapIcon", -93, 0, 90), ModContent.GetSprite(mod, "Scrap-Icon"));

        if (weapon.level < 10)
        {
            weaponCost.Text.text = $"{weapon.cost}";
        }
        else
        {
            weaponCost.Text.fontSize = 55;
            weaponCost.Text.text = "Maxed";
        }

        ModHelperButton upgradeBtn = panel.AddButton(new Info("button", 225, -230, 120), VanillaSprites.BlueBtnSquare, new System.Action(() => {
            if (mod.scrap >= weapon.cost && weapon.level > 0 && weapon.level < 10)
            {
                WeaponMethods.WeaponLevels(weapon, tower);
                foreach (var combo in ModContent.GetContent<ComboTemplate>())
                {
                    ComboMethods.ComboLevels(weapon, combo);
                }

                mod.scrap -= weapon.cost;
                mod.usedScrap += weapon.cost;
                weapon.cost = Mathf.Round(weapon.cost * 1.5f);
                level.Text.text = "Lvl " + weapon.level;
                stats.Text.text = $"Pierce:\n{weapon.pierce}\nRate:\n{weapon.speed}\nDamage:\n{weapon.damage}";
                text.Text.text = $"{mod.scrap}";

                if (weapon.level >= 10)
                {
                    weaponCost.Text.fontSize = 55;
                    weaponCost.Text.text = "Maxed";
                }
                else
                {
                    weaponCost.Text.text = $"{weapon.cost}";
                }

                if (mod.weapon == weapon.WeaponName)
                {
                    WeaponMethods.LevelEquipedWeapon(weapon, tower);
                }
                foreach (var combo in ModContent.GetContent<ComboTemplate>())
                {
                    if (mod.weapon == combo.WeaponName)
                    {
                        ComboMethods.LevelEquipedCombo(combo, tower);
                    }
                }
            }
            if (mod.scrap >= weapon.cost && weapon.level == 0)
            {
                weapon.isUnlocked = true;
                weapon.level++;

                foreach (var combo in ModContent.GetContent<ComboTemplate>())
                {
                    ComboMethods.ComboLevels(weapon, combo);
                }

                mod.scrap -= weapon.cost;
                mod.usedScrap += weapon.cost;
                weapon.cost = 15;
                level.Text.text = "Lvl " + weapon.level;
                text.Text.text = $"{mod.scrap}";
                weaponCost.Text.text = $"{weapon.cost}";
            }
        }));
        ModHelperImage upgradeImage = upgradeBtn.AddImage(new Info("image", 0, 0, 100), VanillaSprites.UpgradeIcon2);

        return panel;
    }
}