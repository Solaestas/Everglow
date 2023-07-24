using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items
{
    public class OceanGlass : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Shining Jelly Brick");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "幻海琉璃瓦");
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
            base.item.createTile = base.mod.TileType("幻海琉璃瓦");
		}
	}
}
