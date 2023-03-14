namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items
{
	public class BlackStarShrub : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemGlowManager.AutoLoadItemGlow(this);
		}

		public override void SetDefaults()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.width = 32;
			Item.height = 24;
			Item.maxStack = 999;
			Item.value = 100;
			Item.rare = ItemRarityID.White;
		}
	}
}