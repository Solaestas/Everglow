using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.PermanentBoosters;

/// <summary>
/// "甘金蜜" Permanently increase life max by 1, at most use for 40 times.
/// </summary>
public class LampBorerHoney : ModItem
{
	public static readonly int MaxExampleLifeHoneys = 10;
	public static readonly int LifePerHoney = 5;

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxExampleLifeHoneys, LifePerHoney);

	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 12;
	}

	public override void SetDefaults()
	{
		Item.CloneDefaults(ItemID.LifeCrystal);
		Item.width = 30;
		Item.height = 26;
		Item.rare = ItemRarityID.Blue;
		Item.value = 15000;
		Item.maxStack = Item.CommonMaxStack;
		Item.useStyle = ItemUseStyleID.EatFood;
	}

	public override bool CanUseItem(Player player)
	{
		return true;
	}

	public override bool? UseItem(Player player)
	{
		if (player.GetModPlayer<LampBorerHoneyPlayer>().LampBorerHoneyCount >= 40)
		{
			return null;
		}

		player.UseHealthMaxIncreasingItem(LifePerHoney);

		player.GetModPlayer<LampBorerHoneyPlayer>().LampBorerHoneyCount++;

		return true;
	}

	public override void Update(ref float gravity, ref float maxFallSpeed)
	{
		Lighting.AddLight(Item.Center, new Vector3(0.6f, 0.4f, 0));
		base.Update(ref gravity, ref maxFallSpeed);
	}
}

public class LampBorerHoneyPlayer : ModPlayer
{
	public int LampBorerHoneyCount;

	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
	{
		health = StatModifier.Default;
		health.Base = LampBorerHoney.LifePerHoney * LampBorerHoneyCount;
		mana = StatModifier.Default;
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		ModPacket packet = Mod.GetPacket();
		packet.Write(MessageID.PlayerLifeMana);
		packet.Write((byte)Player.whoAmI);
		packet.Write((byte)LampBorerHoneyCount);
		packet.Send(toWho, fromWho);
	}

	// Called in ExampleMod.Networking.cs
	public void ReceivePlayerSync(BinaryReader reader)
	{
		LampBorerHoneyCount = reader.ReadByte();
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		var clone = (LampBorerHoneyPlayer)targetCopy;
		clone.LampBorerHoneyCount = LampBorerHoneyCount;
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		var clone = (LampBorerHoneyPlayer)clientPlayer;

		if (LampBorerHoneyCount != clone.LampBorerHoneyCount)
		{
			SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag["LampBorerHoney"] = LampBorerHoneyCount;
	}

	public override void LoadData(TagCompound tag)
	{
		LampBorerHoneyCount = tag.GetInt("LampBorerHoney");
	}
}