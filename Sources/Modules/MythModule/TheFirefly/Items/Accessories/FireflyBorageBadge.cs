using Everglow.Sources.Modules.MythModule.TheFirefly.NPCs.Bosses;
using Terraria.Localization;
using Terraria.ID;
using Everglow.Sources.Modules.MythModule.TheFirefly.Items.Weapons;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Accessories
{
    public class FireflyBorageBadge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.value = 3868;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) *= 1.06f;
            player.moveSpeed *= 1.15f;
            if (CorruptMoth.CorruptMothNPC != null && CorruptMoth.CorruptMothNPC.active)
            {
                if (player.statDefense <= 48)
                {
                    player.statDefense -= (player.statDefense / 4); //48 / 4, would be player.statDefense / 4, up to a value change of -12.
                }
                else
                {
                    player.statDefense -= 12;
                }
                player.GetDamage(DamageClass.Generic) *= 1.18f;
                player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 1.1f;
                player.GetAttackSpeed(DamageClass.Melee) *= 1.1f;
                if (player.GetCritChance(DamageClass.Melee) > 0)
                {
                    player.GetTotalCritChance(DamageClass.Melee);
                    player.GetCritChance(DamageClass.Melee) = 0;
                }
                if (player.GetCritChance(DamageClass.MeleeNoSpeed) > 0)
                {
                    player.GetTotalCritChance(DamageClass.MeleeNoSpeed);
                    player.GetCritChance(DamageClass.MeleeNoSpeed) = 0;
                }
                if (player.GetCritChance(DamageClass.SummonMeleeSpeed) > 0)
                {
                    player.GetTotalCritChance(DamageClass.SummonMeleeSpeed);
                    player.GetCritChance(DamageClass.SummonMeleeSpeed) = 0;
                    player.GetCritChance(DamageClass.SummonMeleeSpeed) *= 0;
                }
            } // Does anyone know how to set all melee weapon crit chances to 0? I can set them to negatives but it would look weird. ~Setnour6
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            {
                if (CorruptMoth.CorruptMothNPC != null && CorruptMoth.CorruptMothNPC.active)
                {
                    tooltips.AddRange(new TooltipLine[]
                    {
                       new(Everglow.Instance, "FFBBadge0", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.FFBBadge0")),
                       new(Everglow.Instance, "FFBBadge1", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.FFBBadge1")),
                       new(Everglow.Instance, "FFBBadge2", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.FFBBadge2")),
                       new(Everglow.Instance, "FFBBadge3", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.FFBBadge3")),
                       new(Everglow.Instance, "FFBBadge4", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.FFBBadge4")),
                    });
                }
                tooltips.Add(new TooltipLine(Everglow.Instance, "UnfinishedItem", Language.GetTextValue("Mods.Everglow.ExtraTooltip.Misc.UnfinishedItem")));
            }
            base.ModifyTooltips(tooltips);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PlantAndFarmModule.Items.Materials.WindMoveSeed>(), 8) // 15
                .AddIngredient(ModContent.ItemType<BlackStarShrub>(), 24)
                .AddIngredient(ModContent.ItemType<GlowingPedal>(), 6)
                .AddTile(TileID.LivingLoom)
                .Register();
        }
        
    }
}