using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Accessories
{
    public class EnergyBand : ModItem
	{
		//Decrease stamina comsumption by 25%
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
			PlayerStamina pPlayer = player.GetModPlayer<PlayerStamina>();
			pPlayer.staminaDecreasingSpeed *= 0.75f;
		}
	}
}