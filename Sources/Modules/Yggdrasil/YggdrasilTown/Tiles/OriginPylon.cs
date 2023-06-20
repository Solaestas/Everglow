using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class OriginPylon : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileLavaDeath[Type] = false;

		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 12;
		TileObjectData.newTile.Width = 16;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			20
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(64, 64, 61));
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		Tile tile = Main.tile[i, j];
		if(tile.TileFrameX < 108)
		{
			r = 0f;
			g = 2f;
			b = 2f;
		}
		else
		{
			r = 2f;
			g = 2f;
			b = 0f;
		}
		base.ModifyLight(i, j, ref r, ref g, ref b);
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}

	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		if(tile.TileFrameX == 0 && tile.TileFrameY == 0)
		{
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;
			Color color0 = Lighting.GetColor(i, j);
			Color color1 = Lighting.GetColor(i + 16, j);
			Color color2 = Lighting.GetColor(i, j + 12);
			Color color3 = Lighting.GetColor(i + 16, j + 12);
			color0.A = 0;
			color1.A = 0;
			color2.A = 0;
			color3.A = 0;
			zero += new Vector2(-3, -2);//Offset
			List<Vertex2D> bars = new List<Vertex2D>()
			{
				new Vertex2D(zero + new Vector2(i, j - 1) * 16 - Main.screenPosition, color0,new Vector3(0, 0, 0)),
				new Vertex2D(zero + new Vector2(i + 16, j - 1) * 16- Main.screenPosition, color1,new Vector3(1, 0, 0)),
				new Vertex2D(zero + new Vector2(i, j + 12) * 16 - Main.screenPosition, color2,new Vector3(0, 1, 0)),
				new Vertex2D(zero + new Vector2(i + 16, j + 12) * 16- Main.screenPosition, color3,new Vector3(1, 1, 0)),
			};
			Main.graphics.GraphicsDevice.Textures[0] = ModAsset.OriginPylon_glow.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		return base.PreDraw(i, j, spriteBatch);
	}
}