using Everglow.Sources.Modules.FoodModule.Buffs;
using Everglow.Sources.Modules.FoodModule.DataStructures;
using Everglow.Sources.Modules.FoodModule.Items;
using Everglow.Sources.Modules.FoodModule.Buffs.VanillaFoodBuffs;

namespace Everglow.Sources.Modules.FoodModule
{
    public class DrinkGlobalItem : GlobalItem
    {

        // 对于原版的饮料进行类型Id到 DrinkInfo 的映射，直接获取DrinkInfo实例
        private static Dictionary<int, DrinkInfo> m_vanillaDrinkInfos;
        public DrinkGlobalItem()
        {
            m_vanillaDrinkInfos = new Dictionary<int, DrinkInfo>
            {
                //麦芽酒
                {
                    ItemID.Ale,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<AleBuff> (),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description =  "短时间内鞭子的范围和速度提升至2.5倍\n“耍酒疯”"
                    }
                },
                //苹果汁
                {
                    ItemID.AppleJuice,
                    new DrinkInfo() {
                       Thirsty = false ,
                       BuffType = ModContent.BuffType<AppleJuiceBuff>(),
                       BuffTime = new FoodDuration(0, 7, 30),
                       Description =  " 短时间内增加80%减伤\n“一天一苹果，医生远离我”"
                    }
                },
                 //冰冻香蕉代基里
                {
                    ItemID.BananaDaiquiri,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<BananaDaiquiriBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description = "短时间内不消耗子弹，大幅增加远程伤害与暴击\n“低体温血压”"
                    }
                },
                 //血腥麝香葡萄
                {
                    ItemID.BloodyMoscato,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<BloodyMoscatoBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内每次攻击回5点生命\n血色"
                    }
                },
                //奶油苏打水
                {
                    ItemID.CreamSoda,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<CreamSodaBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内大幅增加近战能力，但极其吸引仇恨\n“喷射！”"
                    }
                },
                //咖啡
                {
                    ItemID.CoffeeCup,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<CoffeeCupBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内大幅增加铺墙铺砖速度，高亮标记敌人、陷阱和宝藏，你会散发光芒\n“社畜的宝物”"
                    }
                },
                //果汁
                {
                    ItemID.FruitJuice,
                    new DrinkInfo() {
                       Thirsty = false ,
                       BuffType = ModContent.BuffType<FruitJuiceBuff>(),
                       BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内幅大大提升大部分属性\n“维生素！”"
                    }
                },
                //葡萄汁
                {
                    ItemID.GrapeJuice,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<GrapeJuiceBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间极其幸运\n“浓缩的幸福”"
                    }
                },                
                //柠檬水
                {
                    ItemID.Lemonade,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<LemonadeBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内远程击退加倍,仇恨值减2400\n“消炎美容”"
                    }
                },     
                //盒装牛奶
                {
                    ItemID.MilkCarton,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<MilkCartonBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内免疫几乎所有debuff\n“一奶解百毒 ”"
                    }
                },
                //奶昔
                {
                    ItemID.Milkshake,
                    new DrinkInfo() {
                        Thirsty = false ,
                        BuffType = ModContent.BuffType<MilkshakeBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内大幅加移速\n“节食减肥 ”"
                    }
                },
                //桃子果酒
                {
                    ItemID.PeachSangria,
                    new DrinkInfo() {
                        Thirsty = false,
                        BuffType = ModContent.BuffType<PeachSangriaBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内大幅回复生命，增加心的拾取范围\n“我也是桃饱用户”"
                    }
                },
                //椰林飘香
                {
                    ItemID.PinaColada,
                    new DrinkInfo() {
                        Thirsty = false,
                        BuffType = ModContent.BuffType<PinaColadaBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内十倍反伤（上限1000）,防御增加20\n“从不添加香精当生榨 。”"
                    }
                },
                //七彩潘趣酒
                {
                    ItemID.PrismaticPunch,
                    new DrinkInfo() {
                        Thirsty = false,
                        BuffType = ModContent.BuffType<PrismaticPunchBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内增加10召唤栏，增加50%召唤伤害，击退\n“高雅兴致”"
                    }
                },                
                //清酒
                {
                    ItemID.Sake,
                    new DrinkInfo() {
                        Thirsty = false,
                        BuffType = ModContent.BuffType<SakeBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内减18防御，大幅增加近战能力\n“纯度，太高了。”"
                    }
                },
                //暗黑奶昔
                {
                    ItemID.SmoothieofDarkness,
                    new DrinkInfo() {
                        Thirsty = false,
                        BuffType = ModContent.
                        BuffType<SmoothieofDarknessBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内80%闪避\n“腹黑（字面意义上）”"
                    }
                }, 
                //一杯茶
                {
                    ItemID.Teacup,
                    new DrinkInfo() {
                        Thirsty = false,
                        BuffType = ModContent.BuffType<TeacupBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内大幅增加魔力回复，魔法攻击，暴击\n“醇香四溢”"
                    }
                },
                //热带奶昔
                {
                    ItemID.TropicalSmoothie,
                    new DrinkInfo() {
                        Thirsty = false,
                        BuffType = ModContent.BuffType<TropicalSmoothieBuff>(),
                        BuffTime = new FoodDuration(0, 7, 30),
                        Description ="短时间内魔法攻击仅消耗一点魔力，大幅增加5%魔法攻击，暴击\n“热带风暴”"
                    }
                }
            };
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (m_vanillaDrinkInfos.ContainsKey(item.type))
            {
                int firstIndex = -1;
                firstIndex = tooltips.FindIndex((tpline) =>
                {
                    return tpline.Name.Contains("Tooltip");
                });
                // 如果有tooltip，就删掉所有Tooltip的line然后插入到第一个所在位置
                var DrinkInfo = m_vanillaDrinkInfos[item.type];
                if (firstIndex >= 0)
                {
                    tooltips.RemoveAll((tp) => tp.Name.Contains("Tooltip"));
                    tooltips.Insert(firstIndex, new TooltipLine(Mod, item.Name, DrinkInfo.Description));
                }
                else
                {
                    // 否则加到最后面
                    tooltips.Add(new TooltipLine(Mod, item.Name, DrinkInfo.Description));
                }

                int buffTimeIndex = tooltips.FindIndex((tp) => tp.Name.Contains("BuffTime"));
                if (buffTimeIndex != -1)
                {
                    tooltips[buffTimeIndex].Text = DrinkInfo.BuffTime.ToBuffTimeString();
                }
            }
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
            // 如果是原版的饮料，那么就手动处理，因为已经使用了物品，说明玩家满足饱食度要求
            if (m_vanillaDrinkInfos.ContainsKey(item.type))
            {
                var drinkInfo = m_vanillaDrinkInfos[item.type];
                var foodPlayer = player.GetModPlayer<FoodModPlayer>();

                // 变得不渴
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
