using BTD_Mod_Helper;

namespace SpaceMarine;

public class EraserLeveling : BloonsTD6Mod
{
    public static void Levels(ModifierTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 1;
        }
    }
}