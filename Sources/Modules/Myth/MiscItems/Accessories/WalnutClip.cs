using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
	[AutoloadEquip(EquipType.Neck)]
	public class WalnutClip : ModItem
	{
		public override void SetStaticDefaults()
		{
			//TODO:低于半血后伤害增加(半血血量-当前血量)%→低于半血后伤害增加(半血血量-当前血量) * 0.25%
			//DisplayName.SetDefault("Nutcracker");
			//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "胡桃夹子");
			//Tooltip.SetDefault("Increases crit damage by 16%\nWhen your Hp is below 50% of the max Hp, increases damage by (50% of max Hp - current Hp) * 0.25%\n'Besides cracking nuts and crabs, it can crack enemies, too'");
			//Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "暴击伤害增加16%\n低于半血后伤害增加(半血血量-当前血量) * 0.25%\n'除了夹碎胡桃和螃蟹,它还能夹碎敌人'");
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = 21092;
			Item.accessory = true;
			Item.rare = ItemRarityID.Yellow;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MythContentPlayer mplayer = player.GetModPlayer<MythContentPlayer>();
			mplayer.CriticalDamage += 0.16f;
			if (player.statLifeMax2 / 2f > player.statLife)
			{
				player.GetDamage(DamageClass.Generic) *= (player.statLifeMax2 / 2f - player.statLife) / 400f + 1;
			}
		}
	}
}
