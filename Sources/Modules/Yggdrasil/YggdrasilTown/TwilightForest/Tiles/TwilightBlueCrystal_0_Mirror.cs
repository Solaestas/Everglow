using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;
[Pipeline(typeof(ScreenReflectionPipeline))]
public class TwilightBlueCrystal_0_Mirror : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;
	public Vector2 position;
	public Texture2D texture;
	public Point originTile;
	public int originType;
	public override void OnSpawn()
	{
		texture = ModAsset.TwilightBlueCrystal_0_Mirror.Value;
	}
	public override void Update()
	{
		if (originTile.X > 0 && originTile.X < Main.maxTilesX)
		{
			if (originTile.Y > 0 && originTile.Y < Main.maxTilesY)
			{
				Tile tile = Main.tile[originTile.X, originTile.Y];
				if (tile != null)
				{
					if (TileLoader.GetTile(tile.TileType) is ISceneTile)
					{
						if (tile.TileType == originType)
						{
							if (!tile.HasTile)
							{
								Active = false;
								SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
							}
						}
						else
						{
							Active = false;
							SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
						}
					}
					else
					{
						Active = false;
						SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
					}
				}
				else
				{
					Active = false;
					SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
				}
			}
			else
			{
				Active = false;
				SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
			}
		}
		else
		{
			Active = false;
			SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
		}
		Vector2 checkPos = position + texture.Size() / 2;
		if (VFXManager.InScreen(checkPos, Math.Max(texture.Width, texture.Height + 200)))
		{
			Visible = true;
		}
		else
		{
			Active = false;
			SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
		}
	}
	public override void Draw()
	{
		Color color = Color.White;
		Ins.Batch.BindTexture<Vertex2D>(texture);
		Main.graphics.GraphicsDevice.Textures[0] = texture;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position, color, new Vector3(0, 0, 0)),
			new Vertex2D(position + new Vector2(texture.Width, 0),color, new Vector3(1, 0, 0)),

			new Vertex2D(position + new Vector2(0, texture.Height),color, new Vector3(0, 1, 0)),
			new Vertex2D(position + new Vector2(texture.Width, texture.Height), color, new Vector3(1, 1, 0))
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
