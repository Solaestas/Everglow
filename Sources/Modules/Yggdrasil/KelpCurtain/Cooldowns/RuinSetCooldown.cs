using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.KelpCurtain.Cooldowns;

public class RuinSetCooldown : CooldownBase
{
	public static new string ID => nameof(RuinSetCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.RuinSetBuff.Value;

	public override bool EnableCutShader => true;
}