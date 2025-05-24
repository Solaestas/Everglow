using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class ShieldTile : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = false;

		Main.tileBlendAll[Type] = true;
		Main.tileBlockLight[Type] = true;

		DustType = DustID.WhiteTorch;
		HitSound = default;

		AddMapEntry(new Color(200, 200, 200));
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		resetFrame = false;
		noBreak = true;
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		Tile tile = Main.tile[i, j];
		if (tile != null)
		{
			if(tile.TileFrameX > 0)
			{
				tile.TileFrameX -= 18;
			}
			else if(tile.TileFrameY > 0)
			{
				tile.TileFrameY -= 18;
				tile.TileFrameX += 342;
			}
			else
			{
				tile.TileFrameX = 0;
				tile.TileFrameY = 0;
			}
		}
		base.NearbyEffects(i, j, closer);
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) => base.PreDraw(i, j, spriteBatch);

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		var offset = new Vector2(Main.offScreenRange);
		if(Main.drawToScreen)
		{
			offset = Vector2.zeroVector;
		}
		var drawPos = new Point(i, j).ToWorldCoordinates() + offset - Main.screenPosition;
		Texture2D tex = ModAsset.ShieldTile.Value;
		spriteBatch.Draw(tex, drawPos, new Rectangle(tile.TileFrameX, tile.TileFrameY + 54, 16, 16), new Color(0.1f, 0.1f, 0.3f, 0), 0, new Vector2(8), 1f, SpriteEffects.None, 0);
		if(Main.netMode != NetmodeID.Server)
		{
			Player player = Main.LocalPlayer;
			if (player.Bottom.ToTileCoordinates().X == i && player.Bottom.ToTileCoordinates().Y == j - 1)
			{
				spriteBatch.Draw(tex, drawPos, new Rectangle(tile.TileFrameX, tile.TileFrameY + 54, 16, 16), new Color(1f, 1f, 1f, 0), 0, new Vector2(8), 1f, SpriteEffects.None, 0);
			}
		}
		base.PostDraw(i, j, spriteBatch);
	}
}