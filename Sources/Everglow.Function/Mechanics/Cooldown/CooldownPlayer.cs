using Everglow.Commons.Mechanics.Cooldown.Tests;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Cooldown;

public class CooldownPlayer : ModPlayer
{
	public const string PlayerDataCooldownsKey = "everglowCooldowns";

	/// <summary>
	/// The <see cref="CooldownInstance"/>s of all <see cref="CooldownBase"/>s this player has active.
	/// <br/> Only the cooldowns that are currently active will be stored here.
	/// <br/> <see cref="CooldownExtensions.AddCooldown(Player, string, int, bool)"/>, <see cref="ClearBuff(int)"/>, and <see cref="DelBuff(int)"/> should be used to manipulate player buffs.
	/// </summary>
	public Dictionary<string, CooldownInstance> cooldowns = [];

	public override void OnEnterWorld()
	{
		// Testing code ===================================
		//Player.AddCooldown(TestCooldown.ID, 60 * 50000);
		//Player.AddCooldown(TestCooldown2.ID, 60 * 5000);
		//Player.AddCooldown(TestCooldown3.ID, 60 * 500);
		//Player.AddCooldown(TestCooldown4.ID, 60 * 50);
	}

	public override void PostUpdateMiscEffects()
	{
		var expiredCooldowns = new List<string>();
		foreach (var (key, cdInstance) in cooldowns)
		{
			var cdBase = cdInstance.cooldown;
			if (cdBase.CanTickDown)
			{
				cdInstance.timeLeft--;
			}

			cdBase.Update();
			if (cdInstance.timeLeft <= 0)
			{
				cdBase.OnCompleted();
				expiredCooldowns.Add(key);
			}
		}

		foreach (var key in expiredCooldowns)
		{
			cooldowns.Remove(key);
		}

		// TODO: Sync cooldown removal here
	}

	public void SyncCooldownAddition(bool server, CooldownInstance cdInstance)
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
		{
			return;
		}

		// TODO: Implement server sync logic
	}

	public void SyncCooldownRemoval(bool server, IList<string> cooldownIDs)
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
		{
			return;
		}

		// TODO: Implement server sync logic
	}

	public override void LoadData(TagCompound tag)
	{
		tag.TryGet<IList<TagCompound>>(PlayerDataCooldownsKey, out var list);
		for (int i = 0; i < list.Count; i++)
		{
			var cdData = list[i];
			if (!cdData.TryGet<string>(nameof(CooldownBase.ID), out var id))
			{
				continue;
			}

			var timeLeft = cdData.GetInt(nameof(CooldownInstance.timeLeft));
			var timeMax = cdData.GetInt(nameof(CooldownInstance.timeMax));
			cooldowns.Add(id, new CooldownInstance(Player, id, timeMax, timeLeft));
	}
	}

	public override void SaveData(TagCompound tag)
	{
		var list = new List<TagCompound>();
		for (int i = 0; i < cooldowns.Count; i++)
		{
			var cdInstance = cooldowns.ElementAt(i).Value;
			if (cdInstance.cooldown.CooldownNoSave)
			{
				continue;
			}

			list.Add(new TagCompound
			{
				[nameof(cdInstance.cooldown.ID)] = cdInstance.cooldown.TypeID,
				[nameof(cdInstance.timeLeft)] = cdInstance.timeLeft,
				[nameof(cdInstance.timeMax)] = cdInstance.timeMax,
			});
		}

		tag[PlayerDataCooldownsKey] = list;
	}
}