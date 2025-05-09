using Everglow.Commons.VFX.Scene;
using Everglow.SubSpace;
using Everglow.SubSpace.Tiles;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood.Furniture;
using Everglow.Yggdrasil.YggdrasilTown.Walls;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

[Pipeline(typeof(WCSPipeline_PointWrap))]
public class RoadSignPost_ToArenaVFX : ForegroundVFX
{
	public override void Update()
	{
		base.Update();
		float startX = 20;
		float startY = 4;
		if (Main.MouseWorld.X > position.X + startX && Main.MouseWorld.X < position.X + startX + 56)
		{
			if (Main.MouseWorld.Y > position.Y + startY && Main.MouseWorld.Y < position.Y + startY + 16)
			{
				Main.instance.MouseText("Rightclick to Enter Arena.");
				if (Main.mouseRight && Main.mouseRightRelease && !Main.mapFullscreen)
				{
					int i = Main.MouseWorld.ToTileCoordinates().X;
					int j = Main.MouseWorld.ToTileCoordinates().Y;
					for (int x = -8; x < 9; x++)
					{
						for (int y = -8; y < 9; y++)
						{
							Tile tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);
							if (tile.TileType == ModContent.TileType<RoadSignPost_ToArena>())
							{
								if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
								{
									i += x;
									j += y;
									x = 100;
									break;
								}
							}
						}
					}
					Point point = new Point(i, j);
					RoomManager.EnterNextLevelRoom(point, new Point(60, 194), BuildArenaGen);
				}
			}
		}
	}

	public static void BuildArenaGen()
	{
		for (int x = 20; x < Main.maxTilesX - 20; x++)
		{
			for (int y = 20; y < 200; y++)
			{
				Tile tile = YggdrasilWorldGeneration.SafeGetTile(x, y);
				tile.HasTile = false;
				tile.wall = 0;
			}
			for (int y = 200; y < Main.maxTilesY - 20; y++)
			{
				Tile tile = YggdrasilWorldGeneration.SafeGetTile(x, y);
				tile.TileType = (ushort)ModContent.TileType<UnionMarbleTile_Khaki>();
				if (y > 201)
				{
					tile.WallType = (ushort)ModContent.WallType<UnionMarbleWall_Dark>();
				}
				tile.HasTile = true;
			}
		}
		for (int x = 20; x < Main.maxTilesX - 20; x++)
		{
			int y1 = 170;
			int y2 = 185;
			int y3 = 201;

			Tile tile1 = YggdrasilWorldGeneration.SafeGetTile(x, y1);
			tile1.TileType = (ushort)ModContent.TileType<LampWoodPlatform>();
			tile1.HasTile = true;

			Tile tile2 = YggdrasilWorldGeneration.SafeGetTile(x, y2);
			tile2.TileType = (ushort)ModContent.TileType<LampWoodPlatform>();
			tile2.HasTile = true;

			Tile tile3 = YggdrasilWorldGeneration.SafeGetTile(x, y3);
			tile3.TileType = (ushort)ModContent.TileType<ShieldTile>();
			tile3.HasTile = true;
		}

		for (int x = 20; x < 22; x++)
		{
			for (int y = 20; y < 23; y++)
			{
				Tile tile = YggdrasilWorldGeneration.SafeGetTile(x, y);
				tile.wall = 1;
				ushort typeChange = (ushort)ModContent.TileType<ArenaCommandBlock>();
				if (y == 22)
				{
					typeChange = 0;
				}
				else
				{
					tile.TileFrameX = (short)((x - 20) * 18);
					tile.TileFrameY = (short)((y - 20) * 18);
				}
				tile.TileType = typeChange;
				tile.HasTile = true;
			}
		}

		for (int x = 60; x < 65; x++)
		{
			for (int y = 196; y < 200; y++)
			{
				Tile tile = YggdrasilWorldGeneration.SafeGetTile(x, y);
				tile.WallType = (ushort)ModContent.WallType<UnionMarbleWall_Dark>();
				tile.HasTile = false;
			}
		}
		YggdrasilWorldGeneration.PlaceFrameImportantTiles(60, 196, 5, 4, ModContent.TileType<WoodenRoomDoor_exit>());
		Point msg = YggdrasilTownCentralSystem.FightingRequestPlayerNPCType;
		if (msg.X >= 0 && msg.Y >= 0)
		{
			NPC.NewNPCDirect(WorldGen.GetNPCSource_TileBreak(180, 190), new Point(180, 190).ToWorldCoordinates(), msg.Y);
		}
		YggdrasilWorldGeneration.SmoothTile(20, 20, Main.maxTilesX - 20, Main.maxTilesY - 20);
		YggdrasilWorldGeneration.PlaceFrameImportantTiles(70, 198, 2, 2, ModContent.TileType<ArenaChallengeSettingTile>());
		WorldGen.PlaceChest(72, 199, 21, false, 33);
	}

	public override void OnSpawn()
	{
		texture = ModAsset.RoadSignPost_ToArenaVFX.Value;
	}

	public override void Draw()
	{
		Color lightColor0 = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);
		Color lightColor1 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)position.Y / 16);
		Color lightColor2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y + texture.Height) / 16);
		Color lightColor3 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)(position.Y + texture.Height) / 16);

		var drawPos = position + new Vector2(7);
		Ins.Batch.BindTexture<Vertex2D>(texture);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(drawPos + new Vector2(-texture.Width / 2f, 0), lightColor0, new Vector3(0, 0, 0)),
			new Vertex2D(drawPos + new Vector2(texture.Width / 2f, 0), lightColor1, new Vector3(1, 0, 0)),

			new Vertex2D(drawPos + new Vector2(-texture.Width / 2f, texture.Height / 2f), lightColor2, new Vector3(0, 0.5f, 0)),
			new Vertex2D(drawPos + new Vector2(texture.Width / 2f, texture.Height / 2f), lightColor3, new Vector3(1, 0.5f, 0)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

		float startX = 20;
		float startY = 4;
		if (Main.MouseWorld.X > position.X + startX && Main.MouseWorld.X < position.X + startX + 56)
		{
			if (Main.MouseWorld.Y > position.Y + startY && Main.MouseWorld.Y < position.Y + startY + 16)
			{
				var emphasizeColor = Color.White;
				bars = new List<Vertex2D>()
				{
					new Vertex2D(drawPos + new Vector2(0, 0), emphasizeColor, new Vector3(0.5f, 0.5f, 0)),
					new Vertex2D(drawPos + new Vector2(texture.Width / 2f, 0), emphasizeColor, new Vector3(1, 0.5f, 0)),

					new Vertex2D(drawPos + new Vector2(0, texture.Height / 2f), emphasizeColor, new Vector3(0.5f, 1, 0)),
					new Vertex2D(drawPos + new Vector2(texture.Width / 2f, texture.Height / 2f), emphasizeColor, new Vector3(1, 1, 0)),
				};
				Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
			}
		}
	}
}