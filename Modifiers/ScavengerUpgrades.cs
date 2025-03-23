using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;

namespace SpaceMarine;

public class ScavengerUpgrades : BloonsTD6Mod
{
    public static ModHelperPanel CreateModShop(ScavengerTemplate modifier, Tower tower)
    {
        InGame game = InGame.instance;

        var panel = ModHelperPanel.Create(new Info("ModContentScavenger", 0, 0, 650), VanillaSprites.MainBgPanelJukebox);
        ModHelperText name = panel.AddText(new Info("name", -105, 265, 400, 80), "Scavenger", 60);
        ModHelperText level = panel.AddText(new Info("level", 205, 265, 200, 80), "Lvl " + modifier.level, 60);
        ModHelperImage image = panel.AddImage(new Info("image", -105, 55, 300), ModContent.GetSprite(mod, "Scrap-Icon"));
        ModHelperText stats = panel.AddText(new Info("stats", 205, 55, 200, 300), $"% Chance:\n{modifier.bonus}", 35);
        ModHelperText bonus = panel.AddText(new Info("stats", 0, -140, 600, 50), "Popping Bloons gives scrap", 40);
        ModHelperPanel costPanel = panel.AddPanel(new Info("Panel", -135, -230, 300, 120), VanillaSprites.GreyInsertPanel);
        ModHelperText modifierCost = costPanel.AddText(new Info("text", 55, 0, 380, 100), "", 55);
        ModHelperImage scrapIcon = costPanel.AddImage(new Info("scrapIcon", -93, 0, 90), VanillaSprites.CoinIcon);

        if (modifier.level < modifier.MaxLevel)
        {
            modifierCost.Text.text = $"{modifier.CostValues[modifier.level]}";
        }
        else
        {
            modifierCost.Text.fontSize = 55;
            modifierCost.Text.text = "Maxed";
        }

        ModHelperButton upgradeBtn = panel.AddButton(new Info("button", 225, -230, 120), VanillaSprites.BlueBtnSquare, new System.Action(() => {
            if (game.GetCash() >= modifier.Cost[modifier.level] && modifier.level > 0 && modifier.level < modifier.MaxLevel)
            {
                game.AddCash(-modifier.Cost[modifier.level]);
                tower.worth += modifier.Cost[modifier.level] * 0.75f;

                ModifierMethods.ScavengerLevels(modifier, tower);
                level.Text.text = "Lvl " + modifier.level;
                stats.Text.text = $"% Chance:\n{modifier.bonus}";

                if (modifier.level >= modifier.MaxLevel)
                {
                    modifierCost.Text.fontSize = 55;
                    modifierCost.Text.text = "Maxed";
                }
                else
                {
                    modifierCost.Text.text = $"{modifier.CostValues[modifier.level]}";
                }
            }
            if (game.GetCash() >= modifier.Cost[0] && modifier.level == 0)
            {
                modifier.isUnlocked = true;
                modifier.level++;

                game.AddCash(-modifier.Cost[0]);
                tower.worth += modifier.Cost[0] * 0.75f;
                level.Text.text = "Lvl " + modifier.level;
                modifierCost.Text.text = $"{modifier.CostValues[modifier.level]}";
            }
        }));
        ModHelperImage upgradeImage = upgradeBtn.AddImage(new Info("image", 0, 0, 100), VanillaSprites.UpgradeIcon2);
        
        return panel;
    }
}