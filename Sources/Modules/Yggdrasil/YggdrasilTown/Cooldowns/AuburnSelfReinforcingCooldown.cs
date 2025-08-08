using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.YggdrasilTown.Cooldowns;

public class AuburnSelfReinforcingCooldown : CooldownBase
{
	public static new string ID => nameof(AuburnSelfReinforcingCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.AuburnHoodie.Value;

	public override Vector2 TextureScale => base.TextureScale * 0.5f;
}