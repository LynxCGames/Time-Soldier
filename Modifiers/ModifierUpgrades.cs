using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using UnityEngine;

namespace SpaceMarine;

public class ModifierUpgrades : BloonsTD6Mod
{
    public static ModHelperPanel CreateModShop(ModifierTemplate modifier, Tower tower, ModHelperText text)
    {
        var panel = ModHelperPanel.Create(new Info("ModContent" + modifier.ModName, 0, 0, 650), VanillaSprites.MainBgPanelJukebox);
        ModHelperText name = panel.AddText(new Info("name", -105, 265, 400, 80), modifier.ModName, modifier.NameSize);
        ModHelperText level = panel.AddText(new Info("level", 205, 265, 200, 80), "Lvl " + modifier.level, 60);
        ModHelperImage image = panel.AddImage(new Info("image", -105, 55, 300), modifier.Icon);
        ModHelperText stats = panel.AddText(new Info("stats", 205, 55, 200, 300), "", 35);
        if (modifier.Name == "Eraser")
        {
            stats.Text.text = $"{modifier.Stat}:\n";
            for (int i = 0; i < modifier.bonus; i++)
            {
                stats.Text.text += $"{modifier.EraserProperties[i]}\n";
            }
        }
        else
        {
            stats.Text.text = $"{modifier.Stat}\n{modifier.bonus}";
        }
        ModHelperText bonus = panel.AddText(new Info("stats", 0, -140, 600, 50), modifier.Effect, modifier.FontSize);
        ModHelperPanel costPanel = panel.AddPanel(new Info("Panel", -135, -230, 300, 120), VanillaSprites.GreyInsertPanel);
        ModHelperText modifierCost = costPanel.AddText(new Info("text", 55, 0, 380, 100), "", 70);
        ModHelperImage scrapIcon = costPanel.AddImage(new Info("scrapIcon", -93, 0, 90), ModContent.GetSprite(mod, "Scrap-Icon"));

        if (modifier.level < modifier.MaxLevel)
        {
            modifierCost.Text.text = $"{modifier.cost}";
        }
        else
        {
            modifierCost.Text.fontSize = 55;
            modifierCost.Text.text = "Maxed";
        }

        ModHelperButton upgradeBtn = panel.AddButton(new Info("button", 225, -230, 120), VanillaSprites.BlueBtnSquare, new System.Action(() => {
            if (mod.scrap >= modifier.cost && modifier.level > 0 && modifier.level < modifier.MaxLevel)
            {
                ModifierLeveling.ModifierLevels(modifier, tower);

                mod.scrap -= modifier.cost;
                mod.usedScrap += modifier.cost;
                modifier.cost = Mathf.Round(modifier.cost * modifier.CostMultiplier);
                level.Text.text = "Lvl " + modifier.level;
                if (modifier.Name == "Eraser")
                {
                    if (modifier.level >= 0)
                    {
                        stats.Text.text = $"{modifier.Stat}:\nRegrow";
                    }
                    if (modifier.level >= 2)
                    {
                        stats.Text.text += "\nCamo";
                    }
                    if (modifier.level >= 3)
                    {
                        stats.Text.text += "\nFortified";
                    }
                }
                else
                {
                    stats.Text.text = $"{modifier.Stat}:\n{modifier.bonus}";
                }
                text.Text.text = $"{mod.scrap}";
                bonus.Text.text = modifier.Effect;

                if (modifier.level >= modifier.MaxLevel)
                {
                    modifierCost.Text.fontSize = 55;
                    modifierCost.Text.text = "Maxed";
                }
                else
                {
                    modifierCost.Text.text = $"{modifier.cost}";
                }
                
                if (mod.modifier1 == modifier.ModName || mod.modifier2 == modifier.ModName || mod.modifier3 == modifier.ModName)
                {
                    EquipedModifierLevel.LevelEquipedModifier(modifier, tower);
                }
            }
            if (mod.scrap >= modifier.cost && modifier.level == 0)
            {
                modifier.isUnlocked = true;
                modifier.level++;

                mod.scrap -= modifier.cost;
                mod.usedScrap += modifier.cost;
                modifier.cost = modifier.StartingCost;
                level.Text.text = "Lvl " + modifier.level;
                text.Text.text = $"{mod.scrap}";
                modifierCost.Text.text = $"{modifier.cost}";
            }
        }));
        ModHelperImage upgradeImage = upgradeBtn.AddImage(new Info("image", 0, 0, 100), VanillaSprites.UpgradeIcon2);

        return panel;
    }
}