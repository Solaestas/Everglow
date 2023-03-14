namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.LunarFlare.Walls
{
	public class NightEffectWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			AddMapEntry(new Color(0, 0, 0, 0));
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.rand.NextBool(12))
				Main.tile[i, j].WallType = 0;
			return true;
		}
	}
}