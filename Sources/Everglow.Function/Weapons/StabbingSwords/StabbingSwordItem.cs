using Everglow.Commons.FeatureFlags;
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
			stabCD = stabCDMax;
		}
		public int PowerfulStabProj;
		public float StabMulDamage = 4f;
		public int stabCD;
		public int stabCDMax = 30;
		public float staminaCost = 1f;
		public override void UpdateInventory(Player player)
		{		
			if (stabCD > 0)
				stabCD--;
			else
				stabCD = 0;
		}
		public override bool AltFunctionUse(Player player)
		{
            if (stabCD > 0)
                return false;
			if (!player.GetModPlayer<PlayerStamina>().CheckStamina(staminaCost * 45)) 
                return false;
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
                stabCD = stabCDMax;
				Projectile.NewProjectile(source, position, Vector2.Zero, PowerfulStabProj, (int)(damage * StabMulDamage), knockback*2, player.whoAmI, 0f, 0f);
				player.itemTime = Item.useTime / 4;
				player.itemAnimation = Item.useAnimation / 4;

				if (!EverglowConfig.DebugMode) // TODO: Test to see if velocity changes after stab attack is good. If not, delete the entire statement
				{
					if (player.direction == 1)
						player.velocity.X += 1f;
					else
						player.velocity.X -= 1f;
				}
				return false;
			}
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			Item.SetDefaults(Item.type);//调试用
            if (!player.GetModPlayer<PlayerStamina>().CheckStamina(staminaCost,false))
                return false;
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
