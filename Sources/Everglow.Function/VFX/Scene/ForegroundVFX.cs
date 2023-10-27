using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.VFX.Scene;
[Pipeline(typeof(WCSPipeline))]
public abstract class ForegroundVFX : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawPlayers;
	public Vector2 position;
	public Texture2D texture;
	public Point originTile;
	public int originType;
	public int direction = 1;
	public override void Update()
	{
		if(originTile.X > 0 && originTile.X < Main.maxTilesX)
		{
			if (originTile.Y > 0 && originTile.Y < Main.maxTilesY)
			{
				Tile tile = Main.tile[originTile.X, originTile.Y];
				if(tile != null)
				{
					if(TileLoader.GetTile(tile.TileType) is SceneTile)
					{
						if(tile.TileType == originType)
						{
							if (!tile.HasTile)
							{
								Active = false;
							}
						}
						else
						{
							Active = false;
						}
					}
					else
					{
						Active = false;
					}
				}
				else
				{
					Active = false;
				}
			}
			else
			{
				Active = false;
			}
		}
		else
		{
			Active = false;
		}
		Vector2 checkPos = position + texture.Size() / 2;
		if (VFXManager.InScreen(checkPos, Math.Max(texture.Width, texture.Height + 200)))
		{
			Visible = true;
		}
		else
		{
			Visible = false;
		}
	}
	public override void Draw()
	{
		Color lightColor0 = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);
		Color lightColor1 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)position.Y / 16);
		Color lightColor2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y + texture.Height) / 16);
		Color lightColor3 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)(position.Y + texture.Height) / 16);

		Ins.Batch.BindTexture<Vertex2D>(texture);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position, lightColor0, new Vector3(0, 0, 0)),
			new Vertex2D(position + new Vector2(texture.Width, 0),lightColor1, new Vector3(1, 0, 0)),

			new Vertex2D(position + new Vector2(0, texture.Height),lightColor2, new Vector3(0, 1, 0)),
			new Vertex2D(position + new Vector2(texture.Width, texture.Height),lightColor3, new Vector3(1, 1, 0))
		};
		if (direction < 0)
		{
			bars = new List<Vertex2D>()
		    {
		    	new Vertex2D(position, lightColor0, new Vector3(1, 0, 0)),
		    	new Vertex2D(position + new Vector2(texture.Width, 0),lightColor1, new Vector3(0, 0, 0)),

		    	new Vertex2D(position + new Vector2(0, texture.Height),lightColor2, new Vector3(1, 1, 0)),
		    	new Vertex2D(position + new Vector2(texture.Width, texture.Height),lightColor3, new Vector3(0, 1, 0))
		    };
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
