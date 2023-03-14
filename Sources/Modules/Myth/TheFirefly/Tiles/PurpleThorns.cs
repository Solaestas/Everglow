namespace Everglow.Myth.TheFirefly.Tiles
{
	public class PurpleThorns : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			MinPick = 175;
			DustType = 191;
			ItemDrop = ModContent.ItemType<Items.GlowCrystal>();
			AddMapEntry(new Color(35, 9, 35));
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return true;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			base.PostDraw(i, j, spriteBatch);
		}
	}
}