using Everglow.Commons.VFX.CommonVFXDusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class ThermoprobeStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 45;
		Item.height = 45;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTime = 17;
		Item.useAnimation = 17;
		Item.damage = 11;
		Item.knockBack = 1f;
		Item.mana = 9;
		Item.DamageType = DamageClass.Summon;
		Item.noMelee = true;
		Item.shoot = ModContent.ProjectileType<Thermoprobe>();
		Item.UseSound = SoundID.Item20;
		Item.value = Item.buyPrice(0, 2, 0, 0);
		Item.rare = ItemRarityID.Green;
	}

	public override bool CanUseItem(Player player)
	{
		return player.ownedProjectileCounts[Item.shoot] < player.maxMinions * 2;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Vector2 pos = player.Center + player.DirectionTo(Main.MouseWorld) * (float)Math.Min(200, (Main.MouseWorld - player.Center).Length());
		Projectile.NewProjectile(player.GetSource_ItemUse(Item), pos, Vector2.Zero, type, damage, knockback, player.whoAmI, 3);
		return false;
	}
}

public class Thermoprobe : ModProjectile
{
	// 不占用召唤栏
	public override void SetDefaults()
	{
		Projectile.width = 1;
		Projectile.height = 1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.scale = 1;
		Projectile.timeLeft = 60;
		Projectile.tileCollide = false;
		Projectile.aiStyle = -1;
		oldPos = new Vector2[6];
		oldRot = new float[6];
		oldFrame = new int[6];
	}

	public Vector2[] oldPos;
	public float[] oldRot;
	public int[] oldFrame;
	public float DetectLightenValue;

	public override void AI()
	{
		ProjectileUtils.TrackOldValue(oldPos, Projectile.Center);
		ProjectileUtils.TrackOldValue(oldRot, Projectile.rotation);
		ProjectileUtils.TrackOldValue(oldFrame, Projectile.frame);

		if (Projectile.ai[0] > 0)
		{
			Projectile.timeLeft++;
		}
		else
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.velocity *= 0.98f;
			if (Projectile.timeLeft == 1)
			{
				SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
				FireSmogDust smoke = new()
				{
					maxTime = 40,
					position = Projectile.Center,
					velocity = -Vector2.UnitY * Main.rand.NextFloat(2f),
				};
				smoke.scale = Main.rand.Next(50, 110);
				Ins.VFXManager.Add(smoke);
				for (int i = 0; i < 10; i++)
				{
					FireSparkDust dust = new();
					dust.ai = new float[3];
					dust.ai[1] = Main.rand.NextFloatDirection() * 0.1f;
					dust.maxTime = 30;
					dust.velocity = Main.rand.NextVector2Circular(5, 5);
					dust.position = Projectile.Center;
					dust.scale = 15;
					Ins.VFXManager.Add(dust);
				}
			}
			return;
		}
		NPC target = null;
		float mindis = 600;
		foreach (NPC npc in Main.ActiveNPCs)
		{
			if (npc.CanBeChasedBy())
			{
				float dis = Vector2.Distance(npc.Center, Projectile.Center);
				if (dis < mindis)
				{
					target = npc;
					mindis = dis;
				}
			}
		}

