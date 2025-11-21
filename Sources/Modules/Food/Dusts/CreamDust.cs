using Everglow.Commons.Utilities;

namespace Everglow.Food.Dusts;
public class CreamDust : ModDust
{
	public override void OnSpawn(Dust dust)
	{
		dust.rotation = dust.scale;
		dust.dustIndex = 0;
	}

	public override bool Update(Dust dust)
	{
		if (!TileUtils.PlatformCollision(dust.position))
		{
			dust.velocity.Y += 0.15f;
		}
		else
		{
			dust.velocity *= 0;
			dust.scale *= 0.95f;
		}
		dust.dustIndex++;
		dust.scale = MathF.Sin(dust.dustIndex / 10f) * dust.rotation * 0.4f;
		dust.velocity *= 0.98f;
		if (dust.dustIndex > 31)
		{
			dust.active = false;
		}
		dust.position += dust.velocity;
		return false;
	}
}