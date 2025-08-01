using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.YggdrasilTown.Cooldowns;

public class AuburnSelfReinforcingCooldown : CooldownBase
{
	public static new string ID => nameof(AuburnSelfReinforcingCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => Commons.ModAsset.BuffTemplate.Value;
}