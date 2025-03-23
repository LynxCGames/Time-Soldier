using BTD_Mod_Helper.Api.Enums;

namespace SpaceMarine
{
    public class BurstFire : SpecialTemplate
    {
        public override string ModName => "Burst Fire";
        public override string Icon => VanillaSprites.EvenFasterFiringUpgradeIcon;
        public override string Effect => "Fires in bursts";
        public override string Weapon => "Crossbow";
        public override string WeaponIcon => VanillaSprites.CrossBowMasterUpgradeIcon;
        public override int MaxLevel => 6;
        public override float CostMultiplier => 1.8f;
        public override float StartingCost => 30;
        public override string Stat => "Count";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 3;
    }

    public class Sharpshooter : SpecialTemplate
    {
        public override string ModName => "Sharpshooter";
        public override string Icon => VanillaSprites.DeadlyPrecisionUpgradeIcon;
        public override string Effect => "Fires crit bolts for more damage";
        public override string Weapon => "Crossbow";
        public override string WeaponIcon => VanillaSprites.CrossBowMasterUpgradeIcon;
        public override int MaxLevel => 8;
        public override float CostMultiplier => 1.5f;
        public override float StartingCost => 24;
        public override string Stat => "Multiplier";
        public override float FontSize => 35;
        public override float NameSize => 50;
        public override float StartingValue => 3;
    }

    public class Incendiary : SpecialTemplate
    {
        public override string ModName => "Incendiary";
        public override string Icon => VanillaSprites.WallOfFireUpgradeIcon;
        public override string Effect => "Creates a wall of fire";
        public override string Weapon => "Missile";
        public override string WeaponIcon => VanillaSprites.MissileLauncherUpgradeIcon;
        public override int MaxLevel => 8;
        public override float CostMultiplier => 1.5f;
        public override float StartingCost => 21;
        public override string Stat => "Damage";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 1;
    }

    public class ClusterBomb : SpecialTemplate
    {
        public override string ModName => "Cluster Bomb";
        public override string Icon => VanillaSprites.ClusterBombsUpgradeIcon;
        public override string Effect => "Bombs inside bombs";
        public override string Weapon => "Missile";
        public override string WeaponIcon => VanillaSprites.MissileLauncherUpgradeIcon;
        public override int MaxLevel => 6;
        public override float CostMultiplier => 1.8f;
        public override float StartingCost => 28;
        public override string Stat => "Count";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 6;
    }

    public class Icicles : SpecialTemplate
    {
        public override string ModName => "Icicles";
        public override string Icon => VanillaSprites.IciclesUpgradeIcon;
        public override string Effect => "Fires icicles in all directions";
        public override string Weapon => "Ice";
        public override string WeaponIcon => VanillaSprites.PermafrostUpgradeIcon;
        public override int MaxLevel => 8;
        public override float CostMultiplier => 1.5f;
        public override float StartingCost => 18;
        public override string Stat => "Damage";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 1;
    }

    public class Snowstorm : SpecialTemplate
    {
        public override string ModName => "Snowstorm";
        public override string Icon => VanillaSprites.SnowstormUpgradeIcon;
        public override string Effect => "Creates a slowing aura";
        public override string Weapon => "Ice";
        public override string WeaponIcon => VanillaSprites.PermafrostUpgradeIcon;
        public override int MaxLevel => 6;
        public override float CostMultiplier => 1.8f;
        public override float StartingCost => 26;
        public override string Stat => "Slow %";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 20;
    }

    public class PiercingShot : SpecialTemplate
    {
        public override string ModName => "Piercing Shot";
        public override string Icon => VanillaSprites.PlasmaAcceleratorUpgradeIcon;
        public override string Effect => "Occassionally fires upgraded shot";
        public override string Weapon => "Laser";
        public override string[] LaserWeapons => ["Laser", "Railgun", "Precision Laser", "Elite Laser"];
        public override string WeaponIcon => VanillaSprites.LaserCannonUpgradeIcon;
        public override int MaxLevel => 6;
        public override float CostMultiplier => 1.8f;
        public override float StartingCost => 28;
        public override string Stat => "Shots";
        public override float FontSize => 30;
        public override float NameSize => 60;
        public override float StartingValue => 10;
    }

