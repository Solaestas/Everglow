using Everglow.Commons.FeatureFlags;
using Terraria.DataStructures;

namespace Everglow.Commons.Templates.Weapons.StabbingSwords
{
	public abstract class StabbingSwordItem : ModItem
	{
		public override string LocalizationCategory => Utilities.LocalizationUtils.Categories.MeleeWeapons;

		public override void SetDefaults()
		{
			Item.noUseGraphic = false;
			Item.channel = true;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Melee;
			Item.noUseGraphic = true;
			Item.shootSpeed = 16f;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useAnimation = 24;
			Item.useTime = 24;
			CurrentPowerfulStabCD = PowerfulStabCDMax;
			SetCustomDefault();
		}

		/// <summary>
		/// You should modify: <see cref="Item.damage"/>, <see cref="Item.knockBack"/>, <see cref="Item.value"/>, <see cref="Item.rare"/>, <see cref="Item.shoot"/>, <see cref="PowerfulStabProj"/>
		/// </summary>
		public virtual void SetCustomDefault()
		{
		}

		/// <summary>
		/// The projectile type of powerful stab.
		/// </summary>
		public int PowerfulStabProj;

		/// <summary>
		/// The multiply zone of powerful stab, default to 400%(4f).
		/// </summary>
		public float PowerfulStabDamageFlat = 4f;

		/// <summary>
		/// Remaining cooling time.
		/// </summary>
		public int CurrentPowerfulStabCD;

		/// <summary>
		/// Cooling time of powerful stab.
		/// </summary>
		public int PowerfulStabCDMax = 30;

		/// <summary>
		/// Stamina deplete per unit of attack.
		/// </summary>
		public float StaminaCost = 1f;

		/// <summary>
		/// Stamina deplete per powerful stab, default to 45. Notice that this value will multiplied by <see cref="StaminaCost"/>.
		/// </summary>
		public float PowerfulStabStaminaCost = 45f;

		public override void UpdateInventory(Player player)
		{
			if (CurrentPowerfulStabCD > 0)
			{
				CurrentPowerfulStabCD--;
			}
			else
			{
				CurrentPowerfulStabCD = 0;
			}
		}

		public override bool AltFunctionUse(Player player)
		{
			if (CurrentPowerfulStabCD > 0)
			{
				return false;
			}

			if (!player.GetModPlayer<StabbingSwordStaminaPlayer>().CheckStamina(StaminaCost * PowerfulStabStaminaCost))
			{
				return false;
			}

			foreach (Projectile proj in Main.projectile)
			{
				if (proj.owner == player.whoAmI && proj.timeLeft > 1 && proj.type == PowerfulStabProj)
				{
					return false;
				}
			}
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				CurrentPowerfulStabCD = PowerfulStabCDMax;
				Projectile.NewProjectile(source, position, Vector2.Zero, PowerfulStabProj, (int)(damage * PowerfulStabDamageFlat), knockback * 2, player.whoAmI, 0f, 0f);
				player.itemTime = Item.useTime / 4;
				player.itemAnimation = Item.useAnimation / 4;

				if (!EverglowConfig.DebugMode) // TODO: Test to see if velocity changes after stab attack is good. If not, delete the entire statement
				{
					if (player.direction == 1)
					{
						player.velocity.X += 1f;
					}
					else
					{
						player.velocity.X -= 1f;
					}
				}
				return false;
			}
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			// Item.SetDefaults(Item.type); Only Debug
			if (!player.GetModPlayer<StabbingSwordStaminaPlayer>().CheckStamina(StaminaCost, false))
			{
				return false;
			}

			foreach (Projectile proj in Main.projectile)
			{
				if (proj.owner == player.whoAmI && proj.timeLeft > 1 && proj.type == PowerfulStabProj)
				{
					return false;
				}
			}
			return true;
		}
	}
}