namespace Everglow.Myth.TheFirefly.Items
{
	public class GlowingPedal : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemGlowManager.AutoLoadItemGlow(this);
		}

		public override void SetDefaults()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.width = 42;
			Item.height = 26;
			Item.maxStack = 999;
		}
	}
}