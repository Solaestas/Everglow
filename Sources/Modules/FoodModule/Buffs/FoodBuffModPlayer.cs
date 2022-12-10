using Everglow.Sources.Modules.FoodModule.Buffs.VanillaFoodBuffs;

namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class FoodBuffModPlayer : ModPlayer
    {
        public float WingTimeModifier;

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
        public bool RoastedBirdBuff;
        public bool RoastedDuckBuff;
        public bool BloodyMoscatoBuff;
        public bool FriedEggBuff;

        public static float CritDamage;
        public static float AddCritDamage;

        public override void UpdateDead()
        {
            WingTimeModifier = 1f;

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
            RoastedBirdBuff = false;
            RoastedDuckBuff = false;
            BloodyMoscatoBuff = false;
            FriedEggBuff = false;

            CritDamage = 1f;
            AddCritDamage = 0;
    }
        public override void ResetEffects()
        {
            WingTimeModifier = 1f;

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
            RoastedBirdBuff = false;
            RoastedDuckBuff = false;
            BloodyMoscatoBuff = false;
            FriedEggBuff = false;

            CritDamage = 1f;
            AddCritDamage = 0;
        }

        public override void PostUpdate()
        {
            CritDamage += AddCritDamage;
            base.PostUpdate();
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {

            if (BananaBuff && Main.rand.NextBool(20))
            {
                return false;
            }
            if (BananaDaiquiriBuff)
            {
                return false;
            }
            if (BananaSplitBuff && Main.rand.NextBool(10))
            {
                return false;
            }
            return true;
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (Player.whoAmI == Main.myPlayer && SmoothieofDarknessBuff && !Main.rand.NextBool(5))
            {
                Player.NinjaDodge();
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (RoastedBirdBuff)
            {
                Player.wingTimeMax = (int)(Player.wingTimeMax * WingTimeModifier);
            }
            if (RoastedDuckBuff)
            {
                Player.wingTimeMax = (int)(Player.wingTimeMax * WingTimeModifier);
            }
        }
       /* public override void PostUpdateBuffs()
        {
            if (RoastedBirdBuff)
            {
                Player.wingTimeMax = (int)(Player.wingTimeMax * WingTimeModifier);
            }
            if (RoastedDuckBuff)
            {
                Player.wingTimeMax = (int)(Player.wingTimeMax * WingTimeModifier);
            }
        }*/

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (DragonfruitBuff)
            {
                target.AddBuff(BuffID.Oiled, 600);
                target.AddBuff(BuffID.OnFire, 600);
            }
            if (BloodyMoscatoBuff)
            {
                Player.HealEffect(5, true);
                Player.statLife += 5;
            }

        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {

            if (DragonfruitBuff)
            {
                target.AddBuff(BuffID.Oiled, 600);
                target.AddBuff(BuffID.OnFire, 600);
            }
            if (BloodyMoscatoBuff)
            {
                Player.HealEffect(5, true);
                Player.statLife += 5;
            }

        }
        public override void UpdateBadLifeRegen()
        {
            if (GrubSoupBuff)
            {
                if (MangoBuff)
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }

                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 2;
                }
                else
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }

                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 4;
                }

            }
            if (MonsterLasagnaBuff)
            {
                if (MangoBuff)
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }

                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 3;
                }
                else
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }

                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 6;
                }

            }
            if (SashimiBuff)
            {
                if (MangoBuff)
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }

                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 2;
                }
                else
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }

                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 4;
                }

            }
            if (ShuckedOysterBuff)
            {
                if (MangoBuff)
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }

                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 2;
                }
                else
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }

                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 6;
                }
            }

        }

    }
}
