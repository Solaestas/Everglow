using Everglow.Commons.DataStructures;
using Everglow.Commons.MEAC;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Commons.Templates.Weapons.Clubs;

public abstract class ClubProjSmash : MeleeProj
{
	private static readonly List<int> ownSmashClubPlayers = [];

	public static IReadOnlyCollection<int> OwnSmashClubPlayers => ownSmashClubPlayers;

	protected Player Owner => Main.player[Projectile.owner];

	protected float Omega { get; private set; } = 0;

	protected bool Crash { get; private set; } = false;

	protected int FixedDirection { get; private set; } = 1;

	protected Vector2 StopPoint { get; private set; } = Vector2.Zero;

	protected Queue<Vector2> SmashTrailVecs { get; private set; } = new Queue<Vector2>();

	/// <summary>
	/// Represents whether the club projectile can reflecting. Default to <c>false</c>.
	/// </summary>
	public bool EnableReflection { get; protected set; } = false;

	/// <summary>
	/// Reflection strength. Default to <c>4f</c>.
	/// </summary>
	public float ReflectionStrength { get; protected set; } = 4f;

	public override void SetDefaults()
	{
		Projectile.width = 80;
		Projectile.height = 80;
		Projectile.scale = 1f;

		Projectile.aiStyle = -1;
		Projectile.timeLeft = 300;
		Projectile.extraUpdates = 1;

		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;

		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
		Projectile.DamageType = DamageClass.Melee;

		longHandle = false;
		maxAttackType = 2;
		maxSlashTrailLength = 20;
		shaderType = MEAC.Enums.MeleeTrailShaderType.ArcBladeTransparentedByZ;
		autoEnd = false;

		SetDef();

		slashTrail = new Queue<Vector2>(maxSlashTrailLength + 1);
	}

	public override string TrailShapeTex() => ModAsset.Melee_Mod;

	public override string TrailColorTex() => Texture;

	public override float TrailAlpha(float factor) => base.TrailAlpha(factor) * 1.15f;

	public override BlendState TrailBlendState() => BlendState.NonPremultiplied;

	public override void End()
	{
		Owner.legFrame = new Rectangle(0, 0, Owner.legFrame.Width, Owner.legFrame.Height);
		Owner.fullRotation = 0;
		Owner.legRotation = 0;
		Owner.legPosition = Vector2.Zero;
		Projectile.Kill();
		Owner.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
	}

	public override void OnSpawn(IEntitySource source)
	{
		ownSmashClubPlayers.Add(Projectile.owner);
	}

