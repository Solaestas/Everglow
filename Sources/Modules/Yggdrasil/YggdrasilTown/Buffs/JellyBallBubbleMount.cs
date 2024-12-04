using Everglow.Yggdrasil.YggdrasilTown.Items.Mounts;

namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class JellyBallBubbleMount : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.mount.SetMount(ModContent.MountType<JellyBallBubble>(), player);
		player.buffTime[buffIndex] = 10;
	}
}