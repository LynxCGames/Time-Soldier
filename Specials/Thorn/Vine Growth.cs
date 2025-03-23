using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Simulation.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Assets;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;

namespace SpaceMarine;

public class VineGrowthSelect : SpecialSelect
{
    public override string SpecialName => "Vine Growth";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        if (SpaceMarine.mod.modifier1 == modifier.ModName || SpaceMarine.mod.modifier2 == modifier.ModName || SpaceMarine.mod.modifier3 == modifier.ModName)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (SpaceMarine.mod.weapon == "Thorn")
            {
                var growth = Game.instance.model.GetTowerFromId("Druid-030").GetAttackModel(1).Duplicate();
                growth.name = "VineGrowthMod";
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = modifier.bonus;
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.RemoveBehavior<RemoveBloonModifiersModel>();
                growth.weapons[0].RemoveBehavior<PierceFromLivesGainedModel>();

                if (SpaceMarine.mod.camoActive == true)
                {
                    growth.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    growth.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                }

                towerModel.AddBehavior(growth);
            }

            if (SpaceMarine.mod.weapon == "Thorns of Wrath")
            {
                var slowModel = new SlowModel("", 0.5f, 3, "Slow:Weak", 999, null, true, false, null, false, false, false, 1);
                var slowModifier = new SlowModifierForTagModel("", "Moabs", "Slow:Weak", 0.75f, false, false, 3, false);

                var growth = Game.instance.model.GetTowerFromId("Druid-030").GetAttackModel(1).Duplicate();
                growth.name = "VineGrowthMod";
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = modifier.bonus;
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.RemoveBehavior<RemoveBloonModifiersModel>();
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new int[] { 0, -1 };
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(slowModel);
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(slowModifier);
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.ApplyDisplay<VineGrowthFire>();
                growth.weapons[0].RemoveBehavior<PierceFromLivesGainedModel>();

                if (SpaceMarine.mod.camoActive == true)
                {
                    growth.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    growth.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                }

                towerModel.AddBehavior(growth);
            }

            if (SpaceMarine.mod.weapon == "Blizzard")
            {
                var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().Duplicate();
                var effectModel = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();
                effect.effectModel = effectModel;

                var burst = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
                burst.GetDamageModel().damage = modifier.bonus;
                burst.GetDamageModel().immuneBloonProperties = BloonProperties.White | BloonProperties.Lead;
                burst.collisionPasses = new int[] { 0, -1 };
                burst.AddBehavior(new FreezeModel("", 0, 1.5f, "CryoIce:Regular:Freeze", 3, "Ice", true, new Il2CppAssets.Scripts.Models.Bloons.Behaviors.GrowBlockModel(""), null, 0, false, false, false));

                var growth = Game.instance.model.GetTowerFromId("Druid-030").GetAttackModel(1).Duplicate();
                growth.name = "VineGrowthMod";
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = modifier.bonus;
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.RemoveBehavior<RemoveBloonModifiersModel>();
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(effect);
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.ApplyDisplay<VineGrowthSnow>();
                growth.weapons[0].RemoveBehavior<PierceFromLivesGainedModel>();
                growth.weapons[0].AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].GetBehavior<CreateSoundOnProjectileCreatedModel>().Duplicate());

                if (SpaceMarine.mod.camoActive == true)
                {
                    growth.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    burst.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    growth.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                    burst.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                }

                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new CreateProjectileOnExpireModel("", burst, new SingleEmissionModel("", null), false));

                towerModel.AddBehavior(growth);
            }

            if (SpaceMarine.mod.weapon == "Forest Spirit")
            {
                var thorns = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                thorns.display = Game.instance.model.GetTowerFromId("Druid").GetAttackModel().weapons[0].projectile.display;
                thorns.pierce = 3;
                thorns.GetDamageModel().damage = 1;

                var growth = Game.instance.model.GetTowerFromId("Druid-030").GetAttackModel(1).Duplicate();
                growth.name = "VineGrowthMod";
                growth.weapons[0].rate *= 2;
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = modifier.bonus;
                growth.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.RemoveBehavior<RemoveBloonModifiersModel>();
                growth.weapons[0].RemoveBehavior<PierceFromLivesGainedModel>();

                growth.GetDescendants<FilterOutTagModel>().ForEach(model => model.tag = "Zomg");
                growth.weapons[0].projectile.AddFilter(new FilterOutTagModel("FilterOutTagModel_JungleVine", "Bad", System.Array.Empty<string>()));
                growth.weapons[0].projectile.AddFilter(new FilterOutTagModel("FilterOutTagModel_JungleVine", "Ddt", System.Array.Empty<string>()));
                growth.AddFilter(new FilterOutTagModel("FilterOutTagModel_JungleVine", "Bad", System.Array.Empty<string>()));
                growth.AddFilter(new FilterOutTagModel("FilterOutTagModel_JungleVine", "Ddt", System.Array.Empty<string>()));

                if (SpaceMarine.mod.camoActive == true)
                {
                    growth.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    thorns.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                }
                if (SpaceMarine.mod.mibActive == true)
                {
                    growth.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                    thorns.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
                }

                growth.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("", thorns, new ArcEmissionModel("", 8, 0, 360, null, false, false), false, false, false));

                towerModel.AddBehavior(growth);
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}

public class VineGrowthLevel : SpecialLevel
{
    public override string SpecialName => "Vine Growth";
    public override void Level(SpecialTemplate modifier)
    {
        modifier.level++;

        if (modifier.level <= modifier.MaxLevel)
        {
            if (modifier.level < 4)
            {
                modifier.bonus += 1;
            }
            else if (modifier.level >= 4)
            {
                modifier.bonus += 2;
            }
        }
    }
}

public class VineGrowthEquiped : SpecialEquiped
{
    public override string SpecialName => "Vine Growth";
    public override void EditTower(SpecialTemplate modifier, Tower tower)
    {
        var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

        if (SpaceMarine.mod.weapon == "Thorn" || SpaceMarine.mod.weapon == "Thorns of Wrath")
        {
            foreach (var attack in towerModel.GetAttackModels())
            {
                if (attack.name.Contains("VineGrowthMod"))
                {
                    attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        if (SpaceMarine.mod.weapon == "Blizzard")
        {
            foreach (var attack in towerModel.GetAttackModels())
            {
                if (attack.name.Contains("VineGrowthMod"))
                {
                    attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = modifier.bonus;
                    attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = modifier.bonus;
                }
            }
        }

        tower.UpdateRootModel(towerModel);
    }
}