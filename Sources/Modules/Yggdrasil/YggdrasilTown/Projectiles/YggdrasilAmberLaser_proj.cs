using Everglow.Commons.DataStructures;
using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class YggdrasilAmberLaser_proj : HandholdProjectile, IWarpProjectile
{
	public override void SetDef()
	{
		DepartLength = 60;
		TextureRotation = 5 / 18f * MathHelper.Pi;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 5;
		Projectile.ArmorPenetration = 45;
		base.SetDef();
	}
	public override void OnSpawn(IEntitySource source)
	{
		SoundEngine.PlaySound(SoundID.Shimmer1.WithPitchOffset(1), Projectile.Center);
		Player player = Main.player[Projectile.owner];
		Vector2 mouseToPlayer = Main.MouseWorld - player.Center;
		mouseToPlayer = Vector2.Normalize(mouseToPlayer);
		Projectile.rotation = mouseToPlayer.ToRotation() + MathHelper.PiOver4;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		base.OnHitNPC(target, hit, damageDone);
	}
	public override void AI()
	{
		HeldProjectileAI();
		Player player = Main.player[Projectile.owner];
		int timeMax = (int)(player.itemTimeMax / player.meleeSpeed);
		float duration = player.itemTime / (float)timeMax;
		duration *= 1.5f;
		duration -= 0.5f;
		if (duration < 0)
		{
			duration = 0;
		}
		duration = MathF.Sin(duration * MathHelper.Pi);
		if (duration < 0.3f)
		{
			return;
		}
		Vector2 mouseToPlayer = new Vector2(0, 1).RotatedBy(Projectile.rotation - Math.PI * 0.75);
		Vector2 endPoint = Projectile.Center + MaxStep * mouseToPlayer * 8;
		for (int g = 0; g < 3; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 12f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new AmberFlameDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = endPoint,
				maxTime = Main.rand.Next(127, 345),
				scale = Main.rand.NextFloat(12.20f, 32.35f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(somg);
		}
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		Player player = Main.player[Projectile.owner];
		int timeMax = (int)(player.itemTimeMax / player.meleeSpeed);
		float duration = player.itemTime / (float)timeMax;
		duration *= 1.5f;
		duration -= 0.5f;
		if (duration < 0)
		{
			duration = 0;
		}
		duration = MathF.Sin(duration * MathHelper.Pi);
		if (duration < 0.3f)
		{
			return false;
		}
		Vector2 mouseToPlayer = new Vector2(0, 1).RotatedBy(Projectile.rotation - Math.PI * 0.75);
		Vector2 endPoint = Projectile.Center + MaxStep * mouseToPlayer * 8;

		bool k = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), new Vector2(targetHitbox.Width, targetHitbox.Height), Projectile.Center, endPoint);
		if (k)
		{
			for (int g = 0; g < 10; g++)
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
				var somg = new AmberFlameDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = targetHitbox.Center(),
					maxTime = Main.rand.Next(37, 145),
					scale = Main.rand.NextFloat(1.20f, 12.35f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
				};
				Ins.VFXManager.Add(somg);
			}
		}
		return k;
	}
	public override void HeldProjectileAI()
	{
		Player player = Main.player[Projectile.owner];
		if (player.HeldItem.type != ModContent.ItemType<YggdrasilAmberLaser>())
		{
			Projectile.Kill();
		}
		player.heldProj = Projectile.whoAmI;
		ArmRootPos = player.MountedCenter + new Vector2(-4 * player.direction, -2);

		Vector2 mouseToPlayer = Main.MouseWorld - ArmRootPos;
		mouseToPlayer = Vector2.Normalize(mouseToPlayer);
		int timeMax = (int)(player.itemTimeMax / player.meleeSpeed);
		if (Projectile.localNPCHitCooldown > timeMax - 1)
		{
			Projectile.localNPCHitCooldown = timeMax - 1;
		}
		if (player.itemTime == 1)
		{
			Projectile.Kill();
		}
		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.rotation - MathF.PI * 0.25f) * player.gravDir - MathF.PI * 0.5f);
		DepartLength = 60;

		Projectile.Center = ArmRootPos + new Vector2(0, 1).RotatedBy(Projectile.rotation - Math.PI * 0.75) * DepartLength;
		Projectile.timeLeft = timeMax;
		if (Projectile.Center.X < ArmRootPos.X)
			player.direction = -1;
		else
		{
			player.direction = 1;
		}
	}
	public float MaxStep = 0;
	public override bool PreDraw(ref Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		SpriteEffects se = SpriteEffects.None;
		if (player.direction == -1)
			se = SpriteEffects.FlipVertically;
		float rot = Projectile.rotation - (float)(Math.PI * 0.25) + TextureRotation * player.direction;
		var texMain_glow = ModAsset.YggdrasilAmberLaser_glow_proj.Value;
		int timeMax = (int)(player.itemTimeMax / player.meleeSpeed);
		float duration = player.itemTime / (float)timeMax;
		duration *= 1.5f;
		duration -= 0.5f;
		if (duration < 0)
		{
			duration = 0;
		}
		duration = MathF.Sin(duration * MathHelper.Pi);


		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition + DrawOffset - new Vector2(54, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver4), null, lightColor, rot, texMain.Size() / 2f, 1f, se, 0);
		Color powerColor = new Color(duration, duration * duration, duration * duration, 0);
		Main.spriteBatch.Draw(texMain_glow, Projectile.Center - Main.screenPosition + DrawOffset - new Vector2(54, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver4), null, powerColor, rot, texMain_glow.Size() / 2f, 1f, se, 0);


		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = Commons.ModAsset.Trailing.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Vector2 mouseToPlayer = new Vector2(0, 1).RotatedBy(Projectile.rotation - Math.PI * 0.75);
		float timeValue = (float)Main.time * 0.06f;
		List<Vertex2D> bars = new List<Vertex2D>();
		int step = -6;
		while (!Collision.SolidCollision(Projectile.Center + mouseToPlayer * step * 8, 0, 0))
		{
			step++;
			if (step > 200)
			{
				break;
			}
			Vector2 checkPoint = Projectile.Center + mouseToPlayer * step * 8;
			Vector2 toMouseLeft = mouseToPlayer.RotatedBy(MathHelper.PiOver2);
			float width = duration * 25;
			Color drawColor = Color.White;
			float mulWidth = 1f;
			if (step + 4 <= 10)
			{
				mulWidth = MathF.Pow((step + 4) / 10f, 0.3f);
			}
			bars.Add(checkPoint + toMouseLeft * width, drawColor, new Vector3(step * 0.03f - timeValue, 0, mulWidth));
			bars.Add(checkPoint - toMouseLeft * width, drawColor, new Vector3(step * 0.03f - timeValue, 1, mulWidth));
			if(!Main.gamePaused && Main.rand.NextBool(30))
			{
				Vector2 newVelocity = mouseToPlayer.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * 4f * duration;
				var somg = new AmberFlameDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = checkPoint,
					maxTime = Main.rand.Next(37, 55) * duration,
					scale = Main.rand.NextFloat(1.20f, 4.35f) * duration,
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
				};
				Ins.VFXManager.Add(somg);
			}
			MaxStep = step;
		}
		if (bars.Count > 2)
		{
			Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_1_black.Value;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		bars = new List<Vertex2D>();
		step = -6;
		while (!Collision.SolidCollision(Projectile.Center + mouseToPlayer * step * 8, 0, 0))
		{
			step++;
			if (step > 200)
			{
				break;
			}
			Vector2 checkPoint = Projectile.Center + mouseToPlayer * step * 8;
			Vector2 toMouseLeft = mouseToPlayer.RotatedBy(MathHelper.PiOver2);
			float width = duration * duration * 25;
			float duration2 = Math.Max(duration, 0);
			Color drawColor = new Color(duration * 0.7f, duration2 * duration2 * 0.52f, 0, 0);
			float mulWidth = 1f;
			if (step + 4 <= 10)
			{
				mulWidth = MathF.Pow((step + 4) / 10f, 0.3f);
			}
			bars.Add(checkPoint + toMouseLeft * width, drawColor, new Vector3(step * 0.03f - timeValue, 0, mulWidth));
			bars.Add(checkPoint - toMouseLeft * width, drawColor, new Vector3(step * 0.03f - timeValue, 1, mulWidth));
		}
		if (bars.Count > 2)
		{
			Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_1.Value;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		Texture2D star = Commons.ModAsset.Star_black.Value;
		Color drawC = new Color(duration * 0.7f, duration * duration * 0.52f, 0, 0);

		Vector2 starCenter = Projectile.Center + MaxStep * mouseToPlayer * 8 - Main.screenPosition;
		Main.spriteBatch.Draw(star, starCenter, null, Color.White, MathHelper.PiOver2, star.Size() / 2f, 0.9f * duration, se, 0);
		Main.spriteBatch.Draw(star, starCenter, null, Color.White, 0, star.Size() / 2f, 0.9f * duration, se, 0);
		star = Commons.ModAsset.Star.Value;
		Main.spriteBatch.Draw(star, starCenter, null, drawC, MathHelper.PiOver2, star.Size() / 2f, 0.9f * duration, se, 0);
		Main.spriteBatch.Draw(star, starCenter, null, drawC, 0, star.Size() / 2f, 0.9f * duration, se, 0);
		return false;
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{

	}
}