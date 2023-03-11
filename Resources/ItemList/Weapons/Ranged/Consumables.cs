using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Humanizer.In;

namespace Everglow.Resources.ItemList.Weapons.Ranged
{
    public class Consumables : GlobalItem
    {
        public static List<int> vanillaConsumables;
        public override void Unload()
        {
            vanillaConsumables = null;
        }

        public Consumables()
        {
            vanillaConsumables = new List<int>
            {
                //纸飞机
                ItemID.PaperAirplaneA,

                //白纸飞机
                ItemID.PaperAirplaneB,

                //手里剑
                ItemID.Shuriken,

                //投刀
                ItemID.ThrowingKnife,

                //毒刀
                ItemID.PoisonedKnife,
                
                //雪球
                ItemID.Snowball,
                
                 //尖球
                ItemID.SpikyBall,

                //骨头
                ItemID.Bone,

                //臭蛋
                ItemID.RottenEgg,

                //星形茴香
                ItemID.StarAnise,

                //莫洛托夫鸡尾酒
                ItemID.MolotovCocktail,

                //寒霜飞鱼
                ItemID.FrostDaggerfish,

                //标枪
                ItemID.Javelin,

                //骨头标枪
                ItemID.BoneJavelin,

                //骨投刀
                ItemID.BoneDagger,

                //手榴弹
                ItemID.Grenade,

                //粘性手榴弹
                ItemID.StickyGrenade,

                //弹力手榴弹
                ItemID.BouncyGrenade,

                //蜜蜂手榴弹
                ItemID.Beenade,

                //快乐手榴弹
                ItemID.PartyGirlGrenade,

            };
        }
    }
}
