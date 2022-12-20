using Everglow.Sources.Modules.FoodModule.Buffs.VanillaFoodBuffs;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;
using Everglow.Sources.Modules.MythModule;
using Terraria.DataStructures;
using Terraria.Audio;
using Everglow.Resources.ItemList.Weapons.Ranged;
using Everglow.Resources.NPCList.EventNPCs;
using Mono.Cecil;
using Everglow.Sources.Modules.FoodModule.Buffs.ModDrinkBuffs;
using Everglow.Sources.Modules.FoodModule.Buffs.ModFoodBuffs;

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
        public int BloodyMoscatoHealCount;
        public bool FriedEggBuff;
        public bool CherryBuff;
        public bool PiercoldWindBuff;
        public bool PurpleHooterBuff;
        public bool QuinceMarryBuff;
        public bool RedWineBuff;
        public bool SunriseBuff;
        public bool TricolourBuff;
        public bool BlueHawaiiBuff;
        public bool DreamYearningBuff;
        public bool KiwiJuiceBuff;
        public bool KiwiFruitBuff;
        public bool KiwiIceCreamBuff;
        public bool MangosteenBuff;
        public bool DurianBuff;
        public bool StinkyTofuBuff;
        public bool StrawberryBuff;
        public bool StrawberryIcecreamBuff;
        public bool CaramelPuddingBuff;
        public bool CantaloupeJellyBuff;
        public bool GreenStormBuff;

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
            BloodyMoscatoHealCount = 0;
            FriedEggBuff = false;
            CherryBuff = false;
            PiercoldWindBuff = false;
            PurpleHooterBuff = false;
            QuinceMarryBuff = false;
            RedWineBuff = false;
            SunriseBuff = false;
            TricolourBuff = false;
            BlueHawaiiBuff = false;
            DreamYearningBuff = false;
            KiwiJuiceBuff = false;
            KiwiFruitBuff = false;
            KiwiIceCreamBuff = false;
            MangosteenBuff = false;
            DurianBuff = false;
            StinkyTofuBuff = false;
            StrawberryBuff = false;
            StrawberryIcecreamBuff = false;
            CaramelPuddingBuff = false;
            CantaloupeJellyBuff = false;
            GreenStormBuff = false;

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
            BloodyMoscatoHealCount = Math.Max(--BloodyMoscatoHealCount, 0);
            FriedEggBuff = false;
            CherryBuff = false;
            PiercoldWindBuff = false;
            PurpleHooterBuff = false;
            QuinceMarryBuff = false;
            RedWineBuff = false;
            SunriseBuff = false;
            TricolourBuff = false;
            BlueHawaiiBuff = false;
            DreamYearningBuff = false;
            KiwiJuiceBuff = false;
            KiwiFruitBuff = false;
            KiwiIceCreamBuff = false;
            MangosteenBuff = false;
            DurianBuff = false;
            StinkyTofuBuff = false;
            StrawberryBuff = false;
            StrawberryIcecreamBuff = false;
            CaramelPuddingBuff = false;
            CantaloupeJellyBuff = false;
            GreenStormBuff = false;

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
            if (Player.whoAmI == Main.myPlayer && SmoothieofDarknessBuff && Main.rand.NextBool(2))
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

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (DragonfruitBuff)
            {
                target.AddBuff(BuffID.Oiled, 600);
                target.AddBuff(BuffID.OnFire, 600);
            }
            if (BloodyMoscatoBuff && BloodyMoscatoHealCount <= 60)
            {
                Player.HealEffect(2, true);
                Player.statLife += 2;
                BloodyMoscatoHealCount += 10;
            }
            base.OnHitNPC(item, target, damage, knockback, crit);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {

            if (DragonfruitBuff)
            {
                target.AddBuff(BuffID.Oiled, 600);
                target.AddBuff(BuffID.OnFire, 600);
            }
            if (BloodyMoscatoBuff && BloodyMoscatoHealCount <= 60)
            {
                Player.HealEffect(2, true);
                Player.statLife += 2;
                BloodyMoscatoHealCount += 10;
            }
            base.OnHitNPCWithProj(proj, target, damage, knockback, crit);
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (CherryBuff)
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Player.Center);
                ScreenShaker Gsplayer = Player.GetModPlayer<ScreenShaker>();
                Gsplayer.FlyCamPosition = new Vector2(0, 150).RotatedByRandom(6.283);
                Projectile.NewProjectile(Player.GetSource_Death("CherryBuff"), Player.Center, Vector2.Zero, ModContent.ProjectileType<BombShakeWave>(), 0, 0, Player.whoAmI, 0.4f, 2f);
                float k1 = Math.Clamp(Player.velocity.Length(), 1, 3);
                float k2 = Math.Clamp(Player.velocity.Length(), 6, 10);
                float k0 = 1f / 4 * k2;
                for (int j = 0; j < 8 * k0; j++)
                {
                    Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * k1;
                    int dust0 = Dust.NewDust(Player.Center, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), v0.X / 10, v0.Y / 10, 100, default(Color), Main.rand.NextFloat(0.6f, 1.8f) * 2);
                    Main.dust[dust0].noGravity = true;
                }
                for (int j = 0; j < 16 * k0; j++)
                {
                    Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * k1;
                    int dust1 = Dust.NewDust(Player.Center, 0, 0, ModContent.DustType<BlueParticleDark2StoppedByTile>(), v0.X / 10, v0.Y / 10, 100, default(Color), Main.rand.NextFloat(3.7f, 5.1f) * 2);
                    Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50 / k0);
                    Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
                }
                for (int j = 0; j < 16 * k0; j++)
                {
                    Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * k1;
                    int dust1 = Dust.NewDust(Player.Center, 0, 0, ModContent.DustType<MothSmog>(), v0.X / 10, v0.Y / 10, 100, default(Color), Main.rand.NextFloat(3.7f, 5.1f) * 2);
                    Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50 / k0);
                    Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
                }
                foreach (NPC target in Main.npc)
                {
                    float Dis = (target.Center - Player.Center).Length();

                    if (Dis < 250)
                    {
                        if (!target.dontTakeDamage && !target.friendly && target.active)
                        {
                            Player.ApplyDamageToNPC(target, Math.Max(Player.HeldItem.damage * 5, 120), Math.Max(Player.HeldItem.knockBack * 5, 24), 0, Main.rand.NextBool(22, 33));
                        }
                    }
                }
            }
            base.Kill(damage, hitDirection, pvp, damageSource);
        }

        public override void UpdateBadLifeRegen()
        {
            if (GrubSoupBuff)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= MangoBuff ? 2 : 4;
            }
            if (MonsterLasagnaBuff)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= MangoBuff ? 3 : 6;
            }
            if (SashimiBuff)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= MangoBuff ? 2 : 4;
            }
            if (ShuckedOysterBuff)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= MangoBuff ? 2 : 4;
            }
        }
    }
}
