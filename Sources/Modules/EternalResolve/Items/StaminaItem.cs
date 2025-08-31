using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.StaminaItem
{
    internal class StaminaItem : ModItem
    {
        public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Accessories;

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
            staminaPlayer.extraStamina += 500f;
            staminaPlayer.mulStaminaRecoveryValue += 100f;
            staminaPlayer.staminaDecreasingSpeed *= 0.8f;
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