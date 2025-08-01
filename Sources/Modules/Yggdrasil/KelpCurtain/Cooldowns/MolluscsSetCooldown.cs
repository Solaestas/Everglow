using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.KelpCurtain.Cooldowns;

public class MolluscsSetCooldown : CooldownBase
{
	public static new string ID => nameof(MolluscsSetCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => Commons.ModAsset.BuffTemplate.Value; // TODO: No specific texture
}