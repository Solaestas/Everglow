using Terraria.DataStructures;

namespace Everglow.Yggdrasil.GreenCore.Items.Weapons
{
	public class CharonDoubleSickle_Item : ModItem
	{
		private Item item
		{
			get => Item;
		}

		public override void SetStaticDefaults()
		{

			// DisplayName.SetDefault("");
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.damage = 100;

			item.width = 112;
			item.height = 106;
			item.useTime = 3;
			item.useAnimation = 15;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.useStyle = 1;
			item.autoReuse = true;
			item.channel = true;
			item.useLimitPerAnimation = 2;
			item.shoot = ModContent.ProjectileType<CharonDoubleSickle>();
			item.DamageType = DamageClass.MeleeNoSpeed;
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 2;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return true;
		}
	}
}