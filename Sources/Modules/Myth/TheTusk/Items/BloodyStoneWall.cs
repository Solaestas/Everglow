using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items
{
	public class BloodyStoneWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bloody Stone Wall");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血石砖墙");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Walls.BloodyStoneWall>();

		}
	}
}
