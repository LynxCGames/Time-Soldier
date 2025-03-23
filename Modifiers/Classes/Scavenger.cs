using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Bloons;
using HarmonyLib;
using static SpaceMarine.SpaceMarine;
using Il2CppAssets.Scripts.Simulation.Towers;
using MelonLoader;

namespace SpaceMarine;

public class ScavengerModLevel : ScavengerLevel
{
    public override string ModName => "Scavenger";
    public override void Level(ScavengerTemplate modifier, Tower tower)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            if (modifier.level < 3)
            {
                modifier.bonus += 1;
                SpaceMarine.mod.scavenger += 1;
            }
            else
            {
                modifier.bonus += 2;
                SpaceMarine.mod.scavenger += 2;
            }
        }
    }
}

[HarmonyPatch(typeof(BloonManager), nameof(BloonManager.BloonSpawned))]
public static class BloonManagerPatch
{
    [HarmonyPostfix]
    public static void Postfix(Bloon bloon)
    {
        bloon.add_OnBloonDegrade((System.Action<Bloon, BloonDegradeStepper.DamageResult>)OnBloonDegrade);
        return;

        void OnBloonDegrade(Bloon bloon, BloonDegradeStepper.DamageResult result)
        {
            if (mod.modifier1 == "Scavenger" || mod.modifier2 == "Scavenger" || mod.modifier3 == "Scavenger")
            {
                Il2CppSystem.Random rnd = new();
                var num = rnd.Next(1, 100);
                //MelonLogger.Msg(num);

                if (num <= mod.scavenger)
                {
                    mod.scrap += 3;
                }

                if (mod.isSelected == true && mod.almanacOpen == false)
                {
                    MenuUi.scrap.GetComponent<ModHelperText>().Text.text = $"{mod.scrap}";
                }
            }
        }
    }
}
/*
[HarmonyPatch(typeof(Bloon), nameof(Bloon.Damage))]
internal static class Scavenger_ScrapBonus
{
    [HarmonyPostfix]
    private static void Bloon_Damage(Bloon __instance)
    {
        if (mod.modifier1 == "Scavenger" || mod.modifier2 == "Scavenger" || mod.modifier3 == "Scavenger")
        {
            Il2CppSystem.Random rnd = new();
            var num = rnd.Next(1, 100);

            if (num <= mod.scavenger)
            {
                mod.scrap += 3;
            }

            if (mod.isSelected == true && mod.almanacOpen == false)
            {
                MenuUi.scrap.GetComponent<ModHelperText>().Text.text = $"{mod.scrap}";
            }
        }
    }
}*/