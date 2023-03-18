namespace Everglow.Food.ItemList.Weapons.Ranged
{
	public class Handguns : GlobalItem
    {
        public static List<int> vanillaHandguns;
        public override void Unload()
        {
            vanillaHandguns = null;
        }

        public Handguns()
        {
            vanillaHandguns = new List<int>
            {
                //燧发枪
                ItemID.FlintlockPistol,

                //夺命枪
                ItemID.TheUndertaker,

                //左轮手枪
                ItemID.Revolver,

                //手枪
                ItemID.Handgun,

                //凤凰爆破枪
                ItemID.PhoenixBlaster,

                //气喇叭
                ItemID.PewMaticHorn,

                //维纳斯万能枪
                ItemID.VenusMagnum,
            };
        }
    }
}
