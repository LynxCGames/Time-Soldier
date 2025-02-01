using BTD_Mod_Helper;

namespace SpaceMarine;

public class MoabLeveling : BloonsTD6Mod
{
    public static void Levels(ModifierTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            modifier.bonus += 0.5f;
        }
    }
}