using Everglow.Sources.Modules.Food.Buffs;
using Everglow.Sources.Modules.Food.DataStructures;
using Everglow.Sources.Modules.Food.Items;


namespace Everglow.Sources.Modules.Food
{
    public class DrinkGlobalItem : GlobalItem
    {
      
        // 对于原版的饮料进行类型Id到 DrinkInfo 的映射，直接获取DrinkInfo实例
        private static Dictionary<int, DrinkInfo> m_vanillaDrinkInfos;
        public DrinkGlobalItem()
        {
            m_vanillaDrinkInfos = new Dictionary<int, DrinkInfo>
            {
                //麦芽酒 饱食度5 防御减8，伤害、暴击率和近战攻速各增加8%
                {
                    ItemID.Ale,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<AleBuff> (),
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //苹果汁 饱食度5 短时间内60%减伤 
                {
                    ItemID.AppleJuice,
                    new DrinkInfo() {
                       Thirsty = false , 
                       BuffType = ModContent.BuffType<AppleJuiceBuff>(),
                       BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                 //冰冻香蕉代基里 饱食度5 短时间内不消耗子弹，极大增加远程伤害暴击
                {
                    ItemID.BananaDaiquiri,
                    new DrinkInfo() {
                        Thirsty = false , 
                        BuffType = ModContent.BuffType<BananaDaiquiriBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                 //血腥麝香葡萄 饱食度5 短时间内迅速回血
                {
                    ItemID.BloodyMoscato,
                    new DrinkInfo() {
                        Thirsty = false , 
                        BuffType = ModContent.BuffType<BloodyMoscatoBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //奶油苏打水 饱食度5 加8%远程伤害
                {
                    ItemID.CreamSoda,
                    new DrinkInfo() {
                        Thirsty = false , 
                        BuffType = ModContent.BuffType<CreamSodaBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //咖啡 饱食度5 加25%铺墙铺砖速度，5%攻速
                {
                    ItemID.CoffeeCup,
                    new DrinkInfo() {
                        Thirsty = false , 
                        BuffType = ModContent.BuffType<CoffeeCupBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //果汁 饱食度5 小幅提升大部分属性
                {
                    ItemID.FruitJuice,
                    new DrinkInfo() {
                       Thirsty = false , 
                       BuffType = ModContent.BuffType<FruitJuiceBuff>(), 
                       BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //葡萄汁 饱食度5 短时间内加5召唤栏，加100%召唤物伤害，击退，极其幸运
                {
                    ItemID.GrapeJuice,
                    new DrinkInfo() {
                        Thirsty = false , 
                        BuffType = ModContent.BuffType<GrapeJuiceBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },                
                //柠檬水 饱食度5 短时间内远程击退加倍,仇恨值减2400
                {
                    ItemID.Lemonade,
                    new DrinkInfo() {
                        Thirsty = false , 
                        BuffType = ModContent.BuffType<LemonadeBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },     
                //盒装牛奶 饱食度10 同十字章一样的免疫效果
                {
                    ItemID.MilkCarton,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<MilkCartonBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //奶昔 饱食度10 加10%移速
                {
                    ItemID.Milkshake,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<MilkshakeBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //桃子果酒 饱食度5 增加心的拾取范围，1生命回复，减8防御
                {
                    ItemID.PeachSangria,
                    new DrinkInfo() {
                        Thirsty = false, 
                        BuffType = ModContent.BuffType<PeachSangriaBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //椰林飘香 饱食度5 加5%减伤，2防御，33%反伤
                {
                    ItemID.PinaColada,
                    new DrinkInfo() {
                        Thirsty = false, 
                        BuffType = ModContent.BuffType<PinaColadaBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //七彩潘趣酒 饱食度5 加400仇恨值,1召唤栏,减8防御
                {
                    ItemID.PrismaticPunch,
                    new DrinkInfo() {
                        Thirsty = false, 
                        BuffType = ModContent.BuffType<PrismaticPunchBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },                
                //清酒 饱食度5 短时间内减18防御，加80%暴击，加80%伤害， 加80%攻速
                {
                    ItemID.Sake,
                    new DrinkInfo() {
                        Thirsty = false, 
                        BuffType = ModContent.BuffType<SakeBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //暗黑奶昔 饱食度5 短时间内80%闪避
                {
                    ItemID.SmoothieofDarkness,
                    new DrinkInfo() {
                        Thirsty = false, 
                        BuffType = ModContent.
                        BuffType<SmoothieofDarknessBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                }, 
                //一杯茶 饱食度5 加2魔力回复，20魔力上限
                {
                    ItemID.Teacup,
                    new DrinkInfo() {
                        Thirsty = false, 
                        BuffType = ModContent.BuffType<TeacupBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                },
                //热带奶昔 饱食度5 短时间内不消耗魔力，加100%魔力攻击，100%暴击
                {
                    ItemID.TropicalSmoothie,
                    new DrinkInfo() {
                        Thirsty = false, 
                        BuffType = ModContent.BuffType<TropicalSmoothieBuff>(), 
                        BuffTime = new FoodDuration(0, 7, 30)
                    }
                }
            };
        }

        public override void SetStaticDefaults()
        {
            
        }
        
        public override void SetDefaults(Item item)
        {

            // 如果是原版的饮料，那么就手动处理
            if (m_vanillaDrinkInfos.ContainsKey(item.type))
            {
                var drinkInfo = m_vanillaDrinkInfos[item.type];

                // 替换掉原版的 buff 类型
                item.buffType = drinkInfo.BuffType;
                item.buffTime = drinkInfo.BuffTime.TotalFrames;
            }
            base.SetDefaults(item);
        }

        public override void OnConsumeItem(Item item, Player player)
        {
            // 如果是原版的食物，那么就手动处理，因为已经使用了物品，说明玩家满足饱食度要求
            if (m_vanillaDrinkInfos.ContainsKey(item.type))
            {
                var drinkInfo = m_vanillaDrinkInfos[item.type];
                var foodPlayer = player.GetModPlayer<FoodModPlayer>();

                // 变得不渴，并且应用一些特效
                foodPlayer.Thirstystate = drinkInfo.Thirsty;
              
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            var foodPlayer = player.GetModPlayer<FoodModPlayer>();
             // 判断能否喝下物品
            if (m_vanillaDrinkInfos.ContainsKey(item.type))
            {
                var drinkInfo = m_vanillaDrinkInfos[item.type];
                if (!foodPlayer.CanDrink(drinkInfo))
                {
                    Main.NewText($"Cannot drink this!");
                    return false;
                }
            }
            else if (item.ModItem is FoodBase)
            {
                var foodItem = item.ModItem as DrinkBase;
                var drinkInfo = foodItem.DrinkInfo;
                if (!foodPlayer.CanDrink(drinkInfo))
                {
                    Main.NewText($"Cannot drink this!");
                    return false;
                }
            }

            return base.CanUseItem(item, player);
        }

        public override bool ConsumeItem(Item item, Player player)
        {
            var foodPlayer = player.GetModPlayer<FoodModPlayer>();
            // 判断能否喝下物品
            if (m_vanillaDrinkInfos.ContainsKey(item.type))
            {
                var drinkInfo = m_vanillaDrinkInfos[item.type];
                if (!foodPlayer.CanDrink(drinkInfo))
                {
                    Main.NewText($"Cannot drink this!");
                    return false;
                }
            }
            else if (item.ModItem is FoodBase)
            {
                var foodItem = item.ModItem as DrinkBase;
                var drinkInfo = foodItem.DrinkInfo;
                if (!foodPlayer.CanDrink(drinkInfo))
                {
                    Main.NewText($"Cannot drink this!");
                    return false;
                }
            }
            return base.ConsumeItem(item, player);
        }
    }
}
