namespace Everglow.Myth.Misc.Items.Weapons;

public class GoldRoundYoyo : ModItem
{
	//TODO:Translate:日冕
	//攻击过程逐渐充能,直到形成一个环状日冕,内部的敌人会不时受到灼热的射线攻击
	public override void SetStaticDefaults()
	{
		ItemID.Sets.Yoyo[Item.type] = true;
		ItemID.Sets.GamepadExtraRange[Item.type] = 15;
		ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;

	}
	public override void SetDefaults()
	{
		Item.useStyle = 5;
		Item.width = 24;
		Item.height = 24;
		Item.noUseGraphic = true;
		Item.UseSound = SoundID.Item1;
		Item.DamageType = DamageClass.Melee;
		Item.channel = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.Melee.GoldRoundYoyo>();
		Item.useAnimation = 5;
		Item.useTime = 14;
		Item.shootSpeed = 0f;
		Item.knockBack = 0.2f;
		Item.damage = 136;
		Item.noMelee = true;
		Item.value = Item.sellPrice(0, 5, 0, 0);
		Item.rare = ItemRarityID.Yellow;
		ItemID.Sets.Yoyo[Item.type] = true;
	}
}
