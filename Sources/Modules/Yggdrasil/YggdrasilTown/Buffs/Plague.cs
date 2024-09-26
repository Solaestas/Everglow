namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class Plague : ModBuff
{
	public const int PlayerLifeDecreasePerSecond = 4;
	public const int NPCLifeDecreasePerSecond = 12;
	public const int SpreadRange = 6;
	public const int SpreadedBuffDuration = 240;
	public const int EnemySpreadLimit = 6;

	public override void SetStaticDefaults()
	{
		Main.pvpBuff[Type] = true;
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = false;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		npc.lifeRegen -= NPCLifeDecreasePerSecond * 2;
		Console.WriteLine(npc.buffTime[buffIndex]);

		if (npc.buffTime[buffIndex] == 1 && !npc.friendly)
		{
			int counter = 0;

			foreach(var target in Main.npc)
			{
				if (target.friendly || target.dontTakeDamage)
				{
					continue;
				}

				if(npc.position.Distance(target.position) > SpreadRange)
				{
					continue;
				}

				target.AddBuff(Type, SpreadedBuffDuration);

				if (++counter > EnemySpreadLimit)
				{
					return;
				}
			}
		}
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.GetModPlayer<PlagueDebuffPlayer>().EnablePlagueDebuff = true;

		if (player.buffTime[buffIndex] == 1)
		{
			foreach (var target in Main.npc)
			{
				if (!target.friendly || target.dontTakeDamage)
				{
					continue;
				}

				if (player.position.Distance(target.position) > SpreadRange)
				{
					continue;
				}

				target.AddBuff(Type, SpreadedBuffDuration);
			}

			foreach (var playerTarget in Main.player)
			{
				if(player.position.Distance(playerTarget.position) > SpreadRange)
				{
					continue;
				}

				playerTarget.AddBuff(Type, SpreadedBuffDuration);
			}
		}
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