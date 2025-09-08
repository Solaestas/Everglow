using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.YggdrasilTown.Cooldowns;

public class MelterGearCooldown : CooldownBase
{
	public static new string ID => nameof(MelterGearCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.MelterGear.Value;

	public override Vector2 TextureScale => base.TextureScale * 0.6f;
}