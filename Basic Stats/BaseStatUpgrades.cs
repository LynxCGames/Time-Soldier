using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using UnityEngine;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppSystem.Linq;
using System.Linq;
using MelonLoader;

namespace SpaceMarine;

public class BaseStatUpgrades : BloonsTD6Mod
{
    public static void PierceUpgrade(RectTransform rect, Tower tower, ModHelperText scrap)
    {
        if (mod.scrap >= mod.pierceCost && mod.pierceLvl < 5)
        {
            if (mod.weapon != "")
            {
                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

                foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
                {
                    if (weapon.WeaponName == mod.weapon)
                    {
                        ProjectileModel[] projectiles = towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ToArray();

                        for (int i = 0; i < weapon.PierceValue.Count(); i++)
                        {/*
                            MelonLogger.Msg(projectiles[i].name);
                            MelonLogger.Msg(projectiles[i].pierce);
                            MelonLogger.Msg(weapon.PierceValue[i]);*/
                            projectiles[i].pierce += weapon.PierceValue[i];
                            //MelonLogger.Msg(projectiles[i].pierce);
                            //MelonLogger.Msg("");
                        }
                    }
                }

                foreach (var weapon in ModContent.GetContent<ComboTemplate>())
                {
                    if (weapon.WeaponName == mod.weapon)
                    {
                        ProjectileModel[] projectiles = towerModel.GetAttackModel().GetDescendants<ProjectileModel>().ToArray();

                        for (int i = 0; i < weapon.PierceValue.Count(); i++)
                        {/*
                            MelonLogger.Msg(projectiles[i].name);
                            MelonLogger.Msg(projectiles[i].pierce);
                            MelonLogger.Msg(weapon.PierceValue[i]);*/
                            projectiles[i].pierce += weapon.PierceValue[i];
                            //MelonLogger.Msg(projectiles[i].pierce);
                            //MelonLogger.Msg("");
                        }

                        //MelonLogger.Msg("\n");
                    }
                }

                tower.UpdateRootModel(towerModel);
            }

            mod.pierceLvl++;
            mod.scrap -= mod.pierceCost;
            mod.usedScrap += mod.pierceCost;
            mod.pierceCost = Mathf.Round(mod.pierceCost *= 1.5f);
            scrap.Text.text = $"{mod.scrap}";

            if (mod.pierceLvl >= 5)
            {
                MenuUi.pierce.GetComponent<ModHelperText>().Text.text = "Maxed";
                MenuUi.pierce.GetComponent<ModHelperText>().Text.fontSize = 40;
            }
            else
            {
                MenuUi.pierce.GetComponent<ModHelperText>().Text.text = $"{mod.pierceCost}";
            }
        }
    }

    public static void SpeedUpgrade(RectTransform rect, Tower tower, ModHelperText scrap)
    {
        if (mod.scrap >= mod.speedCost && mod.speedLvl < 5)
        {
            if (mod.weapon != "")
            {
                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
                towerModel.GetAttackModel().GetDescendants<WeaponModel>().ForEach(model => model.rate /= 1.06f);
                tower.UpdateRootModel(towerModel);
            }

            mod.speedLvl++;
            mod.scrap -= mod.speedCost;
            mod.usedScrap += mod.speedCost;
            mod.speedCost = Mathf.Round(mod.speedCost *= 1.5f);
            scrap.Text.text = $"{mod.scrap}";

            if (mod.speedLvl >= 5)
            {
                MenuUi.speed.GetComponent<ModHelperText>().Text.text = "Maxed";
                MenuUi.speed.GetComponent<ModHelperText>().Text.fontSize = 40;
            }
            else
            {
                MenuUi.speed.GetComponent<ModHelperText>().Text.text = $"{mod.speedCost}";
            }
        }
    }

