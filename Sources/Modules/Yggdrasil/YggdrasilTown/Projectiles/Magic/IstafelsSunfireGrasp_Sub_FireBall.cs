using Everglow.Commons.DataStructures;
using Everglow.Commons.Templates.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.VFXs.IstafelsEffects;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Magic;

public class IstafelsSunfireGrasp_Sub_FireBall : TrailingProjectile
{
	public Vector2 SpawnPos;

	public Projectile MotherProj = default;

	public override string Texture => Commons.ModAsset.Point_Mod;

	public override void SetDefaults()
	{
		base.SetDefaults();
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
		TrailColor = new Color(1, 1, 1, 0f);
		TrailWidth = 20f;
		TrailTexture = Commons.ModAsset.Trail_8.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_8_black.Value;
		SelfLuminous = false;
		Projectile.magic = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
		foreach (var proj in Main.projectile)
		{
			if (proj is not null && proj.active)
			{
				if (proj.type == ModContent.ProjectileType<IstafelsSunfireGrasp_FireBall>())
				{
					if ((proj.Center - Projectile.Center).Length() < 30)
					{
						MotherProj = proj;
						break;
					}
				}
			}
		}
		if (MotherProj == default)
		{
			Projectile.active = false;
			return;
		}
		Projectile.hide = true;
		Timer = 0;
	}

	public override void AI()
	{
		if (MotherProj == default || !MotherProj.active || MotherProj.type != ModContent.ProjectileType<IstafelsSunfireGrasp_FireBall>())
		{
			Projectile.active = false;
			return;
		}
		if (Timer < 60)
		{
			Timer++;
			Projectile.Center = MotherProj.Center + (Projectile.velocity + MotherProj.velocity).NormalizeSafe() * MotherProj.width * 0.2f - Projectile.velocity;
			return;
		}
		if (Timer == 60)
		{
			for (int i = 0; i < 12; i++)
			{
				Vector2 afterVelocity = Projectile.velocity * Main.rand.NextFloat(0, i / 10f) + new Vector2(0, Main.rand.NextFloat(0, (12 - i) / 10f)).RotatedByRandom(MathHelper.TwoPi);
				float mulScale = Main.rand.NextFloat(4f, 8f);
				var drop = new IstafelsSunfireDrop
				{
					velocity = afterVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center,
					maxTime = Main.rand.Next(122, 204),
					scale = mulScale,
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
				};
				Ins.VFXManager.Add(drop);
			}
		}
		if(Timer > 60)
		{
			if(Main.rand.NextBool(12))
			{
				Vector2 afterVelocity = Projectile.velocity * Main.rand.NextFloat(0.4f, 1);
				float mulScale = Main.rand.NextFloat(4f, 8f);
				var drop = new IstafelsSunfireDrop
				{
					velocity = afterVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center,
					maxTime = Main.rand.Next(62, 124),
					scale = mulScale,
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
				};
				Ins.VFXManager.Add(drop);
			}
			var target = ProjectileUtils.FindTarget(Projectile.Center, 2000);
			if (target >= 0 && Collision.CanHit(Main.npc[target], Projectile))
			{
				var direction = (Main.npc[target].Center - Projectile.Center - Projectile.velocity).NormalizeSafe();
				Projectile.velocity = Projectile.velocity * 0.9f + direction * 0.1f * 12f;
			}
		}
		base.AI();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrailDark();
		DrawTrail();
		if (TimeTokill <= 0)
		{
			DrawSelf();
		}
		return false;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		overPlayers.Add(index);
		base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
	}

	public override void DrawSelf()
	{
		if (Timer < 60)
		{
			float drawSize = Timer / 60f * 0.3f;
			Texture2D star = Commons.ModAsset.StarSlash.Value;
			var drawColor = new Color(1f, 0.7f, 0.6f, 0f);
			Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, drawColor, 0, star.Size() * 0.5f, drawSize, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, drawColor, MathHelper.PiOver2, star.Size() * 0.5f, drawSize, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, drawColor * 0.5f, MathHelper.PiOver4, star.Size() * 0.5f, drawSize * 0.5f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, drawColor * 0.5f, -MathHelper.PiOver4, star.Size() * 0.5f, drawSize * 0.5f, SpriteEffects.None, 0);
		}
		else
		{
			Texture2D proj = Commons.ModAsset.EllipsesProj.Value;
			Main.spriteBatch.Draw(proj, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.7f, 0.1f, 0.8f), Projectile.rotation, proj.Size() * 0.5f, 0.2f, SpriteEffects.None, 0);
		}
	}

	public override void DrawTrail()
	{
		var unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> smoothTrail_current = GraphicsUtils.CatmullRom(unSmoothPos); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < smoothTrail_current.Count - 1; x++)
		{
			SmoothTrail.Add(smoothTrail_current[x]);
		}
		if (unSmoothPos.Count != 0)
		{
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);
		}

		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)Main.time * 0.0005f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC;
			if (i < 6)
			{
				drawC = Color.Lerp(new Color(1f, 0.8f, 0.05f, 0), new Color(1f, 0.2f, 0f, 0), i / 6f);
			}
			else if (i is >= 6 and < 12)
			{
				drawC = Color.Lerp(new Color(1f, 0.2f, 0f, 0), new Color(0f, 0f, 0f, 0), (i - 6) / 6f);
			}
			else
			{
				drawC = new Color(0, 0, 0, 0);
			}
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
		}
		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTexture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}

		if (bars2.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		}

		if (bars3.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public override void DrawTrailDark()
	{
		var unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> smoothTrail_current = GraphicsUtils.CatmullRom(unSmoothPos); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < smoothTrail_current.Count - 1; x++)
		{
			SmoothTrail.Add(smoothTrail_current[x]);
		}
		if (unSmoothPos.Count != 0)
		{
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);
		}

		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = SmoothTrail.Count - 1; i > 0; --i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)Main.time * 0.06f;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = Color.White * 0.4f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 0, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
		}
		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTextureBlack;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}

		if (bars2.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		}

		if (bars3.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public override void KillMainStructure()
	{
		for (int i = 0; i < 8; i++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(4f)).RotatedByRandom(MathHelper.TwoPi);
			float mulScale = Main.rand.NextFloat(4f, 8f);
			var drop = new IstafelsSunfireDrop
			{
				velocity = afterVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(122, 204),
				scale = mulScale,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
			};
			Ins.VFXManager.Add(drop);
		}
		base.KillMainStructure();
	}

	public override void OnKill(int timeLeft) => base.OnKill(timeLeft);
}