using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Everglow.Yggdrasil.KelpCurtain.Items.PermanentBoosters;

/// <summary>
/// "绿琉璃果" Permanently increase life max by 5, at most use for 10 times.
/// </summary>
public class JadeFruit : ModItem
{
	public static readonly int MaxJadeFruits = 10;
	public static readonly int LifePerJadeFruit = 5;

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxJadeFruits, LifePerJadeFruit);

	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 12;
	}

	public override void SetDefaults()
	{
		Item.CloneDefaults(ItemID.LifeCrystal);
		Item.width = 30;
		Item.height = 34;
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
		if (player.GetModPlayer<JadeFruitPlayer>().JadeFruitCount >= 10)
		{
			return null;
		}

		player.UseHealthMaxIncreasingItem(LifePerJadeFruit);

		player.GetModPlayer<JadeFruitPlayer>().JadeFruitCount++;

		return true;
	}

	public override void Update(ref float gravity, ref float maxFallSpeed)
	{
		base.Update(ref gravity, ref maxFallSpeed);
	}
}

public class JadeFruitPlayer : ModPlayer
{
	public int JadeFruitCount;

	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
	{
		health = StatModifier.Default;
		health.Base = JadeFruit.LifePerJadeFruit * JadeFruitCount;
		mana = StatModifier.Default;
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		ModPacket packet = Mod.GetPacket();
		packet.Write(MessageID.PlayerLifeMana);
		packet.Write((byte)Player.whoAmI);
		packet.Write((byte)JadeFruitCount);
		packet.Send(toWho, fromWho);
	}

	// Called in ExampleMod.Networking.cs
	public void ReceivePlayerSync(BinaryReader reader)
	{
		JadeFruitCount = reader.ReadByte();
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		var clone = (JadeFruitPlayer)targetCopy;
		clone.JadeFruitCount = JadeFruitCount;
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		var clone = (JadeFruitPlayer)clientPlayer;

		if (JadeFruitCount != clone.JadeFruitCount)
		{
			SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag["JadeFruit"] = JadeFruitCount;
	}

	public override void LoadData(TagCompound tag)
	{
		JadeFruitCount = tag.GetInt("JadeFruit");
	}
}