using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Yggdrasil.YggdrasilTown.Cooldowns;

public class IstafelsSunfireGraspSkillCooldown : CooldownBase
{
	public static new string ID => nameof(IstafelsSunfireGraspSkillCooldown);

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.IstafelsSunfireGraspSkillBuff.Value;

	public override bool EnableCutShader => true;
}