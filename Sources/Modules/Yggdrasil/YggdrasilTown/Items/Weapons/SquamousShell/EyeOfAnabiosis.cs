namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;

public class EyeOfAnabiosis : ModItem
{
	private int ProjIndex { get; set; } = 0;

	private Projectile Proj => Main.projectile[ProjIndex];

	public override void SetDefaults()
	{
		Item.width = 72;
		Item.height = 66;

		Item.DamageType = DamageClass.Magic;
		Item.damage = 27;
		Item.knockBack = 3f;
		Item.crit = 14;
		Item.mana = 8;

		Item.useTime = 22;
		Item.useAnimation = 22;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;

		Item.value = Item.sellPrice(gold: 1, silver: 3);
		Item.rare = ItemRarityID.Green;
		Item.autoReuse = true;

		Item.shoot = ModContent.ProjectileType<Projectiles.EyeOfAnabiosis>();
		Item.shootSpeed = 7;
	}

	public override void UpdateInventory(Player player)
	{
		bool generated = false;
		if (player.HeldItem == Item && !generated)
		{
			ProjIndex = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Bottom, player.velocity, Item.shoot, 0, 0);
			Console.WriteLine("Created");
		}
		else if (player.HeldItem != Item && generated)
		{
			Proj.Kill();
			Console.WriteLine("Killed");
		}
	}
}