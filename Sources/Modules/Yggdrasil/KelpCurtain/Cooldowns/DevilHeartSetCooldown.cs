using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.KelpCurtain.Cooldowns;

public class DevilHeartSetCooldown : CooldownBase
{
	public static new string ID => nameof(DevilHeartSetCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.DevilHeartHelmet.Value;

	public override Vector2 TextureScale => base.TextureScale * 0.6f;
}