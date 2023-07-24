using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items
{
	public class OlivineLock : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Turquoise Gem Lock");
			base.Tooltip.SetDefault("Right click to place or remove Large Turquoises");
			base.DisplayName.AddTranslation(GameCulture.English, "橄榄石宝石锁");
			base.Tooltip.AddTranslation(GameCulture.English, "右键镶嵌或取下大橄榄石宝石锁");
		}
		public override void SetDefaults()
		{
			base.item.width = 26;
			base.item.height = 26;
			base.item.maxStack = 99;
			base.item.useTurn = true;
			base.item.autoReuse = true;
			base.item.useAnimation = 14;
			base.item.useTime = 10;
			base.item.useStyle = 1;
			base.item.consumable = true;
			base.item.createTile = base.mod.TileType("橄榄石宝石锁s");
			base.item.flame = true;
			base.item.value = 500;
		}
	}
}
