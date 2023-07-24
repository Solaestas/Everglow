using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Bricks
{
    public class VolcanoAsh : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Volcano ask");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "火山灰");
		}
		public override void SetDefaults()
		{
			base.item.width = 20;
			base.item.height = 20;
			base.item.maxStack = 999;
			base.item.value = 0;
			base.item.rare = 0;
			base.item.useTurn = true;
			base.item.autoReuse = true;
			base.item.useAnimation = 15;
			base.item.useTime = 10;
			base.item.useStyle = 1;
			base.item.consumable = true;
            base.item.createTile = base.mod.TileType("VolcanoAsh");
		}
	}
}
