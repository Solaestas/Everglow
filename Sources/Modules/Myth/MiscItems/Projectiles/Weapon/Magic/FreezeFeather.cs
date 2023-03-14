//using MythMod.Buffs;
using Everglow.Myth.MiscItems.Buffs;
using Everglow.Myth.MiscItems.Dusts;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Magic;

public class FreezeFeather : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("FreezeFeather");
		DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "冰羽");
	}
	public override void SetDefaults()
	{
		Projectile.width = 34;
		Projectile.height = 34;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.hostile = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 1080;
		Projectile.alpha = 0;
		Projectile.penetrate = 1;
		Projectile.scale = 1;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
	}
	private bool initialization = true;
	private double X;
	private float Omega;
	private float b;
	public override void AI()
	{
		Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		Projectile.velocity *= 1.01f;
		/*if (Main.rand.NextBool(6))
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(2, 2), 0, 0, ModContent.DustType<Dusts.Snow>(), Alpha: 0, Scale: Main.rand.NextFloat(1f, 2.7f));
                d.noGravity = true;
                d.velocity *= 0.25f;
            }*/
		float num2 = Projectile.Center.X;
		float num3 = Projectile.Center.Y;
		float num4 = 400f;
		bool flag = false;
		for (int j = 0; j < 200; j++)
		{
			if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
			{
				float num5 = Main.npc[j].position.X + Main.npc[j].width / 2;
				float num6 = Main.npc[j].position.Y + Main.npc[j].height / 2;
				float num7 = Math.Abs(Projectile.position.X + Projectile.width / 2 - num5) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num6);
				if (num7 < num4)
				{
					num4 = num7;
					num2 = num5;
					num3 = num6;
					flag = true;
				}
			}
		}
		if (flag)
		{
			float num8 = 20f;
			var vector1 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float num9 = num2 - vector1.X;
			float num10 = num3 - vector1.Y;
			float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
			num11 = num8 / num11;
			num9 *= num11;
			num10 *= num11;
			Projectile.velocity.X = (Projectile.velocity.X * 200f + num9) / 201f;
			Projectile.velocity.Y = (Projectile.velocity.Y * 200f + num10) / 201f;
		}
		int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 12f, 0, 0, ModContent.DustType<Dusts.Freeze>(), 0, 0, 0, default, 4f);
		Main.dust[r].noGravity = true;
		ka = 1;
		if (Projectile.timeLeft < 60f)
			ka = Projectile.timeLeft / 60f;
	}
	public override void Kill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.Center);
		for (int k = 0; k <= 2; k++)
		{
			Vector2 v = new Vector2(0, Main.rand.NextFloat(2.5f, 6f)).RotatedByRandom(MathHelper.TwoPi);
			int num4 = Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, v, ModContent.ProjectileType<FreezeBallBrake>(), Projectile.damage / 5, 1, Projectile.owner, 0f, 10f);
			//Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, (float)((float)l * Math.Cos((float)a)) * 0.06f, (float)((float)l * Math.Sin((float)a)) * 0.06f, ModContent.ProjectileType<Projectiles.Magic.FreezeBallBrake>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner, 0f, 20);
			Main.projectile[num4].timeLeft = (int)(80 * Main.rand.NextFloat(0.2f, 0.7f));
		}
		for (int j = 0; j < 8; j++)
		{
			Vector2 v1 = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotatedByRandom(MathHelper.TwoPi);
			Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 5.6f)).RotatedByRandom(MathHelper.TwoPi);
			int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + v1, 0, 0, ModContent.DustType<Dusts.Freeze>(), v3.X, v3.Y, 0, default, 6f);
			Main.dust[r].noGravity = true;
			Main.dust[r].velocity = v3;
		}
		for (int h = 0; h < 20; h++)
		{
			Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
			int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<PureBlue>(), 0, 0, 0, default, 15f * Main.rand.NextFloat(0.4f, 1.1f));
			Main.dust[r].noGravity = true;
			Main.dust[r].velocity = v3;
			//Main.dust[r].dustIndex = (int)(Math.Cos(h * Math.PI / 10d + Projectile.ai[0]) * 100d);
		}
	}
	public override Color? GetAlpha(Color lightColor)
	{
		if (Projectile.timeLeft > 60)
			return new Color?(new Color(255, 255, 255, 100));
		else
		{
			return new Color?(new Color(Projectile.timeLeft / 60f, Projectile.timeLeft / 60f, Projectile.timeLeft / 60f, Projectile.timeLeft / 60f * 100f / 255f));
		}
	}
	/*public override void PostDraw(Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/FogTrace").Value;
            Color[] colorTex = new Color[tex.Width * tex.Height];
            tex.GetData(colorTex);
            for (int y = 0; y < tex.Height; y += 1)
            {
                for (int z = 0; z < Projectile.oldPos.Length; z += 1)
                {
                    int x = (Projectile.timeLeft * 10 + z) % tex.Width;
                    double Rot = Math.Atan2(Projectile.velocity.Y,Projectile.velocity.X);
                    if (new Color(colorTex[x + y * tex.Width].R, colorTex[x + y * tex.Width].G, colorTex[x + y * tex.Width].B) != new Color(0, 0, 0))
                    {
                        if(colorTex[x + y * tex.Width].B > 35 && (x + y * tex.Width) % 20 == 0)
                        {
                            int C = colorTex[x + y * tex.Width].B;
                            Texture2D tex2 = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/heatmapColdBlueCircle").Value;
                            Main.EntitySpriteDraw((Texture2D)TextureAssets.MagicPixel, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(z - tex.Width / 2f, y - tex.Height / 2f).RotatedBy(Rot), new Rectangle(0, 0, 1, 1), new Color(0, C / 4, C, 0), Projectile.rotation, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0);
                        }
                    }
                }
            }
        }*/
	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
	{
		if (target.type != 396 && target.type != 397 && target.type != 398)
		{
			if (!target.HasBuff(ModContent.BuffType<Freeze>()))
			{
				target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1]);
				target.AddBuff(ModContent.BuffType<Freeze2>(), (int)Projectile.ai[1] + 2);
			}
		}
		if (target.type == 113)
		{
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].type == 113 || Main.npc[i].type == 114)
				{
					if (!target.HasBuff(ModContent.BuffType<Freeze>()))
					{
						target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1]);
						target.AddBuff(ModContent.BuffType<Freeze2>(), (int)Projectile.ai[1] + 2);
					}
				}
			}
		}
		if (target.type == 114)
		{
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].type == 113 || Main.npc[i].type == 114)
				{
					if (!target.HasBuff(ModContent.BuffType<Freeze>()))
					{
						target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1]);
						target.AddBuff(ModContent.BuffType<Freeze2>(), (int)Projectile.ai[1] + 2);
					}
				}
			}
		}
	}
	public override void OnHitPvp(Player target, int damage, bool crit)
	{
		target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1]);
		target.AddBuff(ModContent.BuffType<Freeze2>(), (int)Projectile.ai[1] + 2);
	}
	private Effect ef;
	int TrueL = 1;
	float ka = 0;
	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		float width = 12;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft / 5f;
		TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			TrueL++;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);
			Lighting.AddLight(Projectile.oldPos[i], (255 - Projectile.alpha) * 0.0f / 50f * ka * (1 - factor), (255 - Projectile.alpha) * 0.5f / 50f * ka * (1 - factor), (255 - Projectile.alpha) * 1.3f / 50f * ka * (1 - factor));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(17, 17) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(17, 17) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 0, w)));
		}
		var Vx = new List<Vertex2D>();
		if (bars.Count > 2)
		{
			Vx.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
			Vx.Add(bars[1]);
			Vx.Add(vertex);
			for (int i = 0; i < bars.Count - 2; i += 2)
			{
				Vx.Add(bars[i]);
				Vx.Add(bars[i + 2]);
				Vx.Add(bars[i + 1]);

				Vx.Add(bars[i + 1]);
				Vx.Add(bars[i + 2]);
				Vx.Add(bars[i + 3]);
			}
		}

		Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/IceLine").Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		// SpriteEffects helps to flip texture horizontally and vertically
		SpriteEffects spriteEffects = SpriteEffects.None;
		if (Projectile.spriteDirection == -1)
			spriteEffects = SpriteEffects.FlipHorizontally;

		// Getting texture of Projectile
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

		// Calculating frameHeight and current Y pos dependence of frame
		// If texture without animation frameHeight = texture.Height is always and startY is always 0
		int frameHeight = texture.Height / Main.projFrames[Projectile.type];
		int startY = frameHeight * Projectile.frame;

		// Get this frame on texture
		var sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
		Vector2 origin = sourceRectangle.Size() / 2f;

		// If image isn't centered or symetrical you can specify origin of the sprite
		// (0,0) for the upper-left corner 
		float offsetX = 20f;
		origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

		// If sprite is vertical
		// float offsetY = 20f;
		// origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);


		// Appling lighting and draw current frame
		Color drawColor = Projectile.GetAlpha(lightColor);
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
		// It's important to return false, otherwise we also draw the original texture.
		return false;
	}
}