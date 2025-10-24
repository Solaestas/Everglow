using Everglow.Commons.VFX.Scene;
using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

[Pipeline(typeof(WCSPipeline))]
public class Union_Y_Stairs_Front : ForegroundVFX
{
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
								return;
							}
						}
						else
						{
							Active = false;
							SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
							return;
						}
					}
					else
					{
						Active = false;
						SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
						return;
					}
				}
				else
				{
					Active = false;
					SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
					return;
				}
			}
			else
			{
				Active = false;
				SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
				return;
			}
		}
		else
		{
			Active = false;
			SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
			return;
		}
		Vector2 checkPos = position;
		if (VFXManager.InScreen(checkPos, 1500))
		{
			Visible = true;
		}
		else
		{
			Active = false;
			SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
			return;
		}
	}

	public override void OnSpawn()
	{
		texture = ModAsset.Union_Y_Stairs_Front.Value;
	}

	public override void Draw()
	{
		List<Vertex2D> bars = new List<Vertex2D>();
		float width = 38;
		float height = 25;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Color lightColor0 = Lighting.GetColor((int)position.X / 16 + i, (int)position.Y / 16 + j);
				Color lightColor1 = Lighting.GetColor((int)position.X / 16 + i + 1, (int)position.Y / 16 + j);
				Color lightColor2 = Lighting.GetColor((int)position.X / 16 + i, (int)position.Y / 16 + j + 1);
				Color lightColor3 = Lighting.GetColor((int)position.X / 16 + i + 1, (int)position.Y / 16 + j + 1);

				bars.Add(position + new Vector2(i, j) * 16, lightColor0, new Vector3(i / width, j / height, 0));
				bars.Add(position + new Vector2(i + 1, j) * 16, lightColor1, new Vector3((i + 1) / width, j / height, 0));
				bars.Add(position + new Vector2(i, j + 1) * 16, lightColor2, new Vector3(i / width, (j + 1) / height, 0));

				bars.Add(position + new Vector2(i, j + 1) * 16, lightColor2, new Vector3(i / width, (j + 1) / height, 0));
				bars.Add(position + new Vector2(i + 1, j) * 16, lightColor1, new Vector3((i + 1) / width, j / height, 0));
				bars.Add(position + new Vector2(i + 1, j + 1) * 16, lightColor3, new Vector3((i + 1) / width, (j + 1) / height, 0));
			}
		}

		if (bars.Count <= 0)
		{
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));

			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
		}
		Ins.Batch.Draw(texture, bars, PrimitiveType.TriangleList);
	}
}