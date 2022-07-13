using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class GlowMoth : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        private int spawn = 60;
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 34;
            Projectile.tileCollide = false; // Makes the minion go through tiles freely
            Projectile.hostile = false;
            Projectile.timeLeft = 720;
            Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)							  
            Projectile.minionSlots = 1; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        private Vector2 Targ = Vector2.Zero;
        private int addi = 0;
        private float[] OldRo = new float[12];
        private int[] OldFr = new int[12];
        public override bool PreAI()
        {
            addi++;
            if ((addi + (int)Projectile.ai[1]) % 4 == 0)
            {
                if (Projectile.frame < 3)
                {
                    Projectile.frame++;
                }
                else
                {
                    Projectile.frame = 0;

                }
            }
            OldRo[0] = Projectile.rotation;
            for (int i = 11; i > 0; i--)
            {
                OldRo[i] = OldRo[i - 1];
            }
            OldFr[0] = Projectile.frame;
            for (int i = 11; i > 0; i--)
            {
                OldFr[i] = OldFr[i - 1];
            }
            if (spawn == 60)
            {
                if (Main.rand.NextFloat(0, 10f) > 5)
                {
                    Projectile.spriteDirection = -1;
                }
                else
                {
                    Projectile.spriteDirection = 1;
                }
            }
            if (spawn > 0)
            {
                Projectile.velocity *= 0.92f;
                spawn--;
                return false;
            }
            else
            {
                return true;
            }
        }

        private float FakeAI0 = -1;
        public override void AI()
        {
            if (FakeAI0 == -1)
            {
                FakeAI0 = Main.rand.NextFloat(0, 200f);
            }
            if (cooling > 0)
            {
                cooling--;
                Projectile.friendly = false;
            }
            else
            {
                cooling = 0;
                Projectile.friendly = true;
            }
            if (Math.Abs(Projectile.velocity.X) > 0.3f)
            {
                Projectile.spriteDirection = Projectile.velocity.X > 0 ? -1 : 1;
            }
            bool flag = false;
            Player player = Main.player[Projectile.owner];
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    Vector2 v0 = Main.npc[j].Center;
                    Vector2 v1 = Projectile.Center;

                    if ((v0 - v1).Length() < 600)
                    {
                        flag = true;
                        Targ = v0;

                        break;
                    }
                }
                else
                {
                    Targ = player.Center + new Vector2((float)Math.Sin(Main.time / 9d + FakeAI0) * 85f, (float)Math.Sin(Main.time / 6d + Projectile.ai[1]) * 25f).RotatedBy(Projectile.ai[1]) + new Vector2(-50 * player.direction, -32);
                }
            }
            if (flag && (addi + (int)Projectile.ai[1]) % 20 >= 10)
            {
                Vector2 v2 = Targ - Projectile.Center;
                Projectile.velocity = (Projectile.velocity * 10f + v2 / v2.Length() * 9f) / 11f;
            }
            if (!flag)
            {
                Vector2 v2 = Targ - Projectile.Center;
                Projectile.velocity = (Projectile.velocity * 10f + v2 / v2.Length() * 5f) / 11f;
            }
            produceDust();
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.GlowMothBuff>());
                Projectile.Kill();
            }

            if (player.HasBuff(ModContent.BuffType<Buffs.GlowMothBuff>()))
            {
                Projectile.timeLeft = 2;
            }
            else
            {
                Projectile.Kill();
            }
            float CritC = 0;
            if (cooling > 0)
            {
                return;
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 15 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly)
                {
                    cooling = 4;
                    Main.npc[j].StrikeNPC((int)(Projectile.ai[0] * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < CritC);
                    player.addDPS((int)(Math.Clamp((Projectile.ai[0] - Main.npc[j].defense), 1, 2147483647) * (1 + CritC / 100f)));
                }
            }
        }
        void produceDust()
        {
            if (addi % 144 == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<MothBlue>(), Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0.3f, 1.4f)).RotatedByRandom(Math.PI);

                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 191, v.X + Projectile.velocity.X, v.Y + Projectile.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.3f, 0.7f));
            }
            if (addi % 144 == 72)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<MothBlue2>(), Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0.3f, 1.4f)).RotatedByRandom(Math.PI);

                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 191, v.X + Projectile.velocity.X, v.Y + Projectile.velocity.Y, 0, default(Color), Main.rand.NextFloat(0.3f, 0.7f));
            }
        }
        private int cooling = 0;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            cooling = 4;
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft != 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 191, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default(Color), 0.6f);
                }
            }
        }
        public override void PostDraw(Color lightColor)
        {
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
                Vector2 DrawPos = Projectile.oldPos[i] + new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                Color c0 = Lighting.GetColor((int)(DrawPos.X / 16f), (int)(DrawPos.Y / 16f));
                SpriteEffects sf = SpriteEffects.None;
                if (Projectile.spriteDirection == -1)
                {
                    sf = SpriteEffects.FlipHorizontally;
                }
                Main.spriteBatch.Draw(texture, DrawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle(0, OldFr[i] * Projectile.height, Projectile.width, Projectile.height), new Color((int)(c0.R * (Projectile.oldPos.Length - i) / 10f), (int)(c0.G * (Projectile.oldPos.Length - i) / 10f), (int)(c0.B * (Projectile.oldPos.Length - i) / 10f), (int)(255 * (Projectile.oldPos.Length - i) / 10f)), OldRo[i], new Vector2(texture.Width / 2f, texture.Height / 8f), Projectile.scale, sf, 0);
            }
        }
    }
}
