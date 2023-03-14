namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items
{
	public class GlowingFirefly : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemGlowManager.AutoLoadItemGlow(this);
		}

		public override void SetDefaults()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.width = 32;
			Item.height = 22;
			Item.maxStack = 999;
			Item.bait = 42;
		}
	}
}