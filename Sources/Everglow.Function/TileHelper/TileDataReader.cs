using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.TileHelper;

/// <summary>
/// Visulalize the data of mouse-covered-tile.
/// </summary>
public class TileDataReader : ModItem
{
	public bool EnableResidentEffect = false;

	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.value = 0;
		Item.rare = ItemRarityID.White;
	}

	public override void HoldItem(Player player)
	{
		int i = Main.MouseWorld.ToTileCoordinates().X;
		int j = Main.MouseWorld.ToTileCoordinates().Y;
		if (!TileDataReaderSystem.OwnerPlayerWhoAmI.Contains(player.whoAmI))
		{
			TileDataReaderSystem.OwnerPlayerWhoAmI.Add(player.whoAmI);
			TileDataReaderSystem vfx = new TileDataReaderSystem { FixPoint = new Point(i, j), Active = true, Visible = true, EverLasting = EnableResidentEffect };
			Ins.VFXManager.Add(vfx);
		}

		// Right click to enable resident tile reader effect.
		if (Main.mouseRight && Main.mouseRightRelease && !Main.mapFullscreen)
		{
			EnableResidentEffect = !EnableResidentEffect;
			CombatText.NewText(player.Hitbox, Color.White, (EnableResidentEffect ? "Enable" : "Disable") + "everlasting tile reading effect.");
			if (TileDataReaderSystem.OwnerPlayerWhoAmI.Contains(player.whoAmI))
			{
				TileDataReaderSystem.OwnerPlayerWhoAmI.Remove(player.whoAmI);
			}
		}
	}
}

