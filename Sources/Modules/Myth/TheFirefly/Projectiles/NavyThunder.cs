using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.Audio;

namespace Everglow.Myth.TheFirefly.Projectiles;

internal class NavyThunder : ModProjectile, IWarpProjectile
{
	public override string Texture => "Everglow/Myth/TheFirefly/Projectiles/NavyThunderTex/FlameSkull";

	public override void SetDefaults()
	{
		Projectile.width = 54;
		Projectile.height = 84;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.timeLeft = 90000;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
	}

	/// <summary>
	/// 我不知道干嘛用的，没设置过值，只有判断，先改成 const
	/// </summary>
	private const bool Release = true;

	/// <summary>
	/// 没看到使用的地方
	/// </summary>
	//private Vector2 oldPo = Vector2.Zero;

	private int addi = 0;

	public override void AI()
	{
		// 用的舒服点
		ref float ai0 = ref Projectile.ai[0];
		ref float ai1 = ref Projectile.ai[1];

		Player player = Main.player[Projectile.owner];

		addi++;

		// 为什么要按着才更新位置呢？注释了！
		if (/*Main.mouseLeft &&*/ Release)
		{
			// 拆分是为了看着短一点
			Projectile.Center = Projectile.Center * 0.7f + player.Center * 0.3f;

			// 面朝方向 X 轴偏移
			Projectile.position.X += player.direction * 32 * 0.3f;
			Projectile.position.Y += -22 * player.gravDir * (float)(1.2 + Math.Sin(Main.time / 18d) / 8d) * 0.3f;

			// 防止玩家移动的时候，弹幕贴脸，XY 轴都会因为移动一点点，但是不会很多
			Projectile.position += player.velocity * 0.7f;
			Projectile.position -= Vector2.Clamp(player.velocity * 0.1f, new Vector2(-10), new Vector2(10));

			Projectile.spriteDirection = player.direction;
			Projectile.velocity *= 0;
		}

		if (!Main.mouseLeft && Release)
		{
			if (ai1 > 0)
			{
				ai0 *= 0.9f;
				ai1 -= 1f;
				Vector2 isWhat = Main.MouseWorld - player.MountedCenter;
				Projectile.Center = player.MountedCenter + Vector2.Normalize(isWhat).RotatedBy(ai0 / 4d) * (8f - ai0 * 4);
			}
			else
			{
				if (Projectile.timeLeft > 21)
				{
					Projectile.timeLeft = 20;
				}

				for (int j = 0; j < 16; j++)
				{
					Vector2 iDoNotKnowWhat = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale;
					int dust = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<MothSmog>(), iDoNotKnowWhat.X, iDoNotKnowWhat.Y, 100, default, Main.rand.NextFloat(3.7f, 5.1f) * 0.13f);
					Main.dust[dust].alpha = (int)(Main.dust[dust].scale * 0.3);
					Main.dust[dust].rotation = Main.rand.NextFloat(0, 6.283f);
				}
			}
		}

		if (player.itemTime == 2)
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, Projectile.Center);
		Player player = Main.player[Projectile.owner];
		var v0 = new Vector2(Math.Sign((Main.MouseWorld - Main.player[Projectile.owner].MountedCenter).X), 0.6f * player.gravDir);
		Vector2 ShootCenter = Projectile.Center + new Vector2(0, 16f * player.gravDir);
		ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 2).RotatedByRandom(6.283);
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), ShootCenter, v0 * 3, ModContent.ProjectileType<NavyThunderBomb>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0, 0);
		Vector2 newVelocity = v0;
		newVelocity *= 1f - Main.rand.NextFloat(0.3f);
		newVelocity *= 2f;

		for (int j = 0; j < 30; j++)
		{
			Vector2 v = newVelocity / 27f * j;
			Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
			int num20 = Dust.NewDust(ShootCenter, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), v1.X, v1.Y, 100, default, Main.rand.NextFloat(0.6f, 1.8f) * 0.4f);
			Main.dust[num20].noGravity = true;
		}
		for (int j = 0; j < 30; j++)
		{
			Vector2 v = newVelocity / 54f * j;
			Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
			float Scale = Main.rand.NextFloat(3.7f, 5.1f);
			int num21 = Dust.NewDust(ShootCenter + new Vector2(4, 4.5f), 0, 0, ModContent.DustType<BlueParticleDark2StoppedByTile>(), v1.X, v1.Y, 100, default, Scale);
			Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override void PostDraw(Color lightColor)
	{
		if (!Release)
			return;
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		Vector2 v0 = Projectile.Center - player.MountedCenter;
		if (Main.mouseLeft)
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v0.Y, v0.X) - Math.PI / 2d));

		Texture2D TexMain = MythContent.QuickTexture("TheFirefly/Projectiles/NavyThunderTex/FlameSkull");
		Texture2D TexMainG = MythContent.QuickTexture("TheFirefly/Projectiles/NavyThunderTex/FlameSkullGlow");

		Projectile.frame = (int)(addi % 25 / 5f);
		var DrawRect = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);

		Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		SpriteEffects se = SpriteEffects.None;
		if (player.direction == 1)
			se = SpriteEffects.FlipHorizontally;

		Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition, DrawRect, drawColor, Projectile.rotation, new Vector2(27, 42), 1f, se, 0);
		Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition, DrawRect, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(27, 42), 1f, se, 0);
	}

	public static void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex)
	{
		float Wid = 6f;
		Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

		var vertex2Ds = new List<Vertex2D>();

		for (int x = 0; x < 3; x++)
		{
			float Value0 = (float)(Main.time / 291d + 20) % 1f;
			float Value1 = (float)(Main.time / 291d + 20.03) % 1f;
			vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

			vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 1, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));
		}

		Main.graphics.GraphicsDevice.Textures[0] = tex;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
	}

	public void DrawWarp(VFXBatch sb)
	{
		if (!Release)
			return;
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		Vector2 v0 = Projectile.Center - player.MountedCenter;
		if (Main.mouseLeft)
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v0.Y, v0.X) - Math.PI / 2d));

		Texture2D TexMainG = MythContent.QuickTexture("TheFirefly/Projectiles/NavyThunderTex/FlameSkullWarp");

		Projectile.frame = (int)(addi % 25 / 5f);
		var DrawRect = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);

		SpriteEffects se = SpriteEffects.None;
		if (player.direction == 1)
			se = SpriteEffects.FlipHorizontally;
		sb.Draw(TexMainG, Projectile.Center - Main.screenPosition, DrawRect, new Color(0.3f, 0.3f, 0.2f, 0), Projectile.rotation, new Vector2(27, 42), 1f, se);
	}
}