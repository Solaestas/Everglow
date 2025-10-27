namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class IstafelsSunfireGraspSkillBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.pvpBuff[Type] = false;
		Main.buffNoSave[Type] = false;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		if (Main.rand.NextBool(2))
		{
			Dust dust = Dust.NewDustDirect(player.position - new Vector2(2), player.width + 4, player.height + 4, DustID.LavaMoss, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 1.6f);
			dust.noGravity = true;
			dust.velocity *= 1.8f;
			dust.velocity.Y -= 0.5f;
		}
	}
}