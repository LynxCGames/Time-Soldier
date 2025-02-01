using BTD_Mod_Helper;
using System;
using BTD_Mod_Helper.Api;
using UnityEngine;

namespace SpaceMarine;

public class GravitonLeveling : BloonsTD6Mod
{
    public static void Levels(WeaponTemplate weapon1, ComboTemplate combo)
    {
        foreach (var weapon2 in ModContent.GetContent<WeaponTemplate>())
        {
            if (weapon1.WeaponName == "Missile" && weapon2.WeaponName == "Laser")
            {
                if (weapon1.isUnlocked == true && weapon2.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((weapon1.level + weapon2.level) / 2);

                    if (combo.level > 1)
                    {
                        var speed1 = (2 - weapon1.speed) / 4 + 1;
                        var speed2 = (1 - weapon2.speed) / 4 + 1;

                        combo.speed = MathF.Round((8f / speed1 / speed2) * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round((weapon1.damage + weapon2.damage) / 3);
                    }
                }
            }
            if (weapon1.WeaponName == "Laser" && weapon2.WeaponName == "Missile")
            {
                if (weapon1.isUnlocked == true && weapon2.isUnlocked == true)
                {
                    combo.isUnlocked = true;
                    combo.level = (int)Mathf.Round((weapon1.level + weapon2.level) / 2);

                    if (combo.level > 1)
                    {
                        var speed1 = (1 - weapon1.speed) / 4 + 1;
                        var speed2 = (2 - weapon2.speed) / 4 + 1;

                        combo.speed = MathF.Round((8f / speed2 / speed1) * 100) / 100;
                        combo.damage = 1 + (int)Mathf.Round((weapon2.damage + weapon1.damage) / 3);
                    }
                }
            }
        }
    }
}