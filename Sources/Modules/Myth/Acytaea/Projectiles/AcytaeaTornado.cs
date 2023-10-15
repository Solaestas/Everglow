using Everglow.Myth.Acytaea.Buffs;
using Everglow.Myth.Acytaea.VFXs;
using Everglow.Myth.Misc.Projectiles.Weapon.Magic.FreezeFeatherMagic;
using Terraria.Audio;

namespace Everglow.Myth.Acytaea.Projectiles;
public class AcytaeaTornado : ModProjectile
{
	public override string Texture => "Everglow/Myth/Acytaea/Projectiles/AcytaeaSword_projectile";
	public override void SetDefaults()
	{
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 600;
		Projectile.extraUpdates = 0;
		Projectile.scale = 1f;
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Melee;

		Projectile.width = 80;
		Projectile.height = 80;
	}
	public float Timer = 0;
	public Vector2 TornadoBottom = Vector2.zeroVector;
	public override void AI()
	{
		Player player = Main.player[Player.FindClosest(Projectile.Center, 0, 0)];
		Timer++;
		if(TornadoBottom != Vector2.zeroVector)
		{
			GenerateVFX();
		}
		Projectile.velocity.X *= 0.95f;
		if(player != null)
		{
			float k = (player.Center - Projectile.Center - Projectile.velocity).X;
			if (Math.Abs(k) > 50)
			{
				k = 50 * Math.Sign(k);
			}
			Projectile.velocity.X += k / 100f;
		}
		float value2 = 1f;
		if (Projectile.timeLeft < 60)
		{
			value2 = Projectile.timeLeft / 60f;
		}
		if (Timer < 24)
		{
			value2 = Timer / 24f;
		}
		for (int x = 0; x < 3 * value2; x++)
		{
			Vector2 newVec = new Vector2(0, Main.rand.NextFloat(4f, 12f)).RotatedByRandom(6.283) + new Vector2(0, -7) * value2;
			var positionVFX = new Vector2(Main.rand.NextFloat(-40, 40), Main.rand.NextFloat(-600f, Math.Min(600f, TornadoBottom.Y - Projectile.Center.Y))) + Projectile.Center + newVec * Main.rand.NextFloat(0.7f, 0.9f) + new Vector2(Main.rand.NextFloat(-100, 100), -7);

			var acytaeaFlame = new AcytaeaFlameDust
			{
				velocity = newVec,
				Active = true,
				Visible = true,
				position = positionVFX,
				maxTime = Main.rand.Next(14, 36),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.01f, 0.01f), Main.rand.NextFloat(8f, 25f) * value2 }
			};
			Ins.VFXManager.Add(acytaeaFlame);
		}
	}
	public void GenerateVFX()
	{
		float value2 = 1f;
		if (Projectile.timeLeft < 60)
		{
			value2 = Projectile.timeLeft / 60f;
		}
		if (Timer < 24)
		{
			value2 = Timer / 24f;
		}
		for (int x = 0; x < 4 * value2; x++)
		{
			Vector2 newVec = new Vector2(0, Main.rand.NextFloat(4f, 12f)).RotatedByRandom(6.283) + new Vector2(0, -7) * value2;
			var positionVFX = TornadoBottom + newVec * Main.rand.NextFloat(0.7f, 0.9f) + new Vector2(Main.rand.NextFloat(-100, 100), -7);

			var acytaeaFlame = new AcytaeaFlameDust
			{
				velocity = newVec * x * 0.3f,
				Active = true,
				Visible = true,
				position = positionVFX,
				maxTime = Main.rand.Next(14, 16),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.04f, 0.04f), Main.rand.NextFloat(8f, 16f) * value2 }
			};
			Ins.VFXManager.Add(acytaeaFlame);
		}
		
		for (int x = 0; x < 16 * value2; x++)
		{
			Vector2 newVec = new Vector2(0, Main.rand.NextFloat(4f, 12f)).RotatedByRandom(6.283) + new Vector2(0, -12) * value2;
			var positionVFX = TornadoBottom + newVec * Main.rand.NextFloat(0.7f, 0.9f) + new Vector2(Main.rand.NextFloat(-100, 100), -12);

			var acytaeaFlame = new AcytaeaSparkDust
			{
				velocity = newVec * x * 0.02f,
				Active = true,
				Visible = true,
				position = positionVFX,
				maxTime = Main.rand.Next(14, 56),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.04f, 0.04f), Main.rand.NextFloat(5f, 10f) * value2 }
			};
			Ins.VFXManager.Add(acytaeaFlame);
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		return false;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		if(Projectile.timeLeft < 60 || Timer < 40)
		{
			return false;
		}
		Rectangle r = new Rectangle(projHitbox.X, projHitbox.Y - 600, 80, 1200);
		if(TornadoBottom.Y < projHitbox.Y + 600)
		{
			r.Height = (int)(TornadoBottom.Y - (projHitbox.Y - 600));
		}
		return r.Intersects(targetHitbox);
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		target.AddBuff(ModContent.BuffType<AcytaeaInferno>(), 450);
		base.OnHitPlayer(target, info);
	}
	public void DrawTornado(Texture2D tex, Color color, float width = 80, string pass = "", float dissolveRange = 0.2f, float deltaY = 0, float valueX = 13f, float valueY = 84f)
	{
		float uTime = (float)Main.time * 0.02f * Math.Abs(width) / 80f;
		int tolerance = 3;
		int length = (int)Timer;
		if(length >= 40)
		{
			length = 40;
		}
		var bars = new List<Vertex2D>();
		for(int k = -length; k < length; k++)
		{
			float value = (length - Math.Abs(k)) / 10f;
			if (value > 1)
			{
				value = 1;
			}
			value *= tolerance / 3f;
			float mulWidth = 2 - MathF.Cos(k / 80f * MathF.PI);
			mulWidth *= 0.7f;
			bars.Add(Projectile.Center + new Vector2(-width * mulWidth, k * 20), color * value, new Vector3(0.2f + uTime + k / valueX, k / valueY + deltaY, 0));
			bars.Add(Projectile.Center + new Vector2(width * mulWidth, k * 20), color * value, new Vector3(0.2f + uTime + k / valueX, k / valueY + deltaY, 1));
			if(k > 0 && tolerance == 3)
			{
				if (Collision.SolidCollision(Projectile.Center + new Vector2(0, k * 20 + 20), 0, 0))
				{
					tolerance--;
					Collision.HitTiles(Projectile.Center + new Vector2(-40, k * 20 - 20), new Vector2(0, -50), 80, 40);
				}
			}
			if (tolerance < 3)
			{
				tolerance--;
			}
			if(tolerance <= 0)
			{
				TornadoBottom = Projectile.Center + new Vector2(0, k * 20 + 20);
				break;
			}
			Lighting.AddLight(Projectile.Center + new Vector2(0, k * 20), 1 * value, 0, 0.4f * value);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect tornado = ModAsset.AcytaeaTornadoEffect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		tornado.Parameters["uTransform"].SetValue(model * projection);
		float value2 = 1f;
		if(Projectile.timeLeft < 60)
		{
			value2 = Projectile.timeLeft / 60f;
		}
		if (Timer < 24)
		{
			value2 = Timer / 24f;
		}
		value2 *= (1 + dissolveRange);
		value2 -= dissolveRange;
		tornado.Parameters["duration"].SetValue(value2 * 0.8f);
		tornado.Parameters["dissolveRange"].SetValue(dissolveRange);
		tornado.CurrentTechnique.Passes[pass].Apply();
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		Main.graphics.GraphicsDevice.Textures[0] = tex;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
	public override void PostDraw(Color lightColor)
	{
		DrawTornado(Commons.ModAsset.Noise_crack_dense_black.Value, new Color(1f, 1f, 1f, 1f), -160, "DarkEffect", 0.1f, 0.13f);
		DrawTornado(Commons.ModAsset.Noise_burn.Value, new Color(1f, 0, 0.4f, 0), -110, "RedEffect", 0.2f, 0.13f);
		DrawTornado(Commons.ModAsset.Noise_perlin.Value, new Color(1f, 0, 0.4f, 0.7f), -80, "RedEffect", 0.2f, 0.13f);
		
		DrawTornado(Commons.ModAsset.Noise_perlin.Value, new Color(1f, 0, 0.4f, 0.7f), 80, "RedEffect", 0.2f);
		DrawTornado(Commons.ModAsset.Noise_burn.Value, new Color(1f, 0, 0.4f, 0), 110, "RedEffect", 0.2f);
		DrawTornado(Commons.ModAsset.Noise_crack_dense_black.Value, new Color(1f, 1f, 1f, 1f), 160, "DarkEffect", 0.1f);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
}
