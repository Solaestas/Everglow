using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheTusk.Items.Weapons;
//TODO:Translate:血骨球\n……等等,这里面好像有触手
public class BloodyBoneYoyo : ModItem
{
	public override void SetStaticDefaults()
	{
		ItemID.Sets.Yoyo[Item.type] = true;
		ItemID.Sets.GamepadExtraRange[Item.type] = 15;
		ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.width = 24;
		Item.height = 24;
		Item.noUseGraphic = true;
		Item.UseSound = SoundID.Item1;
		Item.DamageType = DamageClass.Melee;
		Item.channel = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.BloodyBoneYoyo>();
		Item.useAnimation = 5;
		Item.useTime = 14;
		Item.shootSpeed = 0f;
		Item.knockBack = 0.2f;
		Item.damage = 24;
		Item.noMelee = true;
		Item.value = Item.sellPrice(0, 0, 2, 0);
		Item.rare = ItemRarityID.Green;
		ItemID.Sets.Yoyo[Item.type] = true;
	}
}
