using Everglow.Sources.Modules.Food.Buffs;
using Everglow.Sources.Modules.Food.DataStructures;
using Everglow.Sources.Modules.Food.Items;

namespace Everglow.Sources.Modules.Food
{
    public class FoodGlobalItem : GlobalItem
    {
        // 对于原版的食物进行类型Id到 FoodInfo 的映射，直接获取FoodInfo实例
        private static Dictionary<int, FoodInfo> m_vanillaFoodInfos;

        public FoodGlobalItem()
        {
            m_vanillaFoodInfos = new Dictionary<int, FoodInfo>
            {
                //苹果 饱食度10 加8%减伤
                {
                    ItemID.Apple,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<AppleBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //苹果派 加10%减伤,1生命回复
                {
                    ItemID.ApplePie,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<ApplePieBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //杏 饱食度10 魔力再生加4
                {
                    ItemID.Apricot,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ApricotBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //培根 饱食度10 加2生命回复，所受冷系伤害降低
                {
                    ItemID.Bacon,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<MilkCartonBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //香蕉 饱食度10 20%不消耗弹药，加5%远程伤害
                {
                    ItemID.Banana,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<BaconBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //香蕉船 饱食度15 33%不消耗弹药，加8%暴击
                {
                    ItemID.BananaSplit,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<BananaSplitBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //烧烤肋排 饱食度20 加50血量上限
                {
                    ItemID.BBQRibs,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<BBQRibsBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //黑醋栗  饱食度10 获得夜视、危险感知能力
                {
                    ItemID.BlackCurrant,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<BlackCurrantBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //血橙 饱食度10 加25血量上限
                {
                    ItemID.BloodOrange,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<BloodOrangeBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //鱼菇汤 饱食度15 加20魔力上限,5%魔法伤害
                {
                    ItemID.BowlofSoup,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<BowlofSoupBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //炖兔兔 饱食度20 自动跳跃，增加跳跃能力
                {
                    ItemID.BunnyStew,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<BunnyStewBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //汉堡 饱食度15 减少移速，增加防御
                {
                    ItemID.Burger,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<BurgerBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //樱桃 饱食度10 增加移速与跳跃高度
                {
                    ItemID.Cherry,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<CherryBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //鸡块 饱食度10 增加1生命回复、4%攻速
                {
                    ItemID.ChickenNugget,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ChickenNuggetBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //巧克力曲奇饼干 饱食度10 短时间内快速恢复生命与魔力
                {
                    ItemID.ChocolateChipCookie,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ChocolateChipCookieBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //圣诞布丁 饱食度10 仇恨值减600
                {
                    ItemID.ChristmasPudding,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ChristmasPuddingBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //椰子 饱食度10 加4防御，5%减伤
                {
                    ItemID.Coconut,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<CoconutBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //熟鱼 饱食度20 加40魔力上限,8%魔法暴击率
                {
                    ItemID.CookedFish,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<CookedFishBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //熟棉花糖 饱食度5 减40%最大掉落速度，增加额外摔伤距离
                {
                    ItemID.CookedMarshmallow,
                    new FoodInfo() {
                        Satiety = 5,
                        BuffType = ModContent.BuffType<CookedMarshmallowBuff>(),
                        BuffTime = new FoodDuration(5, 0, 0)
                    }
                },
                //熟虾 饱食度20 加10防御,4穿甲
                {
                    ItemID.CookedShrimp,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<CookedShrimpBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //火龙果 饱食度10 攻击造成着火
                {
                    ItemID.Dragonfruit,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<DragonfruitBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //接骨木果 饱食度10 你可以短距离冲刺
                {
                    ItemID.Elderberry,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ElderberryBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //食用蜗牛 饱食度10 大大减速，加60%减伤
                {
                    ItemID.Escargot,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<EscargotBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //煎蛋 饱食度10 加8%伤害
                {
                    ItemID.FriedEgg,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<FriedEggBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //薯条 饱食度10 加4防御，4%暴击
                {
                    ItemID.Fries,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<FriesBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //蛙腿三明治 饱食度20 饱食度20 自动跳跃，增加伤害和跳跃能力
                {
                    ItemID.FroggleBunwich,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<FroggleBunwichBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //水果色拉 饱食度10 中幅提升大部分属性
                {
                    ItemID.FruitSalad,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<FruitSaladBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //姜饼 饱食度10 加1生命回复,保暖
                {
                    ItemID.GingerbreadCookie,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<GingerbreadCookieBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //金美味 饱食度30 攻击造迈达斯，中幅提升大部分属性
                {
                    ItemID.GoldenDelight,
                    new FoodInfo() {
                        Satiety = 30,
                        BuffType = ModContent.BuffType<GoldenDelightBuff>(),
                        BuffTime = new FoodDuration(10, 0, 0)
                    }
                },
                //葡萄柚 饱食度10 加10%召唤物击退
                {
                    ItemID.Grapefruit,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<GrapefruitBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //葡萄 饱食度10 加1召唤栏，幸运值加10%，减8防御
                {
                    ItemID.Grapes,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<GrapesBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //烤松鼠 饱食度20 增加跳跃能力
                {
                    ItemID.GrilledSquirrel,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<GrilledSquirrelBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //蛆虫汤 饱食度15 加25渔力，每秒减2血
                {
                    ItemID.GrubSoup,
                    new FoodInfo() {
                        Satiety = 15, BuffType = ModContent.BuffType<GrubSoupBuff>(), BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //热狗 饱食度15 减少移速，增加防御
                {
                    ItemID.Hotdog,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<HotdogBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //冰淇淋 饱食度10 免疫着火和火块
                {
                    ItemID.IceCream,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<IceCreamBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //柠檬 饱食度10 加5%远程暴击,仇恨值减300
                {
                    ItemID.Lemon,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<LemonBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //龙虾尾 饱食度20 加6防御,25%挖矿速度
                {
                    ItemID.LobsterTail,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<LobsterTailBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //芒果 饱食度10 减缓因食物中毒而产生的持续减血效果
                {
                    ItemID.Mango,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<MangoBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //棉花糖 饱食度5 减50%最大掉落速度，增加额外摔伤距离
                {
                    ItemID.Marshmallow,
                    new FoodInfo() {
                        Satiety = 5,
                        BuffType = ModContent.BuffType<MarshmallowBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //怪物三明治 饱食度20 加25%暴击率，每秒减4生命
                {
                    ItemID.MonsterLasagna,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<MonsterLasagnaBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //玉米片 饱食度20 攻击造成涂油以及所有火焰减益
                {
                    ItemID.Nachos,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<NachosBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //泰式炒面 饱食度20 加50%召唤物击退
                {
                    ItemID.PadThai,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<PadThaiBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //桃子 饱食度10 增加心的拾取范围，1生命回复
                {
                    ItemID.Peach,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<PeachBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //越南河粉 饱食度20 加10%召唤物伤害
                {
                    ItemID.Pho,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<PhoBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //菠萝 饱食度10 加6防御，50%反伤
                {
                    ItemID.Pineapple,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<PineappleBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //披萨 饱食度15 加8穿甲
                {
                    ItemID.Pizza,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<PizzaBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //李子 饱食度10 加600仇恨值，8%攻速
                {
                    ItemID.Plum,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<PlumBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //薯片 饱食度15 加4防御，4%伤害
                {
                    ItemID.PotatoChips,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<PotatoChipsBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //南瓜派 饱食度20 最大生命值加50
                {
                    ItemID.PumpkinPie,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<PumpkinPieBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //红毛丹 饱食度10 免疫中毒和毒液
                {
                    ItemID.Rambutan,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<RambutanBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //烤鸟 饱食度20 中幅增强飞行能力
                {
                    ItemID.RoastedBird,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<RoastedBirdBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //烤鸭 饱食度20 可以在水上行走，小幅增强飞行能力
                {
                    ItemID.RoastedDuck,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<RoastedDuckBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //生鱼片 饱食度20 可以游泳，水下呼吸，加10%伤害，20%移速，每秒减3生命
                {
                    ItemID.Sashimi,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<SashimiBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //炒蛙腿 饱食度20 自动跳跃，增加跳跃能力
                {
                    ItemID.SauteedFrogLegs,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<SauteedFrogLegsBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //海鲜大餐 饱食度30 增加12%暴击，伤害，攻速
                {
                    ItemID.SeafoodDinner,
                    new FoodInfo() {
                        Satiety = 30,
                        BuffType = ModContent.BuffType<SeafoodDinnerBuff>(),
                        BuffTime = new FoodDuration(10, 0, 0)
                    }
                },
                //鲜虾三明治 饱食度15 加6防御,25%挖矿速度.
                {
                    ItemID.ShrimpPoBoy,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<ShrimpPoBoyBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //去壳牡蛎 饱食度20 加10穿甲,减5防御，每秒减3生命
                {
                    ItemID.ShuckedOyster,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<ShuckedOysterBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0)
                    }
                },
                //意大利面 饱食度20 加1召唤栏
                {
                    ItemID.Spaghetti,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<SpaghettiBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //杨桃 饱食度10 使用远程武器时会生成向后的射弹
                {
                    ItemID.Starfruit,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<StarfruitBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
                //牛排 饱食度20 减33%魔力消耗
                {
                    ItemID.Steak,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<SteakBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0)
                    }
                },
                //蜜糖饼干 饱食度10 加10%远程伤害
                {
                    ItemID.SugarCookie,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<SugarCookieBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0)
                    }
                },
            };

        }

        public override void SetStaticDefaults()
        {

            //Tooltip.SetDefault("it can display CurrentSatiety");
        }

        public override void SetDefaults(Item item)
        {
            // 如果是原版的食物，那么就手动处理
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];

                // 替换掉原版的 buff 类型
                item.buffType = foodInfo.BuffType;
                item.buffTime = foodInfo.BuffTime.TotalFrames;
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
                //  Main.NewText($"Added {foodInfo.Satiety}! Current Satiety {foodPlayer.CurrentSatiety} / {foodPlayer.MaximumSatiety}");
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
            return base.CanUseItem(item, player);
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

                    //  Main.NewText($"Cannot eat this!");
                    return false;
                }
            }
            else if (item.ModItem is FoodBase)
            {
                var foodItem = item.ModItem as FoodBase;
                var foodInfo = foodItem.FoodInfo;
                if (!foodPlayer.CanEat(foodInfo))
                {
                    //    Main.NewText($"Cannot eat this!");
                    return false;
                }
            }
            return base.ConsumeItem(item, player);
        }
    }
}
