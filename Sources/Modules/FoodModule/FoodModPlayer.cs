﻿using Everglow.Sources.Modules.FoodModule.Utils;
using Terraria.ModLoader.IO;
using Terraria;
using Everglow.Sources.Modules.FoodModule.Buffs;
using Everglow.Sources.Modules.FoodModule.Buffs.ModFoodBuffs;

namespace Everglow.Sources.Modules.FoodModule
{
    public class FoodModPlayer : ModPlayer
    {

        /// <summary>
        /// 玩家当前饱食度
        /// </summary>
        public int CurrentSatiety
        {
            get; set;
        }

        /// <summary>
        /// 玩家最大饱食度
        /// </summary>
        public int MaximumSatiety
        {
            get; set;
        }
        /// <summary>
        /// 玩家的饱食等级
        /// </summary>
        public int SatietyLevel { get; private set; }
        /// <summary>
        /// 玩家当前渴觉状态
        /// </summary>
        public bool Thirstystate
        {
            get; set;
        }
        public FoodModPlayer()
        {
        }

        /// <summary>
        /// 如果能吃下，返回true，否则为false
        /// </summary>
        /// <param name="foodInfo"></param>
        /// <returns></returns>
        public bool CanEat(FoodInfo foodInfo)
        {
            if (CurrentSatiety + foodInfo.Satiety <= MaximumSatiety)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 如果能喝下，返回true，否则为false
        /// </summary>
        public bool CanDrink(DrinkInfo drinkInfo)
        {
            if (Thirstystate)
            {
                return true;
            }
            return false;
        }

        public bool CanText()
        {
            if (TextTimer <= 0)
            {
                return true;
            }
            return false;
        }
        /*
         
         
         */
        /// <summary>
        /// 以下为计时器
        /// </summary>
        public int SatietyLossTimer
        {
            get; private set;
        }//饱食损失计时器
        public int ThirstyChangeTimer
        {
            get; private set;
        }//口渴变化计时器
        public int TextTimer
        {
            get; set;
        }
        public override void PostUpdateMiscEffects()
        {
            Player.buffImmune[BuffID.WellFed] = true;
            Player.buffImmune[BuffID.WellFed2] = true;
            Player.buffImmune[BuffID.WellFed3] = true;
            Player.buffImmune[BuffID.Tipsy] = true;
        }
        public override void PostUpdate()
        {
            FoodState();
            if (!Player.active)
            {
                CurrentSatiety = 0;
                SatietyLevel = 0;
                Thirstystate = true;
            }
        }
        public override void Initialize()
        {
            CurrentSatiety = 0;
            MaximumSatiety = 50;
            SatietyLevel = 0;
            SatietyLossTimer = 0;

            Thirstystate = true;
            ThirstyChangeTimer = 0;

            TextTimer = 0;

            base.Initialize();
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentSatiety", CurrentSatiety);
            tag.Add("Thirstystate", Thirstystate);
            base.SaveData(tag);
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("CurrentSatiety"))
            {
                CurrentSatiety = tag.GetInt("CurrentSatiety");
            }

            if (tag.ContainsKey("Thirstystate"))
            {
                Thirstystate = tag.GetBool("Thirstystate");
            }
        }

        public void FoodState()
        {
            //从吃食物后开始计时
            if (CurrentSatiety > 0)
            {
                SatietyLossTimer++;
            }
            //从喝饮料后开始计时
            if (!Thirstystate)
            {
                ThirstyChangeTimer++;
            }

            if (!CanText())
            {
                TextTimer--;
            }

            //每三十秒减少一饱食度
            if (Player.GetModPlayer<FoodBuffModPlayer>().DurianBuff)
            {
                if (SatietyLossTimer >= FoodUtils.GetFrames(0, 0, 15, 0))
                {
                    CurrentSatiety -= 1;
                    SatietyLossTimer = 0;
                }
            }
            else
            {

                if (SatietyLossTimer >= FoodUtils.GetFrames(0, 0, 30, 0))
                {
                    CurrentSatiety -= 1;
                    SatietyLossTimer = 0;
                }

            }

            if (CurrentSatiety <= 0)
            {
                CurrentSatiety = 0;
            }
            //每五分钟从口渴变得不口渴
            if (ThirstyChangeTimer >= FoodUtils.GetFrames(0, 5, 0, 0))
            {
                Thirstystate = true;
                ThirstyChangeTimer = 0;
            }
        }
        public override void PostUpdateBuffs()
        {
            if (CurrentSatiety > 0 || !Thirstystate)
            {
                Player.wellFed = true;
                if (CurrentSatiety <= MaximumSatiety * 0.5f) // well fed
                {
                    SatietyLevel = 1;
                    Player.statDefense += 1;
                    Player.GetCritChance(DamageClass.Generic) += 0.01f;
                    Player.GetDamage(DamageClass.Generic) += 0.02f;
                    Player.GetAttackSpeed(DamageClass.Generic) += 0.02f;
                    Player.GetKnockback(DamageClass.Summon) += 0.25f;
                    Player.moveSpeed += 0.1f;
                    Player.pickSpeed -= 0.1f;
                }
                else if (CurrentSatiety > MaximumSatiety * 0.5f && CurrentSatiety <= MaximumSatiety * 0.75f) // plently satisfied
                {
                    SatietyLevel = 2;
                    Player.statDefense += 2;
                    Player.GetCritChance(DamageClass.Generic) += 0.02f;
                    Player.GetDamage(DamageClass.Generic) += 0.04f;
                    Player.GetAttackSpeed(DamageClass.Generic) += 0.04f;
                    Player.GetKnockback(DamageClass.Summon) += 0.5f;
                    Player.moveSpeed += 0.05f;
                    Player.pickSpeed -= 0.15f;
                }
                else // exquisitely stuffed
                {
                    SatietyLevel = 3;
                    Player.statDefense += 4;
                    Player.GetDamage(DamageClass.Generic) += 0.04f;
                    Player.GetKnockback(DamageClass.Summon) += 0.75f;
                    Player.pickSpeed -= 0.1f;
                }
            }
            if (CurrentSatiety <= 0)
            {
                SatietyLevel = 0;
            }
        }
    }
}
