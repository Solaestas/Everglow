namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class Plague : ModBuff
{
	public const int PlayerLifeDecreasePerSecond = 4;
	public const int NPCLifeDecreasePerSecond = 12;

	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = false;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		npc.lifeRegen -= NPCLifeDecreasePerSecond * 2;

		// TODO: Spreading ability implementation
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.GetModPlayer<PlagueDebuffPlayer>().EnablePlagueDebuff = true;

		// TODO: Spreading ability implementation
	}
}

public class PlagueDebuffPlayer : ModPlayer
{
	public bool EnablePlagueDebuff;

	public override void ResetEffects()
	{
		EnablePlagueDebuff = false;
	}

	public override void UpdateBadLifeRegen()
	{
		if (EnablePlagueDebuff)
		{
			if (Player.lifeRegen > 0)
			{
				Player.lifeRegen = 0;
			}

			Player.lifeRegenTime = 0;
			Player.lifeRegen -= Plague.PlayerLifeDecreasePerSecond * 2;
		}
	}
}