using Everglow.Yggdrasil.Common;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.PermanentBoosters;

public class AntiHeavenSicknessPill : ModItem
{
	public const int AntiHeavenSicknessPillMax = 5;
	public static readonly Dictionary<int, int> LifeBonusTable = new()
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

		Item.maxStack = Item.CommonMaxStack;
		Item.consumable = true;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(platinum: 0, gold: 2);
	}

	public override bool? UseItem(Player player) => player.GetModPlayer<YggdrasilPlayer>().UseAntiHeavenSicknessPill();
}