using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.YggdrasilTown.Cooldowns;

public class MelterGearCooldown : CooldownBase
{
	public static new string ID => nameof(MelterGearCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => Commons.ModAsset.BuffTemplate.Value; // TODO: No specific texture
}