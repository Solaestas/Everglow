using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

[Pipeline(typeof(WCSPipeline))]
public class CurrentEnergyTube_LavaBar : BackgroundVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawTiles;

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
		texture = ModAsset.CurrentEnergyTube_LavaBar.Value;
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture<Vertex2D>(texture);
		var bars = new List<Vertex2D>();

		// Lava post
		float frameHeight = 176 * YggdrasilTownFurnaceSystem.CurrentEnergy / (float)YggdrasilTownFurnaceSystem.EnergtMax;
		Vector2 frameSize = new Vector2(16, frameHeight);
		Vector2 frameXY = new Vector2(18, 176 - frameHeight);
		float surfaceY = 176 - frameHeight;
		var drawPos = position + new Vector2(8, (176 - frameHeight * 0.5f) + 8);
		for (int j = 0; j < frameHeight; j += 5)
		{
			Lighting.AddLight(drawPos + new Vector2(0, j), new Vector3(1f, 0.7f, 0) * 0.5f);
		}
		var topLeft = drawPos + new Vector2(-frameSize.X, -frameSize.Y) * 0.5f;
		var topRight = drawPos + new Vector2(frameSize.X, -frameSize.Y) * 0.5f;
		var bottomLeft = drawPos + new Vector2(-frameSize.X, frameSize.Y) * 0.5f;
		var bottomRight = drawPos + new Vector2(frameSize.X, frameSize.Y) * 0.5f;
		var drawColor = Color.White;
		bars.Add(topLeft, drawColor, new Vector3(frameXY / texture.Size(), 0));
		bars.Add(topRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y) / texture.Size(), 0));
		bars.Add(bottomRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / texture.Size(), 0));

		bars.Add(topLeft, drawColor, new Vector3(frameXY / texture.Size(), 0));
		bars.Add(bottomRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / texture.Size(), 0));
		bars.Add(bottomLeft, drawColor, new Vector3(new Vector2(frameXY.X, frameXY.Y + frameSize.Y) / texture.Size(), 0));

		// Lava surface
		frameHeight = 176 * YggdrasilTownFurnaceSystem.CurrentEnergy / (float)YggdrasilTownFurnaceSystem.EnergtMax;
		frameSize = new Vector2(16, 14);
		frameXY = new Vector2(0, 162);
		drawPos = position + new Vector2(8, surfaceY + 2);
		topLeft = drawPos + new Vector2(-frameSize.X, -frameSize.Y) * 0.5f;
		topRight = drawPos + new Vector2(frameSize.X, -frameSize.Y) * 0.5f;
		bottomLeft = drawPos + new Vector2(-frameSize.X, frameSize.Y) * 0.5f;
		bottomRight = drawPos + new Vector2(frameSize.X, frameSize.Y) * 0.5f;
		drawColor = new Color(1f, 1f, 1f, 0);
		bars.Add(topLeft, drawColor, new Vector3(frameXY / texture.Size(), 0));
		bars.Add(topRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y) / texture.Size(), 0));
		bars.Add(bottomRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / texture.Size(), 0));

		bars.Add(topLeft, drawColor, new Vector3(frameXY / texture.Size(), 0));
		bars.Add(bottomRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / texture.Size(), 0));
		bars.Add(bottomLeft, drawColor, new Vector3(new Vector2(frameXY.X, frameXY.Y + frameSize.Y) / texture.Size(), 0));

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