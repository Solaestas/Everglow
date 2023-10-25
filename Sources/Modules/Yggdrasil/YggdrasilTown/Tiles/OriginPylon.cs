using Everglow.Yggdrasil.YggdrasilTown.VFXs;
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
		TileObjectData.newTile.Width = 8;
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
		if (tile.TileFrameX < 108)
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
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
		base.NumDust(i, j, fail, ref num);
	}
	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}
	//TODO:这个物块远离之后仍需绘制
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
		{
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;
			Color color0 = Color.White * 0.4f;
			color0.A = 0;
			zero += new Vector2(-3, -2);//Offset
			List<Vertex2D> bars = new List<Vertex2D>()
			{
				new Vertex2D(zero + new Vector2(i - 11, j - 8.3f) * 16 - Main.screenPosition, color0,new Vector3(0, 0, 0)),
				new Vertex2D(zero + new Vector2(i + 21, j - 8.3f) * 16- Main.screenPosition, color0,new Vector3(1, 0, 0)),
				new Vertex2D(zero + new Vector2(i - 11, j + 17) * 16 - Main.screenPosition, color0,new Vector3(0, 1, 0)),
				new Vertex2D(zero + new Vector2(i + 21, j + 17) * 16- Main.screenPosition, color0,new Vector3(1, 1, 0)),
			};
			Main.graphics.GraphicsDevice.Textures[0] = ModAsset.OriginPylon_hang_glow.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			float timeValue = (float)(Main.time * 0.0004f);

			List<Vertex2D> crack = new List<Vertex2D>();
			for (int k = 0;k < 6;k++)
			{
				Vector2 v0 = new Vector2(290, 0).RotatedBy(k);
				Vector2 v0Left = Vector2.Normalize(v0.RotatedBy(MathHelper.PiOver2)) * 60f;
				Vector2 v0Normal = Vector2.Normalize(v0) * 20;
				Vector2 pylonCenter = zero + new Vector2(i + 5, j + 7) * 16 + v0 - Main.screenPosition;
				crack.Add(pylonCenter + v0Left + v0Normal, Color.Transparent, new Vector3(timeValue * 0.4f + k / 7f, 0.4f, 0));
				crack.Add(pylonCenter + v0Left - v0Normal, Color.Transparent, new Vector3(timeValue * 0.4f + k / 7f, 0.6f, 1));
				crack.Add(pylonCenter + v0Normal, new Color(155, 255, 0, 0), new Vector3(timeValue * 0.4f + 0.1f + k / 7f, 0.4f, 0));
				crack.Add(pylonCenter - v0Normal, new Color(155, 255, 0, 0), new Vector3(timeValue * 0.4f + 0.1f + k / 7f, 0.6f, 1));
				crack.Add(pylonCenter - v0Left + v0Normal, Color.Transparent, new Vector3(timeValue * 0.4f + 0.2f + k / 7f, 0.4f, 0));
				crack.Add(pylonCenter - v0Left - v0Normal, Color.Transparent, new Vector3(timeValue * 0.4f + 0.2f + k / 7f, 0.6f, 1));
			}
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			Effect noise = ModAsset.PurpleCrack.Value;
			noise.CurrentTechnique.Passes["Test"].Apply();
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_perlin.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, crack.ToArray(), 0, crack.Count - 2);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);

			List<Vertex2D> chain = new List<Vertex2D>();
			for (int k = 0; k < 6; k++)
			{
				Vector2 v0 = new Vector2(290, 0).RotatedBy(k);
				Vector2 v0Left = Vector2.Normalize(v0.RotatedBy(MathHelper.PiOver2)) * 4f;
				Vector2 v0Normal = -Vector2.Normalize(v0) * 12;
				Vector2 pylonCenter = zero + new Vector2(i + 5, j + 7) * 16 + v0 - Main.screenPosition;
				chain.Add(pylonCenter + v0Left, Color.Transparent, new Vector3(0, 0, 0));
				chain.Add(pylonCenter - v0Left, Color.Transparent, new Vector3(0, 1, 0));
				for (int l = 1; l < 24; l++)
				{
					chain.Add(pylonCenter + v0Left + v0Normal * l, Color.White, new Vector3(l * 0.5f, 0, 0));
					chain.Add(pylonCenter - v0Left + v0Normal * l, Color.White, new Vector3(l * 0.5f, 1, 0));
				}
				chain.Add(pylonCenter + v0Left + v0Normal * 24, Color.Transparent, new Vector3(12, 0, 0));
				chain.Add(pylonCenter - v0Left + v0Normal * 24, Color.Transparent, new Vector3(12, 1, 0));
			}
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.Textures[0] = ModAsset.OriginPylon_chain.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, chain.ToArray(), 0, chain.Count - 2);

			bars = new List<Vertex2D>()
			{
				new Vertex2D(zero + new Vector2(i - 4, j + 9) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.1f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i - 4, j - 10) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.1f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 1, j + 9) * 16 - Main.screenPosition, color0 * 0.7f,new Vector3(0.2f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 1, j - 28) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.2f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 8, j + 9) * 16 - Main.screenPosition, color0 * 0.7f,new Vector3(0.4f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 8, j - 28) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.4f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 13, j + 9) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.5f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 13, j - 10) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.5f, timeValue, 0))
			};
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_perlin.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

			bars = new List<Vertex2D>()
			{
				new Vertex2D(zero + new Vector2(i - 4, j + 9) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.1f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i - 4, j + 10) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.1f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 1, j + 9) * 16 - Main.screenPosition, color0 * 0.7f,new Vector3(0.2f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 1, j + 28) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.2f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 8, j + 9) * 16 - Main.screenPosition, color0 * 0.7f,new Vector3(0.4f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 8, j + 28) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.4f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 13, j + 9) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.5f, timeValue, 0)),
				new Vertex2D(zero + new Vector2(i + 13, j + 10) * 16 - Main.screenPosition, color0 * 0.0f,new Vector3(0.5f, timeValue, 0))
			};
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_perlin.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

			bars = new List<Vertex2D>()
			{
				new Vertex2D(zero + new Vector2(i - 11, j - 8.3f) * 16 - Main.screenPosition, Color.White,new Vector3(0, 0, 0)),
				new Vertex2D(zero + new Vector2(i + 21, j - 8.3f) * 16- Main.screenPosition, Color.White,new Vector3(1, 0, 0)),
				new Vertex2D(zero + new Vector2(i - 11, j + 17) * 16 - Main.screenPosition, Color.White,new Vector3(0, 1, 0)),
				new Vertex2D(zero + new Vector2(i + 21, j + 17) * 16- Main.screenPosition, Color.White,new Vector3(1, 1, 0)),
			};
			Main.graphics.GraphicsDevice.Textures[0] = ModAsset.OriginPylon_hang.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		return false;
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (!Main.gamePaused && YggdrasilWorld.YggdrasilTimer > 5)
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