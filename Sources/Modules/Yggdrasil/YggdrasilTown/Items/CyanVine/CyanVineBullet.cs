namespace Everglow.Yggdrasil.YggdrasilTown.Items.CyanVine;

public class CyanVineBullet : ModItem
{
	public override void SetDefaults()
	{
		Item.damage = 10;
		Item.ammo = AmmoID.Bullet;
		Item.consumable = true;
		Item.DamageType = DamageClass.Ranged;
		Item.width = 16;
		Item.height = 16;
		Item.value = 50;
		Item.maxStack = Item.CommonMaxStack;
		Item.shoot = ModContent.ProjectileType<Projectiles.CyanBullet>();
		Item.shootSpeed = 16f;
	}
	public override void AddRecipes()
	{
		CreateRecipe(60)
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 1)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