    public class Refraction : SpecialTemplate
    {
        public override string ModName => "Refraction";
        public override string Icon => VanillaSprites.LaserShockUpgradeIcon;
        public override string Effect => "Splits after hitting a Bloon";
        public override string Weapon => "Laser";
        public override string WeaponIcon => VanillaSprites.LaserCannonUpgradeIcon;
        public override int MaxLevel => 8;
        public override float CostMultiplier => 1.5f;
        public override float StartingCost => 24;
        public override string Stat => "Damage";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 1;
    }

    public class FlameSpreader : SpecialTemplate
    {
        public override string ModName => "Flame Spreader";
        public override string Icon => VanillaSprites.RingOfFireUpgradeIcon;
        public override string Effect => "Fires a random spread";
        public override string Weapon => "Fire";
        public override string WeaponIcon => VanillaSprites.FireballUpgradeIcon;
        public override int MaxLevel => 6;
        public override float CostMultiplier => 1.8f;
        public override float StartingCost => 22;
        public override string Stat => "Count";
        public override float FontSize => 40;
        public override float NameSize => 50;
        public override float StartingValue => 2;
    }

    public class Scorcher : SpecialTemplate
    {
        public override string ModName => "Scorcher";
        public override string Icon => VanillaSprites.FirestormAA;
        public override string Effect => "Increases burn potency";
        public override string Weapon => "Fire";
        public override string WeaponIcon => VanillaSprites.FireballUpgradeIcon;
        public override int MaxLevel => 8;
        public override float CostMultiplier => 1.4f;
        public override float StartingCost => 27;
        public override string Stat => "Tick Rate";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 0.9f;
    }

    public class VineGrowth : SpecialTemplate
    {
        public override string ModName => "Vine Growth";
        public override string Icon => VanillaSprites.DruidoftheJungleUpgradeIcon;
        public override string Effect => "Grabs Bloons with vines that damages passing Bloons";
        public override string Weapon => "Thorn";
        public override string WeaponIcon => VanillaSprites.ThornSwarmUpgradeIcon;
        public override int MaxLevel => 8;
        public override float CostMultiplier => 1.5f;
        public override float StartingCost => 24;
        public override string Stat => "Damage";
        public override float FontSize => 25;
        public override float NameSize => 60;
        public override float StartingValue => 1;
    }

    public class Rage : SpecialTemplate
    {
        public override string ModName => "Rage";
        public override string Icon => VanillaSprites.DruidofWrathUpgradeIcon;
        public override string Effect => "Gains attack speed while attacking";
        public override string Weapon => "Thorn";
        public override string WeaponIcon => VanillaSprites.ThornSwarmUpgradeIcon;
        public override int MaxLevel => 6;
        public override float CostMultiplier => 1.8f;
        public override float StartingCost => 27;
        public override string Stat => "Speed Bonus %";
        public override float FontSize => 33;
        public override float NameSize => 60;
        public override float StartingValue => 30;
    }

    public class Hex : SpecialTemplate
    {
        public override string ModName => "Hex";
        public override string Icon => VanillaSprites.SacrificialTotemAA;
        public override string Effect => "Deals damage over time";
        public override string Weapon => "Magic";
        public override string WeaponIcon => VanillaSprites.GuildedMagicUpgradeIcon;
        public override int MaxLevel => 8;
        public override float CostMultiplier => 1.5f;
        public override float StartingCost => 24;
        public override string Stat => "Damage";
        public override float FontSize => 40;
        public override float NameSize => 60;
        public override float StartingValue => 1;
    }

    public class ArcaneSpike : SpecialTemplate
    {
        public override string ModName => "Arcane Spike";
        public override string Icon => VanillaSprites.ArcaneMasteryUpgradeIcon;
        public override string Effect => "Magic bolts release lightning on hit";
        public override string Weapon => "Magic";
        public override string WeaponIcon => VanillaSprites.GuildedMagicUpgradeIcon;
        public override int MaxLevel => 6;
        public override float CostMultiplier => 1.9f;
        public override float StartingCost => 26;
        public override string Stat => "Strength";
        public override float FontSize => 32;
        public override float NameSize => 60;
        public override float StartingValue => 1;
    }
}