[Pipeline(typeof(WCSPipeline))]
public class TileDataReaderSystem : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PreDrawFilter;

	public Texture2D Texture;
	public Point FixPoint;
	public static List<int> OwnerPlayerWhoAmI = new List<int>();
	public List<Point> ContinueTiles = new List<Point>();
	public List<Point> CheckLiquidTiles = new List<Point>();
	public Point OldTilePos = new Point(0, 0);
	public bool EverLasting = false;
	public int MaxContinueCount = 625;

	public override void OnSpawn()
	{
		Texture = ModAsset.TileBlock.Value;
	}

	public override void Update()
	{
		FixPoint = Main.MouseWorld.ToTileCoordinates();
		int i = FixPoint.X;
		int j = FixPoint.Y;
		Player player = Main.LocalPlayer;
		if (!OwnerPlayerWhoAmI.Contains(player.whoAmI))
		{
			Active = false;
			return;
		}
		if (i < 20 || i > Main.maxTilesX - 20)
		{
			if (j < 20 || j > Main.maxTilesY - 20)
			{
				Active = false;
				OwnerPlayerWhoAmI.Remove(player.whoAmI);
				return;
			}
		}

		if (player.HeldItem.type != ModContent.ItemType<TileDataReader>() && !EverLasting)
		{
			Active = false;
			OwnerPlayerWhoAmI.Remove(player.whoAmI);
			return;
		}
		if (OldTilePos != new Point(i, j))
		{
			UpdateContinueTiles(i, j);
		}
		OldTilePos = new Point(i, j);
		base.Update();
	}

	public override void Draw()
	{
		int i = FixPoint.X;
		int j = FixPoint.Y;
		Player player = Main.LocalPlayer;
		if (i < 20 || i > Main.maxTilesX - 20 || j < 20 || j > Main.maxTilesY - 20)
		{
			Active = false;
			OwnerPlayerWhoAmI.Remove(player.whoAmI);
			return;
		}
		Tile tile = Main.tile[i, j];
		Ins.Batch.BindTexture<Vertex2D>(Texture);
		int colorType = ItemRarityID.White;
		Color drawColor = Color.White;
		if (!tile.HasTile)
		{
			colorType = ItemRarityID.Gray;
			drawColor = Color.Gray;
		}
		DrawBlockBound(i, j, drawColor);
		if (ContinueTiles.Count < MaxContinueCount)
		{
			Color drawContinueColor = new Color(0.12f, 0.24f, 0.4f, 0);
			foreach (Point point in ContinueTiles)
			{
				DrawBlockBound(point.X, point.Y, drawContinueColor);
			}
		}
		if (CheckLiquidTiles.Count < MaxContinueCount)
		{
			Color drawContinueColor = new Color(0.0f, 0.0f, 0.6f, 0);
			foreach (Point point in CheckLiquidTiles)
			{
				DrawBlockBound(point.X, point.Y, drawContinueColor);
			}
		}
		string datas = GetDatas(i, j);
		Main.instance.MouseText(datas, colorType);
	}

	public string GetDatas(int i, int j)
	{
		Tile tile = SafeGetTile(i, j);

		string datas = "\nCoordinate: [" + i + ", " + j + "]";
		datas += "\nHasTile: " + tile.HasTile;
		datas += "\nType :" + tile.TileType;
		if (tile.HasTile)
		{
			datas += "\nFrame : [" + tile.TileFrameX + ", " + tile.TileFrameY + "]";
			if (ContinueTiles.Count < MaxContinueCount)
			{
				datas += "\nContinue Tiles : " + ContinueTiles.Count;
			}
		}
		if (!tile.HasTile)
		{
			datas += "\nCan Fill Liquid Blocks: " + CheckLiquidTiles.Count;
		}

		// datas += "\nSlope: " + tile.Slope;
		datas += "\nColliding: " + Collision.IsWorldPointSolid(Main.MouseWorld);
		return datas;
	}

	/// <summary>
	/// When there is an isolated tile area(<=MaxContinueCount tiles), you can check the number of continue tiles.
	/// </summary>
	/// <returns></returns>
	public void UpdateContinueTiles(int i, int j)
	{
		ContinueTiles = new List<Point>();
		CheckLiquidTiles = new List<Point>();
		CheckTileContinue(i, j);
		CheckCanFillLiquidContinue(i, j);
	}

	public void CheckHasNoTileAndAddToCanFillLiquideTile(int i, int j)
	{
		Tile tile = SafeGetTile(i, j);
		if (!Collision.IsWorldPointSolid(new Point(i, j).ToWorldCoordinates(), true))
		{
			if (!CheckLiquidTiles.Contains(new Point(i, j)) && CheckLiquidTiles.Count < MaxContinueCount)
			{
				CheckLiquidTiles.Add(new Point(i, j));
				CheckCanFillLiquidContinue(i, j);
			}
		}
	}

	public void CheckCanFillLiquidContinue(int i, int j)
	{
		Tile tile = SafeGetTile(i, j);
		if (Collision.IsWorldPointSolid(new Point(i, j).ToWorldCoordinates(), true))
		{
			return;
		}
		if (CheckLiquidTiles.Count < MaxContinueCount)
		{
			// CheckHasTileAndAddToContinueTile(i, j);
			List<Point> checkPoints = new List<Point>();
			var leftPoint = new Point(i - 1, j);
			EnlistNewCheckLiquidPoint(checkPoints, leftPoint);
			var rightPoint = new Point(i + 1, j);
			EnlistNewCheckLiquidPoint(checkPoints, rightPoint);

			var bottomPoint = new Point(i, j + 1);
			EnlistNewCheckLiquidPoint(checkPoints, bottomPoint);

			var bottomLeftPoint = new Point(i - 1, j + 1);
			if (!Collision.IsWorldPointSolid(bottomPoint.ToWorldCoordinates(), true) || !Collision.IsWorldPointSolid(leftPoint.ToWorldCoordinates(), true))
			{
				EnlistNewCheckLiquidPoint(checkPoints, bottomLeftPoint);
			}
			var bottomRightPoint = new Point(i + 1, j + 1);
			if (!Collision.IsWorldPointSolid(bottomPoint.ToWorldCoordinates(), true) || !Collision.IsWorldPointSolid(rightPoint.ToWorldCoordinates(), true))
			{
				EnlistNewCheckLiquidPoint(checkPoints, bottomRightPoint);
			}
			for (int t = 0; t < checkPoints.Count; t++)
			{
				var pointC = checkPoints[t];
				CheckHasNoTileAndAddToCanFillLiquideTile(pointC.X, pointC.Y);
			}
		}
	}

	public void CheckHasTileAndAddToContinueTile(int i, int j)
	{
		Tile tile = SafeGetTile(i, j);
		if (tile.HasTile)
		{
			if (!ContinueTiles.Contains(new Point(i, j)) && ContinueTiles.Count < MaxContinueCount)
			{
				ContinueTiles.Add(new Point(i, j));
				CheckTileContinue(i, j);
			}
		}
	}

	public void CheckTileContinue(int i, int j)
	{
		Tile tile = SafeGetTile(i, j);
		if (!tile.HasTile)
		{
			return;
		}
		if (ContinueTiles.Count < MaxContinueCount)
		{
			// CheckHasTileAndAddToContinueTile(i, j);
			List<Point> checkPoints = new List<Point>();
			var leftPoint = new Point(i - 1, j);
			EnlistNewCheckCountinuePoint(checkPoints, leftPoint);
			var rightPoint = new Point(i + 1, j);
			EnlistNewCheckCountinuePoint(checkPoints, rightPoint);
			var topPoint = new Point(i, j - 1);
			EnlistNewCheckCountinuePoint(checkPoints, topPoint);
			var bottomPoint = new Point(i, j + 1);
			EnlistNewCheckCountinuePoint(checkPoints, bottomPoint);

			var topRightPoint = new Point(i + 1, j - 1);
			if (Main.tile[topPoint].HasTile || Main.tile[rightPoint].HasTile)
			{
				EnlistNewCheckCountinuePoint(checkPoints, topRightPoint);
			}
			var topLeftPoint = new Point(i - 1, j - 1);
			if (Main.tile[topPoint].HasTile || Main.tile[leftPoint].HasTile)
			{
				EnlistNewCheckCountinuePoint(checkPoints, topLeftPoint);
			}
			var bottomLeftPoint = new Point(i - 1, j + 1);
			if (Main.tile[bottomPoint].HasTile || Main.tile[leftPoint].HasTile)
			{
				EnlistNewCheckCountinuePoint(checkPoints, bottomLeftPoint);
			}
			var bottomRightPoint = new Point(i + 1, j + 1);
			if (Main.tile[bottomPoint].HasTile || Main.tile[rightPoint].HasTile)
			{
				EnlistNewCheckCountinuePoint(checkPoints, bottomRightPoint);
			}
			for (int t = 0; t < checkPoints.Count; t++)
			{
				var pointC = checkPoints[t];
				CheckHasTileAndAddToContinueTile(pointC.X, pointC.Y);
			}
		}
	}

	public void EnlistNewCheckCountinuePoint(List<Point> checkPoints, Point point)
	{
		if (checkPoints.Count == 0)
		{
			checkPoints.Add(point);
			return;
		}
		int length = GetSearchValue(point);
		List<Point> newCheck = new List<Point>();
		bool added = false;
		for (int i = 0; i < checkPoints.Count; i++)
		{
			if (!added && length < GetSearchValue(checkPoints[i]))
			{
				newCheck.Add(point);
				added = true;
			}
			newCheck.Add(checkPoints[i]);
		}
		if (!added)
		{
			newCheck.Add(point);
		}
		checkPoints.Clear();
		checkPoints.AddRange(newCheck);
	}

	public void EnlistNewCheckLiquidPoint(List<Point> checkPoints, Point point)
	{
		if (checkPoints.Count == 0)
		{
			checkPoints.Add(point);
			return;
		}
		int length = GetSearchValue(point, true);
		List<Point> newCheck = new List<Point>();
		bool added = false;
		for (int i = 0; i < checkPoints.Count; i++)
		{
			if (!added && length < GetSearchValue(checkPoints[i], true))
			{
				newCheck.Add(point);
				added = true;
			}
			newCheck.Add(checkPoints[i]);
		}
		if (!added)
		{
			newCheck.Add(point);
		}
		checkPoints.Clear();
		checkPoints.AddRange(newCheck);
	}

	public int GetSearchDepth(int i, int j)
	{
		return Math.Abs(OldTilePos.X - i) + Math.Abs(OldTilePos.Y - j);
	}

	public int GetSearchValue(int i, int j, bool checkLiquid = false)
	{
		if (i < 20 || i > Main.maxTilesX - 20)
		{
			return int.MaxValue;
		}
		if (j < 20 || j > Main.maxTilesY - 20)
		{
			return int.MaxValue;
		}
		int value = GetSearchDepth(i, j);
		if (!checkLiquid)
		{
			if (ContinueTiles.Contains(new Point(i, j)))
			{
				return int.MaxValue;
			}
			if (!Main.tile[i, j].HasTile)
			{
				return int.MaxValue - 1;
			}
		}
		else
		{
			if (CheckLiquidTiles.Contains(new Point(i, j)))
			{
				return int.MaxValue;
			}
			if (Collision.IsWorldPointSolid(new Point(i, j).ToWorldCoordinates(), true))
			{
				return int.MaxValue - 1;
			}
		}
		return value;
	}

	public int GetSearchValue(Point point, bool checkLiquid = false)
	{
		return GetSearchValue(point.X, point.Y, checkLiquid);
	}

	public void DrawBlockBound(int i, int j, Color color)
	{
		Vector2 pos = new Vector2(i, j) * 16;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(pos, color, new Vector3(0, 0, 0)),
			new Vertex2D(pos + new Vector2(16, 0), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(0, 16), color, new Vector3(0, 1, 0)),

			new Vertex2D(pos + new Vector2(0, 16), color, new Vector3(0, 1, 0)),
			new Vertex2D(pos + new Vector2(16, 0), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(16), color, new Vector3(1, 1, 0)),
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}

	public static Tile SafeGetTile(int i, int j)
	{
		return Main.tile[Math.Clamp(i, 20, Main.maxTilesX - 20), Math.Clamp(j, 20, Main.maxTilesY - 20)];
	}
}