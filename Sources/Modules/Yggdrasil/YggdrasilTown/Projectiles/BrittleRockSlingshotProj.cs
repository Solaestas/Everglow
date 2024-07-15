using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.Weapons.Slingshots;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class BrittleRockSlingshotProj : SlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<BrittleRockSlingshotStone>();
		SlingshotLength = 12;
		MaxPower = 60;
	}

	public override void AI()
	{
		if (Power < MaxPower)
		{
			Power++;
		}
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		if (Power == 24 && player.controlUseItem)
		{
			SoundEngine.PlaySound(new SoundStyle("Everglow/Myth/Misc/Sounds/NewSlingshot" + Main.rand.Next(8).ToString()).WithVolumeScale(0.8f), Projectile.Center);
		}
		Vector2 MouseToPlayer = Main.MouseWorld - player.MountedCenter;
		if (player.controlUseItem && Release)
		{
			Projectile.rotation = (float)(Math.Atan2(MouseToPlayer.Y, MouseToPlayer.X) + Math.PI * 0.25);
			Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer) * 15f + new Vector2(0, -4);
			Projectile.timeLeft = 5 + Power;
		}
		float DrawRot;
		if (Projectile.Center.X < player.MountedCenter.X)
		{
			player.direction = -1;
			DrawRot = Projectile.rotation - MathF.PI / 4f;
		}
		else
		{
			player.direction = 1;
			DrawRot = Projectile.rotation - MathF.PI / 4f;
		}
		Vector2 MinusShootDir = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Projectile.Center - Main.MouseWorld;
		Vector2 SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Vector2.Normalize(MinusShootDir) * Power / 3f;
		if (player.direction == -1)
		{
			MinusShootDir = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Projectile.Center - Main.MouseWorld;
			SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Vector2.Normalize(MinusShootDir) * Power / 3f;
		}
		if (!player.controlUseItem && Release)
		{
			Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer) * 15f + new Vector2(0, -4);
			SoundEngine.PlaySound(new SoundStyle("Everglow/Myth/Misc/Sounds/SlingshotShoot"), Projectile.Center);
			if (Power == MaxPower)
			{
				SoundEngine.PlaySound(new SoundStyle("Everglow/Myth/Misc/Sounds/SlingshotShoot2"), Projectile.Center);
			}
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + SlingshotStringHead, -Vector2.Normalize(MinusShootDir) * (float)(Power / 6f + 8f), ShootProjType, (int)(Projectile.damage * (0.5 + Power / 120)), Projectile.knockBack, player.whoAmI, Power / 450f);

			Projectile.timeLeft = 5;
			Release = false;
		}
		if (!player.controlUseItem && !Release)
		{
			Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer) * 15f + new Vector2(0, -4);
		}
	}

	public override void DrawString()
	{
		base.DrawString();
		Player player = Main.player[Projectile.owner];
		Color drawColor = Lighting.GetColor((int)(Projectile.Center.X / 16.0), (int)(Projectile.Center.Y / 16.0));
		float DrawRot = Projectile.rotation - MathF.PI / 4f;
		Vector2 HeadCenter = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot);
		if (player.direction == -1)
		{
			HeadCenter = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d);
		}
		HeadCenter += Projectile.Center - Main.screenPosition;
		Vector2 SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Projectile.Center - Main.MouseWorld;
		Vector2 SlingshotStringTail = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Vector2.Normalize(SlingshotStringHead) * Power * 0.2625f;
		if (player.direction == -1)
		{
			SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Projectile.Center - Main.MouseWorld;
			SlingshotStringTail = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Vector2.Normalize(SlingshotStringHead) * Power * 0.2625f;
		}
		SlingshotStringTail += Projectile.Center - Main.screenPosition;
		Vector2 Head1 = HeadCenter + HeadCenter.RotatedBy(Math.PI / 8 + DrawRot).SafeNormalize(Vector2.Zero) * SplitBranchDis;
		Vector2 Head2 = HeadCenter - HeadCenter.RotatedBy(Math.PI / 8 + DrawRot).SafeNormalize(Vector2.Zero) * SplitBranchDis;
		if (player.direction == -1)
		{
			Head1 = HeadCenter + HeadCenter.RotatedBy(Math.PI / 8 * 5 + DrawRot).SafeNormalize(Vector2.Zero) * SplitBranchDis;
			Head2 = HeadCenter - HeadCenter.RotatedBy(Math.PI / 8 * 5 + DrawRot).SafeNormalize(Vector2.Zero) * SplitBranchDis;
		}

		var newcolor = Color.Lerp(drawColor, new Color(255, 255, 255, 200), Power / 130f + 0.1f);
		Texture2D tex = ModAsset.BrittleRockString.Value;
		Main.spriteBatch.Draw(tex, Head1 * 0.67f + SlingshotStringTail * 0.33f, null, newcolor, DrawRot, tex.Size() / 2f, 1f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(tex, Head1 * 0.95f + SlingshotStringTail * 0.05f, null, newcolor, DrawRot, tex.Size() / 2f, 0.85f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(tex, Head1 * 0.33f + SlingshotStringTail * 0.67f, null, newcolor, DrawRot, tex.Size() / 2f, 1f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(tex, SlingshotStringTail, null, newcolor, DrawRot, tex.Size() / 2f, 1f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(tex, Head2 * 0.33f + SlingshotStringTail * 0.67f, null, newcolor, DrawRot, tex.Size() / 2f, 1f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(tex, Head2 * 0.95f + SlingshotStringTail * 0.05f, null, newcolor, DrawRot, tex.Size() / 2f, 0.85f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(tex, Head2 * 0.67f + SlingshotStringTail * 0.33f, null, newcolor, DrawRot, tex.Size() / 2f, 1f, SpriteEffects.None, 0);
	}

	
}
