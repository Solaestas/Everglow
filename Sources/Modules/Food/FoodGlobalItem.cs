using Everglow.Sources.Modules.Food.Buffs;
using Everglow.Sources.Modules.Food.Items;

namespace Everglow.Sources.Modules.Food
{
    public class FoodGlobalItem : GlobalItem
    {
        // 对于原版的食物进行类型Id到 FoodInfo 的映射，直接获取FoodInfo实例
        private static Dictionary<Item_id, FoodInfo> m_vanillaFoodInfos;

        public FoodGlobalItem()
        {
            m_vanillaFoodInfos = new Dictionary<Item_id, FoodInfo>
            {
                //麦芽酒 饱食度5 给予近战能力提高，防御力降低
                { ItemID.Ale, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<AleBuff>() , BuffTime = 36000 } },
                { ItemID.Apple, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<AppleBuff>() , BuffTime = 180 } },
                { ItemID.AppleJuice, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<AppleJuiceBuff>() , BuffTime = 180} },
                { ItemID.ApplePie, new FoodInfo() { Satiety =15, BuffType = ModContent.BuffType<ApplePieBuff>() , BuffTime = 180 } },
                { ItemID.Apricot, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ApricotBuff>() , BuffTime = 14400} },
                { ItemID.Bacon, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MilkCartonBuff>() , BuffTime = 28800 } },
                { ItemID.Banana, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<BaconBuff>() , BuffTime = 14400 } },
                { ItemID.BananaDaiquiri, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<BananaDaiquiriBuff>() , BuffTime = 900 } },
                { ItemID.BananaSplit, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<BananaSplitBuff>() , BuffTime = 21600 } },
                { ItemID.BBQRibs, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BBQRibsBuff>() , BuffTime = 28800 } },
                { ItemID.BlackCurrant, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<BlackCurrantBuff>() , BuffTime = 14400 } },
                { ItemID.BloodOrange, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<BloodOrangeBuff>() , BuffTime = 14400 } },
                { ItemID.BloodyMoscato, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<BloodyMoscatoBuff>() , BuffTime = 900 } },
                { ItemID.BowlofSoup, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<BowlofSoupBuff>() , BuffTime = 21600 } },
                { ItemID.BunnyStew, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BunnyStewBuff>() , BuffTime = 28800 } },
                { ItemID.Burger, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<BurgerBuff>() , BuffTime = 21600 } },
                { ItemID.Cherry, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<CherryBuff>() , BuffTime = 14400 } },
                { ItemID.ChickenNugget, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ChickenNuggetBuff>() , BuffTime = 14400 } },
                { ItemID.ChocolateChipCookie, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ChocolateChipCookieBuff>() , BuffTime = 900 } },
                { ItemID.ChristmasPudding, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<ChristmasPuddingBuff>(), BuffTime = 21600 } },
                { ItemID.Coconut, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<CoconutBuff>() , BuffTime = 14400 } },
                { ItemID.CoffeeCup, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<CoffeeCupBuff>() , BuffTime = 18000 } },
                { ItemID.CookedFish, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CookedFishBuff>() , BuffTime = 28800 } },
                { ItemID.CookedMarshmallow, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<CookedMarshmallowBuff>(), BuffTime = 18000 } },
                { ItemID.CookedShrimp, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CookedShrimpBuff>() , BuffTime = 28800 } },
                { ItemID.CreamSoda, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<CreamSodaBuff>(), BuffTime = 18000 } },
                { ItemID.Dragonfruit, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<DragonfruitBuff>() , BuffTime = 14400 } },
                { ItemID.Elderberry, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ElderberryBuff>() , BuffTime = 14400 } },
                { ItemID.Escargot, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<EscargotBuff>() , BuffTime = 28800 } },
                { ItemID.FriedEgg, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<FriedEggBuff>() , BuffTime = 14400 } },
                { ItemID.Fries, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<FriesBuff>() , BuffTime = 14400 } },
                { ItemID.FroggleBunwich, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<FroggleBunwichBuff>() , BuffTime = 28800 } },
                { ItemID.FruitJuice, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<FruitJuiceBuff>() , BuffTime = 900 } },
                { ItemID.FruitSalad, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<FruitSaladBuff>() , BuffTime = 14400 } },
                { ItemID.GingerbreadCookie, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<GingerbreadCookieBuff>() , BuffTime = 14400 } },
                { ItemID.GoldenDelight, new FoodInfo() { Satiety = 30, BuffType = ModContent.BuffType<GoldenDelightBuff>() , BuffTime = 36000 } },
                { ItemID.Grapefruit, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<GrapefruitBuff>() , BuffTime = 14400 } },
                { ItemID.GrapeJuice, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<GrapeJuiceBuff>() , BuffTime = 900 } },
                { ItemID.Grapes, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<GrapesBuff>() , BuffTime = 14400 } },
                { ItemID.GrilledSquirrel, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GrilledSquirrelBuff>(), BuffTime = 28800 } },
                { ItemID.GrubSoup, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<GrubSoupBuff>() , BuffTime = 21600 } },
                { ItemID.Hotdog, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<HotdogBuff>() , BuffTime = 28800 } },
                { ItemID.IceCream, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<IceCreamBuff>() , BuffTime = 14400 } },
                { ItemID.Lemonade, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<LemonadeBuff>() , BuffTime = 900 } },
                { ItemID.Lemon, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<LemonBuff>() , BuffTime = 14400 } },
                { ItemID.LobsterTail, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<LobsterTailBuff>() , BuffTime = 28800 } },
                { ItemID.Mango, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<MangoBuff>() , BuffTime = 14400 } },
                { ItemID.Marshmallow, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MarshmallowBuff>() , BuffTime = 28800 } },
                { ItemID.MilkCarton, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<MilkCartonBuff>() , BuffTime = 36000 } },
                { ItemID.Milkshake, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<MilkshakeBuff>() , BuffTime = 18000 } },
                { ItemID.MonsterLasagna, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MonsterLasagnaBuff>() , BuffTime = 28800 } },
                { ItemID.Nachos, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<NachosBuff>() , BuffTime = 28800 } },
                { ItemID.PadThai, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PadThaiBuff>() , BuffTime = 28800 } },
                { ItemID.Peach, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PeachBuff>() , BuffTime = 14400 } },
                { ItemID.PeachSangria, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<PeachSangriaBuff>() , BuffTime = 18000 } },
                { ItemID.Pho, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PhoBuff>() , BuffTime = 28800 } },
                { ItemID.PinaColada, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<PinaColadaBuff>() , BuffTime = 18000 } },
                { ItemID.Pineapple, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PineappleBuff>() , BuffTime = 14400 } },
                { ItemID.Pizza, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PizzaBuff>() , BuffTime = 28800 } },
                { ItemID.Plum, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PlumBuff>() , BuffTime = 14400 } },
                { ItemID.PotatoChips, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PotatoChipsBuff>() , BuffTime = 14400 } },
                { ItemID.PrismaticPunch, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<PrismaticPunchBuff>() , BuffTime = 18000 } },
                { ItemID.PumpkinPie, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<PumpkinPieBuff>() , BuffTime = 21600 } },
                { ItemID.Rambutan, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<RambutanBuff>() , BuffTime = 14400 } },
                { ItemID.RoastedBird, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<RoastedBirdBuff>() , BuffTime = 28800 } },
                { ItemID.RoastedDuck, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<RoastedDuckBuff>() , BuffTime = 28800 } },
                { ItemID.Sake, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<SakeBuff>() , BuffTime = 900 } },
                { ItemID.Sashimi, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SashimiBuff>() , BuffTime = 28800 } },
                { ItemID.SauteedFrogLegs, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SauteedFrogLegsBuff>() , BuffTime = 28800 } },
                { ItemID.SeafoodDinner, new FoodInfo() { Satiety = 30, BuffType = ModContent.BuffType<SeafoodDinnerBuff>() , BuffTime = 36000 } },
                { ItemID.ShrimpPoBoy, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ShrimpPoBoyBuff>() , BuffTime = 28800 } },
                { ItemID.ShuckedOyster, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<ShuckedOysterBuff>() , BuffTime = 21600 } },
                { ItemID.SmoothieofDarkness, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<SmoothieofDarknessBuff>() , BuffTime = 900 } },
                { ItemID.Spaghetti, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SpaghettiBuff>() , BuffTime = 28800 } },
                { ItemID.Starfruit, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<StarfruitBuff>() , BuffTime = 14400 } },
                { ItemID.Steak, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SteakBuff>() , BuffTime = 28800 } },
                { ItemID.SugarCookie, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<SugarCookieBuff>() , BuffTime = 14400 } },
                { ItemID.Teacup, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<TeacupBuff>() , BuffTime = 18000 } },
                { ItemID.TropicalSmoothie, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<TropicalSmoothieBuff>() , BuffTime = 900 } }
            };
        }

        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults(Item item)
        {
            // 如果是原版的食物，那么就手动处理
          if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];

                // 替换掉原版的 buff 类型
                item.buffType = foodInfo.BuffType;
                item.buffTime = foodInfo.BuffTime;
            }


            base.SetDefaults(item);
        }

        public override void OnConsumeItem(Item item, Player player)
        {
            // 如果是原版的食物，那么就手动处理，因为已经使用了物品，说明玩家满足饱食度要求
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];
                var foodPlayer = player.GetModPlayer<FoodModPlayer>();

                // 增加饱食度，并且应用一些特效
                foodPlayer.CurrentSatiety += foodInfo.Satiety;
                Main.NewText($"Added {foodInfo.Satiety}! Current Satiety {foodPlayer.CurrentSatiety} / {foodPlayer.MaximumSatiety}");
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            var foodPlayer = player.GetModPlayer<FoodModPlayer>();
            // 判断能否吃下物品
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];
                if (!foodPlayer.CanEat(foodInfo))
                {
                    Main.NewText($"Cannot eat this!");
                    return false;

                }
            }
            else if (item.ModItem is FoodBase)
            {
                var foodItem = item.ModItem as FoodBase;
                var foodInfo = foodItem.FoodInfo;
                if (!foodPlayer.CanEat(foodInfo))
                {
                    Main.NewText($"Cannot eat this!");
                    return false;
                }
            }
             return base.CanUseItem(item, player); ;
        }

        public override bool ConsumeItem(Item item, Player player)
        {
            var foodPlayer = player.GetModPlayer<FoodModPlayer>();
            // 判断能否吃下物品
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];
                if (!foodPlayer.CanEat(foodInfo))
                {
                    
                    Main.NewText($"Cannot eat this!");
                    return false;

                }
            }
            else if (item.ModItem is FoodBase)
            {
                var foodItem = item.ModItem as FoodBase;
                var foodInfo = foodItem.FoodInfo;
                if (!foodPlayer.CanEat(foodInfo))
                {
                    Main.NewText($"Cannot eat this!");
                    return false;
                }
            }
            return base.ConsumeItem(item, player);
        }
    }
}
