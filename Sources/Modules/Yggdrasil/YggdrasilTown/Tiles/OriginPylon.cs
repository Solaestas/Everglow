using Everglow.Yggdrasil.WorldGeneration;
using SubworldLibrary;
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
			r = 1f;
			g = 1f;
			b = 1f;
		}
		else
		{
			r = 1f;
			g = 1f;
			b = 1f;
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
			Color color0 = Color.White * 0.4f;
			color0.A = 0;
			zero += new Vector2(-3, -2);//Offset
			List<Vertex2D> bars = new List<Vertex2D>()
			{
				new Vertex2D(zero + new Vector2(i, j - 1.3f) * 16 - Main.screenPosition, color0,new Vector3(0, 0, 0)),
				new Vertex2D(zero + new Vector2(i + 16, j - 1.3f) * 16- Main.screenPosition, color0,new Vector3(1, 0, 0)),
				new Vertex2D(zero + new Vector2(i, j + 12) * 16 - Main.screenPosition, color0,new Vector3(0, 1, 0)),
				new Vertex2D(zero + new Vector2(i + 16, j + 12) * 16- Main.screenPosition, color0,new Vector3(1, 1, 0)),
			};
			Main.graphics.GraphicsDevice.Textures[0] = ModAsset.OriginPylon_glow.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

			bars = new List<Vertex2D>()
			{
				new Vertex2D(zero + new Vector2(i - 4, j - 4) * 16 - Main.screenPosition, color0 * 0.4f,new Vector3(0, 0, 0)),
				new Vertex2D(zero + new Vector2(i + 20, j - 4) * 16- Main.screenPosition, color0* 0.4f,new Vector3(1, 0, 0)),
				new Vertex2D(zero + new Vector2(i - 4, j + 12) * 16 - Main.screenPosition, color0* 0.4f,new Vector3(0, 1, 0)),
				new Vertex2D(zero + new Vector2(i + 20, j + 12) * 16- Main.screenPosition, color0* 0.4f,new Vector3(1, 1, 0)),
			};
			Main.graphics.GraphicsDevice.Textures[0] = ModAsset.OriginPylon_halo.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

			float timeValue = (float)(Main.time * 0.0004f);
			bars = new List<Vertex2D>()
			{
				new Vertex2D(zero + new Vector2(i - 1, j + 12) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.1f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i - 1, j - 10) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.1f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 3, j + 12) * 16 - Main.screenPosition, color0 * 0.7f,new Vector3(0.2f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 3, j - 28) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.2f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 13, j + 12) * 16- Main.screenPosition, color0 * 0.7f,new Vector3(0.4f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 13, j - 28) * 16- Main.screenPosition, color0 * 0.0f,new Vector3(0.4f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 17, j + 12) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.5f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 17, j - 10) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.5f, timeValue, 0))
			};
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_perlin.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

			bars = new List<Vertex2D>()
			{
				new Vertex2D(zero + new Vector2(i - 15, j + 13) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.1f + timeValue, 0, 0)),
				new Vertex2D(zero + new Vector2(i - 15, j + 11) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.1f + timeValue, 0.0f, 0)),
				new Vertex2D(zero + new Vector2(i + 4, j + 13) * 16 - Main.screenPosition, new Color(0, 255, 255, 0),new Vector3(0.2f + timeValue, 0, 0)),
				new Vertex2D(zero + new Vector2(i + 4, j + 4) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.2f + timeValue, 0.2f, 0)),
				new Vertex2D(zero + new Vector2(i + 13, j + 13) * 16- Main.screenPosition, new Color(255, 255, 0, 0),new Vector3(0.4f + timeValue, 0, 0)),
				new Vertex2D(zero + new Vector2(i + 13, j + 4) * 16- Main.screenPosition, color0 * 0.0f,new Vector3(0.4f + timeValue, 0.2f, 0)),
				new Vertex2D(zero + new Vector2(i + 31, j + 13) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.5f + timeValue, 0, 0)),
				new Vertex2D(zero + new Vector2(i + 31, j + 11) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.5f + timeValue, 0.2f, 0))
			};
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_perlin.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		return base.PreDraw(i, j, spriteBatch);
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		if(!Main.gamePaused && YggdrasilWorld.YggdrasilTimer > 5)
		{
			if (Main.rand.NextBool(600))
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(MathHelper.TwoPi);
				Vector2 pos = new Vector2(i, j) * 16;
				Vector2 addPos = new Vector2(Main.rand.NextFloat(1f) * 120f, 0).RotatedByRandom(6.283);
				addPos.Y = -Math.Abs(addPos.Y) * 4 + 100;
				float size = Math.Max(addPos.Y + 500, 0) / 500f;
				size *= (130 - Math.Abs(addPos.X)) / 120f;
				addPos.X += 16;
				pos += addPos;
				if (Collision.SolidCollision(pos, 0, 0))
				{
					return;
				}
				var dust = new WhiteTriangle
				{
					velocity = newVelocity + new Vector2(0, addPos.Y * 0.01f),
					Active = true,
					Visible = true,
					position = pos,
					maxTime = Main.rand.Next(50, 192),
					scale = 0,
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(4.0f, 14.5f) * size, Main.rand.NextFloat(-0.03f, 0.03f) }
				};
				Ins.VFXManager.Add(dust);
			}
		}
	}
}