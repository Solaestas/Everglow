﻿namespace Everglow.Myth.Misc.Dusts;

public class PurePurple : ModDust
{
	public override void OnSpawn(Dust dust)
	{
		dust.noGravity = true;
		dust.noLight = false;
		dust.scale *= 1f;
		dust.alpha = 0;
	}
	public override bool Update(Dust dust)
	{
		dust.position += dust.velocity;
		dust.rotation += Main.rand.NextFloat(-0.3f, 0.3f);
		dust.scale *= 0.95f;
		dust.velocity *= 0.93f;
		dust.velocity = dust.velocity.RotatedBy(-dust.velocity.Length() * dust.dustIndex / 33000f);

		Lighting.AddLight(dust.position, dust.scale * 0.15f, dust.scale * 0f, dust.scale * 0.22f);
		if (dust.scale < 0.25f)
			dust.active = false;
		return false;
	}
	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return new Color?(new Color(0.275f, 0f, 0.4f, 0f));
	}
}
