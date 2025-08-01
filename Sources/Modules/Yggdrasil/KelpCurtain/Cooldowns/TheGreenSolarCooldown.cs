using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.KelpCurtain.Cooldowns;

public class TheGreenSolarCooldown : CooldownBase
{
	public static new string ID => nameof(TheGreenSolarCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.TheGreenSolar.Value;
}