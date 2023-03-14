namespace Everglow.Myth.MiscItems.Weapons;

public class AshBone : ModItem
{
	//public override void SetStaticDefaults()
	//{
	//	DisplayName.SetDefault("Stashed Bones");
	//	DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "烬中白骨");
	//	DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Раздробленные Кости");
	//	Tooltip.SetDefault("No summon tag damage\nYour summons will focus struck enemies\nBrusts out several damaging bones and olden enemies struck, weaken their attack and defense");
	//	Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "无召唤标记伤害\n你的召唤物将集中打击被击中的敌人\n命中时爆出数根伤害性的的白骨并使敌人衰老,削弱其攻防");
	//	Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Урон от метки призыва отсутствует\nваш призыв будет фокусироваться на пораженных врагах\nНаносит несколько повреждений костям и поражая старением врагов, ослабляет их атаку и защиту");
	//}
	public override void SetDefaults()
	{
		DefaultToWhip(ModContent.ProjectileType<Projectiles.Weapon.Summon.AshBone>(), 25, 2f, 5.4f, 30);
		Item.rare = 3;
		Item.damage = 25;
		Item.value = Item.sellPrice(0, 3, 0, 0);
	}
	public override bool? UseItem(Player player)
	{
		if (player.autoReuseGlove)
		{
			Item.autoReuse = true;
			return true;
		}
		Item.autoReuse = false;
		return true;
	}
	private void DefaultToWhip(int projectileId, int dmg, float kb, float shootspeed, int animationTotalTime = 30)
	{
		Player player = Main.LocalPlayer;
		Item.autoReuse = false;
		if (player.autoReuseGlove)
			Item.autoReuse = true;
		Item.autoReuse = false;
		Item.useStyle = 1;
		Item.useAnimation = animationTotalTime;
		Item.useTime = animationTotalTime;
		Item.width = 18;
		Item.height = 18;
		Item.shoot = projectileId;
		Item.UseSound = SoundID.Item152;
		Item.noMelee = true;
		Item.DamageType = DamageClass.Summon;
		Item.noUseGraphic = true;
		Item.damage = dmg;
		Item.knockBack = kb;
		Item.shootSpeed = shootspeed;
	}
}
