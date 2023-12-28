using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class AmberMagicOrb : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 40;
		Item.height = 52;
		Item.useAnimation = 17;
		Item.useTime = 17;
		Item.knockBack = 2f;
		Item.damage = 15;
		Item.rare = ItemRarityID.Blue;
		Item.UseSound = SoundID.Item1;
		Item.value = 4514;
		Item.autoReuse = false;
		Item.DamageType = DamageClass.Magic;
		//Item.channel = true;
		Item.mana = 9;
		Item.noMelee = true;
		//Item.noUseGraphic = true;


		Item.shoot = ModContent.ProjectileType<AmberBall>();
		Item.shootSpeed = 12;
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		return base.Shoot(player, source, position, velocity, type, damage, knockback);
	}
}
