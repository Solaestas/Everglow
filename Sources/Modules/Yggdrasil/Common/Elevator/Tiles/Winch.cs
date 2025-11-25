using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.Yggdrasil.Common.Elevator.Tiles;

public class Winch : WinchTile<YggdrasilElevator>
{
	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		blockDamaged = false;
		return false;
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}

	public override void PlaceInWorld(int i, int j, Item item)
	{
		Tile thisTile = Main.tile[i, j];
		thisTile.TileFrameX = 0;
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var zero = new Vector2(Main.offScreenRange);
		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}

		Texture2D t = ModAsset.Tiles_LiftWinch.Value;
		Color c0 = Lighting.GetColor(i, j);

		Rectangle frame = new Rectangle(0, 0, 38, 18);
		spriteBatch.Draw(t, new Vector2(i * 16, j * 16) - Main.screenPosition + new Vector2(8, 6)/* + new Vector2((int)Vdrag.X, (int)Vdrag.Y)*/ + zero, new Rectangle(0, 0, 38, 18), c0, 0, frame.Size() / 2f, 1, SpriteEffects.None, 0);
	}
}