namespace Everglow.Food.ItemList.Weapons.Ranged
{
	public class MachineGuns : GlobalItem
    {
        public static List<int> vanillaMachineGuns;
        public override void Unload()
        {
            vanillaMachineGuns = null;
        }

        public MachineGuns()
        {
            vanillaMachineGuns = new List<int>
            {
                //迷你鲨
                ItemID.Minishark,

                //链式发条步枪
                ItemID.ClockworkAssaultRifle,

                //鳄式机枪
                ItemID.Gatligator,

                //乌兹冲锋枪
                ItemID.Uzi,

                //巨兽鲨
                ItemID.Megashark,

                //链式机枪
                ItemID.ChainGun,

                //星璇机枪
                ItemID.VortexBeater,

                //太空海豚机枪
                ItemID.SDMG,
            };
        }
    }
}
