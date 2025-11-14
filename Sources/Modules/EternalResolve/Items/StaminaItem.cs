using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items
{
	internal class StaminaItem : ModItem
	{
		public override string LocalizationCategory => Commons.Utilities.LocalizationUtils.Categories.Accessories;

		public bool stamTestItemEquipped = false;

		// public override bool CloneNewInstances => true;
		public override string Texture => "Terraria/Images/UI/Wires_0";

		public override void SetDefaults()
		{
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			PlayerStamina staminaPlayer = Main.LocalPlayer.GetModPlayer<PlayerStamina>();
			staminaPlayer.StaminaRecoveryValue = 100f;
			staminaPlayer.ExtraStamina += 500f;
			staminaPlayer.StaminaRecoveryValueRate += 100f;
			staminaPlayer.StaminaDecreasingSpeed *= 0.8f;
			stamTestItemEquipped = true;
		}

		// public override void AddRecipes()
		// {
		// if (EverglowConfig.DebugMode)
		// {
		// CreateRecipe().Register();
		// }
		// }
	}
}