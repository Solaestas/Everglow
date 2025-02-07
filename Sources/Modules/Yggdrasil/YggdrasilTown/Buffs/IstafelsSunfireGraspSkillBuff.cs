namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class IstafelsSunfireGraspSkillBuff : ModBuff
{
	public override string Texture => ModAsset.IstafelsSunfireGraspFireBallCooldown_Mod;

	public override void SetStaticDefaults()
	{
		Main.pvpBuff[Type] = false;
		Main.buffNoSave[Type] = false;
	}
}