namespace Everglow.Sources.Modules.Food
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
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {

            if (BananaBuff && Main.rand.NextBool(5))
            {
                return false;
            }
            if (BananaDaiquiriBuff)
            {
                return false;
            }
            if (BananaSplitBuff && Main.rand.NextBool(3))
            {
                return false;
            }
            return true;
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (Player.whoAmI == Main.myPlayer && SmoothieofDarknessBuff && !Main.rand.NextBool(5))
            {
                Player.NinjaDodge();
            }
        }
        public override void PostUpdateBuffs()
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

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (DragonfruitBuff)
            {
                target.AddBuff(BuffID.Oiled, 600);
                target.AddBuff(BuffID.OnFire, 600);
            }
            if (NachosBuff)
            {
                target.AddBuff(BuffID.OnFire, 600);
                target.AddBuff(BuffID.CursedInferno, 600);
                target.AddBuff(BuffID.ShadowFlame, 600);
                target.AddBuff(BuffID.Frostburn, 600);
                target.AddBuff(BuffID.Oiled, 600);
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
            if (NachosBuff)
            {
                target.AddBuff(BuffID.OnFire, 600);
                target.AddBuff(BuffID.CursedInferno, 600);
                target.AddBuff(BuffID.ShadowFlame, 600);
                target.AddBuff(BuffID.Frostburn, 600);
                target.AddBuff(BuffID.Oiled, 600);
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
