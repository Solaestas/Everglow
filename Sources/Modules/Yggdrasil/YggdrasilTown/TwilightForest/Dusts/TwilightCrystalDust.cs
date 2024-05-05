namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Dusts;

public class TwilightCrystalDust : ModDust
{
	public override void OnSpawn(Dust dust)
	{

	}

	public override bool Update(Dust dust)
	{
		dust.rotation += (dust.dustIndex - 127.5f) / 6000f * dust.velocity.Length();
		dust.position += dust.velocity;
		int hitboxSize = (int)(4 * dust.scale);
		if (dust.velocity.Length() < 0.2f)
		{
			if (Collision.SolidCollision(dust.position, hitboxSize, hitboxSize))
			{
				dust.velocity *= 0.4f;
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
		Lighting.AddLight(dust.position, new Vector3(0.03f,0.3f, 0.65f) * (1 - dust.alpha / 255f));
		return false;
	}
}