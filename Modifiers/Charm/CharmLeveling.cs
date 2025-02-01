using BTD_Mod_Helper;

namespace SpaceMarine;

public class CharmLeveling : BloonsTD6Mod
{
    public static void Levels(ModifierTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            if (modifier.level > 5)
            {
                modifier.bonus += 2;
            }
            else
            {
                modifier.bonus += 1;
            }
        }
    }
}