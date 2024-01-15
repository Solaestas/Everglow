using Everglow.Commons.DataStructures;
using Everglow.Commons.MEAC;
using Everglow.Commons.Physics;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Terraria.DataStructures;
namespace Everglow.Commons.Weapons.Clubs;

public abstract class ClubProj_Smash : MeleeProj
{
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
		maxAttackType = 2;
		trailLength = 20;
		shadertype = "Trail";
		AutoEnd = false;
		selfWarp = true;
	}
	public bool Crash = false;
	public Vector2 StopPoint = Vector2.Zero;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
	}
	public override string TrailShapeTex()
	{
		return "Everglow/Commons/Textures/Melee";
	}
	public override string TrailColorTex()
	{
		return Texture;
	}
	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * 1.15f;
	}
	public override BlendState TrailBlendState()
	{
		return BlendState.NonPremultiplied;
	}
	public override void End()
	{
		Player player = Main.player[Projectile.owner];
		player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
		player.fullRotation = 0;
		player.legRotation = 0;
		player.legPosition = Vector2.Zero;
		Projectile.Kill();
		player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
	}
	public override void OnSpawn(IEntitySource source)
	{
		OwnSmashClubPlayers.Add(Projectile.owner);
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		base.AI();
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		Tplayer.HideLeg = true;
		useTrail = true;
		float timeMul = 1 / player.meleeSpeed;
		if (attackType == 0)
		{
			if (timer < 14 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(14 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 14 * timeMul && timer < 35 * timeMul)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.32f / timeMul;

				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(110, Projectile.rotation, 0f, -0.3f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;
			}
			if (Collision.SolidCollision(player.BottomLeft, player.width, 64))
			{
				if(timer > 70)
				{
					attackType++;
					Omega = 0.8f;
				}
				if (timer <= 70)
				{
					attackType = 2;
					Omega = 0.4f;
				}
				StopPoint = player.Bottom;
				Crash = true;
				timer = 0;
				
			}
			if (timer > 25 * timeMul)
			{
				player.velocity.Y += 1f;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
		}
		if (attackType == 1)
		{
			if (timer < 8 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(2 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 8 * timeMul && timer < 90 * timeMul)
			{
				isAttacking = true;
				Omega *= 0.98f;
				Projectile.rotation -= Projectile.spriteDirection * Omega / timeMul;
				float theta = 0.4f + timer / 71f;
				float phi = (-1.2f + timer / 50f) * player.direction;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(110, Projectile.rotation, theta, phi), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;
			}
			if (timer > 100 * timeMul)
			{
				End();
				Projectile.Kill();
			}
		}
		if (attackType == 2)
		{
			if (timer < 8 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(2 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 8 * timeMul && timer < 40 * timeMul)
			{
				isAttacking = true;
				Omega *= 0.95f;
				Projectile.rotation -= Projectile.spriteDirection * Omega / timeMul;
				float theta = 0.4f + timer / 71f;
				float phi = (-1.2f + timer / 50f) * player.direction;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(90, Projectile.rotation, theta, phi), 0.9f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((timer - 14 * timeMul) / (25f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;
			}
			if (timer > 50 * timeMul)
			{
				End();
				Projectile.Kill();
			}
		}
		trailVecs2.Enqueue(mainVec + Projectile.Center);
		if (trailVecs2.Count > trailLength)
			trailVecs2.Dequeue();
	}
	internal Queue<Vector2> trailVecs2 = new Queue<Vector2>();
	public float Omega = 0;
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
		base.DrawSelf(spriteBatch, lightColor, diagonal, drawScale, glowTexture);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		if(attackType == 0)
		{
			DrawTrail2(lightColor);
		}
		else
		{
			DrawTrail(lightColor);
		}
		DrawSelf(Main.spriteBatch, lightColor);
		return false;
	}
	public override void DrawTrail(Color color)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x <= SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs.Count != 0)
			SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);

		int length = SmoothTrail.Count;
		if (length <= 3)
			return;
		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = TrailAlpha(factor);
			Color c0 = Color.White;
			if (i == 0)
			{
				c0 = Color.Transparent;
			}
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.3f * Projectile.scale, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, c0, new Vector3(factor, 0, w)));
		}
		bars.Add(new Vertex2D(Projectile.Center + mainVec * 0.3f * Projectile.scale, Color.White, new Vector3(0, 1, 0f)));
		bars.Add(new Vertex2D(Projectile.Center + mainVec * Projectile.scale, Color.White, new Vector3(0, 0, 1)));
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

		Effect MeleeTrail = ModAsset.ClubTrail.Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);

		MeleeTrail.Parameters["tex0"].SetValue(ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		Vector4 vColor = color.ToVector4();
		vColor.W *= 0.1f;
		MeleeTrail.Parameters["Light"].SetValue(vColor);
		MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
	public void DrawTrail2(Color color)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs2.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x <= SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs2.Count != 0)
			SmoothTrail.Add(trailVecs2.ToArray()[trailVecs2.Count - 1]);

		int length = SmoothTrail.Count;
		if (length <= 3)
			return;
		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = TrailAlpha(factor);
			Color c0 = Color.White;
			if (i == 0)
			{
				c0 = Color.Transparent;
			}
			bars.Add(new Vertex2D(Projectile.Center, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(trail[i], c0, new Vector3(factor, 0, w)));
		}

		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

		Effect MeleeTrail = ModAsset.ClubTrail.Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);

		MeleeTrail.Parameters["tex0"].SetValue(ModAsset.Noise_flame_0.Value);
		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		Vector4 vColor = color.ToVector4();
		vColor.W *= 0.1f;
		MeleeTrail.Parameters["Light"].SetValue(vColor);
		MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
	public override void OnKill(int timeLeft)
	{
		Player player = Main.player[Projectile.owner];
		player.fullRotation = 0;
		while (OwnSmashClubPlayers.Remove(Projectile.owner))
			;
	}
	public static List<int> OwnSmashClubPlayers = new List<int>();
}
public class SmashClubOwner : ModPlayer
{
	public override void PostUpdateMiscEffects()
	{
		if (ClubProj_Smash.OwnSmashClubPlayers.Contains(Player.whoAmI))
			Player.maxFallSpeed += 10000f;
	}
}

