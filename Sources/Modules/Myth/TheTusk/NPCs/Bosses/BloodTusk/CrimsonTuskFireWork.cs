using Everglow.Myth.TheTusk;
using Everglow.Myth.TheTusk.Projectiles;
using Everglow.Myth.TheTusk.Projectiles.Weapon;
using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;

public class CrimsonTuskFireWork : ModNPC
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Tusk Spike Fireworks");
		DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "獠牙喷射爪");
	}
	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 30;
		if (Main.expertMode)
			NPC.damage = 60;
		if (Main.masterMode)
			NPC.damage = 90;
		for (int i = 0; i < NPC.buffImmune.Length; i++)
		{
			NPC.buffImmune[i] = true;
		}
		NPC.width = 30;
		NPC.height = 30;
		NPC.defense = 0;
		NPC.lifeMax = 5;
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 0, 0, 0);
		NPC.aiStyle = -1;
		NPC.alpha = 0;
		NPC.lavaImmune = true;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.dontTakeDamage = true;
	}

	private bool Shoot = false;
	private int Bomb = 241;
	private float[] lengTusk = new float[12];
	private float[] delayTusk = new float[12];
	private float[] rotTusk = new float[12];
	private float width = 9;
	public override void AI()
	{
		NPC.TargetClosest(true);
		Player player = Main.player[NPC.target];
		if (!Shoot)
		{
			for (int g = 0; g < 12; g++)
			{
				lengTusk[g] = Main.rand.NextFloat(27f, 38f);
				delayTusk[g] = Main.rand.NextFloat(0f, 74f);
				rotTusk[g] = Main.rand.NextFloat(0f, 0.283f) + (float)(Math.PI * g / 6d);
			}
			Shoot = true;
			NPC.velocity = new Vector2(0, -Main.rand.NextFloat(7f, 10f));
		}
		if (NPC.velocity.Y < 0)
			NPC.velocity.Y += 0.15f;
		else
		{
			if (Bomb > 0)
			{
				NPC.velocity.Y *= 0;
				Bomb--;
			}
			else
			{
				width -= 0.3f;
				if (width <= 0)
					NPC.active = false;
			}
		}
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return false;
	}
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		var bars2 = new List<VertexBase.CustomVertexInfo>();

		for (int i = 1; i < 400; ++i)
		{
			Vector2 VPos = NPC.Center + new Vector2(0, i * 5f);
			Color colori = Lighting.GetColor((int)(VPos.X / 16d), (int)(VPos.Y / 16d));
			var factor = Math.Abs(i / 30f % 2 - 1);
			float width2 = width;
			if (Bomb is <= 240 and > 60)
			{
				if (i == 1)
					width2 += (240 - Bomb) / 12f;
				if (i == 2)
					width2 += (240 - Bomb) / 180f * 23f;
				if (i == 3)
					width2 += (240 - Bomb) / 12f;
				if (i == 4)
					width2 += (240 - Bomb) / 60f;
			}
			if (Bomb is <= 60 and > 5)
			{
				if (i == 1)
					width2 = 24;
				if (i == 2)
					width2 = 32;
				if (i == 3)
					width2 = 24;
				if (i == 4)
					width2 = 12;
				float Addc = (float)(Math.Sin(Bomb / 15f * Math.PI) + 1) / 2f;
				colori.R = (byte)Math.Clamp(colori.R + (int)(Addc * 255f), 0, 255);
				colori.G = (byte)Math.Clamp(colori.G + (int)(Addc * 255f), 0, 255);
				colori.B = (byte)Math.Clamp(colori.B + (int)(Addc * 255f), 0, 255);
			}
			if (Bomb <= 5)
			{
				if (i == 1)
					width2 = 0;
				if (i == 2)
					width2 = 0;
				if (i == 3)
					width2 = 0;
				if (i == 4)
					width2 *= 0.5f;
			}
			if (width <= 3)
			{
				if (Main.rand.NextBool(5))
				{
					Vector2 vd = new Vector2(0, Main.rand.NextFloat(-1.3f, -0.5f)).RotatedByRandom(6.283);
					int a = Dust.NewDust(VPos - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(-8f, 8f)), 0, 0, DustID.Blood, vd.X, vd.Y, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
					Main.dust[a].noGravity = true;
				}
			}
			if (Bomb == 5 && i == 1 && !Main.gamePaused)
			{
				// 弹幕
				Projectile.NewProjectile(null, VPos + new Vector2(0, 4), Vector2.Zero, ModContent.ProjectileType<ToothMagicHit>(), 0, 0);
				for (int g = 0; g < 12; g++)
				{
					Projectile.NewProjectile(null, VPos + new Vector2(0, 4), new Vector2(lengTusk[g] / 2f, 0).RotatedBy(rotTusk[g]), ModContent.ProjectileType<CrimsonTuskProj>(), NPC.damage / 12, 0);
				}
				for (int f = 0; f < 75; f++)
				{
					Vector2 vd = new Vector2(0, Main.rand.NextFloat(-13f, -8f)).RotatedByRandom(6.283);
					int a = Dust.NewDust(NPC.Center - new Vector2(4, 4), 0, 0, DustID.Blood, vd.X, vd.Y, 0, default, Main.rand.NextFloat(0.8f, 1.5f));
					Main.dust[a].noGravity = true;
				}
			}
			if (Bomb < 240 && Bomb > 5 && i == 10)
			{
				for (int g = 0; g < 12; g++)
				{
					var Tusk = new List<VertexBase.CustomVertexInfo>();
					float size = Math.Clamp((240 - Bomb - delayTusk[g]) / 66f, 0, 1f);
					Tusk.Add(new VertexBase.CustomVertexInfo(NPC.Center + new Vector2(lengTusk[g] * size * 2.5f, 0).RotatedBy(rotTusk[g]) - Main.screenPosition, colori, new Vector3(0.5f, 0, 0)));
					Tusk.Add(new VertexBase.CustomVertexInfo(NPC.Center + new Vector2(0, -9 * size).RotatedBy(rotTusk[g]) - Main.screenPosition, colori, new Vector3(0, 1, 0)));
					Tusk.Add(new VertexBase.CustomVertexInfo(NPC.Center + new Vector2(0, 9 * size).RotatedBy(rotTusk[g]) - Main.screenPosition, colori, new Vector3(1, 1, 0)));
					Texture2D t1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/CrimsonTuskflip").Value;
					Main.graphics.GraphicsDevice.Textures[0] = t1;//GlodenBloodScaleMirror
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Tusk.ToArray(), 0, Tusk.Count / 3);
				}
			}
			if (Bomb < 60 && Bomb > 5 && i == 1)
			{
				float yd = 1;
				if (Bomb > 45)
					yd = (60 - Bomb) / 15f;
				if (Bomb < 20)
				{
					yd = (Bomb - 5) / 15f;
					yd *= yd;
				}
				for (int g = 0; g < 12; g++)
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
					var bars = new List<VertexBase.CustomVertexInfo>();
					var barsB = new List<VertexBase.CustomVertexInfo>();

					float step = 4;
					int Count = 0;
					for (int m = 0; m < 500; ++m)
					{
						if (Collision.SolidCollision(NPC.Center + new Vector2(step, 0).RotatedBy(rotTusk[g]) * m, 1, 1))
							break;
						Vlaser[g, m] = NPC.Center + new Vector2(step, 0).RotatedBy(rotTusk[g]) * m;
						++Count;
					}
					for (int iz = 1; iz < Count; ++iz)
					{
						if (Vlaser[g, iz] == Vector2.Zero)
							break;

						var normalDir = Vlaser[g, iz - 1] - Vlaser[g, iz];
						normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

						var lerpvalue = (float)Math.Sqrt(iz + 1) / 9f + Bomb / 60f;
						var w = MathHelper.Lerp(1f, 0.05f, lerpvalue);
						float width3 = 12;
						if (iz <= 25)
							width3 = 12 * (float)Math.Sqrt(iz) / 5f;

						width3 *= yd;
						if (Count - iz < 5)
						{
							int sc = Math.Clamp((int)(255 * (Count - iz - 1) / 5f * (200 - iz) / 200f), 0, 255);
							var c0 = new Color(sc, 0, 0, 0);
							var c1 = new Color(sc / 3, sc / 3, sc / 3, sc / 3);
							if ((iz + Bomb * 2) % 15 < 9)
								c0 *= 0;
							bars.Add(new VertexBase.CustomVertexInfo(Vlaser[g, iz] + normalDir * width3 - Main.screenPosition, c0, new Vector3(lerpvalue % 1f, 1, w)));
							bars.Add(new VertexBase.CustomVertexInfo(Vlaser[g, iz] + normalDir * -width3 - Main.screenPosition, c0, new Vector3(lerpvalue % 1f, 0, w)));
							barsB.Add(new VertexBase.CustomVertexInfo(Vlaser[g, iz] + normalDir * width3 - Main.screenPosition, c1, new Vector3(lerpvalue % 1f, 1, w)));
							barsB.Add(new VertexBase.CustomVertexInfo(Vlaser[g, iz] + normalDir * -width3 - Main.screenPosition, c1, new Vector3(lerpvalue % 1f, 0, w)));
						}
						else
						{
							int sc = Math.Clamp((int)(255 * (200 - iz) / 200f), 0, 255);
							var c0 = new Color(sc, 0, 0, 0);
							var c1 = new Color(sc / 3, sc / 3, sc / 3, sc / 3);
							if ((iz + Bomb * 2) % 15 < 9)
								c0 *= 0;
							bars.Add(new VertexBase.CustomVertexInfo(Vlaser[g, iz] + normalDir * width3 - Main.screenPosition, c0, new Vector3(lerpvalue % 1f, 1, w)));
							bars.Add(new VertexBase.CustomVertexInfo(Vlaser[g, iz] + normalDir * -width3 - Main.screenPosition, c0, new Vector3(lerpvalue % 1f, 0, w)));
							barsB.Add(new VertexBase.CustomVertexInfo(Vlaser[g, iz] + normalDir * width3 - Main.screenPosition, c1, new Vector3(lerpvalue % 1f, 1, w)));
							barsB.Add(new VertexBase.CustomVertexInfo(Vlaser[g, iz] + normalDir * -width3 - Main.screenPosition, c1, new Vector3(lerpvalue % 1f, 0, w)));
						}
					}
					var Vx = new List<VertexBase.CustomVertexInfo>();
					if (bars.Count > 2)
					{
						Vx.Add(bars[0]);
						var vertex = new VertexBase.CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + new Vector2(-5, 0).RotatedBy(rotTusk[g]), new Color(255, 0, 0, 0), new Vector3(0, 0.5f, 1));
						Vx.Add(bars[1]);
						Vx.Add(vertex);
						for (int iz = 0; iz < bars.Count - 2; iz += 2)
						{
							Vx.Add(bars[iz]);
							Vx.Add(bars[iz + 2]);
							Vx.Add(bars[iz + 1]);

							Vx.Add(bars[iz + 1]);
							Vx.Add(bars[iz + 2]);
							Vx.Add(bars[iz + 3]);
						}
					}

					var VxB = new List<VertexBase.CustomVertexInfo>();
					if (barsB.Count > 2)
					{
						VxB.Add(barsB[0]);
						var vertex = new VertexBase.CustomVertexInfo((barsB[0].Position + barsB[1].Position) * 0.5f + new Vector2(-5, 0).RotatedBy(rotTusk[g]), new Color(255, 0, 0, 0), new Vector3(0, 0.5f, 1));
						VxB.Add(barsB[1]);
						VxB.Add(vertex);
						for (int iz = 0; iz < barsB.Count - 2; iz += 2)
						{
							VxB.Add(barsB[iz]);
							VxB.Add(barsB[iz + 2]);
							VxB.Add(barsB[iz + 1]);

							VxB.Add(barsB[iz + 1]);
							VxB.Add(barsB[iz + 2]);
							VxB.Add(barsB[iz + 3]);
						}
					}
					Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/TuskShade").Value;
					Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxB.ToArray(), 0, VxB.Count / 3);

					t = ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/Projectiles/AcytaeaLaser").Value;
					Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);


				}
			}
			bars2.Add(new VertexBase.CustomVertexInfo(VPos + new Vector2(width2, 0) - Main.screenPosition, colori, new Vector3(factor, 1, 0)));
			bars2.Add(new VertexBase.CustomVertexInfo(VPos + new Vector2(-width2, 0) - Main.screenPosition, colori, new Vector3(factor, 0, 0)));
			if (Collision.SolidCollision(VPos, 1, 1))
				break;
		}
		var triangleList2 = new List<VertexBase.CustomVertexInfo>();
		if (bars2.Count > 2)
		{
			Vector2 VPos = NPC.Center;
			Color colori = Lighting.GetColor((int)(VPos.X / 16d), (int)(VPos.Y / 16d));
			triangleList2.Add(bars2[0]);
			var vertex = new VertexBase.CustomVertexInfo((bars2[0].Position + bars2[1].Position) * 0.5f + new Vector2(0, -5), colori, new Vector3(0, 0.5f, 0));
			triangleList2.Add(bars2[1]);
			triangleList2.Add(vertex);
			for (int i = 0; i < bars2.Count - 2; i += 2)
			{
				triangleList2.Add(bars2[i]);
				triangleList2.Add(bars2[i + 2]);
				triangleList2.Add(bars2[i + 1]);

				triangleList2.Add(bars2[i + 1]);
				triangleList2.Add(bars2[i + 2]);
				triangleList2.Add(bars2[i + 3]);
			}
			Texture2D t1 = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/Tusk/BloodRope").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t1;//GlodenBloodScaleMirror
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList2.ToArray(), 0, triangleList2.Count / 3);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		CirR0 += 0.001f;
		CirPro0 += 0.3f;
	}

	private float CirR0 = 0;
	private float CirPro0 = 0;
	private Vector2[,] Vlaser = new Vector2[12, 501];
}
