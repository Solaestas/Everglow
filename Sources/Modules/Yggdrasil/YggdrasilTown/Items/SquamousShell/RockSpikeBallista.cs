using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.SquamousShell;

public class RockSpikeBallista : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 108;
		Item.height = 38;
		Item.rare = ItemRarityID.White;
		Item.value = 3800;

		Item.useTime = 24;
		Item.useAnimation = 24;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.autoReuse = true;
		Item.UseSound = SoundID.Item5;

		Item.DamageType = DamageClass.Ranged;
		Item.damage = 15;
		Item.knockBack = 8f;
		Item.noMelee = true;

		Item.shoot = ProjectileID.WoodenArrowFriendly;
		Item.shootSpeed = 14f;
		Item.useAmmo = AmmoID.Arrow;
	}
	public override Vector2? HoldoutOffset()
	{
		return new Vector2(-12f, 0f);
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		return true;
	}
}
