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
            //if (CorruptMoth.CorruptMothNPC != null && CorruptMoth.CorruptMothNPC.active)
            //{
            player.statDefense -= 12;
            player.GetDamage(DamageClass.Generic) *= 1.18f;
            //}
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            {
                //tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "FireflyBorageBadgeText1", "[c/2888FE:]"));
                tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "FireflyBorageBadgeTextUnfinished", "[c/BA0022:This item is unfinished]"));
            }
            base.ModifyTooltips(tooltips);
        }

        /*    public override void AddRecipes()
            {
                CreateRecipe()
                    .AddIngredient(ModContent.ItemType<MiscItems.Materials.WindMoveSeed>(), 15)
                    .AddIngredient(ModContent.ItemType<BlackStarShrub>(), 24)
                    .AddTile(304)
                    .Register();
            }
        */
    }
}