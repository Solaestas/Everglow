using Everglow.Commons.CustomTiles;

namespace Everglow.Minortopography.Common.Elevator.Tiles;

public class PineWinch : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileBlendAll[Type] = true;
		Main.tileBlockLight[Type] = true;
		AddMapEntry(new Color(112, 75, 75));
		DustType = DustID.BorealWood;
	}
	public override bool CanExplode(int i, int j)
	{
		return true;
	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
		Tile thisTile = Main.tile[i, j];
		thisTile.TileFrameX = 0;
	}
	/// <summary>
	/// 被挖掉时清理电梯
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="fail"></param>
	/// <param name="effectOnly"></param>
	/// <param name="noItem"></param>
	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
		bool hasLift = false;
		for (int x = -2; x < 4; x++)
		{
			for (int y = 1; y < 16; y++)
			{
				if (Main.tile[i + x, j + y].HasTile)
					return;
			}
		}
		foreach (var dTile in ColliderManager.Instance.OfType<PineTreeLiftTile>())
		{
			Vector2 dTileCenter = dTile.Center;
			float dPosY = Math.Abs(dTileCenter.Y / 16f - j);
			//电梯至少要在绞盘下10格
			if (dTileCenter.X / 16f - i == 0 && dPosY > 10)
			{
				hasLift = true;
				//确保这个电梯的所有绞盘是自己,如果不是就手动生成电梯
				for (int y = 0; y < dTileCenter.Y / 16f - 20; y++)
				{
					int coordPosY = (int)(dTileCenter.Y / 16f) - y;
					Tile tile = Main.tile[i, coordPosY];
					if (coordPosY < j + 5)
						break;
					if (tile.TileType == Type)
						hasLift = false;
				}
			}
			if(hasLift)
			{
				dTile.Kill();
			}
		}
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		Tile thisTile = Main.tile[i, j];
		//只有帧位于18的整数倍时才会
		if (thisTile.TileFrameX % 18 == 0)
		{
			bool hasLift = false;
			for (int x = -2; x < 4; x++)
			{
				for (int y = 1; y < 16; y++)
				{
					if (Main.tile[i + x, j + y].HasTile)
						return;
				}
			}
			foreach (var dTile in ColliderManager.Instance.OfType<PineTreeLiftTile>())
			{
				Vector2 dTileCenter = dTile.Center;
				float dPosY = Math.Abs(dTileCenter.Y / 16f - j);
				//电梯至少要在绞盘下10格
				if (dTileCenter.X / 16f - i == 0 && dPosY > 10)
				{
					hasLift = true;
					//确保这个电梯的所有绞盘是自己,如果不是就手动生成电梯
					for (int y = 0; y < dTileCenter.Y / 16f - 20; y++)
					{
						int coordPosY = (int)(dTileCenter.Y / 16f) - y;
						Tile tile = Main.tile[i, coordPosY];
						if (coordPosY < j + 5)
							break;
						if (tile.TileType == Type)
							hasLift = false;
					}
				}
			}
			if (!hasLift)
			{
				ColliderManager.Instance.Add(new PineTreeLiftTile() { Position = new Vector2(i, j + 15) * 16 - new Vector2(48, 8) });
				thisTile.TileFrameX = 1;
			}
		}
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;

		Texture2D t = ModAsset.Tiles_PineLiftWinch.Value;
		Color c0 = Lighting.GetColor(i, j);

		spriteBatch.Draw(t, new Vector2(i * 16, j * 16) - Main.screenPosition + new Vector2(8, 6) + zero, null, c0, 0, t.Size() / 2f, 1, SpriteEffects.None, 0);
		base.PostDraw(i, j, spriteBatch);
	}
}
