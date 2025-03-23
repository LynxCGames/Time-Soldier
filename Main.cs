using MelonLoader;
using BTD_Mod_Helper;
using SpaceMarine;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Data.Behaviors.Abilities;

[assembly: MelonInfo(typeof(SpaceMarine.SpaceMarine), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace SpaceMarine;

public class SpaceMarine : BloonsTD6Mod
{
    internal static readonly ModSettingBool infiniteScrap = new(false)
    {
        icon = VanillaSprites.SandboxBtn,
        description = "Gives 9999 scrap"
    };

    public static SpaceMarine mod;
    public bool isSelected = false;
    public bool almanacOpen = false;
    public float scrap = 0;
    public float usedScrap = 0;
    public bool equipSelected = false;
    public string weapon = "Crossbow";
    public bool comboedWeapon = false;
    public int modifierCount = 0;
    public string modifier1 = "";
    public string modifier2 = "";
    public string modifier3 = "";
    public float scavenger = 1;
    public string upgradeSelect = "Weapons";
    public string specialSelected = "";

    // Stats
    public int pierceLvl = 0;
    public int speedLvl = 0;
    public int rangeLvl = 0;
    public int damageLvl = 0;
    public bool camoActive = false;
    public bool mibActive = false;

    public float pierceCost = 5;
    public float speedCost = 8;
    public float rangeCost = 8;
    public float damageCost = 10;
    public int camoCost = 30;
    public int mibCost = 80;

    public void Reset()
    {
        isSelected = false;
        almanacOpen = false;
        equipSelected = false;
        scrap += usedScrap;
        usedScrap = 0;
        weapon = "Crossbow";
        comboedWeapon = false;
        modifierCount = 0;
        modifier1 = "";
        modifier2 = "";
        modifier3 = "";
        scavenger = 1;
        upgradeSelect = "Weapons";
        specialSelected = "";

    Reseter.SpecialReset();

        pierceLvl = 0;
        speedLvl = 0;
        rangeLvl = 0;
        damageLvl = 0;
        camoActive = false;
        mibActive = false;

        pierceCost = 5;
        speedCost = 8;
        rangeCost = 8;
        damageCost = 10;
        camoCost = 30;
        mibCost = 80;
    }

    public override void OnApplicationStart()
    {
        mod = this;
    }
    public override void OnGameModelLoaded(GameModel model)
    {
        Reset();
    }
    public override void OnNewGameModel(GameModel result)
    {
        Reset();

        foreach (var tower in result.towerSet)
        {
            if (tower.name.Contains("SpaceMarine"))
            {
                tower.GetShopTowerDetails().towerCount = 1;
            }
        }

        if (infiniteScrap)
        {
            scrap = 9999;
        }
        else
        {
            scrap = 0;
        }
    }
    public override void OnRestart()
    {
        MenuUi.instance.CloseMenu();
    }
    public override void OnRoundEnd()
    {
        if (InGame.instance.currentRoundId >= 80)
        {
            Il2CppSystem.Random rnd = new();
            var num = rnd.Next(40, 50);
            mod.scrap += num;
        }
        else if (InGame.instance.currentRoundId >= 60 && InGame.instance.currentRoundId < 80)
        {
            Il2CppSystem.Random rnd = new();
            var num = rnd.Next(24, 28);
            mod.scrap += num;
        }
        else if (InGame.instance.currentRoundId >= 40 && InGame.instance.currentRoundId < 60)
        {
            Il2CppSystem.Random rnd = new();
            var num = rnd.Next(14, 18);
            mod.scrap += num;
        }
        else
        {
            Il2CppSystem.Random rnd = new();
            var num = rnd.Next(5, 8);
            mod.scrap += num;
        }

        if (isSelected == true && almanacOpen == false)
        {
            MenuUi.scrap.GetComponent<ModHelperText>().Text.text = $"{mod.scrap}";
        }
    }
    public override void OnTowerDestroyed(Tower tower)
    {
        if (tower.towerModel.name.Contains("SpaceMarineTower"))
        {
            Reset();
        }
    }
    public override void OnTowerSelected(Tower tower)
    {
        if (tower.towerModel.name.Contains("SpaceMarineTower"))
        {
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            MenuUi.CreateMenu(rect, tower);
            isSelected = true;
        }
    }
    public override void OnTowerDeselected(Tower tower)
    {
        if (tower.towerModel.name.Contains("SpaceMarineTower"))
        {
            isSelected = false;
            if (MenuUi.instance)
            {
                MenuUi.instance.CloseMenu();
            }

        }
    }

    [RegisterTypeInIl2Cpp(false)]
    public class MenuUi : MonoBehaviour
    {
        public static MenuUi? instance;
        public static MenuUi? scrap;

        public static MenuUi? pierce;
        public static MenuUi? speed;
        public static MenuUi? range;
        public static MenuUi? damage;
        public static MenuUi? camo;
        public static MenuUi? mib;

        public static MenuUi[]? pierceIndicate;
        public static MenuUi[]? speedIndicate;
        public static MenuUi[]? rangeIndicate;
        public static MenuUi[]? damageIndicate;
        public static MenuUi? camoIndicate;
        public static MenuUi? mibIndicate;

        public ModHelperInputField? input;
        public void CloseMenu()
        {
            if (gameObject)
            {
                Destroy(gameObject);
            }

        }

        public static void CreateMenu(RectTransform rect, Tower tower)
        {
            if (mod.almanacOpen == true)
                return;

            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1300, 2500, 1850, new Vector2()), VanillaSprites.BrownInsertPanel);
            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            instance = upgradeUi;

            ModHelperButton upgradeBtn = panel.AddButton(new Info("upgradeBtn", -825, 810, 800, 180), VanillaSprites.BrownInsertPanelDark, new System.Action(() => {
                if (mod.equipSelected == true)
                {
                    mod.equipSelected = false;
                    instance.CloseMenu();
                    CreateMenu(rect, tower);
                }
            }));
            ModHelperText upgradeText = upgradeBtn.AddText(new Info("upgradeText", 0, 0, 800, 180), "Upgrades", 100);

            ModHelperButton equipBtn = panel.AddButton(new Info("equipBtn", 0, 810, 800, 180), VanillaSprites.BrownInsertPanelDark, new System.Action(() => {
                if (mod.equipSelected == false)
                {
                    mod.equipSelected = true;
                    instance.CloseMenu();
                    CreateMenu(rect, tower);
                }
            }));
            ModHelperText equipText = equipBtn.AddText(new Info("equipText", 0, 0, 800, 180), "Equipment", 100);

            ModHelperButton almanacBtn = panel.AddButton(new Info("equipBtn", 515, 810, 180), VanillaSprites.BrownInsertPanelDark, new System.Action(() => {
                if (mod.almanacOpen == false)
                {
                    instance.CloseMenu();
                    mod.almanacOpen = true;
                    Almanac.AlmanacMenu(rect, tower);
                }
            }));
            ModHelperImage alamanceImage = almanacBtn.AddImage(new Info("equipText", 0, 0, 160), VanillaSprites.HomeMonkeyKnowledgeBtn);

            ModHelperPanel scrapPanel = panel.AddPanel(new Info("Panel", 975, 810, 500, 180), VanillaSprites.BrownInsertPanelDark);
            ModHelperImage scrapIcon = scrapPanel.AddImage(new Info("scrapIcon", -160, 0, 150), ModContent.GetSprite(mod, "Scrap-Icon"));
            ModHelperText scrapCount = scrapPanel.AddText(new Info("scrapCount", 65, 0, 300, 180), $"{mod.scrap}", 100);
            MenuUi scrapUi = scrapCount.AddComponent<MenuUi>();
            scrap = scrapUi;

            ModHelperPanel tester = panel.AddPanel(new Info("Panel", 0, -100, 80), VanillaSprites.BlueBtnSquareSmall);
            ModHelperPanel mainPanel = panel.AddPanel(new Info("Panel", 0, -90, 2450, 1600), VanillaSprites.BrownInsertPanelDark);
            
            ModHelperPanel helpPanel = panel.AddPanel(new Info("Panel", -1700, -450, 750, 1000), VanillaSprites.BrownInsertPanelDark);
            ModHelperText helpText = helpPanel.AddText(new Info("Text", 0, 425, 700, 110), "Help", 100);
            ModHelperText help = helpPanel.AddText(new Info("Text", 0, -88, 700, 825), "", 38, Il2CppTMPro.TextAlignmentOptions.TopLeft);

            if (mod.equipSelected == true)
            {
                MenuEquip.EquipMenu(rect, tower, mainPanel);
                help.Text.text = "Select any weapon for the tower. Use the reset button to unequip the tower's current weapon. Selecting a bonus weapon creates a combo weapon that can have varying effects " +
                    "different from the two weapons used to make it. Combo weapon stats are a combination of the stats of the two combined weapons. Open the almanac using the book icon at the top to learn more " +
                    "information about the combo weapons. A weapon must be selected before modifiers can be added. General modifiers can be added to any weapon while special modifiers can only be added to " +
                    "the specified weapon or combo weapons made from the specified weapon.";
            }
            else
            {
                MenuUpgrade.UpgradeMenu(rect, tower, mainPanel, scrapCount, tester);
                help.Text.text = "Earn scrap every round to upgrade your weapons, stats, and modifiers. Earn more scrap the higher the rounds are. Selling the tower returns all scrap spent. Leveling up a weapon " +
                    "will increase the stats of all combo weapons it makes if they are unlocked. The basic stats provide varying effects based on the weapon used. Weapons are split into three range catagories: " +
                    "Short, Mid, and Long. Upgrading the range stat will increase the tower's range based on the range catagory of the equiped weapon. The tower cannot see camo Bloons and not all weapons can " +
                    "pop all Bloon types so buying those two upgrades are key to success.";
            }
        }
    }
}