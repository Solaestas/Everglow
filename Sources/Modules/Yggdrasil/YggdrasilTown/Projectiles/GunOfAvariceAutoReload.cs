using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class GunOfAvariceAutoReload : ModProjectile
{
	private bool ReloadStatus { get => !(Projectile.ai[0] == 0); }

	private bool HasNotPlayedSound { get; set; } = true;

	private Player Owner => Main.player[Projectile.owner];

	public override void SetDefaults()
	{
		Projectile.width = 62;
		Projectile.height = 32;
		Projectile.timeLeft = Items.Weapons.GunOfAvarice.AutoReloadDuration;
		Projectile.penetrate = -1;
		Projectile.hide = true;
		Projectile.scale = 0.75f;
	}

	public override void AI()
	{
		if (Projectile.timeLeft <= 15 && HasNotPlayedSound)
		{
			HasNotPlayedSound = false;
			SoundEngine.PlaySound(new SoundStyle("Everglow/Yggdrasil/YggdrasilTown/Sounds/GunReload2"));
			if (Projectile.ai[0] == 1)
			{
				VisualEffectFail(Projectile.ai[1]);
				Owner.Hurt(PlayerDeathReason.ByCustomReason($"{Owner.name} died in explosion!"), Projectile.damage, 0, false, false, 0);
			}
			if (Projectile.ai[0] == 0)
			{
				VisualEffectSuccess(Projectile.ai[1]);
			}
		}
		float timeValue = Projectile.timeLeft / 90f;
		Projectile.rotation = -MathHelper.PiOver2 + Owner.direction * (2 - timeValue * 1.4f);
		Projectile.spriteDirection = Owner.direction;
		Projectile.Center = Owner.Center + new Vector2(-timeValue * 15f * Owner.direction, 0);
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		overPlayers.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D gun = ModAsset.GunOfAvarice.Value;
		lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
		Main.spriteBatch.Draw(gun, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, gun.Size() * 0.5f, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
		return false;
	}

	public void VisualEffectFail(float level)
	{
		for (int i = 0; i < level * 12 + 40; i++)
		{
			Vector2 vel = new Vector2(0, Main.rand.NextFloat(5.6f, 8.4f)).RotatedByRandom(MathHelper.TwoPi);
			var dust = new Avarice_Fail_dust
			{
				velocity = vel,
				Active = true,
				Visible = true,
				position = Owner.Center,
				maxTime = Main.rand.Next(60, 120),
				scale = Main.rand.NextFloat(1f, 2f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(4.0f, 10.93f) },
			};
			Ins.VFXManager.Add(dust);
		}
		for (int i = 0; i < level * 4 + 14; i++)
		{
			Vector2 vel = new Vector2(0, Main.rand.NextFloat(5.6f, 13.4f)).RotatedByRandom(MathHelper.TwoPi);
			var cube = new Avarice_Fail_cube
			{
				velocity = vel,
				Active = true,
				Visible = true,
				position = Owner.Center,
				maxTime = Main.rand.Next(60, 120),
				scale = Main.rand.NextFloat(10f, 50f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(4.0f, 10.93f), 0.06f },
			};
			Ins.VFXManager.Add(cube);
		}
		float waveRot = Main.rand.NextFloat(6.283f);
		for (int i = 0; i < 2; i++)
		{
			var wave = new Avarice_Fail_wave
			{
				velocity = Vector2.zeroVector,
				Active = true,
				Visible = true,
				position = Owner.Center,
				maxTime = Main.rand.Next(30, 40),
				scale = 1 + i * 0.6f,
				rotation = waveRot + i * 2f,
				ai = new float[] { 0.04f * MathF.Sqrt(level) },
			};
			Ins.VFXManager.Add(wave);
		}
	}

	public void VisualEffectSuccess(float level)
	{
		for (int i = 0; i < level * 3 + 10; i++)
		{
			Vector2 vel = new Vector2(0, -Main.rand.NextFloat(3.6f, 6.4f));
			Vector2 pos = new Vector2(0, Main.rand.NextFloat(0f, 22.4f)).RotatedByRandom(MathHelper.TwoPi);
			pos.Y *= 0.1f;
			var dust = new Avarice_Success_dust
			{
				velocity = vel,
				Active = true,
				Visible = true,
				position = Owner.Center + pos,
				maxTime = Main.rand.Next(60, 120),
				scale = Main.rand.NextFloat(1f, 2f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(4.0f, 10.93f) },
			};
			Ins.VFXManager.Add(dust);
		}
		for (int i = 0; i < level * 2 + 8; i++)
		{
			Vector2 vel = new Vector2(0, -Main.rand.NextFloat(1.6f, 6.4f));
			Vector2 pos = new Vector2(0, Main.rand.NextFloat(0f, 50.4f)).RotatedByRandom(MathHelper.TwoPi);
			pos.Y *= 0.1f;
			var cube = new Avarice_Success_cube
			{
				velocity = vel,
				Active = true,
				Visible = true,
				position = Owner.Center + pos,
				maxTime = Main.rand.Next(60, 120),
				scale = Main.rand.NextFloat(10f, 20f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(4.0f, 20.93f), 0.06f },
			};
			Ins.VFXManager.Add(cube);
		}
	}
}