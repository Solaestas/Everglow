using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories.SquamousShell;

[AutoloadEquip(EquipType.Wings)]
public class BlueyWings : ModItem
{
	public override void SetStaticDefaults()
	{
		// Fly time: 120 ticks = 2 seconds
		// Fly speed: 6
		// Acceleration multiplier: 1.5
		ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(120, 6f, 1.5f);
	}

	public override void SetDefaults()
	{
		Item.width = 22;
		Item.height = 20;
		Item.value = Item.sellPrice(gold: 1, silver: 33);
		Item.rare = ItemRarityID.Green;
		Item.accessory = true;
	}

	public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
		ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
	{
		ascentWhenFalling = 0.85f; // Falling glide speed
		ascentWhenRising = 0.15f; // Rising speed
		maxCanAscendMultiplier = 1f;
		maxAscentMultiplier = 3f;
		constantAscend = 0.135f;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.Register();
	}
}