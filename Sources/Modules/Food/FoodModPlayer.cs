using Terraria;
using Terraria.ModLoader;
using System;

namespace Everglow.Sources.Modules.Food
{
    public class FoodModPlayer : ModPlayer
    {
    
        /// <summary>
        /// 玩家当前饱食度
        /// </summary>
        public int CurrentSatiety { get; set; }

        /// <summary>
        /// 玩家最大饱食度
        /// </summary>
        public int MaximumSatiety { get; set; }

        public FoodModPlayer()
        {
            CurrentSatiety = 0;
            MaximumSatiety = 50;
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
        /*







         */
        public bool BananaBuff;
        public bool BananaDaiquiriBuff;
        public bool BananaSplitBuff;
        public bool DragonfruitBuff;
        public bool GoldenDelightBuff;
        public bool SmoothieofDarknessBuff;
        public bool GrubSoupBuff;
        public bool MonsterLasagnaBuff;
        public bool SashimiBuff;
        public bool ShuckedOysterBuff;
        public bool MangoBuff;
        public bool StarfruitBuff;
        public bool NachosBuff;


        public override void ResetEffects()
        {
            BananaBuff = false;
            BananaDaiquiriBuff = false;
            BananaSplitBuff = false;
            DragonfruitBuff = false;
            GoldenDelightBuff = false;
            SmoothieofDarknessBuff = false;
            GrubSoupBuff = false;
            MonsterLasagnaBuff = false;
            SashimiBuff = false;
            ShuckedOysterBuff = false;
            MangoBuff = false;
            StarfruitBuff = false;
            NachosBuff = false;
        }
        
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            
            if (BananaBuff && Main.rand.Next(5) == 0)
            {
                return false;
            }
            if (BananaDaiquiriBuff)
            {
                return false;
            }
            if (BananaSplitBuff && Main.rand.Next(3) == 0)
            {
                return false;
            }
            return true;
        }
        
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (Player.whoAmI == Main.myPlayer && SmoothieofDarknessBuff && Main.rand.Next(5) != 0)
            {
                Player.NinjaDodge();
            }
        }
        

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        { 
            if (DragonfruitBuff)
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
            if (NachosBuff)
            {
                target.AddBuff(BuffID.OnFire, 180);
                target.AddBuff(BuffID.CursedInferno , 180);
                target.AddBuff(BuffID.ShadowFlame , 180);
                target.AddBuff(BuffID.Frostburn , 180);
                target.AddBuff(BuffID.Oiled, 180);
            }

        }
        public override void UpdateBadLifeRegen()
        {
            if (GrubSoupBuff)
            {
                if (MangoBuff)
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 2;
                }
                else
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 4;
                }
                
            }
            if (MonsterLasagnaBuff)
            {
                if (MangoBuff)
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 4;
                }
                else
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 10;
                }
                
            }
            if (SashimiBuff)
            {
                if (MangoBuff)
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 2;
                }
                else
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 6;
                }
                
            }
            if (ShuckedOysterBuff)
            {
                if (MangoBuff)
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 2;
                }
                else
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 6;
                }
            }

        }

    }
}
