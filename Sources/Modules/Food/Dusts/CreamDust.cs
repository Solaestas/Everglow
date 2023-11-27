using Everglow.Commons.Utilities;

namespace Everglow.Food.Dusts;
public class CreamDust : ModDust
{
	public override void OnSpawn(Dust dust)
	{
	}

	public override bool Update(Dust dust)
	{
		if (!TileCollisionUtils.PlatformCollision(dust.position))
		{
			dust.velocity.Y += 0.15f;
		}
		else
		{
			dust.velocity *= 0;
			dust.scale *= 0.95f;
		}
		dust.scale *= 0.98f;
		dust.velocity *= 0.98f;
		if (dust.scale < 0.1f)
		{
			dust.active = false;
		}
		dust.position += dust.velocity;
		return false;
	}
}