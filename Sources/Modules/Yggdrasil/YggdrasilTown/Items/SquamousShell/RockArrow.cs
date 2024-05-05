namespace Everglow.Yggdrasil.YggdrasilTown.Items.SquamousShell
{
	public class RockArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 52;

			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;

			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.knockBack = 2f;
			Item.value = Item.sellPrice(0, 0, 0, 75);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Projectiles.RockArrow>();

			Item.ammo = AmmoID.Arrow; 
		}
	}
}
