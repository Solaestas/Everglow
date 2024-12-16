using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class FurnacePlatingTile : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = false;

		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		Main.tileShine2[Type] = false;

		DustType = ModContent.DustType<FurnacePlatingDust>();
		HitSound = default;

		AddMapEntry(new Color(116, 97, 97));
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		if(tile.Slope != SlopeType.Solid || tile.halfBrick())
		{
			return true;
		}
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var offsetScreen = new Vector2(Main.offScreenRange);
		if (Main.drawToScreen)
		{
			offsetScreen = Vector2.Zero;
		}
		Vector2 drawPos = new Point(i, j).ToWorldCoordinates() - Main.screenPosition + offsetScreen;
		Rectangle frame = new Rectangle(2 + (i % 15) * 16, 182 + (j % 15) * 16, 16, 16);
		spriteBatch.Draw(texture, drawPos, frame, Lighting.GetColor(i, j), 0, frame.Size() * 0.5f, 1, SpriteEffects.None, 0);
		Rectangle frameSide = new Rectangle(tile.TileFrameX, tile.TileFrameY + 90, 16, 16);
		spriteBatch.Draw(texture, drawPos, frameSide, Lighting.GetColor(i, j), 0, frameSide.Size() * 0.5f, 1, SpriteEffects.None, 0);
		return false;
	}

	public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
	{
	}
}