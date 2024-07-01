using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Dusts;

public class RockElemental_Energy : ModDust
{
	public override bool Update(Dust dust)
	{
		dust.position += dust.velocity;
		bool moved = false;
		if(dust.customData is Projectile proj)
		{
			if (proj.active && proj.ModProjectile is RockElemental_ThrowingStone rockElemental_ThrowingStone)
			{
				if (rockElemental_ThrowingStone.PolymerizationTimer > 0)
				{
					Vector2 pierceAim = proj.Center - dust.velocity - dust.position;
					if (pierceAim.Length() < 400)
					{
						dust.velocity = Vector2.Lerp(dust.velocity, Utils.SafeNormalize(pierceAim, Vector2.zeroVector) * 9f, 0.1f);
						if (pierceAim.Length() < 30)
						{
							dust.scale *= 0.9f;
						}
						else
						{
							dust.scale = dust.scale * 0.97f + 0.03f;
						}
						moved = true;
					}
				}
			}
		}
		else
		{
			dust.active = false;
			return false;
		}
		if(!moved)
		{
			dust.scale *= 0.8f;
		}
		if (dust.scale < 0.05f)
		{
			dust.active = false;
		}
		return false;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return new Color(1f, 1f, 1f, 0);
	}
}