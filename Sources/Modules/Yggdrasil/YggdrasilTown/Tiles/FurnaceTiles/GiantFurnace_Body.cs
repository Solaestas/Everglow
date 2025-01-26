using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

[Pipeline(typeof(WCSPipeline))]
public class GiantFurnace_Body : BackgroundVFX
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
		texture = ModAsset.GiantFurnace_Body.Value;
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture<Vertex2D>(texture);
		var bars = new List<Vertex2D>();
		float width = 15;
		float height = 26;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Color lightColor0 = Lighting.GetColor((int)position.X / 16 + i * 2, (int)position.Y / 16 + j * 2);
				Color lightColor1 = Lighting.GetColor((int)position.X / 16 + i * 2 + 2, (int)position.Y / 16 + j * 2);
				Color lightColor2 = Lighting.GetColor((int)position.X / 16 + i * 2, (int)position.Y / 16 + j * 2 + 2);
				Color lightColor3 = Lighting.GetColor((int)position.X / 16 + i * 2 + 2, (int)position.Y / 16 + j * 2 + 2);

				bars.Add(position + new Vector2(i, j) * 32, lightColor0, new Vector3(i / width, j / height, 0));
				bars.Add(position + new Vector2(i + 1, j) * 32, lightColor1, new Vector3((i + 1) / width, j / height, 0));
				bars.Add(position + new Vector2(i, j + 1) * 32, lightColor2, new Vector3(i / width, (j + 1) / height, 0));

				bars.Add(position + new Vector2(i, j + 1) * 32, lightColor2, new Vector3(i / width, (j + 1) / height, 0));
				bars.Add(position + new Vector2(i + 1, j) * 32, lightColor1, new Vector3((i + 1) / width, j / height, 0));
				bars.Add(position + new Vector2(i + 1, j + 1) * 32, lightColor3, new Vector3((i + 1) / width, (j + 1) / height, 0));
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
		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}