using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace SpaceMarine;

public class EquipedModifierLevel : BloonsTD6Mod
{
    public static void LevelEquipedModifier(ModifierTemplate modifier, Tower tower)
    {
        if (modifier.ModName == "MOAB Damage")
        {
            MoabEquiped.Level(modifier, tower);
        }

        if (modifier.ModName == "Sapper")
        {
            SapperEquiped.Level(modifier, tower);
        }

        if (modifier.ModName == "Rapid Fire")
        {
            RapidEquiped.Level(modifier, tower);
        }

        if (modifier.ModName == "Slowdown")
        {
            SlowdownEquiped.Level(modifier, tower);
        }

        if (modifier.ModName == "Eraser")
        {
            EraserEquiped.Level(modifier, tower);
        }

        if (modifier.ModName == "Charm")
        {
            CharmEquiped.Level(modifier, tower);
        }
    }
}