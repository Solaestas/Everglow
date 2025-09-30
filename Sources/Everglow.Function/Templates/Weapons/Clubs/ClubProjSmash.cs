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

	public override void SetDef()
	{
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 300;
		Projectile.extraUpdates = 1;
		Projectile.scale = 1f;
		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
		Projectile.DamageType = DamageClass.Melee;

		Projectile.width = 80;
		Projectile.height = 80;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		longHandle = false;
		MaxAttackType = 2;
		MaxSlashTrailLength = 20;
		shaderType = Commons.MEAC.Enums.MeleeTrailShaderType.ArcBladeTransparentedByZ;
		AutoEnd = false;
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
		UseTrail = true;
		float timeMul = 1 / Owner.meleeSpeed;
		if (CurrantAttackType == 0)
		{
			if (Timer < 14 * timeMul)// 前摇
			{
				UseTrail = false;
				LockPlayerDir(Owner);
				float targetRot = -MathHelper.PiOver2 - Owner.direction * 0.5f;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul) * Projectile.scale;
				MainAxisDirection += Projectile.DirectionFrom(Owner.Center) * 3;
				Projectile.rotation = MainAxisDirection.ToRotation();
			}
			if (Timer == (int)(14 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (Timer > 14 * timeMul && Timer < 35 * timeMul)
			{
				IsAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.32f / timeMul;

				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(110, Projectile.rotation, 0f, -0.3f * Projectile.spriteDirection), 0.4f / timeMul) * Projectile.scale;
				Owner.fullRotationOrigin = new Vector2(10, 42);
				Owner.fullRotation = MathF.Sin((Timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * Owner.direction;
				Owner.legRotation = -Owner.fullRotation;
			}
			if (Owner.gravDir == 1)
			{
				Point bottomPos = Owner.Bottom.ToTileCoordinates();
				bottomPos.X = Math.Clamp(bottomPos.X, 20, Main.maxTilesX - 20);
				bottomPos.Y = Math.Clamp(bottomPos.Y, 20, Main.maxTilesY - 20);
				if (Collision.SolidCollision(Owner.BottomLeft, Owner.width, 64) || TileCollisionUtils.PlatformCollision(Owner.Bottom + new Vector2(0, 16)) || TileCollisionUtils.PlatformCollision(Owner.Bottom + new Vector2(0, 0)) || TileCollisionUtils.PlatformCollision(Owner.Bottom + new Vector2(0, -16)) || ((Owner.waterWalk || Owner.waterWalk2) && Main.tile[bottomPos].LiquidAmount > 0 && !Owner.wet))
				{
					if (Timer <= 70)
					{
						Smash(0);
					}
					if (Timer > 70)
					{
						Smash(1);
					}
				}
			}
			if (Owner.gravDir == -1)
			{
				if (Collision.SolidCollision(Owner.TopLeft - new Vector2(0, 64), Owner.width, 80))
				{
					if (Timer <= 70)
					{
						Smash(0);
					}
					if (Timer > 70)
					{
						Smash(1);
					}
				}
			}
			if (Timer > 25 * timeMul)
			{
				Owner.velocity.Y += Owner.gravDir;
				LockPlayerDir(Owner);
				float targetRot = -MathHelper.PiOver2 - Owner.direction * 0.5f;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul) * Projectile.scale;
				MainAxisDirection += Projectile.DirectionFrom(Owner.Center) * 3;
				Projectile.rotation = MainAxisDirection.ToRotation();
			}
		}
		if (CurrantAttackType == 1)
		{
			ScreenShaker Gsplayer = Owner.GetModPlayer<ScreenShaker>();
			Gsplayer.FlyCamPosition = new Vector2(0, 14).RotatedByRandom(6.283);
			Owner.direction = FixedDirection;
			if (Timer < 8 * timeMul)// 前摇
			{
				UseTrail = false;
				float targetRot = -MathHelper.PiOver2 - Owner.direction * 0.5f;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul) * Projectile.scale;
				MainAxisDirection += Projectile.DirectionFrom(Owner.Center) * 3;
				Projectile.rotation = MainAxisDirection.ToRotation();
			}
			if (Timer == (int)(2 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (Timer > 8 * timeMul && Timer < 90 * timeMul)
			{
				IsAttacking = true;
				Omega *= 0.98f;
				Projectile.rotation -= Projectile.spriteDirection * Omega / timeMul;
				float theta = 1.16f;
				float phi = MathF.Sin((90 - Timer) * (90 - Timer) / 1000f) * Owner.direction;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(110, Projectile.rotation, theta, phi), 0.4f / timeMul) * Projectile.scale;
				Owner.fullRotationOrigin = new Vector2(10, 42);
				Owner.fullRotation = MathF.Sin((Timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * Owner.direction;
				Owner.legRotation = -Owner.fullRotation;
			}
			if (Timer > 100 * timeMul)
			{
				End();
				Projectile.Kill();
			}
		}// 强
		if (CurrantAttackType == 2)
		{
			ScreenShaker Gsplayer = Owner.GetModPlayer<ScreenShaker>();
			Gsplayer.FlyCamPosition = new Vector2(0, 28).RotatedByRandom(6.283);
			Owner.direction = FixedDirection;
			if (Timer < 8 * timeMul)// 前摇
			{
				UseTrail = false;
				float targetRot = -MathHelper.PiOver2 - Owner.direction * 0.5f;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul) * Projectile.scale;
				MainAxisDirection += Projectile.DirectionFrom(Owner.Center) * 3;
				Projectile.rotation = MainAxisDirection.ToRotation();
			}
			if (Timer == (int)(2 * timeMul))
			{
				AttSound(SoundID.Item1);
			}

			if (Timer > 8 * timeMul && Timer < 40 * timeMul)
			{
				IsAttacking = true;
				Omega *= 0.95f;
				Projectile.rotation -= Projectile.spriteDirection * Omega / timeMul;
				float theta = 0.4f + Timer / 71f;
				float phi = (-1.2f + Timer / 50f) * Owner.direction;
				MainAxisDirection = Vector2.Lerp(MainAxisDirection, Vector2Elipse(90, Projectile.rotation, theta, phi), 0.9f / timeMul) * Projectile.scale;
				Owner.fullRotationOrigin = new Vector2(10, 42);
				Owner.fullRotation = MathF.Sin((Timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * Owner.direction;
				Owner.legRotation = -Owner.fullRotation;
			}
			if (Timer > 50 * timeMul)
			{
				End();
				Projectile.Kill();
			}
		}// 弱

		SmashTrailVecs.Enqueue(MainAxisDirection + Projectile.Center);
		if (SmashTrailVecs.Count > MaxSlashTrailLength)
		{
			SmashTrailVecs.Dequeue();
		}
	}

	public virtual void Smash(int level)
	{
		if (level == 1)// 强
		{
			SoundEngine.PlaySound(SoundID.Research);
			CurrantAttackType = 1;
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
			CurrantAttackType = 2;
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
		Timer = 0;

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
			float w = TrailAlpha(factor);
			Color c0 = i == 0 ? Color.Transparent : Color.White;

			bars.Add(new Vertex2D(Projectile.Center, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(smoothedTrail[i], c0, new Vector3(factor, 0, w)));
		}

		var vColor = color.ToVector4();
		vColor.W *= 0.1f;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);

		Effect MeleeTrail = ModAsset.ClubTrail.Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
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