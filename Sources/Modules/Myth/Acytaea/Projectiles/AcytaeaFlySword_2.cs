using Everglow.Commons.Weapons;
using Everglow.Myth.Acytaea.Buffs;
using Everglow.Myth.Acytaea.VFXs;
using Terraria.Audio;

namespace Everglow.Myth.Acytaea.Projectiles;
public class AcytaeaFlySword_2 : TrailingProjectile
{
	public override string Texture => "Everglow/Myth/Acytaea/Projectiles/AcytaeaSword_projectile";
	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 360000;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = true;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
		TrailWidth = 30f;
		SelfLuminous = true;
		SetDef();
	}
	public override void SetDef()
	{
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		TrailTexture = Commons.ModAsset.Trail.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
		TrailColor = new Color(1f, 0, 0.4f, 0);
	}
	public override void AI()
	{
		Timer++;

		if (TimeTokill >= 0 && TimeTokill <= 2)
			Projectile.Kill();
		TimeTokill--;
		if (TimeTokill < 0)
		{
			Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		}
		else
		{
			Projectile.velocity *= 0f;
			return;
		}
	}
	public override void Explosion()
	{
		Vector2 hitCenter = Projectile.Center + new Vector2(0, 50);
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb.WithPitchOffset(-1), hitCenter);
		if (TimeTokill > 0)
		{
			return;
		}
		Player player = Main.player[Projectile.owner];
		TimeTokill = 60;
		Projectile.tileCollide = false;
		Projectile.friendly = false;
		Projectile.ignoreWater = true;
		Projectile.velocity = Projectile.oldVelocity;
		SoundEngine.PlaySound((SoundID.DD2_WitherBeastCrystalImpact.WithVolume(0.3f)).WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)), hitCenter);
		for (int x = 0; x < 8; x++)
		{
			Vector2 newVec = new Vector2(0, Main.rand.NextFloat(4f, 12f)).RotatedByRandom(6.238f);
			var positionVFX = hitCenter + newVec * Main.rand.NextFloat(0.7f, 0.9f);

			var acytaeaFlame = new AcytaeaFlameDust
			{
				velocity = newVec,
				Active = true,
				Visible = true,
				position = positionVFX,
				maxTime = Main.rand.Next(14, 16),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.04f, 0.04f), Main.rand.NextFloat(18f, 30f) }
			};
			Ins.VFXManager.Add(acytaeaFlame);
		}
		for (int x = 0; x < 15; x++)
		{
			Vector2 newVec = new Vector2(0, Main.rand.NextFloat(4f, 24f)).RotatedByRandom(6.238f);
			var positionVFX = hitCenter + newVec * Main.rand.NextFloat(0.7f, 0.9f);

			var acytaeaFlame = new AcytaeaSparkDust
			{
				velocity = newVec,
				Active = true,
				Visible = true,
				position = positionVFX,
				maxTime = Main.rand.Next(37, 152),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.04f, 0.04f), Main.rand.NextFloat(4f, 8f) }
			};
			Ins.VFXManager.Add(acytaeaFlame);
		}
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), hitCenter, Vector2.Zero, ModContent.ProjectileType<AcytaeaFlySwordExplosion>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 14);
		Projectile.position -= Projectile.velocity;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrailDark();
		DrawTrail();
		return false;
	}
	public override void DrawTrail()
	{
		base.DrawTrail();
	}
	public override void DrawTrailDark()
	{
		base.DrawTrailDark();
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		target.AddBuff(ModContent.BuffType<AcytaeaInferno>(), 450);
		base.OnHitPlayer(target, info);
	}

	private void CheckFrame()
	{
	}
	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect dissolve = Commons.ModAsset.Dissolve.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		float dissolveDuration = TimeTokill / 60f - 0.2f;
		if (TimeTokill < 0)
		{
			dissolveDuration = 1.2f;
		}
		dissolve.Parameters["uTransform"].SetValue(model * projection);
		dissolve.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_spiderNet.Value);
		dissolve.Parameters["duration"].SetValue(dissolveDuration);
		dissolve.Parameters["uDissolveColor"].SetValue(new Vector4(1f, 0f, 0f, 1f));
		dissolve.Parameters["uNoiseSize"].SetValue(2f);
		dissolve.Parameters["uNoiseXY"].SetValue(new Vector2(Projectile.ai[1], Projectile.ai[2]));
		dissolve.CurrentTechnique.Passes[0].Apply();
		Texture2D tex = ModAsset.AcytaeaFlySword_red.Value;
		Rectangle projFrame = new Rectangle(0, 0, 80, 80);
		Main.spriteBatch.Draw(tex, Projectile.Center, projFrame, new Color(255, 0, 215, 255), Projectile.rotation + MathHelper.PiOver4, new Vector2(40), Projectile.scale, SpriteEffects.None, 0);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		if (Projectile.timeLeft < 570 && TimeTokill < 0)
		{
			Texture2D tex2 = ModAsset.AcytaeaSword_projectile_highLight.Value;
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, projFrame, new Color(255, 0, 215, 255) * ((570 - Projectile.timeLeft) / 5f), Projectile.rotation + MathHelper.PiOver4, new Vector2(40), Projectile.scale, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * ((570 - Projectile.timeLeft) / 10f), Projectile.rotation + MathHelper.PiOver4, tex2.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		}
	}
}
