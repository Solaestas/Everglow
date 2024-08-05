
using Everglow.Yggdrasil.YggdrasilTown.Buffs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

internal class LightBeam : ModProjectile
{
	private const int InitialDuration = 150;
	private const int MaxDuration = 360;
	private const int DurationAddedPerHit = 90;

	public override void SetDefaults()
	{
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = 48;
		Projectile.friendly = true;
		Projectile.magic = true;
		Projectile.extraUpdates = 100;
		Projectile.timeLeft = 200;
		Projectile.penetrate = 1;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		int buffType = ModContent.BuffType<Photolysis>();

		if (!target.HasBuff(buffType))
		{
			target.AddBuff(buffType, InitialDuration);
		}
		else
		{
			int buffIndex;
			for (buffIndex = 0; buffIndex < target.buffType.Length; buffIndex++)
			{
				if (buffType == target.buffType[buffIndex])
				{
					break;
				}
			}

			if (buffIndex >= target.buffType.Length)
			{
				return;
			}

			if (target.buffTime[buffIndex] + DurationAddedPerHit <= MaxDuration)
			{
				target.buffTime[buffIndex] += DurationAddedPerHit;
			}
			else
			{
				target.buffTime[buffIndex] = MaxDuration;
			}
		}

		for (int i = 0; i < target.buffType.Length; i++)
		{
			if (buffType == target.buffType[i])
			{
				Console.WriteLine(target.buffTime[i]);
				return;
			}
		}
	}
}