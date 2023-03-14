namespace Everglow.Sources.Modules.MythModule.LanternMoon.Buffs
{
	public class BlueImmune : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Blue Flame Immunity");
			//Description.SetDefault("Spiritual Fiery Core's blue flame cannot hurt you");
			//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "蓝色火焰免疫");
			//Description.AddTranslation((int)GameCulture.CultureName.Chinese, "花火幻魂心的蓝色火焰无法伤害到你");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}
