using Everglow.Myth.LanternMoon.LanternCommon;

namespace Everglow.Myth.LanternMoon.NPCs;

public abstract class LanternMoonNPC : ModNPC
{
	public LanternMoonInvasionEvent LanternMoon = ModContent.GetInstance<LanternMoonInvasionEvent>();

	public float LanternMoonScore = 1f;

	public float SpawnFrequencyInLanternMoon = 1f;

	public override bool SpecialOnKill()
	{
		LanternMoon.AddPoint(LanternMoonScore);
		return base.SpecialOnKill();
	}
}