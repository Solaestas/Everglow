using Everglow.Commons.Utilities;

namespace Everglow.Food.Dusts;
public class ChcolateDust : ModDust
{
	public override void OnSpawn(Dust dust)
	{
	}

	public override bool Update(Dust dust)
	{
		dust.scale *= 0.99f;
		if(dust.scale < 0.1f)
		{
			dust.active = false;
		}
		dust.velocity *= 0.98f;
		dust.position += dust.velocity;
		if(!TileUtils.PlatformCollision(dust.position))
		{
			dust.velocity.Y += 0.15f;
		}
		dust.rotation *= dust.velocity.X * 0.4f;
		if(TileUtils.PlatformCollision(dust.position + new Vector2(dust.velocity.X, 0)))
		{
			dust.velocity.X *= -0.7f;
		}
		if (TileUtils.PlatformCollision(dust.position + new Vector2(0, dust.velocity.Y)))
		{
			dust.velocity.Y *= -0.7f;
		}
		return false;
	}
}