using Everglow.Commons.Mechanics.Cooldown.Tests;
using Everglow.Commons.Netcode.Packets;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Cooldown;

public class CooldownPlayer : ModPlayer
{
	public const string PlayerDataCooldownsKey = "everglowCooldowns";

	/// <summary>
	/// The <see cref="CooldownInstance"/>s of all <see cref="CooldownBase"/>s this player has active.
	/// <br/> Only the cooldowns that are currently active will be stored here.
	/// <br/> <see cref="CooldownExtensions.AddCooldown"/>, <see cref="CooldownExtensions.HasCooldown"/>, and <see cref="CooldownExtensions.ClearCooldown"/> should be used to manipulate player buffs.
	/// </summary>
	public Dictionary<string, CooldownInstance> cooldowns = [];

	public override void OnEnterWorld()
	{
		// Testing code ===================================
		// Player.AddCooldown(TestCooldown.ID, 60 * 50000);
		// Player.AddCooldown(TestCooldown2.ID, 60 * 5000);
		// Player.AddCooldown(TestCooldown3.ID, 60 * 500);
		// Player.AddCooldown(TestCooldown4.ID, 60 * 50);
		foreach (var cd in cooldowns.Values)
		{
			SyncCooldownAddition(Main.dedServ, cd);
		}
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

		if (cooldowns.Count > 0)
		{
			SyncCooldownRemoval(Main.dedServ, expiredCooldowns);
		}
	}

	public override void UpdateDead()
	{
		if (cooldowns.Count > 0)
		{
			var removedCooldowns = new List<string>();
			foreach (var (key, cdInstance) in cooldowns)
			{
				if (cdInstance.cooldown.PersistentCooldown)
				{
					continue;
				}

				removedCooldowns.Add(key);
			}

			if (removedCooldowns.Count > 0)
			{
				foreach (var key in removedCooldowns)
				{
					cooldowns.Remove(key);
				}

				SyncCooldownFullUpdate(Main.dedServ);
			}
		}
	}

	public void SyncCooldownAddition(bool server, CooldownInstance cdInstance)
	{
		ModIns.PacketResolver.Send(new CooldownAdditionPacket(cdInstance), ignoreClient: server ? Player.whoAmI : -1);
	}

	public void SyncCooldownRemoval(bool server, IList<string> cooldownIDs)
	{
		ModIns.PacketResolver.Send(new CooldownRemovalPacket(cooldownIDs), ignoreClient: server ? Player.whoAmI : -1);
	}

	public void SyncCooldownFullUpdate(bool server)
	{
		ModIns.PacketResolver.Send(new CooldownFullUpdatePacket(cooldowns), ignoreClient: server ? Player.whoAmI : -1);
	}

	public override void LoadData(TagCompound tag)
	{
		TagCompound cooldownTag;
		try
		{
			cooldownTag = tag.Get<TagCompound>(PlayerDataCooldownsKey);
		}
		catch (IOException)
		{
			Ins.Logger.Error($"Failed to load cooldowns for player {Player.name} because save type mismatch.");
			cooldownTag = default;
		}

		var tagIterator = cooldownTag.GetEnumerator();
		while (tagIterator.MoveNext())
		{
			var key = tagIterator.Current.Key;
			var instance = CooldownInstance.Load(cooldownTag.GetCompound(key), key, Player);
			if (instance is null)
			{
				continue;
			}

			cooldowns.Add(instance.cooldown.TypeID, instance);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		var list = new TagCompound();
		foreach (var (id, instance) in cooldowns.Where(cd => !cd.Value.cooldown.CooldownNoSave))
		{
			list.Add(id, instance.Save());
		}

		tag[PlayerDataCooldownsKey] = list;
	}
}