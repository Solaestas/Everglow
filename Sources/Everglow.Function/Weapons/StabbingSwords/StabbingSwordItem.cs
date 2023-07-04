using Terraria.DataStructures;

namespace Everglow.Commons.Weapons.StabbingSwords
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
		public float StabMulDamage = 4f;
		public override bool AltFunctionUse(Player player)
		{
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
				Projectile.NewProjectile(source, position, Vector2.Zero, PowerfulStabProj, (int)(damage * StabMulDamage), knockback, player.whoAmI, 0f, 0f);
				player.itemTime = Item.useTime / 4;
				player.itemAnimation = Item.useAnimation / 4;
				return false;
			}
			return true;
		}
		public override bool CanUseItem(Player player)
		{
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
