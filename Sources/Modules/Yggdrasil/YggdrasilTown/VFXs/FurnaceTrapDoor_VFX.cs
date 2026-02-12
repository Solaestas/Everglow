using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class FurnaceTrapDoor_VFX : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 position;
	public float[] ai;
	public float timer;
	public float maxTime;
	public bool Open;
	public int tileX;
	public int tileY;

	public override void Update()
	{
		timer++;
		if (timer > maxTime)
		{
			if (!Open)
			{
				CloseTrapDoor(tileX, tileY);
				Active = false;
			}
			else
			{
				if (ai[0] > 0)
				{
					ai[0]--;
				}
				else
				{
					Open = false;
					timer = 0;
				}
			}
		}
		if (timer > maxTime - 50)
		{
			if(Open)
			{
				OpenTrapDoor(tileX, tileY);
			}
		}
	}

	public override void Draw()
	{
		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.FurnaceTrapDoor_VFX.Value;
		float pocession = timer / maxTime;
		if (!Open)
		{
			pocession = 1 - pocession;
		}
		float frameCount = 15;
		int frameY = (int)(pocession * frameCount);
		Color drawColor = Lighting.GetColor(position.ToTileCoordinates() + new Point(4, 0));
		Color light = new Color(1f, 1f, 1f, 0);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + new Vector2(0, 0), drawColor, new Vector3(0, frameY / frameCount, 0)),
			new Vertex2D(position + new Vector2(128, 0), drawColor, new Vector3(0.5f, frameY / frameCount, 0)),

			new Vertex2D(position + new Vector2(0, 32), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),
			new Vertex2D(position + new Vector2(128, 32), drawColor, new Vector3(0.5f, (frameY + 1) / frameCount, 0)),

			new Vertex2D(position + new Vector2(0, 32), light, new Vector3(0.5f, (frameY + 1) / frameCount, 0)),
			new Vertex2D(position + new Vector2(128, 32), light, new Vector3(1f, (frameY + 1) / frameCount, 0)),

			new Vertex2D(position + new Vector2(0, 0), light, new Vector3(0.5f, frameY / frameCount, 0)),
			new Vertex2D(position + new Vector2(128, 0), light, new Vector3(1f, frameY / frameCount, 0)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}

	public void CloseTrapDoor(int i, int j)
	{
		Tile thisTile = TileUtils.SafeGetTile(i, j);
		int startX = i - thisTile.TileFrameX / 18;
		int startY = j - thisTile.TileFrameY / 18;
		var firstTile = TileUtils.SafeGetTile(startX, startY);
		if (firstTile.TileType == ModContent.TileType<FurnaceTrapDoor_Open>())
		{
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 2; y++)
				{
					var checkTile = TileUtils.SafeGetTile(startX + x, startY + y);
					checkTile.TileType = (ushort)ModContent.TileType<FurnaceTrapDoor>();
					checkTile.TileFrameX = (short)(x * 18);
					checkTile.TileFrameY = (short)(y * 18);
				}
			}
		}
	}

	public void OpenTrapDoor(int i, int j)
	{
		Tile thisTile = TileUtils.SafeGetTile(i, j);
		int startX = i - thisTile.TileFrameX / 18;
		int startY = j - thisTile.TileFrameY / 18;
		var firstTile = TileUtils.SafeGetTile(startX, startY);
		if (firstTile.TileType == ModContent.TileType<FurnaceTrapDoor>())
		{
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 2; y++)
				{
					var checkTile = TileUtils.SafeGetTile(startX + x, startY + y);
					checkTile.TileType = (ushort)ModContent.TileType<FurnaceTrapDoor_Open>();
					checkTile.TileFrameX = (short)(x * 18);
					checkTile.TileFrameY = (short)(y * 18);
				}
			}
		}
	}
}