using Everglow.Commons.Mechanics.Events;
using Everglow.Myth.LanternMoon.LanternCommon;

namespace Everglow.Myth.LanternMoon.Items;

public class BloodLamp : ModItem
{
	public override void SetDefaults()
	{
		Item.noUseGraphic = true;
		Item.width = 38;
		Item.height = 60;
		Item.rare = ItemRarityID.Green;
		Item.scale = 1;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.useTurn = true;
		Item.useAnimation = 1;
		Item.useTime = 1;
		Item.autoReuse = false;
		Item.consumable = true;
		Item.maxStack = Item.CommonMaxStack;
		Item.value = 10000;
	}

	public override bool? UseItem(Player player)
	{
		LanternMoonInvasionEvent lMIE = ModContent.GetInstance<LanternMoonInvasionEvent>();
		if (!Main.dayTime && !Main.snowMoon && !Main.pumpkinMoon && lMIE.CanActivate())
		{
			EventSystem.Activate(ModContent.GetInstance<LanternMoonInvasionEvent>());
			lMIE.ProgressAlpha = 0f;
			return true;
		}
		return false;
	}

	public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
	{
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