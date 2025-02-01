using BTD_Mod_Helper.Api.Enums;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using Il2CppAssets.Scripts.Unity;

namespace SpaceMarine
{
    public class MOABDamage : ModifierTemplate
    {
        public override string ModName => "MOAB Damage";
        public override string Icon => VanillaSprites.GrandSaboteurUpgradeIcon;
        public override string Effect => "Deal bonus damage to MOABs";
        public override int MaxLevel => 7;
        public override float CostMultiplier => 1.8f;
        public override float StartingCost => 32;
        public override string Stat => "Multiplier";
        public override float FontSize => 35;
        public override float NameSize => 55;
        public override float StartingValue => 2;
    }

    public class Sapper : ModifierTemplate
    {
        public override string ModName => "Sapper";
        public override string Icon => VanillaSprites.PerishingPotionsUpgradeIcon;
        public override string Effect => "Deal bonus damage to Ceramics";
        public override int MaxLevel => 8;
        public override float CostMultiplier => 1.3f;
        public override float StartingCost => 24;
        public override string Stat => "Multiplier";
        public override float FontSize => 35;
        public override float NameSize => 60;
        public override float StartingValue => 2;
    }

    public class RapidFire : ModifierTemplate
    {
        public override string ModName => "Rapid Fire";
        public override string Icon => VanillaSprites.SemiAutomaticUpgradeIcon;
        public override string Effect => "Attack significantly faster";
        public override int MaxLevel => 13;
        public override float CostMultiplier => 1.3f;
        public override float StartingCost => 18;
        public override string Stat => "Speed %";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 25;
    }

    public class Slowdown : ModifierTemplate
    {
        public override string ModName => "Slowdown";
        public override string Icon => VanillaSprites.ArcticWindUpgradeIcon;
        public override string Effect => "Slow Bloons hit";
        public override int MaxLevel => 5;
        public override float CostMultiplier => 1.6f;
        public override float StartingCost => 16;
        public override string Stat => "Slow %";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 20;
    }

    public class Eraser : ModifierTemplate
    {
        public override string ModName => "Eraser";
        public override string Icon => VanillaSprites.SignalFlareUpgradeIcon;
        public override string Effect => "Removes Bloon properties";
        public override int MaxLevel => 3;
        public override float CostMultiplier => 2.2f;
        public override float StartingCost => 28;
        public override string Stat => "Cleanses";
        public override string[] EraserProperties => ["Regrow", "Camo", "Fortified"];
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 1;
    }

    public class Charm : ModifierTemplate
    {
        public override string ModName => "Charm";
        public override string Icon => "7d34eed972e35af4188f20d9f3a2e9fc";
        public override string Effect => "Summons Charmed Bloons";
        public override int MaxLevel => 9;
        public override float CostMultiplier => 1.4f;
        public override float StartingCost => 25;
        public override string Stat => "Strength";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 1;
        public override PrefabReference[] CharmSprite => [
            Game.instance.model.GetBloon("Red").display,
            Game.instance.model.GetBloon("Blue").display,
            Game.instance.model.GetBloon("Green").display,
            Game.instance.model.GetBloon("Yellow").display,
            Game.instance.model.GetBloon("Pink").display,
            Game.instance.model.GetBloon("Black").display,
            Game.instance.model.GetBloon("Zebra").display,
            Game.instance.model.GetBloon("Rainbow").display,
            Game.instance.model.GetBloon("Ceramic").display
        ];
    }
}