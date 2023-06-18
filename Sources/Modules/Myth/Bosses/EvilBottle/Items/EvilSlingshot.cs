using System;
using Everglow.Myth.MiscItems.Weapons.Slingshots;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Myth.Bosses.EvilBottle.Items;
public class EvilSlingshot : SlingshotItem
{
	public override void SetStaticDefaults()
	{
		// base.DisplayName.SetDefault("妖火弹弓");
		// base.Tooltip.SetDefault("");
	}
	public override void SetDef()
	{
		Item.damage = 22;
		Item.channel = true;
		Item.crit = 6;
		Item.width = 38;
		Item.height = 36;
		Item.knockBack = 2f;
		Item.value = Item.sellPrice(0, 0, 10, 0);
		Item.rare = ItemRarityID.Orange;
		ProjType = ModContent.ProjectileType<Projectiles.EvilSlingshot>();
		//Item.shootSpeed = 11.5f;
	}
	public override Vector2? HoldoutOffset()
	{
		return new Vector2?(Vector2.Zero);
	}
	//public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	//{
	//	Vector2 v0 = velocity;
	//	for (int i = 0; i < 5; i++)
	//	{
	//		Vector2 v = v0.RotatedBy((i - 2f) / 20f);
	//		Projectile.NewProjectile(source, player.Center.X, player.Center.Y, v.X, v.Y, type, damage, 0f, Main.myPlayer, 0f, 0f);
	//	}
	//	return false;
	//}
}
