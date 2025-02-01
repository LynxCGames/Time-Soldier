using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using UnityEngine;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Unity.UI_New.Store;

namespace SpaceMarine;

public class MenuUpgrade : BloonsTD6Mod
{
    public static void UpgradeMenu(RectTransform rect, Tower tower, ModHelperPanel mainPanel, ModHelperText scrapCount, ModHelperPanel tester)
    {
        // Weapon Shop
        ModHelperText weaponText = mainPanel.AddText(new Info("text", -810, 710, 750, 100), "Weapons", 80);
        ModHelperScrollPanel weaponScroll = mainPanel.AddScrollPanel(new Info("scrollPanel", -810, 240, 750, 800), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanel, 15, 50);
        foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
        {
            weaponScroll.AddScrollContent(WeaponUpgrades.CreateWeaponShop(weapon, tower, scrapCount));
        }

        // General Modifier Shop
        ModHelperText modifierText = mainPanel.AddText(new Info("text", 0, 710, 750, 100), "General Modifiers", 70);
        ModHelperScrollPanel modifierScroll = mainPanel.AddScrollPanel(new Info("scrollPanel", 0, 240, 750, 800), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanel, 15, 50);
        foreach (var modifier in ModContent.GetContent<ModifierTemplate>())
        {
            modifierScroll.AddScrollContent(ModifierUpgrades.CreateModShop(modifier, tower, scrapCount));
        }

        // Special Modifier Shop
        ModHelperText specialText = mainPanel.AddText(new Info("text", 810, 710, 750, 100), "Special Modifiers", 70);
        ModHelperScrollPanel specialScroll = mainPanel.AddScrollPanel(new Info("scrollPanel", 810, 240, 750, 800), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanel, 15, 50);
        foreach (var modifier in ModContent.GetContent<SpecialTemplate>())
        {
            specialScroll.AddScrollContent(SpecialUpgrades.CreateSpecialShop(modifier, tower, scrapCount));
        }

        // Pierce Shop
        ModHelperPanel piercePanel = mainPanel.AddPanel(new Info("piercePanel", -750, -290, 870, 150), VanillaSprites.BrownInsertPanel);
        ModHelperText pierceText = piercePanel.AddText(new Info("text", -255, 0, 300, 120), "Pierce", 80);
        ModHelperButton pierceBtn = piercePanel.AddButton(new Info("button", 615, 0, 300, 150), VanillaSprites.BlueBtn, new System.Action(() => {
            if (mod.pierceLvl < 5)
            {
                BaseStatUpgrades.PierceUpgrade(rect, tower, scrapCount);

                for (int i = 0; i < mod.pierceLvl; i++)
                {
                    MenuUi.pierceIndicate[i].GetComponent<ModHelperPanel>().Background.sprite = tester.Background.sprite;
                }
            }
        }));

        ModHelperText pierceCost = pierceBtn.AddText(new Info("text", 50, 0, 300, 180), "", 70);
        ModHelperImage pierceIcon = pierceBtn.AddImage(new Info("scrapIcon", -70, 0, 100), ModContent.GetSprite(mod, "Scrap-Icon"));

        var xValue = -35;
        if (mod.pierceLvl >= 5)
        {
            pierceCost.Text.text = "Maxed";
            pierceCost.Text.fontSize = 40;

            for (int i = 0; i < mod.pierceLvl; i++)
            {
                ModHelperPanel pierceLvl = piercePanel.AddPanel(new Info("Panel", xValue, 0, 80), VanillaSprites.BlueBtnSquareSmall);
                xValue += 100;
            }
        }
        else
        {
            MenuUi[] pierceArray = new MenuUi[5];
            pierceCost.Text.text = $"{mod.pierceCost}";

            for (int i = 0; i < mod.pierceLvl; i++)
            {
                ModHelperPanel pierceLvl = piercePanel.AddPanel(new Info("Panel", xValue, 0, 80), VanillaSprites.BlueBtnSquareSmall);
                xValue += 100;

                pierceArray[i] = pierceLvl.AddComponent<MenuUi>();
            }
            for (int i = 0; i < 5 - mod.pierceLvl; i++)
            {
                ModHelperPanel pierceLvl = piercePanel.AddPanel(new Info("Panel", xValue, 0, 80), VanillaSprites.RedBtnSquareSmall);
                xValue += 100;

                pierceArray[mod.pierceLvl + i] = pierceLvl.AddComponent<MenuUi>();
            }
            MenuUi.pierceIndicate = pierceArray;
        }
        MenuUi pierceUi = pierceCost.AddComponent<MenuUi>();
        MenuUi.pierce = pierceUi;

        // Speed Shop
        ModHelperPanel speedPanel = mainPanel.AddPanel(new Info("speedPanel", -750, -480, 870, 150), VanillaSprites.BrownInsertPanel);
        ModHelperText speedText = speedPanel.AddText(new Info("text", -255, 0, 300, 120), "Attack Speed", 55);
        ModHelperButton speedBtn = speedPanel.AddButton(new Info("button", 615, 0, 300, 150), VanillaSprites.BlueBtn, new System.Action(() => {
            if (mod.speedLvl < 5)
            {
                BaseStatUpgrades.SpeedUpgrade(rect, tower, scrapCount);

                for (int i = 0; i < mod.speedLvl; i++)
                {
                    MenuUi.speedIndicate[i].GetComponent<ModHelperPanel>().Background.sprite = tester.Background.sprite;
                }
            }
        }));

        ModHelperText speedCost = speedBtn.AddText(new Info("text", 50, 0, 300, 180), "", 70);
        ModHelperImage speedIcon = speedBtn.AddImage(new Info("scrapIcon", -70, 0, 100), ModContent.GetSprite(mod, "Scrap-Icon"));

        xValue = -35;
        if (mod.speedLvl >= 5)
        {
            speedCost.Text.text = "Maxed";
            speedCost.Text.fontSize = 40;

            for (int i = 0; i < mod.speedLvl; i++)
            {
                ModHelperPanel speedLvl = speedPanel.AddPanel(new Info("Panel", xValue, 0, 80), VanillaSprites.BlueBtnSquareSmall);
                xValue += 100;
            }
        }
        else
        {
            MenuUi[] speedArray = new MenuUi[5];
            speedCost.Text.text = $"{mod.speedCost}";

            for (int i = 0; i < mod.speedLvl; i++)
            {
                ModHelperPanel speedLvl = speedPanel.AddPanel(new Info("Panel", xValue, 0, 80), VanillaSprites.BlueBtnSquareSmall);
                xValue += 100;

                speedArray[i] = speedLvl.AddComponent<MenuUi>();
            }
            for (int i = 0; i < 5 - mod.speedLvl; i++)
            {
                ModHelperPanel speedLvl = speedPanel.AddPanel(new Info("Panel", xValue, 0, 80), VanillaSprites.RedBtnSquareSmall);
                xValue += 100;

                speedArray[mod.speedLvl + i] = speedLvl.AddComponent<MenuUi>();
            }
            MenuUi.speedIndicate = speedArray;
        }
        MenuUi speedUi = speedCost.AddComponent<MenuUi>();
        MenuUi.speed = speedUi;

        // Range Shop
        ModHelperPanel rangePanel = mainPanel.AddPanel(new Info("rangePanel", -750, -670, 870, 150), VanillaSprites.BrownInsertPanel);
        ModHelperText rangeText = rangePanel.AddText(new Info("text", -255, 0, 300, 120), "Range", 80);
        ModHelperButton rangeBtn = rangePanel.AddButton(new Info("button", 615, 0, 300, 150), VanillaSprites.BlueBtn, new System.Action(() => {
            if (mod.rangeLvl < 5)
            {
                BaseStatUpgrades.RangeUpgrade(rect, tower, scrapCount);

                for (int i = 0; i < mod.rangeLvl; i++)
                {
                    MenuUi.rangeIndicate[i].GetComponent<ModHelperPanel>().Background.sprite = tester.Background.sprite;
                }
            }
        }));

        ModHelperText rangeCost = rangeBtn.AddText(new Info("scrapCount", 50, 0, 200, 180), "", 70);
        ModHelperImage rangeIcon = rangeBtn.AddImage(new Info("scrapIcon", -70, 0, 100), ModContent.GetSprite(mod, "Scrap-Icon"));

        xValue = -35;
        if (mod.rangeLvl >= 5)
        {
            rangeCost.Text.text = "Maxed";
            rangeCost.Text.fontSize = 40;

            for (int i = 0; i < mod.rangeLvl; i++)
            {
                ModHelperPanel rangeLvl = rangePanel.AddPanel(new Info("Panel", xValue, 0, 80), VanillaSprites.BlueBtnSquareSmall);
                xValue += 100;
            }
        }
        else
        {
            MenuUi[] rangeArray = new MenuUi[5];
            rangeCost.Text.text = $"{mod.rangeCost}";

            for (int i = 0; i < mod.rangeLvl; i++)
            {
                ModHelperPanel rangeLvl = rangePanel.AddPanel(new Info("Panel", xValue, 0, 80), VanillaSprites.BlueBtnSquareSmall);
                xValue += 100;

                rangeArray[i] = rangeLvl.AddComponent<MenuUi>();
            }
            for (int i = 0; i < 5 - mod.rangeLvl; i++)
            {
                ModHelperPanel rangeLvl = rangePanel.AddPanel(new Info("Panel", xValue, 0, 80), VanillaSprites.RedBtnSquareSmall);
                xValue += 100;

                rangeArray[mod.rangeLvl + i] = rangeLvl.AddComponent<MenuUi>();
            }
            MenuUi.rangeIndicate = rangeArray;
        }
        MenuUi rangeUi = rangeCost.AddComponent<MenuUi>();
        MenuUi.range = rangeUi;

        // Damage Shop
        ModHelperPanel damagePanel = mainPanel.AddPanel(new Info("damagePanel", 470, -290, 770, 150), VanillaSprites.BrownInsertPanel);
        ModHelperText damageText = damagePanel.AddText(new Info("text", -205, 0, 300, 120), "Damage", 75);
        ModHelperButton damageBtn = damagePanel.AddButton(new Info("button", 565, 0, 300, 150), VanillaSprites.BlueBtn, new System.Action(() => {
            if (mod.damageLvl < 4)
            {
                BaseStatUpgrades.DamageUpgrade(rect, tower, scrapCount);

                for (int i = 0; i < mod.damageLvl; i++)
                {
                    MenuUi.damageIndicate[i].GetComponent<ModHelperPanel>().Background.sprite = tester.Background.sprite;
                }
            }
        }));

        ModHelperText damageCost = damageBtn.AddText(new Info("scrapCount", 50, 0, 150, 180), "", 70);
        ModHelperImage damageIcon = damageBtn.AddImage(new Info("scrapIcon", -70, 0, 100), ModContent.GetSprite(mod, "Scrap-Icon"));

        var xDamageValue = 15;
        if (mod.damageLvl >= 4)
        {
            damageCost.Text.text = "Maxed";
            damageCost.Text.fontSize = 40;

            for (int i = 0; i < mod.damageLvl; i++)
            {
                ModHelperPanel damageLvl = damagePanel.AddPanel(new Info("Panel", xDamageValue, 0, 80), VanillaSprites.BlueBtnSquareSmall);
                xDamageValue += 100;
            }
        }
        else
        {
            MenuUi[] damageArray = new MenuUi[4];
            damageCost.Text.text = $"{mod.damageCost}";

            for (int i = 0; i < mod.damageLvl; i++)
            {
                ModHelperPanel damageLvl = damagePanel.AddPanel(new Info("Panel", xDamageValue, 0, 80), VanillaSprites.BlueBtnSquareSmall);
                xDamageValue += 100;

                damageArray[i] = damageLvl.AddComponent<MenuUi>();
            }
            for (int i = 0; i < 4 - mod.damageLvl; i++)
            {
                ModHelperPanel damageLvl = damagePanel.AddPanel(new Info("Panel", xDamageValue, 0, 80), VanillaSprites.RedBtnSquareSmall);
                xDamageValue += 100;

                damageArray[mod.damageLvl + i] = damageLvl.AddComponent<MenuUi>();
            }
            MenuUi.damageIndicate = damageArray;
        }
        MenuUi damageUi = damageCost.AddComponent<MenuUi>();
        MenuUi.damage = damageUi;

        // Camo Shop
        ModHelperPanel camoPanel = mainPanel.AddPanel(new Info("camoPanel", 470, -480, 470, 150), VanillaSprites.BrownInsertPanel);
        ModHelperText camoText = camoPanel.AddText(new Info("text", -55, 0, 300, 120), "Camo Detection", 55);
        ModHelperButton camoBtn = camoPanel.AddButton(new Info("button", 415, 0, 300, 150), VanillaSprites.BlueBtn, new System.Action(() => {
            if (mod.camoActive == false)
            {
                BaseStatUpgrades.CamoUpgrade(rect, tower, scrapCount);

                if (mod.camoActive == true)
                {
                    MenuUi.camoIndicate.GetComponent<ModHelperPanel>().Background.sprite = tester.Background.sprite;
                }
            }
        }));

        ModHelperText camoCost = camoBtn.AddText(new Info("scrapCount", 50, 0, 200, 180), "", 70);
        ModHelperPanel camoLvl = camoPanel.AddPanel(new Info("Panel", 165, 0, 80), VanillaSprites.RedBtnSquareSmall);
        ModHelperImage camoIcon = camoBtn.AddImage(new Info("scrapIcon", -70, 0, 100), ModContent.GetSprite(mod, "Scrap-Icon"));

        if (mod.camoActive == true)
        {
            camoCost.Text.text = "Maxed";
            camoCost.Text.fontSize = 40;
            camoLvl.Background.sprite = tester.Background.sprite;
        }
        else
        {
            camoCost.Text.text = $"{mod.camoCost}";
        }
        MenuUi camoUi = camoCost.AddComponent<MenuUi>();
        MenuUi.camo = camoUi;
        MenuUi camoIndicater = camoLvl.AddComponent<MenuUi>();
        MenuUi.camoIndicate = camoIndicater;

        // MIB Shop
        ModHelperPanel mibPanel = mainPanel.AddPanel(new Info("mibPanel", 470, -670, 470, 150), VanillaSprites.BrownInsertPanel);
        ModHelperText mibText = mibPanel.AddText(new Info("text", -55, 0, 300, 120), "Pop All Bloon Types", 45);
        ModHelperButton mibBtn = mibPanel.AddButton(new Info("button", 415, 0, 300, 150), VanillaSprites.BlueBtn, new System.Action(() => {
            if (mod.mibActive == false)
            {
                BaseStatUpgrades.MibUpgrade(rect, tower, scrapCount);

                if (mod.mibActive == true)
                {
                    MenuUi.mibIndicate.GetComponent<ModHelperPanel>().Background.sprite = tester.Background.sprite;
                }
            }
        }));

        ModHelperText mibCost = mibBtn.AddText(new Info("scrapCount", 50, 0, 200, 180), $"{mod.mibCost}", 70);
        ModHelperPanel mibLvl = mibPanel.AddPanel(new Info("Panel", 165, 0, 80), VanillaSprites.RedBtnSquareSmall);
        ModHelperImage mibIcon = mibBtn.AddImage(new Info("scrapIcon", -70, 0, 100), ModContent.GetSprite(mod, "Scrap-Icon"));

        if (mod.mibActive == true)
        {
            mibCost.Text.text = "Maxed";
            mibCost.Text.fontSize = 40;
            mibLvl.Background.sprite = tester.Background.sprite;
        }
        else
        {
            mibCost.Text.text = $"{mod.mibCost}";
        }
        MenuUi mibUi = mibCost.AddComponent<MenuUi>();
        MenuUi.mib = mibUi;
        MenuUi mibIndicater = mibLvl.AddComponent<MenuUi>();
        MenuUi.mibIndicate = mibIndicater;
    }
}
