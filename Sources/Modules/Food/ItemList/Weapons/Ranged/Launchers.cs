namespace Everglow.Food.ItemList.Weapons.Ranged
{
	public class Launchers : GlobalItem
    {
        public static List<int> vanillaLaunchers;
        public override void Unload()
        {
            vanillaLaunchers = null;
        }

        public Launchers()
        {
            vanillaLaunchers = new List<int>
            {
                //榴弹发射器
                ItemID.GrenadeLauncher,

                //感应雷发射器
                ItemID.ProximityMineLauncher,

                //火箭发射器
                ItemID.RocketLauncher,

                //雪人炮
                ItemID.SnowmanCannon,
                
                //喜庆弹射器
                ItemID.FireworksLauncher,

                //电圈发射器
                ItemID.ElectrosphereLauncher,

                //喜庆弹射器Mk2
                ItemID.Celeb2,
            };
        }
    }
}
