namespace Everglow.Myth.LanternMoon.Buffs
{
	public class BrownImmune : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Brown Flame Immunity");
			//Description.SetDefault("Spiritual Fiery Core's Brown flame cannot hurt you");
			//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "棕色火焰免疫");
			//Description.AddTranslation((int)GameCulture.CultureName.Chinese, "花火幻魂心的棕色火焰无法伤害到你");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}
