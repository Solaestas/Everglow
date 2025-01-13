namespace Everglow.Commons.Utilities.ItemList.Weapons.Magic
{
	public class Wands : GlobalItem
	{
		public static List<int> vanillaWands;
		public override void Unload()
		{
			vanillaWands = null;
		}

		public Wands()
		{
			vanillaWands = new List<int>
			{
                //火花魔棒
                ItemID.WandofSparking,

                //霹雳法杖
                ItemID.ThunderStaff,

                //紫晶法杖
                ItemID.AmethystStaff,

                //黄玉法杖
                ItemID.TopazStaff,

                //蓝玉法杖
                ItemID.SapphireStaff,

                //翡翠法杖
                ItemID.EmeraldStaff,

                //红玉法杖
                ItemID.RubyStaff,

                //钻石法杖
                ItemID.DiamondStaff,

                //琥珀法杖
                ItemID.AmberStaff,

                //魔刺
                ItemID.Vilethorn,
                
                //血雨法杖
                ItemID.CrimsonRod,

                //天候棒
                ItemID.WeatherPain,

                //魔法导弹
                ItemID.MagicMissile,
                
                //海蓝权杖
                ItemID.AquaScepter,

                //烈焰火鞭
                ItemID.Flamelash,

                //火之花
                ItemID.FlowerofFire,

                //冰雪魔杖
                ItemID.IceRod,

                //寒霜之花
                ItemID.FlowerofFrost,

                //裂天剑
                ItemID.SkyFracture,

                //水晶蛇
                ItemID.CrystalSerpent,

                //魔晶碎块
                ItemID.CrystalVileShard,

                //夺命杖
                ItemID.SoulDrain,
                
                //爬藤怪法杖
                ItemID.ClingerStaff,

                //流星法杖
                ItemID.MeteorStaff,

                //邪恶三叉戟
                ItemID.UnholyTrident,

                //雨云魔杖
                ItemID.NimbusRod,

                //剧毒法杖
                ItemID.PoisonStaff,

                //寒霜法杖
                ItemID.FrostStaff,

                //彩虹魔杖
                ItemID.RainbowRod,

                //毒液法杖
                ItemID.VenomStaff,

                //无限智慧巨著
                ItemID.BookStaff,
                
                //爆裂藤蔓
                ItemID.NettleBurst,

                //暗影束法杖
                ItemID.ShadowbeamStaff,

                //狱火叉
                ItemID.InfernoFork,

                //幽灵法杖
                ItemID.SpectreStaff,

                //共鸣权杖
                ItemID.PrincessWeapon,

                //大地法杖
                ItemID.StaffofEarth,

                //蝙蝠权杖
                ItemID.BatScepter,

                //剃刀松
                ItemID.Razorpine,

                //暴雪法杖
                ItemID.BlizzardStaff,

                //双足翼龙怒气
                ItemID.ApprenticeStaffT3
,
			};
		}
	}
}
