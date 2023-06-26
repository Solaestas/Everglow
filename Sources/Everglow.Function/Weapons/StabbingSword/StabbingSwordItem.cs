using Terraria.DataStructures;

namespace Everglow.Commons.Weapons
{
    public abstract class StabbingSwordItem : ModItem
    {
		/// <summary>
		/// 以下属性仍需手动设置:damage,knockBack,value,rare,shoot,PowerfulStabProj
		/// </summary>
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
		}
		public int PowerfulStabProj;
		public override bool AltFunctionUse(Player player)
		{
			return player.ownedProjectileCounts[PowerfulStabProj] < 1;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectile(source, position, Vector2.Zero, PowerfulStabProj, damage, knockback, player.whoAmI, 0f, 0f);
				return false;
			}
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[PowerfulStabProj] < 1;
		}
	}
}
