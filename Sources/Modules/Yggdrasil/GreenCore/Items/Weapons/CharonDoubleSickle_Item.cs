namespace Everglow.Yggdrasil.GreenCore.Items.Weapons;

public class CharonDoubleSickle_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.damage = 100;

		Item.width = 112;
		Item.height = 106;
		Item.useTime = 3;
		Item.useAnimation = 15;
		Item.noUseGraphic = true;
		Item.noMelee = true;
		Item.useStyle = 1;
		Item.autoReuse = true;
		Item.channel = true;
		Item.useLimitPerAnimation = 2;
		Item.shoot = ModContent.ProjectileType<CharonDoubleSickle>();
		Item.DamageType = DamageClass.MeleeNoSpeed;
	}

	public override bool CanUseItem(Player player)
	{
		return player.ownedProjectileCounts[Item.shoot] < 2;
	}
}