    public static void RangeUpgrade(RectTransform rect, Tower tower, ModHelperText scrap)
    {
        if (mod.scrap >= mod.rangeCost && mod.rangeLvl < 5)
        {
            if (mod.weapon != "")
            {
                foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
                {
                    SelectingStat.WeaponRangeSelect(tower, weapon);
                }

                foreach (var weapon in ModContent.GetContent<ComboTemplate>())
                {
                    SelectingStat.ComboRangeSelect(tower, weapon);
                }
            }

            mod.rangeLvl++;
            mod.scrap -= mod.rangeCost;
            mod.usedScrap += mod.rangeCost;
            mod.rangeCost = Mathf.Round(mod.rangeCost *= 1.5f);
            scrap.Text.text = $"{mod.scrap}";

            if (mod.rangeLvl >= 5)
            {
                MenuUi.range.GetComponent<ModHelperText>().Text.text = "Maxed";
                MenuUi.range.GetComponent<ModHelperText>().Text.fontSize = 40;
            }
            else
            {
                MenuUi.range.GetComponent<ModHelperText>().Text.text = $"{mod.rangeCost}";
            }
        }
    }

    public static void DamageUpgrade(RectTransform rect, Tower tower, ModHelperText scrap)
    {
        if (mod.scrap >= mod.damageCost && mod.damageLvl < 4)
        {
            if (mod.weapon != "")
            {
                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
                towerModel.GetAttackModel().GetDescendants<DamageModel>().ForEach(model => model.damage += 1);

                if (mod.weapon == "Graviton")
                {
                    towerModel.GetDescendant<CreateTowerModel>().tower.GetDescendants<DamageModel>().ForEach(model => model.damage += 1);
                }

                tower.UpdateRootModel(towerModel);
            }

            mod.damageLvl++;
            mod.scrap -= mod.damageCost;
            mod.usedScrap += mod.damageCost;
            mod.damageCost = Mathf.Round(mod.damageCost *= 1.5f);
            scrap.Text.text = $"{mod.scrap}";

            if (mod.damageLvl >= 4)
            {
                MenuUi.damage.GetComponent<ModHelperText>().Text.text = "Maxed";
                MenuUi.damage.GetComponent<ModHelperText>().Text.fontSize = 40;
            }
            else
            {
                MenuUi.damage.GetComponent<ModHelperText>().Text.text = $"{mod.damageCost}";
            }
        }
    }

    public static void CamoUpgrade(RectTransform rect, Tower tower, ModHelperText scrap)
    {
        if (mod.scrap >= mod.camoCost && mod.camoActive == false)
        {
            if (mod.weapon != "")
            {
                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

                if (mod.weapon == "Graviton")
                {
                    towerModel.GetDescendant<CreateTowerModel>().tower.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }

                tower.UpdateRootModel(towerModel);
            }

            mod.camoActive = true;
            mod.scrap -= mod.camoCost;
            mod.usedScrap += mod.camoCost;
            scrap.Text.text = $"{mod.scrap}";

            MenuUi.camo.GetComponent<ModHelperText>().Text.text = "Maxed";
            MenuUi.camo.GetComponent<ModHelperText>().Text.fontSize = 40;
        }
    }

    public static void MibUpgrade(RectTransform rect, Tower tower, ModHelperText scrap)
    {
        if (mod.scrap >= mod.mibCost && mod.mibActive == false)
        {
            if (mod.weapon != "")
            {
                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
                towerModel.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);

                if (mod.weapon == "Forest Spirit")
                {
                    towerModel.GetBehavior<SpiritOfTheForestModel>().GetDescendant<DamageOverTimeCustomModel>().immuneBloonProperties = BloonProperties.None;
                }
                if (mod.weapon == "Graviton")
                {
                    towerModel.GetDescendant<CreateTowerModel>().tower.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                }

                tower.UpdateRootModel(towerModel);
            }

            mod.mibActive = true;
            mod.scrap -= mod.mibCost;
            mod.usedScrap += mod.mibCost;
            scrap.Text.text = $"{mod.scrap}";

            MenuUi.mib.GetComponent<ModHelperText>().Text.text = "Maxed";
            MenuUi.mib.GetComponent<ModHelperText>().Text.fontSize = 40;
        }
    }
}
