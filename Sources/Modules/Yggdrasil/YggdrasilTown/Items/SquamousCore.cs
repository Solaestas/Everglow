using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Everglow.Yggdrasil.YggdrasilTown.Items;

/// <summary>
/// Permanently increase life max by 30.
/// </summary>
public class SquamousCore : ModItem
{
	public static readonly int LifePerCore = 30;

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(30, 1);

	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 4;
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
		// This check prevents this item from being used before vanilla health upgrades are maxed out.
		return true;
	}

	public override bool? UseItem(Player player)
	{
		// Moving the SquamousCoreCount check from CanUseItem to here allows this example fruit to still "be used" like Life Fruit can be
		// when at the max allowed, but it will just play the animation and not affect the player's max life
		if (player.GetModPlayer<SquamousCorePlayer>().SquamousCoreCount >= 1)
		{
			// Returning null will make the item not be consumed
			return null;
		}

		// This method handles permanently increasing the player's max health and displaying the green heal text
		player.UseHealthMaxIncreasingItem(LifePerCore);

		// This field tracks how many of the example fruit have been consumed
		player.GetModPlayer<SquamousCorePlayer>().SquamousCoreCount++;

		return true;
	}
}

public class SquamousCorePlayer : ModPlayer
{
	public int SquamousCoreCount;

	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
	{
		health = StatModifier.Default;
		health.Base = SquamousCore.LifePerCore;
		mana = StatModifier.Default;
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		ModPacket packet = Mod.GetPacket();
		packet.Write(MessageID.PlayerLifeMana);
		packet.Write((byte)Player.whoAmI);
		packet.Write((byte)SquamousCoreCount);
		packet.Send(toWho, fromWho);
	}

	// Called in ExampleMod.Networking.cs
	public void ReceivePlayerSync(BinaryReader reader)
	{
		SquamousCoreCount = reader.ReadByte();
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		SquamousCorePlayer clone = (SquamousCorePlayer)targetCopy;
		clone.SquamousCoreCount = SquamousCoreCount;
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		SquamousCorePlayer clone = (SquamousCorePlayer)clientPlayer;

		if (SquamousCoreCount != clone.SquamousCoreCount)
		{
			SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag["squamousCore"] = SquamousCoreCount;
	}

	public override void LoadData(TagCompound tag)
	{
		SquamousCoreCount = tag.GetInt("squamousCore");
	}
}