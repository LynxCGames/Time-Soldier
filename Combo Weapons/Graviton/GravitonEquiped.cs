using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation.Towers;
using static SpaceMarine.SpaceMarine;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

namespace SpaceMarine
{
    public class GravitonEquiped
    {
        public static void Level(ComboTemplate weapon, Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.pierce + mod.pierceLvl;
            towerModel.GetAttackModel().weapons[0].rate = weapon.speed;
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.damage + mod.damageLvl;

            for (int i = 0; i < mod.speedLvl; i++)
            {
                towerModel.GetAttackModel().weapons[0].rate /= 1.06f;
            }

            foreach (var modifier in ModContent.GetContent<ModifierTemplate>())
            {
                if (modifier.ModName == "Rapid Fire")
                {
                    if (mod.modifier1 == "Rapid Fire" || mod.modifier2 == "Rapid Fire" || mod.modifier3 == "Rapid Fire")
                    {
                        towerModel.GetAttackModel().weapons[0].rate /= (modifier.bonus / 100 + 1);
                    }
                }
            }

            if (mod.modifier1 == "Cluster Bomb" || mod.modifier2 == "Cluster Bomb" || mod.modifier3 == "Cluster Bomb")
            {
                foreach (var modifier in ModContent.GetContent<SpecialTemplate>())
                {
                    if (modifier.ModName == "Cluster Bomb")
                    {
                        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>())
                        {
                            if (behavior.name.Contains("ClusterBomb"))
                            {
                                behavior.projectile.GetDamageModel().damage = weapon.damage;
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}