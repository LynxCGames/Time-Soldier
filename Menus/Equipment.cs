using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Api.Components;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using UnityEngine;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;

namespace SpaceMarine;

public class Equipment : BloonsTD6Mod
{
    public static ModHelperPanel WeaponEquip(WeaponTemplate weapon, Tower tower)
    {
        var panel = ModHelperPanel.Create(new Info("panel", 0, 0, 500), VanillaSprites.MainBgPanelJukebox);
        ModHelperText name = panel.AddText(new Info("name", 0, 190, 500, 80), weapon.WeaponName, weapon.NameSize);
        ModHelperText level = panel.AddText(new Info("level", -140, -170, 180, 80), "Lvl " + weapon.level, 60);
        ModHelperImage image = panel.AddImage(new Info("image", 0, 0, 300), weapon.Icon);

        ModHelperButton selectBtn = panel.AddButton(new Info("button", 170, -170, 120), VanillaSprites.BlueBtnSquare, new System.Action(() => {
            mod.weapon = weapon.WeaponName;
            WeaponMethods.WeaponSelect(weapon, tower);

            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            MenuUi.instance.CloseMenu();
            MenuUi.CreateMenu(rect, tower);
        }));
        ModHelperImage selectImage = selectBtn.AddImage(new Info("image", 0, 0, 80), VanillaSprites.ContinueIcon);

        return panel;
    }

    public static ModHelperPanel BonusEquip(WeaponTemplate weapon, Tower tower)
    {
        var panel = ModHelperPanel.Create(new Info("panel", 0, 0, 500), VanillaSprites.MainBgPanelJukebox);
        ModHelperText name = panel.AddText(new Info("name", 0, 190, 500, 80), weapon.WeaponName, 60);
        ModHelperText level = panel.AddText(new Info("level", -140, -170, 180, 80), "Lvl " + weapon.level, 60);
        ModHelperImage image = panel.AddImage(new Info("image", 0, 0, 300), weapon.Icon);

        ModHelperButton selectBtn = panel.AddButton(new Info("button", 170, -170, 120), VanillaSprites.BlueBtnSquare, new System.Action(() => {
            if (mod.modifierCount == 0)
            {
                mod.comboedWeapon = true;

                foreach (var combo in ModContent.GetContent<ComboTemplate>())
                {
                    ComboMethods.ComboWeaponSwitcher(weapon, combo);
                    ComboMethods.ComboSelect(combo, tower);
                }

                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                MenuUi.instance.CloseMenu();
                MenuUi.CreateMenu(rect, tower);
            }
        }));
        ModHelperImage selectImage = selectBtn.AddImage(new Info("image", 0, 0, 80), VanillaSprites.ContinueIcon);

        return panel;
    }

    public static ModHelperPanel ModifierEquip(ModifierTemplate modifier, Tower tower)
    {
        var panel = ModHelperPanel.Create(new Info("panel", 0, 0, 500), VanillaSprites.MainBgPanelJukebox);
        ModHelperText name = panel.AddText(new Info("name", 0, 190, 500, 80), modifier.ModName, 60);
        ModHelperText level = panel.AddText(new Info("level", -140, -170, 180, 80), "Lvl " + modifier.level, 60);
        ModHelperImage image = panel.AddImage(new Info("image", 0, 0, 300), modifier.Icon);

        if (modifier.ModName == "MOAB Damage")
        {
            name.Text.fontSize = 50;
        }

        if (modifier.Name == "Scavenger")
        {
            image.Image.sprite = ModContent.GetSprite(mod, "Scrap-Icon");
        }

        ModHelperButton selectBtn = panel.AddButton(new Info("button", 170, -170, 120), VanillaSprites.BlueBtnSquare, new System.Action(() =>
        {
            if (mod.modifierCount < 3)
            {
                if (mod.modifierCount == 0)
                {
                    mod.modifier1 = modifier.ModName;

                    if (modifier.ModName == "Charm" && mod.weapon == "Magic")
                    {
                        mod.comboedWeapon = true;
                        mod.weapon = "Necromancer";
                        modifier.icon = VanillaSprites.DarkRitualAA;

                        foreach (var combo in ModContent.GetContent<ComboTemplate>())
                        {
                            if (combo.WeaponName == "???")
                            {
                                combo.discovered = false;
                            }
                            if (combo.WeaponName == "Necromancer")
                            {
                                combo.discovered = true;
                            }

                            ComboMethods.ComboSelect(combo, tower);
                        }
                    }
                }
                if (mod.modifierCount == 1)
                {
                    mod.modifier2 = modifier.ModName;
                }
                if (mod.modifierCount == 2)
                {
                    mod.modifier3 = modifier.ModName;
                }
                mod.modifierCount++;
                ModifierMethods.ModifierSelect(modifier, tower);

                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                MenuUi.instance.CloseMenu();
                MenuUi.CreateMenu(rect, tower);
            }
        }));
        ModHelperImage selectImage = selectBtn.AddImage(new Info("image", 0, 0, 80), VanillaSprites.ContinueIcon);

        return panel;
    }

