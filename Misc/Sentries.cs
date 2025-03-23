using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Filters;

namespace Sentries;

public class SnowstormTower : ModTower
{
    public override string Portrait => VanillaSprites.SnowstormUpgradeIcon;
    public override string Name => "Snowstorm";
    public override TowerSet TowerSet => TowerSet.Support;
    public override string BaseTower => TowerType.DartMonkey;

    public override bool DontAddToShop => true;
    public override int Cost => 0;

    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 0;
    public override int BottomPathUpgrades => 0;

    public override string DisplayName => "Snowstorm";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.radius /= 10;
        towerModel.range = 24;

        foreach (var attack in towerModel.GetAttackModels())
        {
            towerModel.RemoveBehavior(attack);
        }

        foreach (var display in Game.instance.model.GetTowerFromId("IceMonkey-030").GetBehaviors<DisplayModel>())
        {
            if (display.name == "DisplayModel_")
            {
                var newDisplay = display.Duplicate();
                newDisplay.name = "SnowstormModDisplay";
                towerModel.AddBehavior(newDisplay);
            }
        }

        var slowDisplay = Game.instance.model.GetTowerFromId("IceMonkey-030").GetBehavior<LinkDisplayScaleToTowerRangeModel>().Duplicate();
        slowDisplay.name = "SnowstormModScale";
        slowDisplay.baseTowerRange = 100f;
        slowDisplay.displayRadius = 10;

        var slowZone = Game.instance.model.GetTowerFromId("IceMonkey-030").GetBehavior<SlowBloonsZoneModel>().Duplicate();
        slowZone.name = "SnowstormModZone";
        slowZone.zoneRadius = towerModel.range;

        towerModel.AddBehavior(slowDisplay);
        towerModel.AddBehavior(slowZone);

        towerModel.isSubTower = true;
        towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 1.5f, 5, false, false));
        towerModel.AddBehavior(new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_"));
    }

    public class SnowstormTowerDisplay : ModTowerDisplay<SnowstormTower>
    {
        public override float Scale => 1f;
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display.guidRef;

        public override bool UseForTower(int[] tiers)
        {
            return true;
        }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {

        }
    }
}

public class VortexTower : ModTower
{
    public override string Portrait => VanillaSprites.FxPortalIcon;
    public override string Name => "Vortex";
    public override TowerSet TowerSet => TowerSet.Support;
    public override string BaseTower => TowerType.DartMonkey;

    public override bool DontAddToShop => true;
    public override int Cost => 0;

    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 0;
    public override int BottomPathUpgrades => 0;

    public override string DisplayName => "Vortex";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.radius /= 10;
        towerModel.range = 35;

        foreach (var attack in towerModel.GetAttackModels())
        {
            towerModel.RemoveBehavior(attack);
        }
        towerModel.RemoveBehavior<CreateSoundOnTowerPlaceModel>();

        var displayEffect = Game.instance.model.GetTowerFromId("TranceTotem").GetAttackModel().weapons[0].GetBehavior<EjectEffectModel>().effectModel.Duplicate();
        var display = Game.instance.model.GetTowerFromId("TranceTotem").GetAttackModel().weapons[0].projectile.GetBehavior<DisplayModel>().Duplicate();
        display.display = displayEffect.assetId;
        towerModel.AddBehavior(new LinkDisplayScaleToTowerRangeModel("", displayEffect.assetId, 35, 28, ""));
        towerModel.AddBehavior(display);

        var damage = Game.instance.model.GetTowerFromId("IceMonkey").GetAttackModel().Duplicate();
        damage.range = towerModel.range;
        damage.weapons[0].projectile.radius = towerModel.range;
        damage.GetDescendants<FilterOutTagModel>().ForEach(model => model.tag = "");
        damage.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
        damage.weapons[0].projectile.RemoveBehavior<FreezeModel>();
        damage.weapons[0].projectile.pierce = 999;
        damage.weapons[0].RemoveBehavior<CreateSoundOnProjectileCreatedModel>();
        damage.weapons[0].rate = 0.5f;
        towerModel.AddBehavior(damage);

        var vortex = Game.instance.model.GetTowerFromId("TranceTotem").GetAttackModel().Duplicate();
        vortex.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = true);
        vortex.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("TranceTotem").GetAttackModel().weapons[0].GetBehavior<EjectEffectModel>().effectModel.assetId;
        vortex.weapons[0].GetBehavior<EjectEffectModel>().lifespan = 6;
        vortex.weapons[0].projectile.pierce = 10;
        vortex.weapons[0].projectile.GetBehavior<AgeModel>().Lifespan = 6;
        vortex.weapons[0].fireWithoutTarget = true;
        vortex.weapons[0].projectile.RemoveBehavior<RemoveBloonModifiersModel>();
        vortex.weapons[0].projectile.GetBehavior<TranceBloonModel>().orbitRadius = 32;
        vortex.weapons[0].startInCooldown = false;
        towerModel.AddBehavior(vortex);
                
        towerModel.isSubTower = true;
        towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 6f, 5, false, false));
        towerModel.AddBehavior(new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_"));
    }
    
    public class VortexTowerDisplay : ModTowerDisplay<VortexTower>
    {
        public override float Scale => 1f;
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display.guidRef;

        public override bool UseForTower(int[] tiers)
        {
            return true;
        }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {

        }
    }
}