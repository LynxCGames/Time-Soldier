using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace SpaceMarine;

public class ModifierLeveling : BloonsTD6Mod
{
    public static void ModifierLevels(ModifierTemplate modifier, Tower tower)
    {
        if (modifier.ModName == "MOAB Damage")
        {
            MoabLeveling.Levels(modifier);
        }

        if (modifier.ModName == "Sapper")
        {
            SapperLeveling.Levels(modifier);
        }

        if (modifier.ModName == "Rapid Fire")
        {
            RapidLeveling.Levels(modifier);
        }

        if (modifier.ModName == "Slowdown")
        {
            SlowdownLeveling.Levels(modifier);
        }

        if (modifier.ModName == "Eraser")
        {
            EraserLeveling.Levels(modifier);
        }

        if (modifier.ModName == "Charm")
        {
            CharmLeveling.Levels(modifier);
        }
    }
}