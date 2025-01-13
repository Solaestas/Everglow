namespace Everglow.Commons.Utilities.ItemList.Weapons.Ranged
{
	public class LongGuns : GlobalItem
    {
        public static List<int> vanillaLongGuns;
        public override void Unload()
        {
            vanillaLongGuns = null;
        }

        public LongGuns()
        {
            vanillaLongGuns = new List<int>
            {
                //红莱德枪
                ItemID.RedRyder,

                //火枪
                ItemID.Musket,

                //狙击枪
                ItemID.SniperRifle,
            };
        }
    }
}
