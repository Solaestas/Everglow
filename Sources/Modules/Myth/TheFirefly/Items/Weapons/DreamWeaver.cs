using Everglow.SpellAndSkull.Items;

namespace Everglow.Myth.TheFirefly.Items.Weapons;

public class DreamWeaver : SpellTomeItem
{
	//TODO:Translate:织梦丝雨\n洒出蓝色的荧光雨滴攻击敌人
	public override void SetDefaults()
	{

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
		Item.shoot = ModContent.ProjectileType<Projectiles.DreamWeaver_proj>();
		Item.shootSpeed = 12f;
		DecorativeProjectileTypes.Add(ModContent.ProjectileType<TheFirefly.Projectiles.DreamWeaver.DreamWeaverBook>());
		DecorativeProjectileTypes.Add(ModContent.ProjectileType<TheFirefly.Projectiles.DreamWeaver.DreamWeaverArray>());
	}
}