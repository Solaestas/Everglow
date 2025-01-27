using Everglow.Commons.VFX.Scene;
using Everglow.SubSpace;
using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

[Pipeline(typeof(WCSPipeline_PointWrap))]
public class MarbleGate_BackgroundTile : BackgroundVFX
{
	public override void Update()
	{
		base.Update();
		if (Main.MouseWorld.X > position.X && Main.MouseWorld.X < position.X + 112)
		{
			if (Main.MouseWorld.Y > position.Y && Main.MouseWorld.Y < position.Y + 128)
			{
				Main.instance.MouseText("Rightclick to Enter Union.");
				if (Main.mouseRight && Main.mouseRightRelease)
				{
					int i = Main.MouseWorld.ToTileCoordinates().X;
					int j = Main.MouseWorld.ToTileCoordinates().Y;
					for (int x = -8; x < 9; x++)
					{
						for (int y = -8; y < 9; y++)
						{
							Tile tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);
							if (tile.TileType == ModContent.TileType<MarbleGate>())
							{
								i += x;
								j += y;
								x = 100;
								break;
							}
						}
					}
					Point point = new Point(i, j);
					RoomManager.EnterNextLevelRoom(point + new Point(3, 6), ModAsset.HallOfUnion237x110_Path, 30, 110, new Point(60, 192));
				}
			}
		}
	}

	public override void OnSpawn()
	{
		texture = ModAsset.MarbleGate.Value;
	}

	public override void Draw()
	{
		Color lightColor0 = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);
		Color lightColor1 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)position.Y / 16);
		Color lightColor2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y + texture.Height) / 16);
		Color lightColor3 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)(position.Y + texture.Height) / 16);

		Vector2 subBackgroundScale = texture.Size() / ModAsset.MarbleGate_Background.Value.Size();

		// Vector2 offsetMouse = new Vector2((Main.MouseScreen.X / 150) % 1f, (Main.MouseScreen.Y / 150) % 1f);
		Vector2 viewOffset = (position - Main.LocalPlayer.position) * 0.0004f + new Vector2(10000) + new Vector2(0.44f);

		// Main.NewText(offsetMouse);
		viewOffset.X %= 1;
		viewOffset.Y %= 1;
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.MarbleGate_Background.Value);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position, Color.White, new Vector3(viewOffset, 0)),
			new Vertex2D(position + new Vector2(texture.Width, 0), Color.White, new Vector3(viewOffset + new Vector2(subBackgroundScale.X, 0), 0)),

			new Vertex2D(position + new Vector2(0, texture.Height), Color.White, new Vector3(viewOffset + new Vector2(0, subBackgroundScale.Y), 0)),
			new Vertex2D(position + new Vector2(texture.Width, texture.Height), Color.White, new Vector3(viewOffset + subBackgroundScale, 0)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
		Ins.Batch.BindTexture<Vertex2D>(texture);
		bars = new List<Vertex2D>()
		{
			new Vertex2D(position, lightColor0, new Vector3(0, 0, 0)),
			new Vertex2D(position + new Vector2(texture.Width, 0), lightColor1, new Vector3(1, 0, 0)),

			new Vertex2D(position + new Vector2(0, texture.Height), lightColor2, new Vector3(0, 1, 0)),
			new Vertex2D(position + new Vector2(texture.Width, texture.Height), lightColor3, new Vector3(1, 1, 0)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}