    public static ModHelperPanel ScavengerEquip(ScavengerTemplate modifier, Tower tower)
    {
        var panel = ModHelperPanel.Create(new Info("panel", 0, 0, 500), VanillaSprites.MainBgPanelJukebox);
        ModHelperText name = panel.AddText(new Info("name", 0, 190, 500, 80), "Scavenger", 60);
        ModHelperText level = panel.AddText(new Info("level", -140, -170, 180, 80), "Lvl " + modifier.level, 60);
        ModHelperImage image = panel.AddImage(new Info("image", 0, 0, 300), ModContent.GetSprite(mod, "Scrap-Icon"));
        ModHelperButton selectBtn = panel.AddButton(new Info("button", 170, -170, 120), VanillaSprites.BlueBtnSquare, new System.Action(() =>
        {
            if (mod.modifierCount < 3)
            {
                if (mod.modifierCount == 0)
                {
                    mod.modifier1 = "Scavenger";
                }
                if (mod.modifierCount == 1)
                {
                    mod.modifier2 = "Scavenger";
                }
                if (mod.modifierCount == 2)
                {
                    mod.modifier3 = "Scavenger";
                }
                mod.modifierCount++;

                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                MenuUi.instance.CloseMenu();
                MenuUi.CreateMenu(rect, tower);
            }
        }));
        ModHelperImage selectImage = selectBtn.AddImage(new Info("image", 0, 0, 80), VanillaSprites.ContinueIcon);

        return panel;
    }

    public static ModHelperPanel SpecialEquip(SpecialTemplate modifier, Tower tower)
    {
        var panel = ModHelperPanel.Create(new Info("panel", 0, 0, 500), VanillaSprites.MainBgPanelJukebox);
        ModHelperText name = panel.AddText(new Info("name", 0, 190, 500, 80), modifier.ModName, 60);
        ModHelperText level = panel.AddText(new Info("level", -140, -170, 180, 80), "Lvl " + modifier.level, 60);
        ModHelperImage image = panel.AddImage(new Info("image", 0, 0, 300), modifier.Icon);

        if (modifier.ModName == "Sharpshooter")
        {
            name.Text.fontSize = 50;
        }

        ModHelperButton selectBtn = panel.AddButton(new Info("button", 170, -170, 120), VanillaSprites.BlueBtnSquare, new System.Action(() => {
            if (mod.modifierCount < 3)
            {
                if (mod.modifierCount == 0)
                {
                    mod.modifier1 = modifier.ModName;
                }
                if (mod.modifierCount == 1)
                {
                    mod.modifier2 = modifier.ModName;
                }
                if (mod.modifierCount == 2)
                {
                    mod.modifier3 = modifier.ModName;
                }
                mod.modifierCount++;
                SpecialMethods.SpecialSelect(modifier, tower);

                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                MenuUi.instance.CloseMenu();
                MenuUi.CreateMenu(rect, tower);
            }
        }));
        ModHelperImage selectImage = selectBtn.AddImage(new Info("image", 0, 0, 80), VanillaSprites.ContinueIcon);

        return panel;
    }
}