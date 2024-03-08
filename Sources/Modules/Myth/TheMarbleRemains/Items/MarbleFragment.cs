using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Myth.TheMarbleRemains.Items
{
    public class MarbleFragment : ModItem
	{
		public override void SetStaticDefaults()
		{
            //base.DisplayName.SetDefault("MarbleFragment");
            //base.DisplayName.AddTranslation(GameCulture.Chinese, "碎片堆");
		}
		public override void SetDefaults()
		{
			base.Item.width = 70;
			base.Item.height = 30;
			base.Item.maxStack = 99;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
            Item.rare = 4;
            base.Item.useAnimation = 15;
			base.Item.useTime = 10;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
			base.Item.value = 500;
            base.Item.createTile = ModContent.TileType<Tiles.MarbleFragment>();
		}
	}
}
