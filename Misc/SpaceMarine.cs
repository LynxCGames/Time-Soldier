using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;

namespace SpaceMarineTower
{
    public class SpaceMarineMonkey : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Primary;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 0;
        public override string DisplayName => "Time Soldier";
        public override string Name => "SpaceMarineTower";
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;
        public override string Description => "";
        public override string Portrait => VanillaSprites.Dartmonkey005;
        public override string Icon => VanillaSprites.Dartmonkey005;
        public override bool IsValidCrosspath(int[] tiers) =>
            ModHelper.HasMod("UltimateCrosspathing") || base.IsValidCrosspath(tiers);

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.towerSelectionMenuThemeId = "SelectPointInput";
            towerModel.dontDisplayUpgrades = true;
            towerModel.GetAttackModel().RemoveWeapon(towerModel.GetAttackModel().weapons[0]);
            towerModel.GetAttackModel().name = "MainAttacks";
            towerModel.GetAttackModel().range = 40;
            towerModel.range = 40;
            towerModel.GetAttackModel().AddBehavior(new TargetFirstPrioCamoModel("", true, false));

            var crossbow = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].Duplicate();
            crossbow.projectile.GetDamageModel().damage = 2;
            crossbow.projectile.pierce = 3;
            crossbow.projectile.display = Game.instance.model.GetTowerFromId("DartMonkey-003").GetAttackModel().weapons[0].projectile.display;
            crossbow.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.1f;
            crossbow.rate = 0.9f;

            towerModel.GetAttackModel().AddWeapon(crossbow);
        }

        public class MarineDisplay : ModTowerDisplay<SpaceMarineMonkey>
        {
            public override float Scale => 1f;
            public override string BaseDisplay => GetDisplay(TowerType.DartMonkey, 0, 0, 5);

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {

            }
        }
    }
}
