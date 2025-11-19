using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Accessories
{
	public class EnergyBand : ModItem
	{
		public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Accessories;

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 28;
			Item.value = 1545;
			Item.accessory = true;
			Item.rare = ItemRarityID.White;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// 25% decreased stamina depletion
			player.GetModPlayer<StabbingSwordStaminaPlayer>().StaminaDepletionBonus *= 0.75f;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.GoldBar, 4).AddIngredient(ItemID.Wire).AddTile(TileID.Anvils).Register();
		}
	}
}