using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

[Pipeline(typeof(WCSPipeline))]
public class FloorScaleForPlayers_Screen : TileVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawTiles;

	public override void Update()
	{
		if (OriginTilePos.X > 0 && OriginTilePos.X < Main.maxTilesX)
		{
			if (OriginTilePos.Y > 0 && OriginTilePos.Y < Main.maxTilesY)
			{
				Tile tile = Main.tile[OriginTilePos.X, OriginTilePos.Y];
				if (tile != null)
				{
					if (TileLoader.GetTile(tile.TileType) is ISceneTile)
					{
						if (tile.TileType == OriginTileType)
						{
							if (!tile.HasTile)
							{
								Active = false;
								SceneVFXSystem.TilePointHasScene[(OriginTilePos.X, OriginTilePos.Y)] = false;
								return;
							}
						}
						else
						{
							Active = false;
							SceneVFXSystem.TilePointHasScene[(OriginTilePos.X, OriginTilePos.Y)] = false;
							return;
						}
					}
					else
					{
						Active = false;
						SceneVFXSystem.TilePointHasScene[(OriginTilePos.X, OriginTilePos.Y)] = false;
						return;
					}
				}
				else
				{
					Active = false;
					SceneVFXSystem.TilePointHasScene[(OriginTilePos.X, OriginTilePos.Y)] = false;
					return;
				}
			}
			else
			{
				Active = false;
				SceneVFXSystem.TilePointHasScene[(OriginTilePos.X, OriginTilePos.Y)] = false;
				return;
			}
		}
		else
		{
			Active = false;
			SceneVFXSystem.TilePointHasScene[(OriginTilePos.X, OriginTilePos.Y)] = false;
			return;
		}
		Vector2 checkPos = Position;
		if (VFXManager.InScreen(checkPos, 1500))
		{
			Visible = true;
		}
		else
		{
			Active = false;
			SceneVFXSystem.TilePointHasScene[(OriginTilePos.X, OriginTilePos.Y)] = false;
			return;
		}
	}

	public override void OnSpawn()
	{
		Texture = ModAsset.FloorScaleForPlayers_Screen.Value;
	}

	public override void Draw()
	{
		var bars = new List<Vertex2D>();

		// Screen
		Vector2 frameSize = new Vector2(24, 30);
		Vector2 frameXY = new Vector2(0, 0);
		var drawPos = Position + new Vector2(4, -15);
		var topLeft = drawPos + new Vector2(-frameSize.X, -frameSize.Y) * 0.5f;
		var topRight = drawPos + new Vector2(frameSize.X, -frameSize.Y) * 0.5f;
		var bottomLeft = drawPos + new Vector2(-frameSize.X, frameSize.Y) * 0.5f;
		var bottomRight = drawPos + new Vector2(frameSize.X, frameSize.Y) * 0.5f;
		bars.Add(topLeft, GetColor(topLeft), new Vector3(frameXY / Texture.Size(), 0));
		bars.Add(topRight, GetColor(topRight), new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y) / Texture.Size(), 0));
		bars.Add(bottomRight, GetColor(bottomRight), new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / Texture.Size(), 0));

		bars.Add(topLeft, GetColor(topLeft), new Vector3(frameXY / Texture.Size(), 0));
		bars.Add(bottomRight, GetColor(bottomRight), new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / Texture.Size(), 0));
		bars.Add(bottomLeft, GetColor(bottomLeft), new Vector3(new Vector2(frameXY.X, frameXY.Y + frameSize.Y) / Texture.Size(), 0));

		// Fresh Effect
		if (YggdrasilTownFurnaceSystem.CurrentPlayer != null)
		{
			frameXY = new Vector2(48, 0);
			float colorValue = YggdrasilTownFurnaceSystem.SwitchPlayerCooling / 30f;
			var drawColor = new Color(colorValue, colorValue, colorValue, 0);
			bars.Add(topLeft, drawColor, new Vector3(frameXY / Texture.Size(), 0));
			bars.Add(topRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y) / Texture.Size(), 0));
			bars.Add(bottomRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / Texture.Size(), 0));

			bars.Add(topLeft, drawColor, new Vector3(frameXY / Texture.Size(), 0));
			bars.Add(bottomRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / Texture.Size(), 0));
			bars.Add(bottomLeft, drawColor, new Vector3(new Vector2(frameXY.X, frameXY.Y + frameSize.Y) / Texture.Size(), 0));
		}

		// Content
		if (YggdrasilTownFurnaceSystem.CurrentPlayer != null)
		{
			frameXY = new Vector2(24, 0);
			var drawColor = new Color(1f, 1f, 1f, 0);
			bars.Add(topLeft, drawColor, new Vector3(frameXY / Texture.Size(), 0));
			bars.Add(topRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y) / Texture.Size(), 0));
			bars.Add(bottomRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / Texture.Size(), 0));

			bars.Add(topLeft, drawColor, new Vector3(frameXY / Texture.Size(), 0));
			bars.Add(bottomRight, drawColor, new Vector3(new Vector2(frameXY.X + frameSize.X, frameXY.Y + frameSize.Y) / Texture.Size(), 0));
			bars.Add(bottomLeft, drawColor, new Vector3(new Vector2(frameXY.X, frameXY.Y + frameSize.Y) / Texture.Size(), 0));
		}

		if (bars.Count <= 0)
		{
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));

			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
		}
		Ins.Batch.Draw(Texture, bars, PrimitiveType.TriangleList);
	}

	public Color GetColor(Vector2 drawPos)
	{
		return Lighting.GetColor(drawPos.ToTileCoordinates());
	}
}