namespace Everglow.Food.ItemList.Weapons.Ranged
{
    public class Bows : GlobalItem
    {
        public static List<int> vanillaBows;
        public override void Unload()
        {
            vanillaBows = null;
        }

        public Bows()
        {
            vanillaBows = new List<int>
            {
                //木弓
                ItemID.WoodenBow,

                //针叶木弓
                ItemID.BorealWoodBow,

                //棕榈木弓
                ItemID.PalmWoodBow,

                //红木弓
                ItemID.RichMahoganyBow,

                //乌木弓
                ItemID.EbonwoodBow,
                
                //暗影木弓
                ItemID.ShadewoodBow,
                
                 //珍珠木弓
                ItemID.PearlwoodBow,

                //铜弓
                ItemID.CopperBow,

                //锡弓
                ItemID.TinBow,

                //铁弓
                ItemID.IronBow,

                //铅弓
                ItemID.LeadBow,

                //银弓
                ItemID.SilverBow,

                //钨弓
                ItemID.TungstenBow,

                //金弓
                ItemID.GoldBow,

                //铂金弓
                ItemID.PlatinumBow,

                //恶魔弓
                ItemID.DemonBow,

                //肌腱弓
                ItemID.TendonBow,

                //血雨弓
                ItemID.BloodRainBow,

                //熔火之弓
                ItemID.MoltenFury,

                //蜂膝弓
                ItemID.BeesKnees,

                //地狱之翼弓
                ItemID.HellwingBow,

                //骨弓
                ItemID.Marrow,

                //冰霜弓
                ItemID.IceBow,

                //代达罗斯风暴弓
                ItemID.DaedalusStormbow,

                //暗影焰弓
                ItemID.ShadowFlameBow,

                //幽灵凤凰
                ItemID.DD2PhoenixBow,//😅

                //脉冲弓
                ItemID.PulseBow,

                //空中祸害
                ItemID.DD2BetsyBow,//😅

                 //海啸
                ItemID.Tsunami,

                //日暮
                ItemID.FairyQueenRangedItem,//😅😅

                //幻象
                ItemID.Phantasm,

            };
        }
    }
}
