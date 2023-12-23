using Everglow.Commons.VFX.Scene;
using Microsoft.Xna.Framework.Graphics;
using static Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood.DarkTaro;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;
[Pipeline(typeof(WCSPipeline))]
public class DarkForestGrass_grass_fore : ForegroundVFX
{
	public override void OnSpawn()
	{
		texture = ModAsset.DarkForestGrass_grass.Value;
	}
	public override void Update()
	{
		foreach (Player player in Main.player)
		{
			if (player != null && player.active && !player.dead)
			{

			}
		}
		base.Update();
	}
	public float scale;
	public int style;
	public override void Draw()
	{
		Color lightColor = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);

		Ins.Batch.BindTexture<Vertex2D>(texture);
		List<Vertex2D> bars = new List<Vertex2D>();
		Tile tile = Main.tile[originTile];
		if (tile.Slope == SlopeType.Solid && !tile.halfBrick())
		{
			if (tile.TileFrameX >= 18 && tile.TileFrameX <= 54 && tile.TileFrameY == 0)
			{
				AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
			}
			if (tile.TileFrameX >= 18 && tile.TileFrameX <= 54 && tile.TileFrameY == 36)
			{
				AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
			}
			if (tile.TileFrameX == 0 && tile.TileFrameY <= 36 && tile.TileFrameY >= 0)
			{
				AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
			}
			if (tile.TileFrameX == 72 && tile.TileFrameY <= 36 && tile.TileFrameY >= 0)
			{
				AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
			}
			if (tile.TileFrameX == 90 && tile.TileFrameY <= 36 && tile.TileFrameY >= 0)
			{
				AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
			}
			if (tile.TileFrameY == 72 && tile.TileFrameX >= 108 && tile.TileFrameX <= 144)
			{
				AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
			}
			if (tile.TileFrameY == 0 && tile.TileFrameX >= 108 && tile.TileFrameX <= 144)
			{
				AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
			}
			if (tile.TileFrameY == 54 && tile.TileFrameX >= 108 && tile.TileFrameX <= 144)
			{
				AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
			}
			if (tile.TileFrameX == 162 && tile.TileFrameY <= 36 && tile.TileFrameY >= 0)
			{
				AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
				AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
			}
			if (tile.TileFrameX == 216 && tile.TileFrameY <= 36 && tile.TileFrameY >= 0)
			{
				AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
				AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
			}
			if (tile.TileFrameY == 54 && tile.TileFrameX >= 162 && tile.TileFrameX <= 198)
			{
				AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
			}
			if (tile.TileFrameY == 54 && tile.TileFrameX % 36 == 0 && tile.TileFrameX <= 90)
			{
				AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
			}
			if (tile.TileFrameY == 72 && tile.TileFrameX % 36 == 0 && tile.TileFrameX <= 90)
			{
				AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
			}
			if (tile.TileFrameY == 54 && tile.TileFrameX % 36 == 18 && tile.TileFrameX <= 90)
			{
				AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
			}
			if (tile.TileFrameY == 72 && tile.TileFrameX % 36 == 18 && tile.TileFrameX <= 90)
			{
				AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
				AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
			}
		}
		else
		{
			if (tile.Slope == SlopeType.SlopeUpLeft)
			{
				AddDrawingFace(bars, new Vector2(1, 1) * 0.85f, position, lightColor, 1.4143f, new Vector2(-5, -5));
				Tile upTile = Main.tile[originTile + new Point(0, -1)];
				if (!upTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
				}
				Tile leftTile = Main.tile[originTile + new Point(-1, 0)];
				if (!leftTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
				}
				Tile downTile = Main.tile[originTile + new Point(0, 1)];
				if (downTile.HasTile && downTile.TileType == originType)
				{
					AddDrawingFace(bars, new Vector2(0, -1), position, lightColor, 1, new Vector2(0, 16));
				}
			}
			if (tile.Slope == SlopeType.SlopeDownLeft)
			{
				AddDrawingFace(bars, new Vector2(1, -1) * 0.85f, position, lightColor, 1.4143f, new Vector2(-5, 5));
				Tile downTile = Main.tile[originTile + new Point(0, 1)];
				if (!downTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
				}
				Tile leftTile = Main.tile[originTile + new Point(-1, 0)];
				if (!leftTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor);
				}
				Tile upTile = Main.tile[originTile + new Point(0, -1)];
				if (upTile.HasTile && upTile.TileType == originType)
				{
					AddDrawingFace(bars, new Vector2(0, 1), position, lightColor, 1, new Vector2(0, -16));
				}
			}
			if (tile.Slope == SlopeType.SlopeUpRight)
			{
				AddDrawingFace(bars, new Vector2(-1, 1) * 0.85f, position, lightColor, 1.4143f, new Vector2(5, -5));
				Tile upTile = Main.tile[originTile + new Point(0, -1)];
				if (!upTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(0, -1), position, lightColor);
				}
				Tile rightTile = Main.tile[originTile + new Point(1, 0)];
				if (!rightTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
				}
				Tile downTile = Main.tile[originTile + new Point(0, 1)];
				if (downTile.HasTile && downTile.TileType == originType)
				{
					AddDrawingFace(bars, new Vector2(0, -1), position, lightColor, 1, new Vector2(0, 16));
				}
			}
			if (tile.Slope == SlopeType.SlopeDownRight)
			{
				AddDrawingFace(bars, new Vector2(-1, -1) * 0.85f, position, lightColor, 1.4143f, new Vector2(5, 5));
				Tile downTile = Main.tile[originTile + new Point(0, 1)];
				if (!downTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
				}
				Tile rightTile = Main.tile[originTile + new Point(1, 0)];
				if (!rightTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(1, 0), position, lightColor);
				}
				Tile upTile = Main.tile[originTile + new Point(0, -1)];
				if (upTile.HasTile && upTile.TileType == originType)
				{
					AddDrawingFace(bars, new Vector2(0, 1), position, lightColor, 1, new Vector2(0, -16));
				}
			}
			if (tile.halfBrick())
			{
				AddDrawingFace(bars, new Vector2(0, -1), position, lightColor, 1, new Vector2(0, 6));
				if (!Main.tile[originTile + new Point(0, 1)].HasTile)
				{
					AddDrawingFace(bars, new Vector2(0, 1), position, lightColor);
				}
				Tile upTile = Main.tile[originTile + new Point(0, -1)];
				if (upTile.HasTile && upTile.TileType == originType)
				{
					AddDrawingFace(bars, new Vector2(0, 1), position, lightColor, 1, new Vector2(0, -16));
				}
				Tile leftTile = Main.tile[originTile + new Point(-1, 0)];
				if (leftTile.HasTile && leftTile.TileType == originType)
				{
					AddDrawingFace(bars, new Vector2(1, 0), position, lightColor, 0.5f, new Vector2(-14, -4));
				}
				if (!leftTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor, 0.5f, new Vector2(0, 4));
				}
				Tile rightTile = Main.tile[originTile + new Point(1, 0)];
				if (rightTile.HasTile && rightTile.TileType == originType)
				{
					AddDrawingFace(bars, new Vector2(-1, 0), position, lightColor, 0.5f, new Vector2(14, -4));
				}
				if (!rightTile.HasTile)
				{
					AddDrawingFace(bars, new Vector2(1, 0), position, lightColor, 0.5f, new Vector2(0, 4));
				}
			}
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
	public void AddDrawingFace(List<Vertex2D> bars, Vector2 direction, Vector2 position, Color lightColor, float length = 1, Vector2 offset = new Vector2())
	{
		Vector2 drawCenter = position + new Vector2(8) + offset;
		Vector2 directionN = Vector2.Normalize(direction);
		float rot = -MathF.Asin(Vector3.Cross(new Vector3(directionN, 0), new Vector3(0, -1, 0)).Z);
		float cos = Vector2.Dot(directionN, new Vector2(0, -1));
		if (cos < 0)
		{
			rot = MathHelper.Pi - rot;
		}
		Vector2 windPush = new Vector2(Main.windSpeedCurrent * (1 + MathF.Sin(position.X * position.Y * 0.0003f + (float)Main.time * 0.09f)) * 3f, 0);
		float dLength = 16 / 52f;
		bars.Add(drawCenter + new Vector2(-8 * length, -8).RotatedBy(rot) + direction * 12 + windPush, lightColor, new Vector3(18 * style / 52f, 0, 0));//这三个点需要碰撞
		bars.Add(drawCenter + new Vector2(8 * length, -8).RotatedBy(rot) + direction * 12 + windPush, lightColor, new Vector3(dLength + 18 * style / 52f, 0, 0));//这三个点需要碰撞
		bars.Add(drawCenter + new Vector2(8 * length, 8).RotatedBy(rot) + direction * 12, lightColor, new Vector3(dLength + 18 * style / 52f, 1, 0));

		bars.Add(drawCenter + new Vector2(-8 * length, 8).RotatedBy(rot) + direction * 12, lightColor, new Vector3(18 * style / 52f, 1, 0));
		bars.Add(drawCenter + new Vector2(8 * length, 8).RotatedBy(rot) + direction * 12, lightColor, new Vector3(dLength + 18 * style / 52f, 1, 0));
		bars.Add(drawCenter + new Vector2(-8 * length, -8).RotatedBy(rot) + direction * 12 + windPush, lightColor, new Vector3(18 * style / 52f, 0, 0));//这三个点需要碰撞
	}
}
