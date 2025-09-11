using Everglow.Commons.DataStructures;
using Everglow.Commons.Templates.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Magic;

public class BronzeLotusLamp_Blossom : TrailingProjectile
{
	private const int SearchDistance = 600;

	private int targetWhoAmI = -1;

	private int TargetWhoAmI
	{
		get => targetWhoAmI;
		set => targetWhoAmI = value;
	}

	private NPC Target => Main.npc[TargetWhoAmI];

	private bool HasTarget => TargetWhoAmI >= 0;

	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 2400;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 30;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 21;
		TrailColor = new Color(0, 1f, 0.96f, 0f);
		TrailWidth = 20f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail_10.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_10_black.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
	}

	public override void OnSpawn(IEntitySource source)
	{
		targetWhoAmI = FindTarget(Projectile.Center);
	}

	public override void AI()
	{
		base.AI();

		if (TimeAfterEntityDestroy <= 0)
		{
			Projectile.velocity.Y += 0.5f;
			Projectile.velocity = Projectile.velocity.RotatedBy(Math.Sin(Main.time * 0.16 + Projectile.whoAmI) * 0.03f);
			Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 8;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			float size = Main.rand.NextFloat(0.1f, 0.96f);
			var lotusFlame = new CyanLotusFlameDust
			{
				Velocity = new Vector2(0, Main.rand.NextFloat(0.3f, 1f)).RotatedByRandom(MathHelper.TwoPi),
				Active = true,
				Visible = true,
				Position = Projectile.Center,
				MaxTime = Main.rand.Next(24, 36),
				Scale = 14f * size,
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
				Frame = Main.rand.Next(3),
				ai = new float[] { Main.rand.NextFloat(-0.8f, 0.8f) },
			};
			Ins.VFXManager.Add(lotusFlame);
			if (HasTarget)
			{
				if (!Target.active || Target.friendly || !Target.CanBeChasedBy())
				{
					targetWhoAmI = -1;
					return;
				}
				else
				{
					var targetPos = HasTarget ? Main.npc[targetWhoAmI].Center : Vector2.Zero;
					var targetVel = Vector2.Normalize(targetPos - Projectile.Center) * 10f;
					Projectile.velocity = (targetVel + Projectile.velocity * 10) / 11f;
				}
			}
			else
			{
				// If there is no target after spawned, search target by projectile center.
				targetWhoAmI = FindTarget(Projectile.Center);
			}
		}
		else
		{
			Projectile.velocity *= 0.1f;
		}
	}

	/// <summary>
	/// Find closest target by given position.
	/// </summary>
	/// <param name="fromWhere"></param>
	/// <returns></returns>
	public int FindTarget(Vector2 fromWhere)
	{
		int target = -1;
		float minDis = SearchDistance;
		foreach (NPC npc in Main.npc)
		{
			if (npc != null && npc.active)
			{
				if (npc.CanBeChasedBy() && !npc.dontTakeDamage && npc.life > 0)
				{
					float dis = (npc.Center - fromWhere).Length() - npc.Hitbox.Size().Length() * 0.5f;
					if (dis < minDis)
					{
						minDis = dis;
						target = npc.whoAmI;
					}
				}
			}
		}
		return target;
	}

	public override void DestroyEntity()
	{
		int n = 6;
		Vector2 shootSpeed = new Vector2(0, Main.rand.NextFloat(4, 8)).RotatedByRandom(MathHelper.TwoPi);
		for (int i = 0; i < n; i++)
		{
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, shootSpeed.RotatedBy(i / 6d * MathHelper.TwoPi) * 0.5f, ModContent.ProjectileType<BronzeLotusLamp_SubBlossom>(), 0, 0, Projectile.owner, 0);

			// Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, shootSpeed.RotatedBy((i + 0.5f) / 6d * MathHelper.TwoPi) * 0.75f, ModContent.ProjectileType<BronzeLotusLamp_SubBlossom>(), 0, 0, Projectile.owner, 1);
			for (int j = 0; j < 6; j++)
			{
				var lotusFlame = new CyanLotusFlameDust
				{
					Velocity = shootSpeed.RotatedBy((i + 0.5f) / 6d * MathHelper.TwoPi) * (j + 6) * 0.05f,
					Active = true,
					Visible = true,
					Position = Projectile.Center,
					MaxTime = Main.rand.Next(30, 34) + j * 8,
					Scale = 8f,
					Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
					Frame = Main.rand.Next(3),
					ai = new float[] { Main.rand.NextFloat(-0.8f, 0.8f) },
				};
				Ins.VFXManager.Add(lotusFlame);
			}
		}
		Projectile.height = Projectile.width = (int)(15 * shootSpeed.Length());
		base.DestroyEntity();
		Projectile.friendly = true;
		Projectile.tileCollide = false;
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (TimeAfterEntityDestroy < 0)
		{
			DestroyEntity();
		}
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (TimeAfterEntityDestroy < 0)
		{
			DestroyEntity();
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		if (TimeAfterEntityDestroy <= 0)
		{
			Texture2D texture = ModAsset.BronzeLotusLamp_Blossom.Value;
			Vector2 drawCenter = Projectile.Center - Main.screenPosition;
			lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
			Main.EntitySpriteDraw(texture, drawCenter, null, new Color(1f, 1f, 1f, 0f), Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		}
		return false;
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
			float timeValue = (float)-Main.time * 0.014f;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = TrailColor;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth * Projectile.scale, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth * Projectile.scale, drawC, new Vector3(factor + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth * Projectile.scale, drawC, new Vector3(factor + timeValue, 1, width)));
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
}