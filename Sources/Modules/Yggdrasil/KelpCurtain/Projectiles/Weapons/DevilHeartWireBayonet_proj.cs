using Everglow.Commons.Weapons.StabbingSwords;
using Terraria.Audio;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Weapons
{
	public class DevilHeartBayonet_proj : StabbingProjectile
	{
		public float Power = 0;

		public override void SetDefaults()
		{
			Color = new Color(255, 107, 171);
			base.SetDefaults();
			TradeLength = 4;
			TradeShade = 0.3f;
			Shade = 0.2f;
			FadeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 1.05f;
			DrawWidth = 0.4f;
		}

		public override void DrawEffect(Color lightColor) => base.DrawEffect(lightColor);

		public override void DrawItem(Color lightColor)
		{
			base.DrawItem(lightColor);
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			UpdateTimer++;
			Projectile.extraUpdates = (int)(NormalExtraUpdates * player.meleeSpeed);
			int animation = 9;
			float rotationRange = Main.rand.NextFloatDirection() * (MathF.PI * 2f) * 0.05f;
			Projectile.ai[0] += 1f / 20f;
			if (Projectile.ai[0] >= 8f)
			{
				Projectile.ai[0] = 0f;
			}

			Projectile.soundDelay--;
			if (Projectile.soundDelay <= 0)
			{
				Projectile.soundDelay = SoundTimer * (1 + NormalExtraUpdates);
				SoundStyle ss = SoundID.Item1;
				SoundEngine.PlaySound(ss.WithPitchOffset(player.meleeSpeed - 1), Projectile.Center);
			}
			if (Main.myPlayer == Projectile.owner)
			{
				if (player.channel && !player.noItems && !player.CCed)
				{
					float hitSize = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
					Vector2 toMouse = Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter);
					toMouse.Normalize();
					if (toMouse.HasNaNs())
					{
						toMouse = Vector2.UnitX * player.direction;
					}
					toMouse *= hitSize;
					if (toMouse.X != Projectile.velocity.X || toMouse.Y != Projectile.velocity.Y)
					{
						Projectile.netUpdate = true;
					}
					Projectile.velocity = toMouse;
					Projectile.timeLeft = TradeLength * (NormalExtraUpdates + 1);
				}
			}
			if (!player.controlUseItem && Projectile.timeLeft > TradeLength * (NormalExtraUpdates + 1))
			{
				Projectile.timeLeft = TradeLength * (NormalExtraUpdates + 1);
			}
			if (player.HeldItem.ModItem is StabbingSwordItem modItem)
			{
				if (!player.GetModPlayer<PlayerStamina>().CheckStamina(modItem.staminaCost / Projectile.extraUpdates))
				{
					player.Heal(5);
					Projectile.Kill();
				}
			}
			Projectile.position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - Projectile.Size / 2f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.spriteDirection = Projectile.direction;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(animation);
			player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction) + rotationRange);
			player.itemAnimation = animation - (int)Projectile.ai[0];
			UpdateItemDraw();
			UpdateDarkDraw();
			UpdateLightDraw();
			if (Main.rand.NextBool(NormalExtraUpdates))
			{
				VisualParticle();
			}
		}
	}
}