	public override void AI()
	{
		base.AI();

		TestPlayerDrawer modPlayer = Owner.GetModPlayer<TestPlayerDrawer>();
		modPlayer.HideLeg = true;
		useSlash = true;
		float timeMul = 1 / Owner.meleeSpeed;
		if (currantAttackType == 0)
		{
			if (timer < 14 * timeMul)// 前摇
			{
				useSlash = false;
				LockPlayerDir(Owner);
				float targetRot = -MathHelper.PiOver2 - Owner.direction * 0.5f;
				mainAxisDirection = Vector2.Lerp(mainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul) * Projectile.scale;
				mainAxisDirection += Projectile.DirectionFrom(Owner.Center) * 3;
				Projectile.rotation = mainAxisDirection.ToRotation();
			}
			if (timer == (int)(14 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (timer > 14 * timeMul && timer < 35 * timeMul)
			{
				canHit = true;
				Projectile.rotation += Projectile.spriteDirection * 0.32f / timeMul;

				mainAxisDirection = Vector2.Lerp(mainAxisDirection, Vector2Elipse(110, Projectile.rotation, 0f, -0.3f * Projectile.spriteDirection), 0.4f / timeMul) * Projectile.scale;
				Owner.fullRotationOrigin = new Vector2(10, 42);
				Owner.fullRotation = MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * Owner.direction;
				Owner.legRotation = -Owner.fullRotation;
			}
			if (Owner.gravDir == 1)
			{
				Point bottomPos = Owner.Bottom.ToTileCoordinates();
				bottomPos.X = Math.Clamp(bottomPos.X, 20, Main.maxTilesX - 20);
				bottomPos.Y = Math.Clamp(bottomPos.Y, 20, Main.maxTilesY - 20);
				if (Collision.SolidCollision(Owner.BottomLeft, Owner.width, 64) || TileCollisionUtils.PlatformCollision(Owner.Bottom + new Vector2(0, 16)) || TileCollisionUtils.PlatformCollision(Owner.Bottom + new Vector2(0, 0)) || TileCollisionUtils.PlatformCollision(Owner.Bottom + new Vector2(0, -16)) || ((Owner.waterWalk || Owner.waterWalk2) && Main.tile[bottomPos].LiquidAmount > 0 && !Owner.wet))
				{
					if (timer <= 70)
					{
						Smash(0);
					}
					if (timer > 70)
					{
						Smash(1);
					}
				}
			}
			if (Owner.gravDir == -1)
			{
				if (Collision.SolidCollision(Owner.TopLeft - new Vector2(0, 64), Owner.width, 80))
				{
					if (timer <= 70)
					{
						Smash(0);
					}
					if (timer > 70)
					{
						Smash(1);
					}
				}
			}
			if (timer > 25 * timeMul)
			{
				Owner.velocity.Y += Owner.gravDir;
				LockPlayerDir(Owner);
				float targetRot = -MathHelper.PiOver2 - Owner.direction * 0.5f;
				mainAxisDirection = Vector2.Lerp(mainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul) * Projectile.scale;
				mainAxisDirection += Projectile.DirectionFrom(Owner.Center) * 3;
				Projectile.rotation = mainAxisDirection.ToRotation();
			}
		}
		if (currantAttackType == 1)
		{
			ScreenShaker Gsplayer = Owner.GetModPlayer<ScreenShaker>();
			Gsplayer.FlyCamPosition = new Vector2(0, 14).RotatedByRandom(6.283);
			Owner.direction = FixedDirection;
			if (timer < 8 * timeMul)// 前摇
			{
				useSlash = false;
				float targetRot = -MathHelper.PiOver2 - Owner.direction * 0.5f;
				mainAxisDirection = Vector2.Lerp(mainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul) * Projectile.scale;
				mainAxisDirection += Projectile.DirectionFrom(Owner.Center) * 3;
				Projectile.rotation = mainAxisDirection.ToRotation();
			}
			if (timer == (int)(2 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (timer > 8 * timeMul && timer < 90 * timeMul)
			{
				canHit = true;
				Omega *= 0.98f;
				Projectile.rotation -= Projectile.spriteDirection * Omega / timeMul;
				float theta = 1.16f;
				float phi = MathF.Sin((90 - timer) * (90 - timer) / 1000f) * Owner.direction;
				mainAxisDirection = Vector2.Lerp(mainAxisDirection, Vector2Elipse(110, Projectile.rotation, theta, phi), 0.4f / timeMul) * Projectile.scale;
				Owner.fullRotationOrigin = new Vector2(10, 42);
				Owner.fullRotation = MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * Owner.direction;
				Owner.legRotation = -Owner.fullRotation;
			}
			if (timer > 100 * timeMul)
			{
				End();
				Projectile.Kill();
			}
		}// 强
		if (currantAttackType == 2)
		{
			ScreenShaker Gsplayer = Owner.GetModPlayer<ScreenShaker>();
			Gsplayer.FlyCamPosition = new Vector2(0, 28).RotatedByRandom(6.283);
			Owner.direction = FixedDirection;
			if (timer < 8 * timeMul)// 前摇
			{
				useSlash = false;
				float targetRot = -MathHelper.PiOver2 - Owner.direction * 0.5f;
				mainAxisDirection = Vector2.Lerp(mainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul) * Projectile.scale;
				mainAxisDirection += Projectile.DirectionFrom(Owner.Center) * 3;
				Projectile.rotation = mainAxisDirection.ToRotation();
			}
			if (timer == (int)(2 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (timer > 8 * timeMul && timer < 40 * timeMul)
			{
				canHit = true;
				Omega *= 0.95f;
				Projectile.rotation -= Projectile.spriteDirection * Omega / timeMul;
				float theta = 0.4f + timer / 71f;
				float phi = (-1.2f + timer / 50f) * Owner.direction;
				mainAxisDirection = Vector2.Lerp(mainAxisDirection, Vector2Elipse(90, Projectile.rotation, theta, phi), 0.9f / timeMul) * Projectile.scale;
				Owner.fullRotationOrigin = new Vector2(10, 42);
				Owner.fullRotation = MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * Owner.direction;
				Owner.legRotation = -Owner.fullRotation;
			}
			if (timer > 50 * timeMul)
			{
				End();
				Projectile.Kill();
			}
		}// 弱

		SmashTrailVecs.Enqueue(mainAxisDirection + Projectile.Center);
		if (SmashTrailVecs.Count > maxSlashTrailLength)
		{
			SmashTrailVecs.Dequeue();
		}
	}

	public virtual void Smash(int level)
	{
		if (level == 1)// 强
		{
			SoundEngine.PlaySound(SoundID.Research);
			currantAttackType = 1;
			Omega = 0.8f;
			for (int g = 0; g < 24; g++)
			{
				Vector2 newVelocity = new Vector2(0, -Main.rand.NextFloat(35f, 74f) * Owner.gravDir).RotatedBy(Main.rand.NextFloat(-1.4f, 1.4f));
				Vector2 pos = Owner.Bottom + new Vector2(0, 48) - newVelocity * 0.2f;
				if (Owner.gravDir == -1)
				{
					pos = Owner.Top - new Vector2(0, 48) - newVelocity * 0.2f;
				}
				var somg = new ClubSmog
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = pos,
					maxTime = Main.rand.Next(15, 48),
					scale = Main.rand.NextFloat(40f, 60f),
					ai = new float[] { 0, 0 },
				};
				Ins.VFXManager.Add(somg);
			}
			Projectile.damage = (int)(Projectile.damage * 1.85f);
		}
		if (level == 0)// 弱
		{
			SoundEngine.PlaySound(SoundID.DrumFloorTom);
			currantAttackType = 2;
			Omega = 0.4f;
			for (int g = 0; g < 12; g++)
			{
				Vector2 newVelocity = new Vector2(0, -Main.rand.NextFloat(35f, 44f) * Owner.gravDir).RotatedBy(Main.rand.NextFloat(-1.4f, 1.4f));
				Vector2 pos = Owner.Bottom + new Vector2(0, 48) - newVelocity * 0.2f;
				if (Owner.gravDir == -1)
				{
					pos = Owner.Top - new Vector2(0, 48) - newVelocity * 0.2f;
				}
				var somg = new ClubSmog
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = pos,
					maxTime = Main.rand.Next(15, 38),
					scale = Main.rand.NextFloat(40f, 60f),
					ai = new float[] { 0, 0 },
				};
				Ins.VFXManager.Add(somg);
			}
		}
		StopPoint = Owner.Bottom;
		Crash = true;
		timer = 0;

		FixedDirection = Owner.direction;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawSmashTrail(lightColor);
		DrawSelf(Main.spriteBatch, lightColor);
		return false;
	}

	public virtual void DrawSmashTrail(Color color)
	{
		if (!SmashTrailVecs.Smooth(out var smoothedTrail))
		{
			return;
		}

		var length = smoothedTrail.Count;
		var bars = new List<Vertex2D>();
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);

			float w;
			if (!EnableReflection)
			{
				w = TrailAlpha(factor);
			}
			else
			{
				Vector2 trailSelfPos = smoothedTrail[i] - Projectile.Center;
				w = 1 - Math.Abs((trailSelfPos.X * 0.5f + trailSelfPos.Y * 0.5f) / trailSelfPos.Length());
				float w2 = MathF.Sqrt(TrailAlpha(factor));
				w *= w2 * w;
				w *= ReflectionStrength;
			}

			Color c0 = i == 0 ? Color.Transparent : Color.White;

			bars.Add(new Vertex2D(Projectile.Center, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(smoothedTrail[i], c0, new Vector3(factor, 0, w)));
		}

		var vColor = color.ToVector4();
		if (!EnableReflection)
		{
			vColor.W *= 0.1f;
		}
		else
		{
			vColor.W *= 0.15f;
		}

		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);

		Effect MeleeTrail = ModAsset.ClubTrail.Value;
		MeleeTrail.Parameters["uTransform"].SetValue(Main.GameViewMatrix.TransformationMatrix_WorldToScreen());
		MeleeTrail.Parameters["tex0"].SetValue(ModAsset.Noise_flame_0.Value);
		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.Parameters["Light"].SetValue(vColor);
		MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public override void OnKill(int timeLeft)
	{
		Owner.fullRotation = 0;
		ownSmashClubPlayers.RemoveAll(p => p == Projectile.owner);
	}
}