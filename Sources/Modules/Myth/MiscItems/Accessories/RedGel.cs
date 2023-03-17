using Everglow.Myth.Common;

namespace Everglow.Myth.MiscItems.Accessories;

[AutoloadEquip(EquipType.Neck)]
public class RedGel : ModItem
{
	public override void SetStaticDefaults()
	{
		//DisplayName.SetDefault("Coagulated Red Gel");
		//		//Tooltip.SetDefault("Increases crit chance by 6%\nInceases crit damage by 9%\n'Some slimes were also affected by the power of the Blood of Gods, though they didn't have any blood'");
		//Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "暴击率增加6%\n暴击伤害增加9%\n'有的史莱姆也被神之血的力量所影响了,即使它们没有血液'");
	}
	public override void SetDefaults()
	{
		Item.width = 34;
		Item.height = 34;
		Item.value = 1824;
		Item.accessory = true;
		Item.rare = ItemRarityID.Orange;
	}
	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.GetCritChance(DamageClass.Generic) += 6;
		MythContentPlayer mplayer = player.GetModPlayer<MythContentPlayer>();
		mplayer.CriticalDamage += 0.09f;
	}
}
