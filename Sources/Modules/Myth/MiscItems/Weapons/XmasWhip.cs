using Everglow.Myth;

namespace Everglow.Myth.MiscItems.Weapons
{
	public class XmasWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			/*DisplayName.SetDefault("Christmas Tree Whip");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "圣诞树鞭");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Хлыст Рождественской елки");
			Tooltip.SetDefault("No summon tag damage\nYour summons will focus struck enemies\nBrusts out damaging Christmas lights on hitting enemies\n'Much better than the Christmas Tree Sword'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "无标记伤害\n你的召唤物将集中打击被击中的敌人\n击中敌人时爆出伤害性的彩灯\n'比圣诞树剑好太多'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Нет урона от метки призыва \nВаше призываемое существо будет фокусироваться на пораженных врагах \nНаносит урон рождественским огням при попадании во врагов \n 'Намного лучше, чем меч Рождественской елки'");*/
			ItemGlowManager.AutoLoadItemGlow(this);
		}
		public static short GetGlowMask = 0;
		public override void SetDefaults()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			DefaultToWhip(ModContent.ProjectileType<Projectiles.Weapon.Summon.XmasWhip>(), 348, 2f, 5.4f, 30);
			Item.rare = 11;
			Item.damage = 308;
			Item.value = Item.sellPrice(0, 10, 0, 0);
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
}