		if (target != null) // 攻击
		{
			Vector2 targetPos = target.Center + target.DirectionTo(Projectile.Center) * 240;
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(targetPos) * 8, 0.1f);
			Projectile.rotation = Terraria.Utils.AngleLerp(Projectile.rotation, Projectile.DirectionTo(target.Center).ToRotation(), 0.2f);
			if (Vector2.Distance(Projectile.Center, targetPos) < 360)
			{
				if (++Projectile.ai[1] > 40 && Projectile.ai[0] > 0)
				{
					SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);

					Projectile.ai[0]--;
					Projectile.ai[1] = 0;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Projectile.rotation.ToRotationVector2() * 20, Projectile.DirectionTo(target.Center) * 30 + target.velocity, ModContent.ProjectileType<Thermoprobe_Fireball>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
				}
			}
		}
		else // 游荡
		{
			Player player = Main.player[Projectile.owner];
			Vector2 targetPos = player.Center + player.DirectionTo(Projectile.Center) * 100;
			FlyTo(targetPos, 0.2f);
			if (Projectile.velocity.Length() > 12 && Vector2.Distance(Projectile.Center, player.Center) < 800)
			{
				Projectile.velocity *= 0.96f;
			}

			if (Vector2.Distance(Projectile.Center, player.Center) > 1200)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center) * 8, 0.1f);
			}
			if (Vector2.Distance(Projectile.Center, player.Center) > 5000)
			{
				Projectile.Center = player.Center;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		Dust d = Dust.NewDustPerfect(Projectile.Center + (Projectile.rotation + 1.8f).ToRotationVector2() * 10 * MathF.Sin(Projectile.ai[2] / 24 * MathF.PI), 6);
		d.noGravity = true;

		d = Dust.NewDustPerfect(Projectile.Center + (Projectile.rotation - 1.8f).ToRotationVector2() * 10 * MathF.Sin(Projectile.ai[2] / 24 * MathF.PI), 6);
		d.noGravity = true;
	}

	public void FlyTo(Vector2 pos, float acc)
	{
		if (Projectile.Center.X < pos.X)
		{
			Projectile.velocity.X += acc;
		}
		else
		{
			Projectile.velocity.X -= acc;
		}
		if (Projectile.Center.Y < pos.Y)
		{
			Projectile.velocity.Y += acc;
		}
		else
		{
			Projectile.velocity.Y -= acc;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		int maxFrame = 6;
		if (!Main.gamePaused)
		{
			Projectile.frameCounter++;
			Projectile.ai[2]++;
			if (Projectile.frameCounter > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame > maxFrame - 1)
				{
					Projectile.frame = 0;
					Projectile.ai[2] = 0;
				}
			}
			if (Collision.SolidCollision(Projectile.Center, 0, 0))
			{
				DetectLightenValue = (float)Utils.Lerp(DetectLightenValue, 1f, 0.25f);
			}
			else
			{
				if (DetectLightenValue > 0.01f)
				{
					DetectLightenValue = (float)Utils.Lerp(DetectLightenValue, 0f, 0.25f);
				}
				else
				{
					DetectLightenValue = 0;
				}
			}
		}
		Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
		Vector2 origin = new Vector2(tex.Width, tex.Height / maxFrame) / 2;
		for (int i = 0; i < oldPos.Length; i++)
		{
			Main.EntitySpriteDraw(tex, oldPos[i] - Main.screenPosition, tex.Frame(1, maxFrame, 0, oldFrame[i]), lightColor * (float)Math.Pow(1 - ((float)i / oldPos.Length), 1.5f) * 0.2f, oldRot[i] + MathHelper.PiOver2, origin, Projectile.scale, 0, 0);
		}
		Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, tex.Frame(1, maxFrame, 0, Projectile.frame), lightColor, Projectile.rotation + MathHelper.PiOver2, origin, Projectile.scale, 0, 0);
		if (DetectLightenValue > 0)
		{
			Texture2D star = Commons.ModAsset.StarSlash.Value;
			Texture2D bloom = Commons.ModAsset.Point.Value;
			Color detectLight = new Color(0.7f + 0.3f * DetectLightenValue, 0.7f * DetectLightenValue, 0.6f * DetectLightenValue * DetectLightenValue, 0f);
			Color bloomLight = new Color(0.7f + 0.3f * DetectLightenValue, 0.2f * DetectLightenValue, 0.1f * DetectLightenValue * DetectLightenValue, 0f);
			Texture2D glow = ModAsset.Thermoprobe_glow.Value;
			Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, detectLight * DetectLightenValue, Projectile.rotation + MathHelper.PiOver2, new Vector2(glow.Width * 0.5f, origin.Y), Projectile.scale, 0, 0);
			Main.EntitySpriteDraw(bloom, Projectile.Center - Main.screenPosition, null, bloomLight * DetectLightenValue * 0.4f, 0, bloom.Size() * 0.5f, DetectLightenValue * 0.5f + 0.2f, 0, 0);
			Main.EntitySpriteDraw(star, Projectile.Center - Main.screenPosition, null, detectLight, 0, star.Size() * 0.5f, new Vector2(1, DetectLightenValue) * 0.7f, 0, 0);
			Main.EntitySpriteDraw(star, Projectile.Center - Main.screenPosition, null, detectLight, MathHelper.PiOver2, star.Size() * 0.5f, new Vector2(1, DetectLightenValue) * 0.7f, 0, 0);
			Lighting.AddLight(Projectile.Center, new Vector3(0.7f + 0.3f * DetectLightenValue, 0.7f * DetectLightenValue, 0.6f * DetectLightenValue * DetectLightenValue) * 3);
			for (int i = 0; i < 8; i++)
			{
				Lighting.AddLight(Projectile.Center + new Vector2(0, 32).RotatedBy(i / 8d * MathHelper.TwoPi), new Vector3(0.7f + 0.3f * DetectLightenValue, 0.7f * DetectLightenValue, 0.6f * DetectLightenValue * DetectLightenValue) * 1.4f);
			}
			for (int i = 0; i < 16; i++)
			{
				Lighting.AddLight(Projectile.Center + new Vector2(0, 64).RotatedBy(i / 16d * MathHelper.TwoPi), new Vector3(0.7f + 0.3f * DetectLightenValue, 0.7f * DetectLightenValue, 0.6f * DetectLightenValue * DetectLightenValue) * 0.7f);
			}
		}
		return false;
	}
}

public class Thermoprobe_Fireball : ModProjectile
{
	public override string Texture => "Terraria/Images/Projectile_0";

	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.timeLeft = 200;
		Projectile.extraUpdates = 0;
		Projectile.tileCollide = false;
		Projectile.aiStyle = -1;
	}

	public override void AI()
	{
		Dust d = Dust.NewDustPerfect(Projectile.Center, 6);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		lightColor = Color.White;
		Texture2D tex = ModContent.Request<Texture2D>("Terraria/Images/Projectile_666").Value;
		Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation() + 1.57f, new Vector2(tex.Width, tex.Height) / 2, Projectile.scale, 0, 0);

		return true;
	}
}