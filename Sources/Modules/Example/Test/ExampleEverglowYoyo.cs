using Everglow.Commons.Templates.Weapons.Yoyos;

namespace Everglow.Example.Test;

public class ExampleEverglowYoyo : YoyoItem
{
	public override void SetCustomDefaults()
	{
		Item.damage = 114514;
		Item.value = 114514;
		Item.rare = ItemRarityID.White;
		Item.shoot = ModContent.ProjectileType<ExampleEverglowYoyo_Projectile>();
	}
}