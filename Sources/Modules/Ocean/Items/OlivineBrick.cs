using System;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Everglow.Ocean.Items.Bricks
{
    public class OlivineBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			// // base.DisplayName.SetDefault("");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石晶莹宝石块");
		}
		public override void SetDefaults()
		{
			base.Item.width = 24;
			base.Item.height = 24;
			base.Item.maxStack = 999;
			base.Item.value = 0;
			base.Item.rare = 0;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 15;
			base.Item.useTime = 10;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
            base.Item.createTile = base.Mod.Find<ModTile>("橄榄石晶莹宝石块").Type;
		}
        public override void PostUpdate()
        {
            Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 0.25f, 0.65f, 0f);
        }
    }
}
