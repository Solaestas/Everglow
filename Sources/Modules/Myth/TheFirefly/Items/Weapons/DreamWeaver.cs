using Everglow.Myth;

namespace Everglow.Myth.TheFirefly.Items.Weapons
{
	public class DreamWeaver : ModItem//TODO:织梦丝雨
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
			ItemGlowManager.AutoLoadItemGlow(this);
		}

		public override void SetDefaults()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.damage = 13;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 4;
			Item.width = 34;
			Item.height = 46;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item42;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.DreamWeaver>();
			Item.shootSpeed = 12f;
		}
	}
}