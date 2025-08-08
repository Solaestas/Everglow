using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.YggdrasilTown.Cooldowns;

public class IstafelsSunfireGraspFireBallCooldown : CooldownBase
{
	public static new string ID => nameof(IstafelsSunfireGraspFireBallCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.IstafelsSunfireGraspFireBallBuff.Value;

	public override bool EnableCutShader => true;
}