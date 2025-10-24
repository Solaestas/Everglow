using Everglow.Commons.DataStructures;
using Everglow.Commons.Templates.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Summon;

public class WiltedForestLamp_Proj_shoot : TrailingProjectile
{
	public override string Texture => Commons.ModAsset.Empty_Mod;

	public override void SetDefaults()
	{
		base.SetDefaults();
		Projectile.timeLeft = 12000;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 6;
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.DamageType = DamageClass.Summon;
		TrailColor = new Color(0.7f, 0.7f, 0.2f, 0);
		TrailWidth = 6;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
		ProjectileID.Sets.DrawScreenCheckFluff[Type] = 180000;
	}

	public Vector2 OldVel = default;

	public override void OnSpawn(IEntitySource source) => base.OnSpawn(source);

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		NPC npc = Projectile.FindTargetWithinRange(800);
		int playerTarget = player.MinionAttackTargetNPC;
		if (playerTarget is >= 0 and < 200)
		{
			npc = Main.npc[playerTarget];
		}
		Vector2 oldOldVel = OldVel;
		if (npc == null || !npc.active)
		{
			OldVel = Projectile.velocity;
			Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 12;
			var dustVFX = new Leaf_VFX
			{
				velocity = new Vector2(0, 0.5f).RotatedByRandom(Math.PI * 2) + Projectile.velocity,
				omega = 0,
				beta = 0,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(70, 120),
				scale = Main.rand.Next(8, 10),
				color = new Color(0.1f, 1f, 0.4f, 1),
				ai = new float[] { Main.rand.NextFloat(1f, 8f) },
			};
			Ins.VFXManager.Add(dustVFX);
			base.AI();
			return;
		}
		OldVel = Projectile.velocity;
		var toTarget = Vector2.Normalize(npc.Center - Projectile.Center - Projectile.velocity);
		Projectile.velocity = Projectile.velocity * 0.4f + toTarget;
		Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 12;
		float omegaVel = Vector3.Cross(new Vector3(OldVel / 12f, 0), new Vector3(Projectile.velocity / 12f, 0)).Z;
		omegaVel = MathF.Asin(omegaVel);
		float betaVel = Vector3.Cross(new Vector3(oldOldVel / 12f, 0), new Vector3(OldVel / 12f, 0)).Z;
		betaVel = MathF.Asin(betaVel);
		betaVel = omegaVel - betaVel;
		if (TimeTokill <= 0 && Projectile.timeLeft < 11998)
		{
			var dustVFX = new Leaf_VFX
			{
				velocity = new Vector2(0, 0.5f).RotatedByRandom(Math.PI * 2) + Projectile.velocity,
				omega = omegaVel,
				beta = betaVel,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(70, 120),
				scale = Main.rand.Next(8, 10),
				color = new Color(0.1f, 1f, 0.4f, 1),
				ai = new float[] { Main.rand.NextFloat(1f, 8f) },
			};
			Ins.VFXManager.Add(dustVFX);
		}
		var colorLight = new Vector3(0.4f, 0.7f, 0.2f);
		if(TimeTokill > 0)
		{
			colorLight *= TimeTokill / 40f;
		}
		Lighting.AddLight(Projectile.Center, colorLight);
		base.AI();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		if (TimeTokill <= 0)
		{
			DrawSelf();
		}
		return false;
	}

	public override void DrawSelf() => base.DrawSelf();

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

		var bars4 = new List<Vertex2D>();
		var bars5 = new List<Vertex2D>();
		var bars6 = new List<Vertex2D>();

		var bars7 = new List<Vertex2D>();
		var bars8 = new List<Vertex2D>();
		var bars9 = new List<Vertex2D>();
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float timeValue = (float)Main.time * 0.0005f;
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			var drawC = Color.Lerp(TrailColor, new Color(0.4f, 0.1f, 0f, 0f), i / (float)SmoothTrail.Count);

			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));

			var coverColor = new Color(0.05f, 0.7f, 0.3f, 0);
			Vector4 coverEnvironment = coverColor.ToVector4() * Lighting.GetColor(drawPos.ToTileCoordinates()).ToVector4();
			coverColor = new Color(coverEnvironment.X, coverEnvironment.Y, coverEnvironment.Z, coverEnvironment.W);
			if (i > 12)
			{
				coverColor *= MathF.Max(0, (25 - i) / 13f);
			}
			if (i > SmoothTrail.Count - 5)
			{
				coverColor *= (SmoothTrail.Count - i) / 5f;
			}
			float factor2 = i / (float)SmoothTrail.Count * mulFac * 4.5f;
			factor2 -= timeValue * 100;
			float headWidth = 2f;
			bars4.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth * headWidth, coverColor, new Vector3(factor2 + timeValue, 0f, width)));
			bars4.Add(new Vertex2D(drawPos, coverColor, new Vector3(factor2 + timeValue, 0.5f, width)));
			bars5.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth * headWidth, coverColor, new Vector3(factor2 + timeValue, 1, width)));
			bars5.Add(new Vertex2D(drawPos, coverColor, new Vector3(factor2 + timeValue, 0.5f, width)));
			bars6.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth * headWidth, coverColor, new Vector3(factor2 + timeValue, 0, width)));
			bars6.Add(new Vertex2D(drawPos, coverColor, new Vector3(factor2 + timeValue, 0.5f, width)));

			Color coverColor2 = Color.Black;
			if (i > 13)
			{
				coverColor2 *= MathF.Max(0, (25 - i) / 13f);
			}
			if (i > SmoothTrail.Count - 5)
			{
				coverColor2 *= (SmoothTrail.Count - i) / 5f;
			}
			bars7.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth * headWidth, coverColor2, new Vector3(factor2 + timeValue, 0f, width)));
			bars7.Add(new Vertex2D(drawPos, coverColor2, new Vector3(factor2 + timeValue, 0.5f, width)));
			bars8.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth * headWidth, coverColor2, new Vector3(factor2 + timeValue, 1, width)));
			bars8.Add(new Vertex2D(drawPos, coverColor2, new Vector3(factor2 + timeValue, 0.5f, width)));
			bars9.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth * headWidth, coverColor2, new Vector3(factor2 + timeValue, 0, width)));
			bars9.Add(new Vertex2D(drawPos, coverColor2, new Vector3(factor2 + timeValue, 0.5f, width)));
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
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_7.Value;
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
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect2 = ModAsset.WiltedForestLamp_Proj_shoot_dissolve.Value;
		effect2.Parameters["uTransform"].SetValue(model * projection);
		effect2.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_flame_2.Value;
		if (bars7.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars7.ToArray(), 0, bars7.Count - 2);
		}

		if (bars8.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars8.ToArray(), 0, bars8.Count - 2);
		}

		if (bars9.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars9.ToArray(), 0, bars9.Count - 2);
		}
		if (bars4.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars4.ToArray(), 0, bars4.Count - 2);
		}

		if (bars5.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars5.ToArray(), 0, bars5.Count - 2);
		}

		if (bars6.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars6.ToArray(), 0, bars6.Count - 2);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public override void DrawTrailDark()
	{
	}

	public override void KillMainStructure()
	{
		for (int i = 0; i < 18; i++)
		{
			var dust = Dust.NewDustDirect(Projectile.Center, 2, 2, ModContent.DustType<LeafMagic>(), Projectile.velocity.X, Projectile.velocity.Y);
			dust.velocity = Projectile.velocity.RotateRandom(0.4) * Main.rand.NextFloat(0.7f, 1.1f);
			dust.scale = Main.rand.NextFloat(0.3f, 1.1f);
		}
		base.KillMainStructure();
	}
}