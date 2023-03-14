namespace Everglow.Myth.LanternMoon.Buffs;

public class RedImmune : ModBuff
{
	public override void SetStaticDefaults()
	{
		//DisplayName.SetDefault("Red Flame Immunity");
		//Description.SetDefault("Spiritual Fiery Core's red flame cannot hurt you");
		//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "红色火焰免疫");
		//Description.AddTranslation((int)GameCulture.CultureName.Chinese, "花火幻魂心的红色火焰无法伤害到你");
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = false;
	}
}
