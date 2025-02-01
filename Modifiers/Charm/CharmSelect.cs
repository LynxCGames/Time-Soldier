﻿using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using HarmonyLib;

namespace SpaceMarine;

public class CharmSelect : BloonsTD6Mod
{
    public static void Select(ModifierTemplate modifier, Tower tower)
    {
        if (mod.modifier1 == modifier.ModName || mod.modifier2 == modifier.ModName || mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            var targetSelect = Game.instance.model.GetTowerFromId("EngineerMonkey-025").GetAttackModel(1).GetBehavior<TargetSelectedPointModel>().Duplicate();
            var agemodel = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.GetBehavior<AgeModel>().Duplicate();
            var summon = Game.instance.model.GetTowerFromId("WizardMonkey-004").GetAttackModel(2).Duplicate();
            summon.name = "CharmMod";
            summon.weapons[0].emission = new NecromancerEmissionModel("BaseDeploy_", 3, 3, 1, 3, 1, 1, 0, null, null, null, 1, 1, 1, 1, 2);
            summon.weapons[0].rate = (float)(7 / (0.12f * (modifier.bonus - 1) + 1));
            summon.weapons[0].projectile.GetDamageModel().damage = (2 * modifier.bonus) - 1;
            summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            summon.weapons[0].projectile.pierce = modifier.bonus + 3;
            summon.range = towerModel.range;
            summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames = 0;
            summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespan = 15f;
            summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed = 25 + (modifier.bonus * 2);
            agemodel.lifespanFrames = 0;
            agemodel.lifespan = 15f;
            agemodel.rounds = 9999;
            summon.weapons[0].projectile.AddBehavior(agemodel);
            summon.weapons[0].projectile.display = modifier.CharmSprite[modifier.level - 1];
            summon.AddBehavior(targetSelect);

            towerModel.AddBehavior(summon);
            towerModel.GetAttackModel().GetDescendants<DamageModel>().ForEach(model => model.damage -= 1);
            tower.UpdateRootModel(towerModel);
        }
    }

    [HarmonyPatch(typeof(NecroData), nameof(NecroData.RbePool))]
    internal static class Necro_RbePool
    {
        [HarmonyPrefix]
        private static bool Postfix(NecroData __instance, ref int __result)
        {
            var towerModel = __instance.tower.towerModel;
            if (towerModel.name.Contains("SpaceMarine"))
            {
                if (mod.modifier1 == "Charm" || mod.modifier2 == "Charm" || mod.modifier3 == "Charm")
                {
                    __result = 9999;
                }
            }
            return false;
        }
    }
}