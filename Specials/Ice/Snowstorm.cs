using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Sentries;
using Il2CppSystem.Linq;

namespace SpaceMarine;

public class SnowstormSelect : SpecialSelect
{
    public override string SpecialName => "Snowstorm";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Ice" || SpaceMarine.mod.weapon == "Icicle Impale" || SpaceMarine.mod.weapon == "Blizzard")
            {
                foreach (var display in Game.instance.model.GetTowerFromId("IceMonkey-030").GetBehaviors<DisplayModel>())
                {
                    if (display.name == "DisplayModel_")
                    {
                        var newDisplay = display.Duplicate();
                        newDisplay.name = "SnowstormTSModDisplay";
                        towerModel.AddBehavior(newDisplay);
                    }
                }

                var slowDisplay = Game.instance.model.GetTowerFromId("IceMonkey-030").GetBehavior<LinkDisplayScaleToTowerRangeModel>().Duplicate();
                slowDisplay.name = "SnowstormTSModScale";
                slowDisplay.baseTowerRange = 100f;
                slowDisplay.displayRadius = 20;

                var slowZone = Game.instance.model.GetTowerFromId("IceMonkey-030").GetBehavior<SlowBloonsZoneModel>().Duplicate();
                slowZone.name = "SnowstormTSModZone";
                slowZone.zoneRadius = towerModel.range;
                slowZone.speedScale = (100 - modifier.bonus) / 100;

                if (SpaceMarine.mod.camoActive == true)
                {
                    slowZone.GetDescendant<FilterInvisibleModel>().isActive = false;
                }

                towerModel.AddBehavior(slowDisplay);
                towerModel.AddBehavior(slowZone);
            }

            if (SpaceMarine.mod.weapon == "Icy Barrage")
            {
                var snowstorm = GetTowerModel<SnowstormTower>().Duplicate();
                snowstorm.GetBehavior<SlowBloonsZoneModel>().speedScale = (100 - modifier.bonus) / 100;

                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.
                    AddBehavior(new CreateTowerModel("SnowstormTSMod", snowstorm, 1, true, false, false, true, false));
            }

            if (SpaceMarine.mod.weapon == "Precision Laser")
            {
                var snowstorm = GetTowerModel<SnowstormTower>().Duplicate();
                snowstorm.range = 15;
                snowstorm.GetBehavior<SlowBloonsZoneModel>().zoneRadius = 15;
                snowstorm.GetBehavior<SlowBloonsZoneModel>().speedScale = (100 - modifier.bonus) / 100;

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateTowerModel("SnowstormTSMod", snowstorm, 1, true, false, false, true, false));

                if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot")
                {
                    PiercingShotMod.PiercingShot(towerModel);
                }
            }

            if (SpaceMarine.mod.weapon == "Arctic Knight")
            {
                var snowstorm = GetTowerModel<SnowstormTower>().Duplicate();
                snowstorm.range = 15;
                snowstorm.GetBehavior<SlowBloonsZoneModel>().zoneRadius = 15;
                snowstorm.GetBehavior<SlowBloonsZoneModel>().speedScale = (100 - modifier.bonus) / 100;

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateTowerModel("SnowstormTSMod", snowstorm, 1, true, false, false, true, false));
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class SnowstormLevel : SpecialLevel
{
    public override string SpecialName => "Snowstorm";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 6;
        }
    }
}

public class SnowstormEquip : SpecialEquiped
{
    public override string SpecialName => "Snowstorm";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Ice" || SpaceMarine.mod.weapon == "Icicle Impale" || SpaceMarine.mod.weapon == "Blizzard")
        {
            foreach (var behavior in towerModel.GetBehaviors<SlowBloonsZoneModel>())
            {
                if (behavior.name.Contains("SnowstormTSMod"))
                {
                    behavior.speedScale = (100 - modifier.bonus) / 100;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Icy Barrage")
        {
            foreach (var sentry in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateTowerModel>().ToArray())
            {
                if (sentry.name.Contains("SnowstormTSMod"))
                {
                    sentry.tower.GetBehavior<SlowBloonsZoneModel>().speedScale = (100 - modifier.bonus) / 100;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Precision Laser")
        {
            foreach (var sentry in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateTowerModel>().ToArray())
            {
                if (sentry.name.Contains("SnowstormTSMod"))
                {
                    sentry.tower.GetBehavior<SlowBloonsZoneModel>().speedScale = (100 - modifier.bonus) / 100;
                }
            }

            if (SpaceMarine.mod.modifier1 == "Piercing Shot" || SpaceMarine.mod.modifier2 == "Piercing Shot" || SpaceMarine.mod.modifier3 == "Piercing Shot")
            {
                PiercingShotMod.PiercingShot(towerModel);
            }
        }

        if (SpaceMarine.mod.weapon == "Arctic Knight")
        {
            foreach (var sentry in towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<CreateTowerModel>().ToArray())
            {
                if (sentry.name.Contains("SnowstormTSMod"))
                {
                    sentry.tower.GetBehavior<SlowBloonsZoneModel>().speedScale = (100 - modifier.bonus) / 100;
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}