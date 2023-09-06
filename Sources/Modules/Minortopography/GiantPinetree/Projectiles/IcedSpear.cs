using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Minortopography.GiantPinetree.Dusts;
using Terraria.Audio;
namespace Everglow.Minortopography.GiantPinetree.Projectiles;

public class IcedSpear : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 240;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
	}
	internal bool shot = false;
	internal int power = 0;
	public int stickNPC = -1;
	public float relativeAngle = 0;
	public float hitTargetAngle = 0;
	public float hitTargetScale = 1;
	public Vector2 relativePos = Vector2.zeroVector;
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		int playerDir = -1;
		if (Main.MouseWorld.X > player.Center.X)
			playerDir = 1;
		if (shot)
		{
			if(Projectile.wet)
			{
				Projectile.timeLeft -= 2;
			}
			if (Projectile.lavaWet)
			{
				Projectile.timeLeft -= 24;
			}

			if (stickNPC != -1)
			{
				NPC stick = Main.npc[stickNPC];
				if (stick != null && stick.active)
				{
					Projectile.rotation = stick.rotation + relativeAngle;
					Projectile.Center = stick.Center + relativePos.RotatedBy(stick.rotation + relativeAngle - hitTargetAngle) * stick.scale / hitTargetScale;
					stick.AddBuff(BuffID.Frostburn, 5);
				}
				else
				{
					stickNPC = -1;
				}
			}
			else
			{
				Projectile.tileCollide = true;
				if (Collide(Projectile.Center))
				{
					Projectile.damage = (int)(Projectile.damage * 0.1f);
					if (Projectile.damage == 0)
					{
						Projectile.damage = 1;
					}
					Projectile.knockBack = 0;
				}
				else if (!Collision.SolidCollision(Projectile.Center, 0, 0))
				{
					Projectile.velocity.Y += 0.25f;
					Projectile.velocity *= 0.995f;
					Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
				}
			}
		}
		else
		{
			Projectile.timeLeft = 240;
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.Center, new Vector2(0, -1 * player.gravDir));
			Projectile.Center = player.Center + Projectile.velocity.RotatedBy(Math.PI * -0.5) * 20 * playerDir - Projectile.velocity * (power / 3f - 16);
			Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
			if (power < 100)
				power++;

			player.heldProj = Projectile.whoAmI;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + (float)(Math.PI * 0.25 + Math.PI * 0.6 * playerDir - (power / 40d - 1.0) * playerDir));
			player.direction = playerDir;
		}

		if (!player.controlUseItem && !shot)
		{
			shot = true;
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.Center, new Vector2(0, -1 * player.gravDir)) * (power + 100) / 8f;
			Projectile.damage = (int)(Projectile.damage * (power + 100) / 100f);
			SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
		}
		if (Projectile.velocity.Length() > 3f)
		{
			GenerateDust();
		}
	}
	public void GenerateDust()
	{
		if (Projectile.Center.X > Main.screenPosition.X - 100 && Projectile.Center.X < Main.screenPosition.X + Main.screenWidth + 100 && Projectile.Center.Y > Main.screenPosition.Y - 100 && Projectile.Center.Y < Main.screenPosition.Y + Main.screenWidth + 100)
		{
			if (Main.rand.NextBool(2))
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
				var smog = new IceSmogDust
				{
					velocity = newVelocity + Projectile.velocity * Main.rand.NextFloat(0f, 0.03f),
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-3f, 2f),
					maxTime = Main.rand.Next(137, 245),
					scale = Main.rand.NextFloat(30f, 75f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.005f, 0.005f) }
				};
				Ins.VFXManager.Add(smog);
			}
			else
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
				var smog = new IceSmogDust2
				{
					velocity = newVelocity + Projectile.velocity * Main.rand.NextFloat(0f, 0.03f),
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-3f, 2f),
					maxTime = Main.rand.Next(137, 245),
					scale = Main.rand.NextFloat(30f, 75f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.005f, 0.005f) }
				};
				Ins.VFXManager.Add(smog);
			}
			if (Main.rand.NextBool(3))
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
				var smog = new SnowPieceDust
				{
					velocity = newVelocity + Projectile.velocity * Main.rand.NextFloat(0f, 0.03f),
					Active = true,
					Visible = true,
					coord0 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
					coord1 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-3f, 2f),
					maxTime = Main.rand.Next(37, 125),
					scale = Main.rand.NextFloat(3, 6f),
					rotation = Main.rand.NextFloat(6.283f),
					rotation2 = Main.rand.NextFloat(6.283f),
					omega = Main.rand.NextFloat(-10f, 10f),
					phi = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(-0.005f, 0.005f) }
				};
				Ins.VFXManager.Add(smog);
			}
		}
		if (Main.rand.NextBool(3))
		{
			Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 0, 0, DustID.Ice, 0, 0, 0, default, Main.rand.NextFloat(0.75f, 1.25f));
			dust.noGravity = true;
		}
	}
	public bool Collide(Vector2 positon)
	{
		foreach (NPC npc in Main.npc)
		{
			if (npc.active && !npc.dontTakeDamage && !npc.friendly && !npc.townNPC)
			{
				if ((new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1)).Intersects(npc.Hitbox))
				{
					Projectile.velocity *= 0;
					relativeAngle = Projectile.rotation - npc.rotation;
					hitTargetAngle = Projectile.rotation;
					relativePos = Projectile.Center - npc.Center;
					hitTargetScale = npc.scale;
					stickNPC = npc.whoAmI;
					return true;
				}
			}
		}
		return Collision.SolidCollision(positon, 0, 0);
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		return true;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texStick = ModAsset.IcedSpear_stick.Value;
		Texture2D texIce = ModAsset.IcedSpear_ice.Value;
		Main.spriteBatch.Draw(texStick, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texStick.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		Color iceColor = lightColor;
		if (Projectile.timeLeft < 180)
		{
			iceColor *= Projectile.timeLeft / 180f;
		}
		Main.spriteBatch.Draw(texIce, Projectile.Center - Main.screenPosition, null, iceColor, Projectile.rotation, texIce.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
	public override void Kill(int timeLeft)
	{
		if(timeLeft > 60)
		{
			for (int x = 0; x < 16; x++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, 40, 40, ModContent.DustType<IceParticle>());
				d.velocity *= Projectile.velocity.Length() / 10f;
			}
			SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
		}

		Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.283d) * Projectile.velocity.Length() / 10f;
		Gore gore =  Gore.NewGoreDirect(null, Projectile.Center + v, v, ModContent.Find<ModGore>("Everglow/IcedSpear_gore").Type, 1f);
		gore.velocity = Projectile.velocity;
		gore.rotation = Projectile.rotation - MathF.PI / 4f;
	}
}

