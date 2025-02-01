using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace SpaceMarine;

public class SelectingModifier : BloonsTD6Mod
{
    public static void ModifierSelect(ModifierTemplate modifier, Tower tower)
    {
        if (modifier.ModName == "MOAB Damage")
        {
            MoabSelect.Select(modifier, tower);
        }

        if (modifier.ModName == "Sapper")
        {
            SapperSelect.Select(modifier, tower);
        }

        if (modifier.ModName == "Rapid Fire")
        {
            RapidSelect.Select(modifier, tower);
        }

        if (modifier.ModName == "Slowdown")
        {
            SlowdownSelect.Select(modifier, tower);
        }

        if (modifier.ModName == "Eraser")
        {
            EraserSelect.Select(modifier, tower);
        }

        if (modifier.ModName == "Charm")
        {
            CharmSelect.Select(modifier, tower);
        }
    }
}