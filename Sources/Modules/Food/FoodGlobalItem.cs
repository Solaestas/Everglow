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
                { ItemID.Ale, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<AleBuff>() , BuffTime = 18000 } },
                { ItemID.Apple, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<AppleBuff>() , BuffTime = 180 } },
                { ItemID.AppleJuice, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<AppleJuiceBuff>() , BuffTime = 180} },
                { ItemID.ApplePie, new FoodInfo() { Satiety =15, BuffType = ModContent.BuffType<ApplePieBuff>() , BuffTime = 180 } },
                { ItemID.Apricot, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ApricotBuff>() , BuffTime = 18000} },
                { ItemID.Bacon, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MilkCartonBuff>() , BuffTime = 18000 } },
                { ItemID.Banana, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<BaconBuff>() , BuffTime = 18000 } },
                { ItemID.BananaDaiquiri, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<BananaDaiquiriBuff>() , BuffTime = 18000 } },
                { ItemID.BananaSplit, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<BananaSplitBuff>() , BuffTime = 18000 } },
                { ItemID.BBQRibs, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BBQRibsBuff>() , BuffTime = 18000 } },
                { ItemID.BlackCurrant, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<BlackCurrantBuff>() , BuffTime = 7200 } },
                { ItemID.BloodOrange, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<BloodOrangeBuff>() , BuffTime = 600 } },
                { ItemID.BloodyMoscato, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<BloodyMoscatoBuff>() , BuffTime = 18000 } },
                { ItemID.BowlofSoup, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<BowlofSoupBuff>() , BuffTime = 18000 } },
                { ItemID.BunnyStew, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BunnyStewBuff>() , BuffTime = 18000 } },
                { ItemID.Burger, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<BurgerBuff>() , BuffTime = 18000 } },
                { ItemID.Cherry, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<CherryBuff>() , BuffTime = 18000 } },
                { ItemID.ChickenNugget, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ChickenNuggetBuff>() , BuffTime = 18000 } },
                { ItemID.ChocolateChipCookie, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ChocolateChipCookieBuff>() , BuffTime = 18000 } },
                { ItemID.ChristmasPudding, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<ChristmasPuddingBuff>() } },
                { ItemID.Coconut, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<CoconutBuff>() , BuffTime = 18000 } },
                { ItemID.CoffeeCup, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<CoffeeCupBuff>() , BuffTime = 18000 } },
                { ItemID.CookedFish, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CookedFishBuff>() , BuffTime = 18000 } },
                { ItemID.CookedMarshmallow, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<CookedMarshmallowBuff>(), BuffTime = 18000 } },
                { ItemID.CookedShrimp, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CookedShrimpBuff>() , BuffTime = 18000 } },
                { ItemID.CreamSoda, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<CreamSodaBuff>(), BuffTime = 18000 } },
                { ItemID.Dragonfruit, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<DragonfruitBuff>() , BuffTime = 18000 } },
                { ItemID.Elderberry, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ElderberryBuff>() , BuffTime = 18000 } },
                { ItemID.Escargot, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<EscargotBuff>() , BuffTime = 18000 } },
                { ItemID.FriedEgg, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<FriedEggBuff>() , BuffTime = 18000 } },
                { ItemID.Fries, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<FriesBuff>() , BuffTime = 18000 } },
                { ItemID.FroggleBunwich, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<FroggleBunwichBuff>() , BuffTime = 18000 } },
                { ItemID.FruitJuice, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<FruitJuiceBuff>() , BuffTime = 18000 } },
                { ItemID.FruitSalad, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<FruitSaladBuff>() , BuffTime = 18000 } },
                { ItemID.GingerbreadCookie, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<GingerbreadCookieBuff>() , BuffTime = 18000 } },
                { ItemID.GoldenDelight, new FoodInfo() { Satiety = 30, BuffType = ModContent.BuffType<GoldenDelightBuff>() , BuffTime = 18000 } },
                { ItemID.Grapefruit, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<GrapefruitBuff>() , BuffTime = 18000 } },
                { ItemID.GrapeJuice, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<GrapeJuiceBuff>() , BuffTime = 18000 } },
                { ItemID.Grapes, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<GrapesBuff>() , BuffTime = 18000 } },
                { ItemID.GrilledSquirrel, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GrilledSquirrelBuff>(), BuffTime = 18000 } },
                { ItemID.GrubSoup, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<GrubSoupBuff>() , BuffTime = 18000 } },
                { ItemID.Hotdog, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<HotdogBuff>() , BuffTime = 18000 } },
                { ItemID.IceCream, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<IceCreamBuff>() , BuffTime = 18000 } },
                { ItemID.Lemonade, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<LemonadeBuff>() , BuffTime = 18000 } },
                { ItemID.Lemon, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<LemonBuff>() , BuffTime = 18000 } },
                { ItemID.LobsterTail, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<LobsterTailBuff>() , BuffTime = 18000 } },
                { ItemID.Mango, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<MangoBuff>() , BuffTime = 18000 } },
                { ItemID.Marshmallow, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MarshmallowBuff>() , BuffTime = 18000 } },
                { ItemID.MilkCarton, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<MilkCartonBuff>() , BuffTime = 36000 } },
                { ItemID.Milkshake, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<MilkshakeBuff>() , BuffTime = 18000 } },
                { ItemID.MonsterLasagna, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MonsterLasagnaBuff>() , BuffTime = 18000 } },
                { ItemID.Nachos, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<NachosBuff>() , BuffTime = 18000 } },
                { ItemID.PadThai, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PadThaiBuff>() , BuffTime = 18000 } },
                { ItemID.Peach, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PeachBuff>() , BuffTime = 18000 } },
                { ItemID.PeachSangria, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<PeachSangriaBuff>() , BuffTime = 18000 } },
                { ItemID.Pho, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PhoBuff>() , BuffTime = 18000 } },
                { ItemID.PinaColada, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<PinaColadaBuff>() , BuffTime = 18000 } },
                { ItemID.Pineapple, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PineappleBuff>() , BuffTime = 18000 } },
                { ItemID.Pizza, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PizzaBuff>() , BuffTime = 18000 } },
                { ItemID.Plum, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PlumBuff>() , BuffTime = 18000 } },
                { ItemID.PotatoChips, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PotatoChipsBuff>() , BuffTime = 18000 } },
                { ItemID.PrismaticPunch, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<PrismaticPunchBuff>() , BuffTime = 18000 } },
                { ItemID.PumpkinPie, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<PumpkinPieBuff>() , BuffTime = 18000 } },
                { ItemID.Rambutan, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<RambutanBuff>() , BuffTime = 18000 } },
                { ItemID.RoastedBird, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<RoastedBirdBuff>() , BuffTime = 18000 } },
                { ItemID.RoastedDuck, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<RoastedDuckBuff>() , BuffTime = 18000 } },
                { ItemID.Sake, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<SakeBuff>() , BuffTime = 18000 } },
                { ItemID.Sashimi, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SashimiBuff>() , BuffTime = 18000 } },
                { ItemID.SauteedFrogLegs, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SauteedFrogLegsBuff>() , BuffTime = 18000 } },
                { ItemID.SeafoodDinner, new FoodInfo() { Satiety = 30, BuffType = ModContent.BuffType<SeafoodDinnerBuff>() , BuffTime = 18000 } },
                { ItemID.ShrimpPoBoy, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ShrimpPoBoyBuff>() , BuffTime = 18000 } },
                { ItemID.ShuckedOyster, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<ShuckedOysterBuff>() , BuffTime = 18000 } },
                { ItemID.SmoothieofDarkness, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<SmoothieofDarknessBuff>() , BuffTime = 18000 } },
                { ItemID.Spaghetti, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SpaghettiBuff>() , BuffTime = 18000 } },
                { ItemID.Starfruit, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<StarfruitBuff>() , BuffTime = 18000 } },
                { ItemID.Steak, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SteakBuff>() , BuffTime = 18000 } },
                { ItemID.SugarCookie, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<SugarCookieBuff>() , BuffTime = 18000 } },
                { ItemID.Teacup, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<TeacupBuff>() , BuffTime = 18000 } },
                { ItemID.TropicalSmoothie, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<TropicalSmoothieBuff>() , BuffTime = 18000 } }
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
