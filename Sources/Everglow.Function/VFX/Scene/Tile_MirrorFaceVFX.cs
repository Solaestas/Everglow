using Everglow.Commons.Enums;
using Everglow.Commons.VFX.Pipelines;
using static Everglow.Commons.VFX.Pipelines.ScreenReflectionPipeline;

namespace Everglow.Commons.VFX.Scene;

[Pipeline(typeof(ScreenReflectionPipeline))]
public abstract class Tile_MirrorFaceVFX : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;

	public Vector2 position;
	public Texture2D texture;
	public Point originTile;
	public int originType;
	public float DepthZ = -50;

	public override void OnSpawn()
	{
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
		Main.graphics.GraphicsDevice.Textures[1] = texture;
		float z = DepthZ;
		List<MirrorFaceVertex> bars = new List<MirrorFaceVertex>()
		{
			new MirrorFaceVertex(new Vector3(position, z), color, new Vector3(0, 0, 0)),
			new MirrorFaceVertex(new Vector3(position + new Vector2(texture.Width, 0), z), color, new Vector3(1, 0, 0)),

			new MirrorFaceVertex(new Vector3(position + new Vector2(0, texture.Height), z), color, new Vector3(0, 1, 0)),
			new MirrorFaceVertex(new Vector3(position + new Vector2(texture.Width, texture.Height), z), color, new Vector3(1, 1, 0)),
		};
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
}