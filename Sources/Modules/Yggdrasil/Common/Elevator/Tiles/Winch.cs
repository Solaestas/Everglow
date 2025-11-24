using Everglow.Commons.CustomTiles;
using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.Common.Elevator.Tiles;

public class Winch : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileBlendAll[Type] = true;
		Main.tileBlockLight[Type] = true;

		AddMapEntry(new Color(112, 75, 75));

		DustType = DustID.Iron;
	}

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

	public override void NearbyEffects(int i, int j, bool closer)
	{
		Tile tile = Main.tile[i, j];
		if(!CheckEmpty(i - 2, j + 1, 3, 3))
		{
			return;
		}
		if (!CheckEmpty(i - 2, j + 1, 3, 3))
		{
			return;
		}

		bool hasLift = false;
		foreach (var boxEntity in ColliderManager.Instance.OfType<YggdrasilElevator>())
		{
			if (boxEntity is YggdrasilElevator elevator)
			{
				if (elevator.WinchCoord == new Point(i, j))
				{
					hasLift = true;
					break;
				}
			}
		}
		if (!hasLift)
		{
			ColliderManager.Instance.Add(new YggdrasilElevator() { Position = new Vector2(i, j + 15) * 16 - new Vector2(48, 8), WinchCoord = new Point(i, j) });
			tile.TileFrameY = 0;
		}
		if (hasLift)
		{
			tile.TileFrameY = 18;
		}
	}

	public bool CheckEmpty(int x, int y, int width, int height)
	{
		for (int i = x; i < x + width; i++)
		{
			for (int j = y; j < y + height; j++)
			{
				if (TileUtils.SafeGetTile(i, j).HasTile)
				{
					return false;
				}
			}
		}
		return true;
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