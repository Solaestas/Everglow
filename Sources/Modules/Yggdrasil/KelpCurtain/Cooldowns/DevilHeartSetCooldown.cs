using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.KelpCurtain.Cooldowns;

public class DevilHeartSetCooldown : CooldownBase
{
	public static new string ID => nameof(DevilHeartSetCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.DevilHeartSetBuff.Value;
}