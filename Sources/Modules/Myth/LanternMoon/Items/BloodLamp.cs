using Everglow.Commons.Mechanics.Events;
using Everglow.Myth.Common;
using Everglow.Myth.LanternMoon.LanternCommon;

namespace Everglow.Myth.LanternMoon.Items;

public class BloodLamp : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 38;
		Item.height = 60;
		Item.rare = ItemRarityID.Green;
		Item.scale = 1;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.useTurn = true;
		Item.useAnimation = 30;
		Item.useTime = 30;
		Item.autoReuse = false;
		Item.consumable = true;
		Item.maxStack = Item.CommonMaxStack;
		Item.value = 10000;
	}

	public override bool? UseItem(Player player)
	{
		EventSystem.Activate<LanternMoonInvasionEvent>();
		return null;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
		   .AddIngredient(ItemID.ChineseLantern, 1)
		   .AddIngredient(ItemID.FlowerofFire, 10)
		   .AddIngredient(ItemID.Ectoplasm, 5)
		   .AddTile(TileID.DemonAltar)
		   .Register();
	}
}