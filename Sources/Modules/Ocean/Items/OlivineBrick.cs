using System;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MythMod.Items.Bricks
{
    public class OlivineBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石晶莹宝石块");
		}
		public override void SetDefaults()
		{
			base.item.width = 24;
			base.item.height = 24;
			base.item.maxStack = 999;
			base.item.value = 0;
			base.item.rare = 0;
			base.item.useTurn = true;
			base.item.autoReuse = true;
			base.item.useAnimation = 15;
			base.item.useTime = 10;
			base.item.useStyle = 1;
			base.item.consumable = true;
            base.item.createTile = base.mod.TileType("橄榄石晶莹宝石块");
		}
        public override void PostUpdate()
        {
            Lighting.AddLight((int)((item.position.X + item.width / 2) / 16f), (int)((item.position.Y + item.height / 2) / 16f), 0.25f, 0.65f, 0f);
        }
    }
}
