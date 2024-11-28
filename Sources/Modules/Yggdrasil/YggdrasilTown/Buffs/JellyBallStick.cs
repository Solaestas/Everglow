namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class JellyBallStick : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.velocity.X *= 0.6f;
		base.Update(player, ref buffIndex);
	}
}