using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Terraria.GameContent;

namespace Everglow.Example.TileLayers;
public class TileDrawer : ModSystem
{
	public static int Layer => TileLayerSystem.PlayerZoneLayer[Main.LocalPlayer.whoAmI];
	public static int Timer => TileLayerSystem.SwitchingTimer;
	public override void OnModLoad()
	{
		if (Main.netMode != NetmodeID.Server)
		{
			Ins.HookManager.AddHook(CodeLayer.PostDrawDusts, BlackTileDraw);
		}
	}
	public static List<Vertex2D> BlackArea = new List<Vertex2D>();
	public static void BlackTileDraw()
	{
		int startX = (int)(Main.screenPosition.X / 16f - 8);
		int startY = (int)(Main.screenPosition.Y / 16f - 8);
		int endX = (int)(Main.screenPosition.X / 16f + Main.screenWidth / 16f + 8);
		int endY = (int)(Main.screenPosition.Y / 16f + Main.screenHeight / 16f + 8);
		BatchBegin();
		for (int i = startX; i <= endX; i++)
		{
			for (int j = startY; j <= endY; j++)
			{
				if (Timer > 0)
				{
					if (TileLayerSystem.InSwitchingTileCoords.Contains((i, j)))
					{
						int x = (int)(i * 16 - Main.screenPosition.X);
						int y = (int)(j * 16 - Main.screenPosition.Y);
						DrawBlackTile(x, y, new Color(0, 0, 0, (float)(TileLayerSystem.SwitchingTimer / 60f)));
					}
					if (TileLayerSystem.OutSwitchingTileCoords.Contains((i, j)))
					{
						int x = (int)(i * 16 - Main.screenPosition.X);
						int y = (int)(j * 16 - Main.screenPosition.Y);
						DrawBlackTile(x, y, new Color(0, 0, 0, (float)(1 - TileLayerSystem.SwitchingTimer / 60f)));
					}
				}
				if (Layer < 0)
				{
					if (!TileLayerSystem.RoomsInsideLayers[Layer].Contains((i, j)))
					{
						int x = (int)(i * 16 - Main.screenPosition.X);
						int y = (int)(j * 16 - Main.screenPosition.Y);
						DrawBlackTile(x, y, new Color(0, 0, 0, (float)(1 - TileLayerSystem.SwitchingTimer / 60f)));
					}
				}
			}
		}
		BatchEnd();
	}
	public static void BatchBegin()
	{
		BlackArea = new List<Vertex2D>();
	}
	public static void DrawBlackTile(int i, int j, Color color)
	{
		BlackArea.Add(new Vertex2D(new Vector2(i, j), Color.Transparent, Vector3.zero));
		BlackArea.Add(new Vertex2D(new Vector2(i + 16, j), Color.Transparent, Vector3.zero));

		BlackArea.Add(new Vertex2D(new Vector2(i, j), color, Vector3.zero));
		BlackArea.Add(new Vertex2D(new Vector2(i + 16, j), color, Vector3.zero));

		BlackArea.Add(new Vertex2D(new Vector2(i, j + 16), color, Vector3.zero));
		BlackArea.Add(new Vertex2D(new Vector2(i + 16, j + 16), color, Vector3.zero));

		BlackArea.Add(new Vertex2D(new Vector2(i, j + 16), Color.Transparent, Vector3.zero));
		BlackArea.Add(new Vertex2D(new Vector2(i + 16, j + 16), Color.Transparent, Vector3.zero));
	}
	public static void BatchEnd()
	{
		if(BlackArea.Count > 0)
		{
			Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, BlackArea.ToArray(), 0, BlackArea.Count - 2);
		}
	}
}
