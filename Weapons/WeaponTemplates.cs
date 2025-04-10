﻿using BTD_Mod_Helper.Api.Enums;

namespace SpaceMarine
{
    public class Crossbow : WeaponTemplate
    {
        public override string WeaponName => "Crossbow";
        public override string Icon => VanillaSprites.CrossBowMasterUpgradeIcon;
        public override string Bonus => "No additional effects";
        public override string[] Combos => ["Missile", "Ice", "Laser", "Fire", "Thorn"];
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float[] StartingValues => [3, 0.9f, 2];
        public override string Range => "Long-Range";
        public override int[] PierceValue => [1];
    }

    public class Missile : WeaponTemplate
    {
        public override string WeaponName => "Missile";
        public override string Icon => VanillaSprites.MissileLauncherUpgradeIcon;
        public override string Bonus => "Explodes on contact";
        public override string[] Combos => ["Crossbow", "Ice", "Laser", "Fire", "Magic"];
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float[] StartingValues => [8, 1.4f, 1];
        public override string Range => "Mid-Range";
        public override int[] PierceValue => [0, 2];
    }

    public class Ice : WeaponTemplate
    {
        public override string WeaponName => "Ice";
        public override string Icon => VanillaSprites.PermafrostUpgradeIcon;
        public override string Bonus => "Freezes nearby Bloons";
        public override string[] Combos => ["Crossbow", "Missile", "Laser", "Thorn", "Magic"];
        public override float FontSize => 35;
        public override float NameSize => 60;
        public override float[] StartingValues => [18, 1.8f, 1];
        public override string Range => "Short-Range";
        public override int[] PierceValue => [3];
    }

    public class Laser : WeaponTemplate
    {
        public override string WeaponName => "Laser";
        public override string Icon => VanillaSprites.LaserCannonUpgradeIcon;
        public override string Bonus => "No additional effects";
        public override string[] Combos => ["Crossbow", "Missile", "Ice", "Fire"];
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float[] StartingValues => [2, 0.65f, 1];
        public override string Range => "Mid-Range";
        public override int[] PierceValue => [1];
    }

    public class Fire : WeaponTemplate
    {
        public override string WeaponName => "Fire";
        public override string Icon => VanillaSprites.FireballUpgradeIcon;
        public override string Bonus => "Sets Bloons on fire";
        public override string[] Combos => ["Crossbow", "Missile", "Laser", "Thorn"];
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float[] StartingValues => [3, 0.8f, 1];
        public override string Range => "Mid-Range";
        public override int[] PierceValue => [1];
    }

    public class Thorn : WeaponTemplate
    {
        public override string WeaponName => "Thorn";
        public override string Icon => VanillaSprites.ThornSwarmUpgradeIcon;
        public override string Bonus => "Fires 5 at a time";
        public override string[] Combos => ["Crossbow", "Ice", "Fire"];
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float[] StartingValues => [1, 1.1f, 1];
        public override string Range => "Mid-Range";
        public override int[] PierceValue => [1];
    }

    public class Magic : WeaponTemplate
    {
        public override string WeaponName => "Magic";
        public override string Icon => VanillaSprites.GuildedMagicUpgradeIcon;
        public override string Bonus => "Homes in on Bloons";
        public override string[] Combos => ["Missile", "Ice"];
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float[] StartingValues => [3, 1.1f, 1];
        public override string Range => "Mid-Range";
        public override int[] PierceValue => [1];
    }
}