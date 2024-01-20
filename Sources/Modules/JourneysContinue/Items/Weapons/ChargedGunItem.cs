// Namespace for the charged gun item base class
using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.JourneysContinue.Items.Weapons
{
	// ChargedGunItem class extends ModItem from Terraria.ModLoader
	public abstract class ChargedGunItem : ModItem
	{
		// VARIABLES
		/// <summary>
		/// Indicates whether the weapon is charged or not. True for charged false otherwise
		/// </summary>
		public bool charged;

		/// <summary>
		/// The ID of the item used for charging the weapon.
		/// </summary>
		public int chargeItemID;

		/// <summary>
		/// The maximum energy capacity of the weapon.
		/// </summary>
		public int maxEnergy;

		/// <summary>
		/// The current amount of energy left in the weapon.
		/// </summary>
		public int energyLeft;

		/// <summary>
		/// The energy cost per shot while the gun is charged.
		/// </summary>
		public int energyPerShot;

		/// <summary>
		/// The amount of energy decrement per 10 frames (1/6 second) while the weapon is not being used.
		/// </summary>
		public int passiveEnergyDecreaseRate;
		public override void SetDefaults()
		{
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Shoot;
		}

		/// <summary>
		/// Abstract method for stationary behavior of the gun, to be implemented by derived classes.
		/// <br/>
		/// whats happening to the gun when it is NOT charged, e.g, A good way of thinking about this is that it will bring the weapon back to whatever being descirbed in SetDefaults()
		/// <br/>
		/// but it also take place when energyLeft runs out.
		/// </summary>
		public abstract void StationaryBehavior(Player player);

		/// <summary>
		/// Abstract method for charged behavior of the gun, to be implemented by derived classes.
		/// <br/>
		/// E.g., whats happening to the gun when it is fully charged, remember to set the value of energyLeft to desired value (or simply maxEnergy)
		/// </summary>
		public abstract void ChargedBehavior(Player player);


		/// <summary>
		/// Method to handle charging the gun, to be implemented by derived classes.
		/// <br/>
		/// For example, play a charging animation, consume a specific item, etc. use the follwing function to consume the chargeitem:
		/// <code>
		/// consumeItemHandler(Player player)
		/// </code>
		/// <br/>
		/// make sure to set charged = true at the end of the function
		/// </summary>
		/// <returns>
		/// return true for successful charging, false otherwise
		/// </returns>
		public abstract void ChargeGun(Player player);

		/// <summary>
		/// Method to hadle the item consumption, this function will consume the item as defined in the chargeItem variable in the derived class
		/// </summary>
		/// <returns>
		/// return true for successful cosuming the item, false if the item is not in the inventory
		/// </returns>
		public bool ConsumeItemHandler(Player player)
		{
			// Check if the player has the required item
			if (player.HasItem(chargeItemID))
			{
				// Consume the item
				player.ConsumeItem(chargeItemID);

				return true;
			}
			else
			{
				return false;
			}
		}

		//this is a counter to keep track of the FPS, it is used in PassiveEnergyDecrement() for "decrement the energyLeft variable once per 10 frames while the weapon is charged"
		int frameCounter = 0;

		/// <summary>
		/// Method to handle the passive energy decrement, this function will decrement the energyLeft variable once per 10 frames while the weapon is charged
		/// <br/>
		/// the amount of decrement is defined by the passiveEnergyDecreaseRate
		/// </summary>
		/// <param name="passiveEnergyDecreaseRate"> 
		/// control the rate of decreasing of the energy, default value is 1
		/// </param>
		public void PassiveEnergyDecrement(int passiveEnergyDecreaseRate = 1)
		{
			if (charged == true)
			{
				frameCounter++;
				if (frameCounter % 10 == 0)
				{
					energyLeft = Math.Max(0, energyLeft - passiveEnergyDecreaseRate); // Ensure energy doesn't go below 0
				}

			}
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Vector2 PosOffset = new Vector2(30, 15);
			Player player = Main.LocalPlayer;

			if (player.direction == 1)
			{
				PosOffset = new Vector2(310, 310);
			}
			else if (player.direction == -1)
			{
				PosOffset = new Vector2(398, 310);
			}

			float value = energyLeft / (float)maxEnergy;
			Color color = Main.hslToRgb(value * 0.36f - 0.15f, 1f, 0.75f);

			if (player.HeldItem == Item)
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.MagicPixel.Value;
				//draw the energy bar
				spriteBatch.Draw(tex, position + PosOffset, new Rectangle(0, 0, 2, 2), color, 0, new Vector2(1f), new Vector2(1, 20 * value), 0, 0);
				//Main.NewText("energyLeft: " + energyLeft, Color.Gold);
			}
		}

	}


}