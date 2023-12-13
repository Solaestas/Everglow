using Everglow.Commons.Weapons.Yoyos;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee;

public class GoldRoundYoyo : YoyoProjectile
{
	public override void SetDef()
	{
		MaxRopeLength = 380;
		MaxStaySeconds = 120;
		RotatedSpeed = 0.3f;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
	}
	private float Timer;
	private float TrailWidth;
	private int HitCounter;
	public float Power;
	public override void OnSpawn(IEntitySource source)
	{
		TrailWidth = 18f;
		HitCounter = 0;
		Power = 0;
		base.OnSpawn(source);
	}
	public override void AI()
	{
		Timer++;
		if(Projectile.ai[0] < 0)
		{
			TrailWidth *= 0.8f;
		}
		Lighting.AddLight(Projectile.Center,0.5f,0.2f,0);
		base.AI();
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		HitCounter++;
		Power++;
		if (HitCounter >= 6)
		{
			HitCounter = 0;
			for (int x = 0;x < 15;x++)
			{
				Vector2 v0 = new Vector2(Main.rand.NextFloat(3f, 12f), 0).RotatedByRandom(MathHelper.TwoPi);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, v0, ModContent.ProjectileType<GoldRound>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
			}
		}
		for (int i = 0; i < 15; i++)
		{
			Vector2 v = new Vector2(0, Main.rand.NextFloat(1.5f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 87, 0f, 0f, 100, default, 1.2f);
			Main.dust[num].velocity *= v;
			Main.dust[num].noGravity = true;
		}
		base.OnHitNPC(target, hit, damageDone);
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		return base.Colliding(projHitbox, targetHitbox);
	}
	public override void PostDraw(Color lightColor)
	{
		Texture2D texture = ModAsset.GoldRoundYoyoGlow.Value;
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.7f, 0.1f, 0), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		DrawCorona();
		DrawString();
		Texture2D texture = ModAsset.Melee_GoldRoundYoyo.Value;
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
	public override void DrawString(Vector2 to = default)
	{
		Player player = Main.player[Projectile.owner];
		Vector2 mountedCenter = player.MountedCenter;
		Vector2 vector = mountedCenter;
		vector.Y += player.gfxOffY;
		if (to != default)
			vector = to;
		float num = Projectile.Center.X - vector.X;
		float num2 = Projectile.Center.Y - vector.Y;
		Math.Sqrt((double)(num * num + num2 * num2));
		float rotation;
		if (!Projectile.counterweight)
		{
			int num3 = -1;
			if (Projectile.position.X + Projectile.width / 2 < player.position.X + player.width / 2)
				num3 = 1;
			num3 *= -1;
			player.itemRotation = (float)Math.Atan2((double)(num2 * num3), (double)(num * num3));
		}
		bool checkSelf = true;
		if (num == 0f && num2 == 0f)
			checkSelf = false;
		else
		{
			float num4 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			num4 = 12f / num4;
			num *= num4;
			num2 *= num4;
			vector.X -= num * 0.1f;
			vector.Y -= num2 * 0.1f;
			num = Projectile.position.X + Projectile.width * 0.5f - vector.X;
			num2 = Projectile.position.Y + Projectile.height * 0.5f - vector.Y;
		}
		Color color = new Color(0, 0.03f, 0.05f, 1f);
		while (checkSelf)
		{
			float num5 = 12f;
			float num6 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			float num7 = num6;
			if (float.IsNaN(num6) || float.IsNaN(num7))
				checkSelf = false;
			else
			{
				if (num6 < 20f)
				{
					num5 = num6 - 8f;
					checkSelf = false;
				}
				num6 = 12f / num6;
				num *= num6;
				num2 *= num6;
				vector.X += num;
				vector.Y += num2;
				num = Projectile.position.X + Projectile.width * 0.5f - vector.X;
				num2 = Projectile.position.Y + Projectile.height * 0.1f - vector.Y;
				if (num7 > 12f)
				{
					float num8 = 0.3f;
					float num9 = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
					if (num9 > 16f)
						num9 = 16f;
					num9 = 1f - num9 / 16f;
					num8 *= num9;
					num9 = num7 / 80f;
					if (num9 > 1f)
						num9 = 1f;
					num8 *= num9;
					if (num8 < 0f)
						num8 = 0f;
					num8 *= num9;
					num8 *= 0.5f;
					if (num2 > 0f)
					{
						num2 *= 1f + num8;
						num *= 1f - num8;
					}
					else
					{
						num9 = Math.Abs(Projectile.velocity.X) / 3f;
						if (num9 > 1f)
							num9 = 1f;
						num9 -= 0.5f;
						num8 *= num9;
						if (num8 > 0f)
							num8 *= 2f;
						num2 *= 1f + num8;
						num *= 1f - num8;
					}
				}
				rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;

				color = Color.Lerp(color, new Color(1f, 0.7f, 0.1f, 0), 0.02f);
				Texture2D tex = TextureAssets.FishingLine.Value;
				Main.spriteBatch.Draw(tex, new Vector2(vector.X - Main.screenPosition.X + tex.Width * 0.5f, vector.Y - Main.screenPosition.Y + tex.Height * 0.5f) - new Vector2(6f, 0f), new Rectangle?(new Rectangle(0, 0, tex.Width, (int)num5)), color, rotation, new Vector2(tex.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
			}
		}
	}
	public void DrawTrail()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

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
			float timeValue = (float)Main.time * 0.015f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = new Color(1f, 0.4f, 0.0f, 0);
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor * 1 + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor * 1 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor * 1 + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor * 1 + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor * 1 + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor * 1 + timeValue, 0.5f, width)));
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = Commons.ModAsset.Trailing.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_0.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		if (bars3.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public void DrawCorona()
	{
		Color drawC = new Color(1f, 0.4f, 0.05f, 0) * (0.5f * TrailWidth / 18f);
		float timeValue = (float)Main.time * 0.0015f + MathF.Sin(Projectile.whoAmI);
		var bars = new List<Vertex2D>();

		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 8).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 70).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue, 0)));
		}
		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 8).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 50).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue, 0)));
		}
		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 8).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 30).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue, 0)));
		}
		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 8).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 15).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue, 0)));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_spiderNet.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

	}
	public float TrailWidthFunction(float factor)
	{
		factor *= 6;
		factor -= 1;
		if (factor < 0)
		{
			return MathF.Pow(MathF.Cos(factor * MathHelper.PiOver2), 0.5f);
		}
		return MathF.Pow(MathF.Cos(factor / 5f * MathHelper.PiOver2), 3);
	}
}
