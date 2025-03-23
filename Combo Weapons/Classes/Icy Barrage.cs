using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using UnityEngine;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace SpaceMarine;

public class IcyBarrage : ComboTemplate
{
    public override string WeaponName => "Icy Barrage";
    public override string Icon => VanillaSprites.ScatterMissileAA;
    public override string[] comboWeapons => ["Missile", "Ice"];
    public override string Bonus => "Aerial Bombardment – Launches 3 targeted aerial missiles";
    public override float FontSize => 60;
    public override float[] StartingValues => [6, 2.2f, 1];
    public override string Range => "Long-Range";
    public override int[] PierceValue => [0, 1];
    public override string SpecialMods =>
        "Fires 3 aerial missiles near the targeted Bloon that can freeze Bloons hit.\n\n" +
        " - Long-Range weapon\n" +
        " - Incenciary effect is created at points of impact\n" +
        " - Cluster Bomb effect is halved\n" +
        " - Cluster Bomb projectiles freeze Bloons\n" +
        " - Icicles are shot out from points of impact\n" +
        " - Icicle effect is reduced to 6 instead of 12\n" +
        " - Temporary snowstorm fields are created at points of impact";
}

public class IcyBarrageSelect : ComboSelect
{
    public override string WeaponName => "Icy Barrage";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        // Creating Attack Model
        var missiles = Game.instance.model.GetTowerFromId("Rosalia 3").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Duplicate();
        missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().maxProjectileCount = 3;
        missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.Black;
        missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.RemoveBehavior<SlowModel>();
        missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.RemoveBehavior<SlowModifierForTagModel>();
        missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.collisionPasses = new int[] { 0, -1 };
        missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.AddBehavior(new FreezeModel("", 0, 1.5f, "CryoIce:Regular:Freeze", 3, "Ice", true, new Il2CppAssets.Scripts.Models.Bloons.Behaviors.GrowBlockModel(""), null, 0, false, false, false));
        missiles.RemoveBehavior<AnimateAirUnitOnFireModel>();

        // Stat Setter
        missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        missiles.rate = weapon.speed;
        missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        // Basic Stat Adjusters
        towerModel.GetAttackModel().range = 40 + (SpaceMarine.mod.rangeLvl * 12);
        towerModel.range = 40 + (SpaceMarine.mod.rangeLvl * 12);

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            missiles.rate /= 1.06f;
        }

        if (SpaceMarine.mod.camoActive == true)
        {
            missiles.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        if (SpaceMarine.mod.mibActive == true)
        {
            missiles.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        }

        towerModel.GetAttackModel().weapons[0] = missiles;

        tower.UpdateRootModel(towerModel);
    }
}

public class IcyBarrageLevel : ComboLevel
{
    public override string WeaponName => "Icy Barrage";
    public override void Level(WeaponTemplate weapon1, ComboTemplate combo)
    {
        foreach (var weapon2 in ModContent.GetContent<WeaponTemplate>())
        {
            if (weapon1.WeaponName == combo.comboWeapons[0] && weapon2.WeaponName == combo.comboWeapons[1])
            {
                if (weapon1.isUnlocked == true && weapon2.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((weapon1.level + weapon2.level) / 2);

                    if (combo.level > 1)
                    {
                        var speed1 = (2 - weapon1.speed) / 3 + 1;
                        var speed2 = (2 - weapon2.speed) / 3 + 1;

                        combo.pierce = 6 + (int)Mathf.Round((weapon1.pierce + weapon2.pierce) / 6);
                        combo.speed = Mathf.Round((2.2f / speed1 / speed2) * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round((weapon1.damage + weapon2.damage) / 2);
                    }
                }
            }
            if (weapon1.WeaponName == combo.comboWeapons[1] && weapon2.WeaponName == combo.comboWeapons[0])
            {
                if (weapon1.isUnlocked == true && weapon2.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((weapon1.level + weapon2.level) / 2);

                    if (combo.level > 1)
                    {
                        var speed1 = (2 - weapon1.speed) / 3 + 1;
                        var speed2 = (2 - weapon2.speed) / 3 + 1;

                        combo.pierce = 6 + (int)Mathf.Round((weapon2.pierce + weapon1.pierce) / 6);
                        combo.speed = Mathf.Round((2.2f / speed2 / speed1) * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round((weapon2.damage + weapon1.damage) / 2);
                    }
                }
            }
        }
    }
}

public class IcyBarrageEquip : ComboEquiped
{
    public override string WeaponName => "Icy Barrage";
    public override void EditTower(ComboTemplate weapon, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.pierce = weapon.pierce + SpaceMarine.mod.pierceLvl;
        towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetDamageModel().damage = weapon.damage + SpaceMarine.mod.damageLvl;

        for (int i = 0; i < SpaceMarine.mod.speedLvl; i++)
        {
            towerModel.GetAttackModel().GetDescendants<WeaponModel>().ForEach(model => model.rate /= 1.06f);
        }

        foreach (var modifier in GetContent<ModifierTemplate>())
        {
            if (modifier.ModName == "Rapid Fire")
            {
                if (SpaceMarine.mod.modifier1 == "Rapid Fire" || SpaceMarine.mod.modifier2 == "Rapid Fire" || SpaceMarine.mod.modifier3 == "Rapid Fire")
                {
                    towerModel.GetAttackModel().GetDescendants<WeaponModel>().ForEach(model => model.rate /= (modifier.bonus / 100) + 1);
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}