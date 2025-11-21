using Everglow.Commons.Enums;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.DeveloperContent.Items;

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
			var vfx = new TileDataReaderSystem { FixPoint = new Point(i, j), Active = true, Visible = true, EverLasting = EnableResidentEffect };
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
			var drawContinueColor = new Color(0.12f, 0.24f, 0.4f, 0);
			foreach (Point point in ContinueTiles)
			{
				DrawBlockBound(point.X, point.Y, drawContinueColor);
			}
		}
		if (CheckLiquidTiles.Count < MaxContinueCount)
		{
			var drawContinueColor = new Color(0.0f, 0.0f, 0.6f, 0);
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
		Tile tile = TileUtils.SafeGetTile(i, j);

		string datas = "\nCoordinate: [" + i + ", " + j + "]";
		datas += "\nHasTile: " + tile.HasTile;
		datas += "\nType :" + tile.TileType;
		if (tile.HasTile)
		{
			datas += " " + TileID.Search.GetName(tile.TileType);
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
		BFSContinueTile(i, j);
		BFSFillLiquid(i, j);
	}

	private static readonly (int, int)[] directions =
	{
		(0, 1),
		(1, 0),
		(0, -1),
		(-1, 0),
	};

	private static readonly (int, int)[] directionsLiquid =
	{
		(1, 0),
		(0, 1),
		(-1, 0),
	};

	public void BFSContinueTile(int i, int j)
	{
		BFSContinueTile(new Point(i, j));
	}

	public void BFSContinueTile(Point checkPoint)
	{
		var queueChecked = new Queue<Point>();

		// 将起始点加入队列
		queueChecked.Enqueue(checkPoint);
		var visited = new List<Point>();

		while (queueChecked.Count > 0)
		{
			var tilePos = queueChecked.Dequeue();

			foreach (var (dx, dy) in directions)
			{
				int checkX = tilePos.X + dx;
				int checkY = tilePos.Y + dy;
				var point = new Point(checkX, checkY);
				Tile tile = TileUtils.SafeGetTile(checkX, checkY);

				// 检查边界和障碍物
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					tile.HasTile && !visited.Contains(point))
				{
					queueChecked.Enqueue(point);
					visited.Add(point);
				}
			}
			if (queueChecked.Count > MaxContinueCount || visited.Count > MaxContinueCount)
			{
				break;
			}
		}
		ContinueTiles = visited;
	}

	public void BFSFillLiquid(int i, int j)
	{
		BFSFillLiquid(new Point(i, j));
	}

	public void BFSFillLiquid(Point checkPoint)
	{
		var queueChecked = new Queue<Point>();

		// 将起始点加入队列
		queueChecked.Enqueue(checkPoint);
		var visited = new List<Point>();

		while (queueChecked.Count > 0)
		{
			var tilePos = queueChecked.Dequeue();

			foreach (var (dx, dy) in directionsLiquid)
			{
				int checkX = tilePos.X + dx;
				int checkY = tilePos.Y + dy;
				var point = new Point(checkX, checkY);
				Tile tile = TileUtils.SafeGetTile(checkX, checkY);

				// 检查边界和障碍物
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					!Collision.IsWorldPointSolid(point.ToWorldCoordinates()) && !visited.Contains(point))
				{
					queueChecked.Enqueue(point);
					visited.Add(point);
				}
			}
			if (queueChecked.Count > MaxContinueCount || visited.Count > MaxContinueCount)
			{
				break;
			}
		}
		CheckLiquidTiles = visited;
	}

	public void DrawBlockBound(int i, int j, Color color)
	{
		Vector2 pos = new Vector2(i, j) * 16;
		var bars = new List<Vertex2D>()
		{
			new Vertex2D(pos, color, new Vector3(0, 0, 0)),
			new Vertex2D(pos + new Vector2(16, 0), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(0, 16), color, new Vector3(0, 1, 0)),

			new Vertex2D(pos + new Vector2(0, 16), color, new Vector3(0, 1, 0)),
			new Vertex2D(pos + new Vector2(16, 0), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(16), color, new Vector3(1, 1, 0)),
		};

		Ins.Batch.Draw(Texture, bars, PrimitiveType.TriangleList);
	}
}