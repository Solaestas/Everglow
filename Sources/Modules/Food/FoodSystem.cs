using Everglow.Sources.Modules.Food.Buffs;

namespace Everglow.Sources.Modules.Food
{
    internal class FoodSystem : ModSystem
    {
        // 对于原版的食物进行类型Id到 FoodInfo 的映射，直接获取FoodInfo实例
        private Dictionary<Item_id, FoodInfo> m_vanillaFoodInfos;

        public Dictionary<Item_id, FoodInfo> VanillaFoodInfos
        {
            get { return m_vanillaFoodInfos; }
        }

        public override void PostSetupContent()
        {
            m_vanillaFoodInfos = new Dictionary<Item_id, FoodInfo>
            {
                //麦芽酒 饱食度5 防御减8，伤害、暴击率和近战攻速各增加8%
                { ItemID.Ale, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<AleBuff>() , BuffTime = 36000 } },
                //
                { ItemID.Apple, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<AppleBuff>() , BuffTime = 450 } },
                //
                { ItemID.AppleJuice, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<AppleJuiceBuff>() , BuffTime = 450} },
                //
                { ItemID.ApplePie, new FoodInfo() { Satiety =15, BuffType = ModContent.BuffType<ApplePieBuff>() , BuffTime = 450 } },
                //杏 饱食度10 魔力再生加4
                { ItemID.Apricot, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ApricotBuff>() , BuffTime = 14400} },
                //培根 饱食度10 加2生命回复，所受冷系伤害降低
                { ItemID.Bacon, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MilkCartonBuff>() , BuffTime = 28800 } },
                //香蕉 饱食度10 20%不消耗弹药，加5%远程伤害
                { ItemID.Banana, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<BaconBuff>() , BuffTime = 14400 } },
                //冰冻香蕉代基里 饱食度5 短时间内不消耗子弹，极大增加远程伤害暴击
                { ItemID.BananaDaiquiri, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<BananaDaiquiriBuff>() , BuffTime = 450 } },
                //香蕉船 饱食度15 33%不消耗弹药，加8%暴击
                { ItemID.BananaSplit, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<BananaSplitBuff>() , BuffTime = 21600 } },
                //烧烤肋排 饱食度20 加50血量上限
                { ItemID.BBQRibs, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BBQRibsBuff>() , BuffTime = 28800 } },
                //黑醋栗  饱食度10 获得夜视、危险感知能力
                { ItemID.BlackCurrant, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<BlackCurrantBuff>() , BuffTime = 14400 } },
                //血橙 饱食度10 加25血量上限
                { ItemID.BloodOrange, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<BloodOrangeBuff>() , BuffTime = 14400 } },
                //血腥麝香葡萄 饱食度5 短时间内迅速回血
                { ItemID.BloodyMoscato, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<BloodyMoscatoBuff>() , BuffTime = 450 } },
                //鱼菇汤 饱食度15 加20魔力上限,5%魔法伤害
                { ItemID.BowlofSoup, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<BowlofSoupBuff>() , BuffTime = 21600 } },
                //炖兔兔 饱食度20 自动跳跃，增加跳跃能力
                { ItemID.BunnyStew, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BunnyStewBuff>() , BuffTime = 28800 } },
                //汉堡 饱食度15 减少移速，增加防御
                { ItemID.Burger, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<BurgerBuff>() , BuffTime = 21600 } },
                //樱桃 饱食度10 增加移速与跳跃高度
                { ItemID.Cherry, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<CherryBuff>() , BuffTime = 14400 } },
                //鸡块 饱食度10 增加1生命回复、4%攻速
                { ItemID.ChickenNugget, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ChickenNuggetBuff>() , BuffTime = 14400 } },
                //巧克力曲奇饼干 饱食度10 短时间内快速恢复生命与魔力
                { ItemID.ChocolateChipCookie, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ChocolateChipCookieBuff>() , BuffTime = 450 } },
                //圣诞布丁 饱食度10 仇恨值减600
                { ItemID.ChristmasPudding, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ChristmasPuddingBuff>(), BuffTime = 21600 } },
                //椰子 饱食度10 加4防御，5%减伤
                { ItemID.Coconut, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<CoconutBuff>() , BuffTime = 14400 } },
                //咖啡 饱食度5 加25%铺墙铺砖速度，5%攻速
                { ItemID.CoffeeCup, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<CoffeeCupBuff>() , BuffTime = 18000 } },
                //熟鱼 饱食度20 加40魔力上限,8%魔法暴击率
                { ItemID.CookedFish, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CookedFishBuff>() , BuffTime = 28800 } },
                 //熟棉花糖 饱食度5 减40%最大掉落速度，增加额外摔伤距离
                { ItemID.CookedMarshmallow, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<CookedMarshmallowBuff>(), BuffTime = 18000 } },
                //熟虾 饱食度20 加10防御,4穿甲
                { ItemID.CookedShrimp, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CookedShrimpBuff>() , BuffTime = 28800 } },
                //奶油苏打水 饱食度5 加8%远程伤害
                { ItemID.CreamSoda, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<CreamSodaBuff>(), BuffTime = 18000 } },
                //火龙果 饱食度10 攻击造成着火
                { ItemID.Dragonfruit, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<DragonfruitBuff>() , BuffTime = 14400 } },
                //接骨木果 饱食度10 你可以短距离冲刺
                { ItemID.Elderberry, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<ElderberryBuff>() , BuffTime = 14400 } },
                //食用蜗牛 饱食度10 大大减速，加60%减伤
                { ItemID.Escargot, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<EscargotBuff>() , BuffTime = 28800 } },
                //煎蛋 饱食度10 加8%伤害
                { ItemID.FriedEgg, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<FriedEggBuff>() , BuffTime = 14400 } },
                //薯条 饱食度10 加4防御，4%暴击
                { ItemID.Fries, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<FriesBuff>() , BuffTime = 14400 } },
                //蛙腿三明治 饱食度20 饱食度20 自动跳跃，增加伤害和跳跃能力
                { ItemID.FroggleBunwich, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<FroggleBunwichBuff>() , BuffTime = 28800 } },
                //果汁 饱食度5 小幅提升大部分属性
                { ItemID.FruitJuice, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<FruitJuiceBuff>() , BuffTime = 14400 } },
                //水果色拉 饱食度10 中幅提升大部分属性
                { ItemID.FruitSalad, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<FruitSaladBuff>() , BuffTime = 14400 } },
                //姜饼 饱食度10 加1生命回复,保暖
                { ItemID.GingerbreadCookie, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<GingerbreadCookieBuff>() , BuffTime = 14400 } },
                //金美味 饱食度30 攻击造迈达斯，中幅提升大部分属性
                { ItemID.GoldenDelight, new FoodInfo() { Satiety = 30, BuffType = ModContent.BuffType<GoldenDelightBuff>() , BuffTime = 36000 } },
                //葡萄柚 饱食度10 加10%召唤物击退
                { ItemID.Grapefruit, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<GrapefruitBuff>() , BuffTime = 14400 } },
                //葡萄汁 饱食度5 短时间内加5召唤栏，加100%召唤物伤害，击退，极其幸运
                { ItemID.GrapeJuice, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<GrapeJuiceBuff>() , BuffTime = 450 } },
                //葡萄 饱食度10 加1召唤栏，幸运值加10%，减8防御
                { ItemID.Grapes, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<GrapesBuff>() , BuffTime = 14400 } },
                //烤松鼠 饱食度20 增加跳跃能力
                { ItemID.GrilledSquirrel, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GrilledSquirrelBuff>(), BuffTime = 28800 } },
                //蛆虫汤 饱食度15 加25渔力，每秒减2血
                { ItemID.GrubSoup, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<GrubSoupBuff>() , BuffTime = 21600 } },
                //热狗 饱食度15 减少移速，增加防御
                { ItemID.Hotdog, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<HotdogBuff>() , BuffTime = 21600 } },
                //冰淇淋 饱食度10 免疫着火和火块
                { ItemID.IceCream, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<IceCreamBuff>() , BuffTime = 14400 } },
                //柠檬水 饱食度5 短时间内远程击退加倍,仇恨值减2400
                { ItemID.Lemonade, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<LemonadeBuff>() , BuffTime = 450 } },
                //柠檬 饱食度10 加5%远程暴击,仇恨值减300
                { ItemID.Lemon, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<LemonBuff>() , BuffTime = 14400 } },
                //龙虾尾 饱食度20 加6防御,25%挖矿速度
                { ItemID.LobsterTail, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<LobsterTailBuff>() , BuffTime = 28800 } },
                //芒果 饱食度10 减缓因食物中毒而产生的持续减血效果
                { ItemID.Mango, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<MangoBuff>() , BuffTime = 14400 } },
                //棉花糖 饱食度5 减50%最大掉落速度，增加额外摔伤距离
                { ItemID.Marshmallow, new FoodInfo() { Satiety = 5 , BuffType = ModContent.BuffType<MarshmallowBuff>() , BuffTime = 14400 } },
                //盒装牛奶 饱食度10 同十字章一样的免疫效果
                { ItemID.MilkCarton, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<MilkCartonBuff>() , BuffTime = 36000 } },
                //奶昔 饱食度10 加10%移速
                { ItemID.Milkshake, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<MilkshakeBuff>() , BuffTime = 18000 } },
                //怪物三明治 饱食度20 加25%暴击率，每秒减4生命
                { ItemID.MonsterLasagna, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MonsterLasagnaBuff>() , BuffTime = 28800 } },
                //玉米片 饱食度20 攻击造成涂油以及所有火焰减益
                { ItemID.Nachos, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<NachosBuff>() , BuffTime = 28800 } },
                //泰式炒面 饱食度20 加50%召唤物击退
                { ItemID.PadThai, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PadThaiBuff>() , BuffTime = 28800 } },
                //桃子 饱食度10 增加心的拾取范围，1生命回复
                { ItemID.Peach, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PeachBuff>() , BuffTime = 14400 } },
                //桃子果酒 饱食度5 增加心的拾取范围，1生命回复，减8防御
                { ItemID.PeachSangria, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<PeachSangriaBuff>() , BuffTime = 18000 } },
                //越南河粉 饱食度20 加10%召唤物伤害
                { ItemID.Pho, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PhoBuff>() , BuffTime = 28800 } },
                //椰林飘香 饱食度5 加5%减伤，2防御，33%反伤
                { ItemID.PinaColada, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<PinaColadaBuff>() , BuffTime = 18000 } },
                //菠萝 饱食度10 加6防御，50%反伤
                { ItemID.Pineapple, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PineappleBuff>() , BuffTime = 14400 } },
                //披萨 饱食度15 加8穿甲
                { ItemID.Pizza, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<PizzaBuff>() , BuffTime = 216 } },
                //李子 饱食度10 加600仇恨值，8%攻速
                { ItemID.Plum, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<PlumBuff>() , BuffTime = 14400 } },
                //薯片 饱食度15 加4防御，4%伤害
                { ItemID.PotatoChips, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<PotatoChipsBuff>() , BuffTime = 21600 } },
                //七彩潘趣酒 饱食度5 加400仇恨值,1召唤栏,减8防御
                { ItemID.PrismaticPunch, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<PrismaticPunchBuff>() , BuffTime = 18000 } },
                //南瓜派 饱食度20 最大生命值加50
                { ItemID.PumpkinPie, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PumpkinPieBuff>() , BuffTime = 28800 } },
                //红毛丹 饱食度10 免疫中毒和毒液
                { ItemID.Rambutan, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<RambutanBuff>() , BuffTime = 14400 } },
                //烤鸟 饱食度20 中幅增强飞行能力
                { ItemID.RoastedBird, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<RoastedBirdBuff>() , BuffTime = 28800 } },
                //烤鸭 饱食度20 可以在水上行走，小幅增强飞行能力
                { ItemID.RoastedDuck, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<RoastedDuckBuff>() , BuffTime = 28800 } },
                //清酒 饱食度5 短时间内减18防御，加80%暴击，加80%伤害， 加80%攻速
                { ItemID.Sake, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<SakeBuff>() , BuffTime = 450 } },
                //生鱼片 饱食度20 可以游泳，水下呼吸，加10%伤害，20%移速，每秒减3生命
                { ItemID.Sashimi, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SashimiBuff>() , BuffTime = 28800 } },
                //炒蛙腿 饱食度20 自动跳跃，增加跳跃能力
                { ItemID.SauteedFrogLegs, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SauteedFrogLegsBuff>() , BuffTime = 28800 } },
                //海鲜大餐 饱食度30 增加12%暴击，伤害，攻速
                { ItemID.SeafoodDinner, new FoodInfo() { Satiety = 30, BuffType = ModContent.BuffType<SeafoodDinnerBuff>() , BuffTime = 36000 } },
                //鲜虾三明治 饱食度15 加6防御,25%挖矿速度.
                { ItemID.ShrimpPoBoy, new FoodInfo() { Satiety = 15, BuffType = ModContent.BuffType<ShrimpPoBoyBuff>() , BuffTime = 21600 } },
                //去壳牡蛎 饱食度20 加10穿甲,减5防御，每秒减3生命
                { ItemID.ShuckedOyster, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ShuckedOysterBuff>() , BuffTime = 21600 } },
                //暗黑奶昔 饱食度5 短时间内80%闪避
                { ItemID.SmoothieofDarkness, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<SmoothieofDarknessBuff>() , BuffTime = 450 } },
                //意大利面 饱食度20 加1召唤栏
                { ItemID.Spaghetti, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SpaghettiBuff>() , BuffTime = 28800 } },
                //杨桃 饱食度10 使用远程武器时会生成向后的射弹
                { ItemID.Starfruit, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<StarfruitBuff>() , BuffTime = 14400 } },
                //牛排 饱食度20 减33%魔力消耗
                { ItemID.Steak, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SteakBuff>() , BuffTime = 28800 } },
                //蜜糖饼干 饱食度10 加10%远程伤害
                { ItemID.SugarCookie, new FoodInfo() { Satiety = 10, BuffType = ModContent.BuffType<SugarCookieBuff>() , BuffTime = 14400 } },
                //一杯茶 饱食度5 加2魔力回复，20魔力上限
                { ItemID.Teacup, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<TeacupBuff>() , BuffTime = 18000 } },
                //热带奶昔 饱食度5 短时间内不消耗魔力，加100%魔力攻击，100%暴击
                { ItemID.TropicalSmoothie, new FoodInfo() { Satiety = 5, BuffType = ModContent.BuffType<TropicalSmoothieBuff>() , BuffTime = 450 } }
            };
            base.PostSetupContent();
        }
    }
}
