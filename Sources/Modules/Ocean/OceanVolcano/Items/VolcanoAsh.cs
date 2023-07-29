using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Bricks
{
    public class VolcanoAsh : ModItem
	{
		public override void SetStaticDefaults()
		{
			// // base.DisplayName.SetDefault("Volcano ask");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "火山灰");
		}
		public override void SetDefaults()
		{
			base.Item.width = 20;
			base.Item.height = 20;
			base.Item.maxStack = 999;
			base.Item.value = 0;
			base.Item.rare = 0;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 15;
			base.Item.useTime = 10;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
            base.Item.createTile = base.Mod.Find<ModTile>("VolcanoAsh").Type;
		}
	}
}
