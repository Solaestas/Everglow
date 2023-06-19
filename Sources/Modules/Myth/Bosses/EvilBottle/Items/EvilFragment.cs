using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace Everglow.Myth.Bosses.EvilBottle.Items
{
    public class EvilFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("封印碎片");
            // base.Tooltip.SetDefault("这是封印碎片堆里的最后一块碎块,放回碎片堆以修复一件上古魔器\n召唤封魔石瓶");
        }
        public override void SetDefaults()
        {
            base.Item.width = 32;
            base.Item.height = 30;
            base.Item.useAnimation = 45;
            base.Item.useTime = 60;
            base.Item.useStyle = 4;
            Item.maxStack = 20;
            base.Item.consumable = true;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<EvilBottle>());
        }
        public override void AddRecipes()
        {
			CreateRecipe()
			.AddIngredient(ItemID.Marble, 50)
			.AddIngredient(ItemID.StrangeBrew, 1)
			.AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
