using Terraria.ModLoader.IO;

namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class AntiHeavenSicknessPill : ModItem
{
	public const int AntiHeavenSicknessPillMax = 5;
	private static readonly Dictionary<int, int> LifeBonusTable = new()
	{
		{ 1, 20 },
		{ 2, 10 },
		{ 3, 5 },
		{ 4, 3 },
		{ 5, 2 },
	};

	public override void SetDefaults()
	{
		Item.SetNameOverride("Anti-heaven Sickness Pill");

		Item.width = 18;
		Item.height = 18;

		Item.useStyle = ItemUseStyleID.DrinkLiquid;
		Item.useTime = Item.useAnimation = 30;
		Item.UseSound = SoundID.Item4;

		Item.maxStack = 9999;
		Item.consumable = true;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(platinum: 0, gold: 2);
	}

	public override bool CanUseItem(Player player) => player.GetModPlayer<AntiHeavenSicknessPillPlayer>().AntiHeavenSicknessPills < AntiHeavenSicknessPillMax;

	public override bool? UseItem(Player player)
	{
		var usedPills = player.GetModPlayer<AntiHeavenSicknessPillPlayer>().AntiHeavenSicknessPills;
		if (usedPills >= AntiHeavenSicknessPillMax)
		{
			return null;
		}

		player.UseHealthMaxIncreasingItem(LifeBonusTable[usedPills + 1]);
		player.GetModPlayer<AntiHeavenSicknessPillPlayer>().AntiHeavenSicknessPills++;

		return true;
	}

	public static int GetHealthMaxIncreasing(int usedPills)
	{
		var healthMaxIncreasing = 0;
		if (usedPills <= 0)
		{
			return healthMaxIncreasing;
		}
		else if (usedPills > AntiHeavenSicknessPillMax)
		{
			usedPills = AntiHeavenSicknessPillMax;
		}

		for (int i = 1; i <= usedPills; i++)
		{
			healthMaxIncreasing += LifeBonusTable[i];
		}
		return healthMaxIncreasing;
	}
}

public class AntiHeavenSicknessPillPlayer : ModPlayer
{
	private int antiHeavenSicknessPills;

	public int AntiHeavenSicknessPills
	{
		get => antiHeavenSicknessPills;
		set => antiHeavenSicknessPills = value > AntiHeavenSicknessPill.AntiHeavenSicknessPillMax
			? AntiHeavenSicknessPill.AntiHeavenSicknessPillMax
			: value;
	}

	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
	{
		health = StatModifier.Default;
		health.Base = AntiHeavenSicknessPill.GetHealthMaxIncreasing(antiHeavenSicknessPills);
		mana = StatModifier.Default;
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		ModPacket packet = Mod.GetPacket();

		// TODO: Add netcode
		// packet.Write((byte)Everglow.); // Need a 
		// packet.Write((byte)Player.whoAmI);
		// packet.Write((byte)antiHeavenSicknessPills);
		// packet.Send(toWho, fromWho);
	}

	public void ReceivePlayerSync(BinaryReader reader)
	{
		antiHeavenSicknessPills = reader.ReadByte();
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		AntiHeavenSicknessPillPlayer clone = (AntiHeavenSicknessPillPlayer)targetCopy;
		clone.antiHeavenSicknessPills = antiHeavenSicknessPills;
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		AntiHeavenSicknessPillPlayer clone = (AntiHeavenSicknessPillPlayer)clientPlayer;
		if (antiHeavenSicknessPills != clone.antiHeavenSicknessPills)
		{
			SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag[nameof(antiHeavenSicknessPills)] = antiHeavenSicknessPills;
	}

	public override void LoadData(TagCompound tag)
	{
		antiHeavenSicknessPills = tag.GetInt(nameof(antiHeavenSicknessPills));
	}
}