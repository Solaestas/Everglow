namespace Everglow.Yggdrasil.YggdrasilTown.Dusts.TownNPCAttack;

public class Betty_Plate_Shatter_Dust : ModDust
{
	public override bool Update(Dust dust)
	{
		dust.rotation += (dust.dustIndex - 127.5f) / 6000f * dust.velocity.Length();
		dust.position += dust.velocity;
		int hitboxSize = (int)(4 * dust.scale);
		if (dust.velocity.Length() < 3f)
		{
			if (Collision.SolidCollision(dust.position, hitboxSize, hitboxSize))
			{
				dust.velocity *= 0f;
				dust.alpha += 5;
				if (dust.alpha >= 255)
				{
					dust.active = false;
				}
				return false;
			}
		}
		if (!dust.noGravity)
		{
			dust.velocity.Y += 0.2f * dust.scale;
		}

		if (Collision.SolidCollision(dust.position + new Vector2(dust.velocity.X, 0), hitboxSize, hitboxSize))
		{
			dust.velocity.X *= -1;
			dust.velocity *= 0.4f;
			dust.alpha += 5;
			if (dust.alpha >= 255)
			{
				dust.active = false;
			}
		}
		if (Collision.SolidCollision(dust.position + new Vector2(0, dust.velocity.Y), hitboxSize, hitboxSize))
		{
			dust.velocity.Y *= -1;
			dust.velocity *= 0.4f;
			dust.alpha += 5;
			if (dust.alpha >= 255)
			{
				dust.active = false;
			}
		}
		return false;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return lightColor * ((255 - dust.alpha) / 255f);
	}
}