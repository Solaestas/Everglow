using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.Misc.Buffs;
using Everglow.Myth.Misc.Projectiles.Weapon.Magic.FreezeFeatherMagic;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Magic;

public class FreezeFeather : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.ignoreWater = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 360;
		Projectile.penetrate = 1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 2;
	}
	internal int timeTokill = -1;
	ModProjectile MagicArray = null;
	public override void OnSpawn(IEntitySource source)
	{
		foreach (Projectile projectile in Main.projectile)
		{
			if (projectile.active)
			{
				if (projectile.type == ModContent.ProjectileType<FreezeFeatherMagicArray>())
				{
					if (projectile.owner == Projectile.owner)
					{
						MagicArray = projectile.ModProjectile;
						break;
					}
				}
			}
		}
	}
	public override void AI()
	{
		if (timeTokill >= 0 && timeTokill <= 2)
			Projectile.Kill();
		if (timeTokill <= 15 && timeTokill > 0)
			Projectile.velocity = Projectile.oldVelocity;
		timeTokill--;
		if (timeTokill >= 0)
		{
			if (timeTokill < 10)
			{
				Projectile.damage = 0;
				Projectile.friendly = false;
			}
			Projectile.velocity *= 0f;
		}
		else
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			if (Projectile.timeLeft >= 320)
			{
				Projectile.velocity *= 1.01f;
			}
			else
			{
				if (Projectile.wet)
				{
					Projectile.velocity.Y -= 0.3f;
					Projectile.velocity *= 0.96f;
					Projectile.timeLeft -= Main.rand.Next(40, 80);
				}
				if (Projectile.timeLeft < 300)
				{
					if (Projectile.extraUpdates == 0)
					{
						Projectile.velocity.Y += 0.1f;
					}
				}
			}
		}
		if (Main.rand.NextBool(6))
		{
			Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 2)).RotatedByRandom(MathHelper.TwoPi);
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.FreezeFeather>(), v.X, v.Y, 150, default, Main.rand.NextFloat(0.8f, 1.7f));
		}
		for (int g = 0; g < 1; g++)
		{
			Vector2 iceV = new Vector2(0, Main.rand.NextFloat(0, 0.9f)).RotatedByRandom(MathHelper.TwoPi);
			Dust ice = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.IceCrystal>(), iceV.X, iceV.Y, 150, default, Main.rand.NextFloat(0.2f, 0.6f));
			ice.velocity = iceV;
			ice.color.G = (byte)Main.rand.Next(240);
		}

		if(Projectile.Center.X > Main.screenPosition.X - 100 && Projectile.Center.X < Main.screenPosition.X + Main.screenWidth + 100 && Projectile.Center.Y > Main.screenPosition.Y - 100 && Projectile.Center.Y < Main.screenPosition.Y + Main.screenWidth + 100)
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
					maxTime = Main.rand.Next(67, 120),
					scale = Main.rand.NextFloat(18f, 45f),
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
					maxTime = Main.rand.Next(67, 120),
					scale = Main.rand.NextFloat(18f, 45f),
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
					scale = Main.rand.NextFloat(2f, 8f),
					rotation = Main.rand.NextFloat(6.283f),
					rotation2 = Main.rand.NextFloat(6.283f),
					omega = Main.rand.NextFloat(-10f, 10f),
					phi = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(-0.005f, 0.005f) }
				};
				Ins.VFXManager.Add(smog);
			}
		}
		if (Projectile.timeLeft <= 100 && timeTokill < 0)
		{
			if (MagicArray != null)
			{
				var arrayProj = MagicArray as FreezeFeatherMagicArray;
				arrayProj.WingPower += 0.1f;
			}
			AmmoHit();
		}
		if (Projectile.position.X <= 320 || Projectile.position.X >= Main.maxTilesX * 16 - 320)
		{
			Projectile.Kill();
		}
		if (Projectile.position.Y <= 320 || Projectile.position.Y >= Main.maxTilesY * 16 - 320)
		{
			Projectile.Kill();
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public override void PostDraw(Color lightColor)
	{
		if (timeTokill >= 0)
		{
			return;
		}
		SpriteEffects spriteEffects = SpriteEffects.None;
		if (Projectile.spriteDirection == -1)
			spriteEffects = SpriteEffects.FlipHorizontally;
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
		int frameHeight = texture.Height / Main.projFrames[Projectile.type];
		int startY = frameHeight * Projectile.frame;
		var sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
		Vector2 origin = sourceRectangle.Size() / 2f;
		float offsetX = 20f;
		origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;
		float amount = 1f;
		if (Projectile.timeLeft >= 1040)
		{
			amount = (1080 - Projectile.timeLeft) / 40f;
		}
		Color aimColor = new Color(1f, 1f, 1f, 1f);
		Color drawColor = Color.Lerp(lightColor, aimColor, amount);
		if (Projectile.wet)
		{
			float value = 0.6f;
			if (Projectile.timeLeft < 700)
			{
				value = (Projectile.timeLeft - 100) / 1000f;
			}
			aimColor = new Color(value / 6f, value / 6f, value / 2f, 1f);
			drawColor = aimColor;
		}

		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (MagicArray != null)
		{
			var arrayProj = MagicArray as FreezeFeatherMagicArray;
			arrayProj.WingPower += 0.1f;
		}
		AmmoHit();
		return false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (target.type is not NPCID.MoonLordHead and not NPCID.MoonLordHand and not NPCID.MoonLordCore)
		{
			if (!target.HasBuff(BuffID.Frostburn) && !target.HasBuff(BuffID.Frostburn2))
			{
				target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1]);
				if (Main.rand.NextBool(7))
				{
					if (MagicArray == null)
					{
						target.AddBuff(BuffID.Frostburn, (int)Projectile.ai[1] * 37);
					}
					else
					{
						target.AddBuff(BuffID.Frostburn2, (int)Projectile.ai[1] * 37);
					}
				}
			}
			else
			{
				if (Main.rand.NextBool(6))
				{
					target.AddBuff(ModContent.BuffType<Freeze>(), (int)Projectile.ai[1] * 4);
				}
			}
		}
		if (MagicArray != null)
		{
			var arrayProj = MagicArray as FreezeFeatherMagicArray;
			arrayProj.WingPower += 3f;
		}
		AmmoHit();
	}
	public  void AmmoHit()
	{
		timeTokill = 20;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.velocity = Projectile.oldVelocity;

		SoundEngine.PlaySound((SoundID.DD2_WitherBeastCrystalImpact.WithVolume(0.3f)).WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)), Projectile.Center);
		for (int j = 0; j < 4; j++)
		{
			Vector2 v = new Vector2(0, Main.rand.NextFloat(7, 20)).RotatedByRandom(MathHelper.TwoPi);
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.FreezeFeather>(), v.X, v.Y, 150, default, Main.rand.NextFloat(1.8f, 3.7f));
		}
		for (int j = 0; j < 20; j++)
		{
			Vector2 v = new Vector2(0, Main.rand.NextFloat(2f, 4.6f)).RotatedByRandom(MathHelper.TwoPi);
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.IceCrystal2>(), v.X, v.Y, 150, default, Main.rand.NextFloat(0.6f, 1.4f));
			d.velocity = v;
		}
		for (int j = 0; j < 60; j++)
		{
			Vector2 v = new Vector2(0, Main.rand.NextFloat(1f, 2.6f)).RotatedByRandom(MathHelper.TwoPi);
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.IceCrystal>(), v.X, v.Y, 150, default, Main.rand.NextFloat(0.4f, 0.6f));
			d.velocity = v;
			d.color.G = 120;
		}
		GenerateSmog(4);
	}
	public void GenerateSmog(int Frequency)
	{
		float mulVelocity = 1f;
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new IceSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + newVelocity * 16,
				maxTime = Main.rand.Next(87, 175),
				scale = Main.rand.NextFloat(320f, 435f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(somg);
		}
		for (int g = 0; g < Frequency / 2 - 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new IceSmogDust2
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + newVelocity * 16,
				maxTime = Main.rand.Next(87, 175),
				scale = Main.rand.NextFloat(220f, 235f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(somg);
		}
		for (int g = 0; g < Frequency * 6; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 4.6f)).RotatedByRandom(MathHelper.TwoPi);
			var smog = new SnowPieceDust
			{
				velocity = newVelocity + Projectile.velocity * Main.rand.NextFloat(0f, 0.03f),
				Active = true,
				Visible = true,
				coord0 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
				coord1 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-3f, 2f),
				maxTime = Main.rand.Next(47, 85),
				scale = Main.rand.NextFloat(2f, 12f),
				rotation = Main.rand.NextFloat(6.283f),
				rotation2 = Main.rand.NextFloat(6.283f),
				omega = Main.rand.NextFloat(-10f, 10f),
				phi = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(-0.005f, 0.005f) }
			};
			Ins.VFXManager.Add(smog);
		}
	}
}