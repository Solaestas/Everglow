using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items
{
	public class OlivineLock : ModItem
	{
		public override void SetStaticDefaults()
		{
			// // base.DisplayName.SetDefault("Turquoise Gem Lock");
			// base.Tooltip.SetDefault("Right click to place or remove Large Turquoises");
			// base.// DisplayName.AddTranslation(GameCulture.English, "橄榄石宝石锁");
			base.Tooltip.AddTranslation(GameCulture.English, "右键镶嵌或取下大橄榄石宝石锁");
		}
		public override void SetDefaults()
		{
			base.Item.width = 26;
			base.Item.height = 26;
			base.Item.maxStack = 99;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 14;
			base.Item.useTime = 10;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
			base.Item.createTile = base.Mod.Find<ModTile>("橄榄石宝石锁s").Type;
			base.Item.flame = true;
			base.Item.value = 500;
		}
	}
}
