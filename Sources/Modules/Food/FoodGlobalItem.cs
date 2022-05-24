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
                //苹果 
                {
                    ItemID.Apple,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<AppleBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="增加8%减伤\n“一天一苹果，医生远离我”"
                    }
                },
                //苹果派 
                {
                    ItemID.ApplePie,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<ApplePieBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="加8%减伤,1生命回复\n“一天一苹果，医生远离我”"
                    }
                },
                //杏 
                {
                    ItemID.Apricot,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ApricotBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="增加加4魔力再生\n“止渴润肺”"
                    }
                },
                //培根
                {
                    ItemID.Bacon,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<MilkCartonBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="加2生命回复，所受冷系伤害降低\n“开胃祛寒”"
                    }
                },
                //香蕉
                {
                    ItemID.Banana,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<BaconBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="20%不消耗弹药，加5%远程伤害\n“低血压”"
                    }
                },
                //香蕉船
                {
                    ItemID.BananaSplit,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<BananaSplitBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="33%不消耗弹药，增加8%远程暴击\n“低血压”"
                    }
                },
                //烧烤肋排
                {
                    ItemID.BBQRibs,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<BBQRibsBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="加50血量上限\n“滋阴补血”"
                    }
                },
                //黑醋栗
                {
                    ItemID.BlackCurrant,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<BlackCurrantBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="获得夜视、危险感知能力\n“改善視力” "
                    }
                },
                //血橙
                {
                    ItemID.BloodOrange,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<BloodOrangeBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="增加加25血量上限\n“这里不是崽饿”"
                    }
                },
                //鱼菇汤
                {
                    ItemID.BowlofSoup,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<BowlofSoupBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="增加20魔力上限,5%魔法伤害\n“补脑”"
                    }
                },
                //炖兔兔
                {
                    ItemID.BunnyStew,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<BunnyStewBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="自动跳跃，增加跳跃能力\n“没有任何兔子在制作过程中受到伤害 ”"
                    }
                },
                //汉堡
                {
                    ItemID.Burger,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<BurgerBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="减少移速，增加防御\n“热量炸弹”"
                    }
                },
                //樱桃
                {
                    ItemID.Cherry,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<CherryBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="增加移速与跳跃高度\n“你所热爱的，就是你的生活”"
                    }
                },
                //鸡块
                {
                    ItemID.ChickenNugget,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ChickenNuggetBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="增加1生命回复、8%攻速\n“数一数二的鸡块！”"
                    }
                },
                //巧克力曲奇饼干
                {
                    ItemID.ChocolateChipCookie,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ChocolateChipCookieBuff>(),
                        BuffTime = new FoodDuration(0, 6,0 ),
                        Description ="增加2点生命回复和魔力回复\n“补充能量”"
                    }
                },
                //圣诞布丁
                {
                    ItemID.ChristmasPudding,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ChristmasPuddingBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="仇恨值减少800\n“美容养颜”"
                    }
                },
                //椰子
                {
                    ItemID.Coconut,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<CoconutBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="增加4防御，5%减伤\n“我从小啃到大”"
                    }
                },
                //熟鱼
                {
                    ItemID.CookedFish,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<CookedFishBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="加40魔力上限,8%魔法暴击率\n“益气健脾”"
                    }
                },
                //熟棉花糖
                {
                    ItemID.CookedMarshmallow,
                    new FoodInfo() {
                        Satiety = 5,
                        BuffType = ModContent.BuffType<CookedMarshmallowBuff>(),
                        BuffTime = new FoodDuration(5, 0, 0),
                        Description ="减75%最大掉落速度,但无法操控下落速度\n“轻飘飘的”"
                    }
                },
                //熟虾
                {
                    ItemID.CookedShrimp,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<CookedShrimpBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="加10防御,4穿甲\n“补钙”"
                    }
                },
                //火龙果
                {
                    ItemID.Dragonfruit,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<DragonfruitBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description = "攻击造成着火以及涂油\n“红红火火恍恍惚惚”"
                    }
                },
                //接骨木果
                {
                    ItemID.Elderberry,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<ElderberryBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="你可以短距离冲刺\n“抗氧化”"
                    }
                },
                //食用蜗牛
                {
                    ItemID.Escargot,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<EscargotBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="大大减速，加50%减伤\n“这不是神龟药水！”"
                    }
                },
                //煎蛋
                {
                    ItemID.FriedEgg,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<FriedEggBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="加8%伤害\n“蛋白质”"
                    }
                },
                //薯条
                {
                    ItemID.Fries,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<FriesBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="加4防御，8%暴击\n“高油高盐”"
                    }
                },
                //蛙腿三明治
                {
                    ItemID.FroggleBunwich,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<FroggleBunwichBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="自动跳跃，增加伤害和跳跃能力\n“你能真正尝到沼泽的味道。”"
                    }
                },
                //水果色拉
                {
                    ItemID.FruitSalad,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<FruitSaladBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="提升大部分属性\n“长久的维生素！”"
                    }
                },
                //姜饼
                {
                    ItemID.GingerbreadCookie,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<GingerbreadCookieBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="加1生命回复,保暖\n“驱寒排毒”"
                    }
                },
                //金美味
                {
                    ItemID.GoldenDelight,
                    new FoodInfo() {
                        Satiety = 30,
                        BuffType = ModContent.BuffType<GoldenDelightBuff>(),
                        BuffTime = new FoodDuration(10, 0, 0),
                        Description ="攻击造迈达斯，提升大部分属性\n“金灿灿!”"
                    }
                },
                //葡萄柚
                {
                    ItemID.Grapefruit,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<GrapefruitBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="加50%召唤物击退\n “拒绝肾透支”"
                    }
                },
                //葡萄
                {
                    ItemID.Grapes,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<GrapesBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="加1召唤栏，幸运值加10%，减8防御\n“多子多福 ”"
                    }
                },
                //烤松鼠
                {
                    ItemID.GrilledSquirrel,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<GrilledSquirrelBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="增加跳跃能力\n“欢跃”"
                    }
                },
                //蛆虫汤
                {
                    ItemID.GrubSoup,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<GrubSoupBuff>(), 
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="加25渔力，每秒减2生命\n“吃啥补啥，就是有点恶心”"
                    }
                },
                //热狗
                {
                    ItemID.Hotdog,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<HotdogBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="减少移速，增加近战伤害\n“热量炸弹”"
                    }
                },
                //冰淇淋
                {
                    ItemID.IceCream,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<IceCreamBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="免疫着火和火块\n“吃雪（bushi”"
                    }
                },
                //柠檬
                {
                    ItemID.Lemon,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<LemonBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="加5%远程暴击,仇恨值减300\n“消炎美容”"
                    }
                },
                //龙虾尾
                {
                    ItemID.LobsterTail,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<LobsterTailBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="加6防御,25%挖矿速度\n“壮阳”"
                    }
                },
                //芒果
                {
                    ItemID.Mango,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<MangoBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="减缓因食物中毒而产生的持续减血效果\n“清胃解毒”"
                    }
                },
                //棉花糖
                {
                    ItemID.Marshmallow,
                    new FoodInfo() {
                        Satiety = 5,
                        BuffType = ModContent.BuffType<MarshmallowBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="可以二段跳\n“像云一样”"
                    }
                },
                //怪物三明治
                {
                    ItemID.MonsterLasagna,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<MonsterLasagnaBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="增加25%暴击率，但每秒减4生命\n“弄熟的恶魔还是恶魔，在千层饼中也不会改变。”"
                    }
                },
                //玉米片
                {
                    ItemID.Nachos,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<NachosBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="攻击造成涂油以及所有火焰减益\n“爆米花”"
                    }
                },
                //泰式炒面
                {
                    ItemID.PadThai,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<PadThaiBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="增加50%召唤物击退\n“异域风情”"
                    }
                },
                //桃子
                {
                    ItemID.Peach,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<PeachBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="增加心的拾取范围，1生命回复\n“在想peach”"
                    }
                },
                //越南河粉
                {
                    ItemID.Pho,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<PhoBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="增加10%召唤物伤害\n“异域风情 ”"
                    }
                },
                //菠萝
                {
                    ItemID.Pineapple,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<PineappleBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="增加6防御，50%反伤\n“菠萝碱”"
                    }
                },
                //披萨
                {
                    ItemID.Pizza,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<PizzaBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="增加10穿甲\n“会让意大利人破防的菠萝披萨。”"
                    }
                },
                //李子
                {
                    ItemID.Plum,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<PlumBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="加600仇恨值，8%攻速\n“至于我的建议，还是再等等吧”"
                    }
                },
                //薯片
                {
                    ItemID.PotatoChips,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<PotatoChipsBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="加4防御，8%伤害\n“高油高盐”"
                    }
                },
                //南瓜派
                {
                    ItemID.PumpkinPie,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<PumpkinPieBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="最大生命值加50\n“丰收的喜悦”"
                    }
                },
                //红毛丹
                {
                    ItemID.Rambutan,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<RambutanBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="免疫中毒和毒液以及十字章一样的免疫效果\n“提高免疫”"
                    }
                },
                //烤鸟
                {
                    ItemID.RoastedBird,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<RoastedBirdBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="增加飞行能力\n“我是一只小小小鸟”"
                    }
                },
                //烤鸭
                {
                    ItemID.RoastedDuck,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<RoastedDuckBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="可以在水上行走，增加飞行能力\n“这是烤鸭”"
                    }
                },
                //生鱼片
                {
                    ItemID.Sashimi,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<SashimiBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="可以游泳，水下呼吸，增加10%伤害，20%移速，但每秒减3生命\n“寄生虫！”"
                    }
                },
                //炒蛙腿
                {
                    ItemID.SauteedFrogLegs,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<SauteedFrogLegsBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="自动跳跃，增加跳跃能力\n“钓不到蛙腿？那就吃吧。”"
                    }
                },
                //海鲜大餐
                {
                    ItemID.SeafoodDinner,
                    new FoodInfo() {
                        Satiety = 30,
                        BuffType = ModContent.BuffType<SeafoodDinnerBuff>(),
                        BuffTime = new FoodDuration(10, 0, 0),
                        Description ="增加12%暴击，伤害，攻速\n“够生猛！”"
                    }
                },
                //鲜虾三明治
                {
                    ItemID.ShrimpPoBoy,
                    new FoodInfo() {
                        Satiety = 15,
                        BuffType = ModContent.BuffType<ShrimpPoBoyBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="加6防御,25%挖矿速度\n“壮阳”"
                    }
                },
                //去壳牡蛎
                {
                    ItemID.ShuckedOyster,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<ShuckedOysterBuff>(),
                        BuffTime = new FoodDuration(6, 0, 0),
                        Description ="减5防御，加10穿甲，每秒减3生命\n“寄生虫！”"
                    }
                },
                //意大利面
                {
                    ItemID.Spaghetti,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<SpaghettiBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="增加1召唤栏\n“异域风情”"
                    }
                },
                //杨桃
                {
                    ItemID.Starfruit,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<StarfruitBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="使用远程武器时会生成向后的射弹\n“1437大帝的淫威”"
                    }
                },
                //牛排
                {
                    ItemID.Steak,
                    new FoodInfo() {
                        Satiety = 20,
                        BuffType = ModContent.BuffType<SteakBuff>(),
                        BuffTime = new FoodDuration(8, 0, 0),
                        Description ="减少33%魔力消耗\n“上流 ”"
                    }
                },
                //蜜糖饼干
                {
                    ItemID.SugarCookie,
                    new FoodInfo() {
                        Satiety = 10,
                        BuffType = ModContent.BuffType<SugarCookieBuff>(),
                        BuffTime = new FoodDuration(4, 0, 0),
                        Description ="加5%远程伤害\n“养胃滋润”"
                    }
                },
            };

        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                tooltips.RemoveRange(2,3);
                var foodInfo = m_vanillaFoodInfos[item.type];
                TooltipLine tooltip = new TooltipLine(Mod,item.Name, foodInfo.Description);
                tooltips.Add(tooltip);
            }
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
