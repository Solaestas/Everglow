using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Walls;

public class SharpBarnacleWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<SharpBarnacle_Dust>();
		HitSound = SoundID.Grass;
		AddMapEntry(new Color(94, 75, 10));
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		if (j % 2 == 0 && tile.WallFrameY < 180)
		{
			tile.WallFrameY += 180;
		}
		if (j % 2 == 1 && tile.WallFrameY >= 180)
		{
			tile.WallFrameY -= 180;
		}
		base.PostDraw(i, j, spriteBatch);
	}
}