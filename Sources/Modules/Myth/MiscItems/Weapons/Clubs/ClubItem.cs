using Terraria.DataStructures;
namespace Everglow.Myth.MiscItems.Weapons.Clubs
{
	/// <summary>
	/// Default damage = 5 width*height = 48*48 useT = useA = 4 useStyle = ItemUseStyleID.Shoot rare = ItemRarityID.White value = 50 
	/// noMelee = true noUseGraphic = true autoReuse = true
	/// DamageType = DamageClass.Melee
	/// knockBack = 4f shootSpeed = 1f
	/// </summary>
	public abstract class ClubItem : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 4;
			Item.value = 50;
			Item.useTime = 4;
			Item.useAnimation = 4;

			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.shootSpeed = 1f;
			Item.knockBack = 16f;

			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.rare = ItemRarityID.White;

			SetDef();
			Item.shoot = ProjType;
		}
		public virtual void SetDef()
		{

		}
		internal int ProjType;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.ownedProjectileCounts[type] < 1)
				Projectile.NewProjectile(source, position + velocity * 2f, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
