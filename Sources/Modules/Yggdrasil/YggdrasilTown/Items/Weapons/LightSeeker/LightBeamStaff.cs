using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.LightSeeker;

internal class LightBeamStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 34;
		Item.height = 40;

		Item.damage = 12;
		Item.DamageType = DamageClass.Magic;
		Item.crit = 4;
		Item.knockBack = 3.5f;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = 36;
		Item.useAnimation = 36;
		Item.UseSound = SoundID.Item12;
		Item.autoReuse = true;

		Item.mana = 5;

		Item.SetShopValues(
			ItemRarityColor.Green2,
			Item.buyPrice(silver: 8));

		Item.shoot = ModContent.ProjectileType<LightBeam>();
		Item.shootSpeed = 15f;
	}

	public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
	{
		rotation += MathF.PI / 2;

		return true;
	}
}