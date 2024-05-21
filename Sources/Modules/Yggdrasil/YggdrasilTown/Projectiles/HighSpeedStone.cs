using Everglow.Yggdrasil.YggdrasilTown.NPCs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class HighSpeedStone : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 99999999;
	}

	public NPC MyOwner;

	public override void AI()
	{
		if (MyOwner == null || !MyOwner.active || MyOwner.type != ModContent.NPCType<RockElemental>())
		{
			if (Projectile.timeLeft > 60)
			{
				Projectile.timeLeft = 60;
			}
		}
		if (Projectile.timeLeft <= 60)
		{
			Projectile.velocity.Y += 0.7f;
			Projectile.velocity *= 0.97f;
			return;
		}
		else
		{
			RockElemental rockOwner = MyOwner.ModNPC as RockElemental;
			if (rockOwner != null)
			{
				Vector2 toOwner = MyOwner.Center - Projectile.Center;
				Vector2 radius = Utils.SafeNormalize(toOwner, Vector2.zeroVector);
				float timeValue = (float)Main.time * 0.05f;
				Vector2 axis = new Vector2(0, 125f).RotatedBy(timeValue + Projectile.whoAmI);
				Projectile.velocity *= 0f;
				if (toOwner.Length() is > 100 and < 150)
				{
					Projectile.Center = Vector2.Lerp(Projectile.Center, MyOwner.Center - axis, 0.25f);
				}
				else
				{
					Projectile.Center = Vector2.Lerp(Projectile.Center, MyOwner.Center - radius * 125, 0.25f);
				}
			}
			else
			{
				return;
			}
		}
	}

	public override void OnSpawn(IEntitySource source)
	{
		if (MyOwner == null)
		{
			foreach (var npc in Main.npc)
			{
				if (npc.type == ModContent.NPCType<RockElemental>())
				{
					if (npc.active && npc.life >= 0)
					{
						if ((npc.Center - Projectile.Center).Length() < 500)
						{
							MyOwner = npc;
							break;
						}
					}
				}
			}
		}
		if (MyOwner == null)
		{
			Projectile.Kill();
		}
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}

	public override void OnKill(int timeLeft)
	{
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}
}