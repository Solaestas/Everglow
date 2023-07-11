using Everglow.Commons;
using Everglow.Commons.FeatureFlags;
using Everglow.Commons.Weapons.StabbingSwords;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.StaminaItem
{
	internal class StaminaItem : ModItem
	{
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
			staminaPlayer.staminaRecoveryValue = 100f;
			stamTestItemEquipped = true;
		}

		//public override void AddRecipes()
		//{
		//	if (EverglowConfig.DebugMode)
		//	{
		//		CreateRecipe().Register();
		//	}
		//}
	}
}