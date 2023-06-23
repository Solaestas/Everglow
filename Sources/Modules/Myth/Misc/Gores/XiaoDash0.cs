﻿using Everglow.Myth.LanternMoon.Gores;

namespace Everglow.Myth.Misc.Gores;

public class XiaoDash0 : DissolveGore
{
	public override string EffectPath()
	{
		return "Effects/WindDust";
	}
	public override bool Update(Gore gore)
	{
		gore.velocity.Y -= LightValue;
		gore.timeLeft -= 20;
		gore.rotation += 0.4f;

		float value = Math.Min(0.3f, gore.timeLeft / 500f);
		Lighting.AddLight(gore.position, new Vector3(value * 0, value, value));
		return base.Update(gore);
	}
}
