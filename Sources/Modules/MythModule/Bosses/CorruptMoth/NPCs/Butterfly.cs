using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.NPCs
{
    public class Butterfly : ModNPC
    {
        public Vector2 targetPos;
        private int Timer
        {
            get => (int)NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        private NPC Owner => Main.npc[(int)NPC.ai[3]];
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("幻蝶");
            Main.npcFrameCount[base.NPC.type] = 3;
        }
        public override void SetDefaults()
        {
            base.NPC.width = 24;
            base.NPC.height = 24;
            base.NPC.friendly = false;
            base.NPC.noGravity = true;
            base.NPC.noTileCollide = false;
            base.NPC.lifeMax = 1;
            base.NPC.aiStyle = -1;
            base.NPC.damage = 20;
            base.NPC.dontTakeDamageFromHostiles = true;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        private void CheckOwnerActive()
        {
            if (Owner.active && Owner.type == ModContent.NPCType<CorruptMoth>())
            {

            }
            else
            {
                NPC.active = false;
                NPC.life = 0;
            }
        }
        public override void AI()
        {

            if (NPC.ai[0] == -2)
            {
                base.NPC.spriteDirection = Math.Sign(base.NPC.velocity.X);
                NPC.dontTakeDamage = false;
                Timer++;
                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 2;
                }

                NPC.ai[2] += 0.1f;
                if (Vector2.Distance(NPC.Center, Owner.Center) > 80)
                {
                    MoveTo(Owner.Center + NPC.ai[2].ToRotationVector2() * 200, 20, 20);
                }

                NPC.friendly = false;
                CheckOwnerActive();
            }
            if (NPC.ai[0] == -1)
            {
                base.NPC.spriteDirection = Math.Sign(base.NPC.velocity.X);
                NPC.dontTakeDamage = true;
                Timer++;
                if (NPC.alpha < 120)
                {
                    NPC.alpha += 2;
                }

                if (Vector2.Distance(NPC.Center, Owner.Center) > 300)
                {
                    MoveTo(Owner.Center + Main.rand.NextVector2Unit() * 100, 10, 20);
                    NPC.netUpdate2 = true;
                }

                NPC.friendly = true;
                CheckOwnerActive();


            }//在boss附近游荡
            if (NPC.ai[0] == 0)
            {
                base.NPC.spriteDirection = Math.Sign(base.NPC.velocity.X);
                base.NPC.TargetClosest(false);
                Player player = Main.player[base.NPC.target];
                if (Timer < 30)
                {
                    NPC.dontTakeDamage = true;
                }
                else
                {
                    NPC.dontTakeDamage = false;
                }

                if (Timer == 0)
                {
                    NPC.ai[2] = Main.rand.Next(60, 200);
                    base.NPC.frame.Y = Main.rand.Next(2) * 24;
                    NPC.netUpdate2 = true;
                }
                if (++Timer > NPC.ai[2] && Timer < NPC.ai[2] + 350)//追踪玩家
                {
                    base.NPC.TargetClosest(false);
                    MoveTo(player.Center, 8, 80);
                }
                if (Timer > NPC.ai[2] + 600)
                {
                    NPC.scale -= 0.05f;
                    if (NPC.scale < 0)
                    {
                        NPC.HitEffect(1);
                        NPC.active = false;
                    }
                }
            }//延迟后朝玩家运动
            if (NPC.ai[0] == 1)//弓,rot:ai2
            {
                base.NPC.spriteDirection = Math.Sign(base.NPC.velocity.X);
                Vector2 trueTargetPos = Owner.Center + targetPos.RotatedBy(NPC.ai[2]);
                CheckOwnerActive();

                Player player = Main.player[Owner.target];
                if (Timer == 0)
                {
                    NPC.dontTakeDamage = true;
                    NPC.ai[2] = -1.57f;
                    NPC.velocity = Vector2.Zero;
                }
                if (++Timer < 60)
                {
                    NPC.alpha += 2;
                    NPC.friendly = true;
                    MoveTo(trueTargetPos, 10, 40);
                }
                if (Timer > 60 && Timer < 120)
                {
                    NPC.alpha -= 2;
                    NPC.ai[2] = Utils.AngleLerp(NPC.ai[2], Owner.DirectionTo(player.Center).ToRotation(), 0.1f);
                    NPC.Center = Vector2.Lerp(NPC.Center, trueTargetPos, 0.1f);
                }
                if (Timer > 120)
                {
                    NPC.friendly = false;
                    NPC.ai[2] = Utils.AngleLerp(NPC.ai[2], Owner.DirectionTo(player.Center + player.velocity * 20).ToRotation(), 0.15f);
                    NPC.Center = Vector2.Lerp(NPC.Center, trueTargetPos, 0.4f);
                }

            }
            if (NPC.ai[0] == 2)//箭,rot:ai2，distance:localAI0
            {
                CheckOwnerActive();

                base.NPC.spriteDirection = Math.Sign(base.NPC.velocity.X);
                Vector2 trueTargetPos = Owner.Center + targetPos.RotatedBy(NPC.ai[2]);
                Player player = Main.player[Owner.target];
                if (Timer == 0)
                {
                    NPC.dontTakeDamage = true;
                    NPC.ai[2] = -1.57f;
                    NPC.localAI[0] = 0;
                    NPC.velocity = Vector2.Zero;
                }
                if (++Timer < 60)
                {
                    NPC.alpha += 2;
                    NPC.friendly = true;
                    MoveTo(trueTargetPos, 10, 40);
                }
                if (Timer > 60 && Timer < 120)
                {
                    NPC.alpha -= 2;
                    NPC.ai[2] = Utils.AngleLerp(NPC.ai[2], Owner.DirectionTo(player.Center).ToRotation(), 0.1f);
                    NPC.Center = Vector2.Lerp(NPC.Center, trueTargetPos, 0.1f);
                }
                if (Timer > 120 && Timer < 160)
                {
                    NPC.friendly = false;
                    Vector2 d = Owner.DirectionTo(player.Center + player.velocity * 20);
                    NPC.ai[2] = Utils.AngleLerp(NPC.ai[2], d.ToRotation(), 0.2f);
                    NPC.Center = Vector2.Lerp(NPC.Center, trueTargetPos, 0.4f) + d * NPC.localAI[0];
                    NPC.localAI[0] = MathHelper.Lerp(NPC.localAI[0], -60, 0.05f);
                }
                if (Timer == 160)
                {
                    NPC.velocity = NPC.ai[2].ToRotationVector2() * 30;
                }
                if (Timer == 220)
                {
                    NPC.velocity *= 0.5f;
                    NPC.velocity += Main.rand.NextVector2Unit() * 10;
                    NPC.friendly = true;
                    NPC.netUpdate2 = true;
                }
                if (Timer > 240)
                {
                    Timer = 0;
                }
            }
            if (NPC.ai[0] == 3)//剑
            {
                CheckOwnerActive();

                Vector2 trueTargetPos = Owner.Center + targetPos.RotatedBy(NPC.ai[2]);
                if (Timer == 0)
                {
                    NPC.friendly = false;
                    NPC.dontTakeDamage = true;
                    NPC.ai[2] = -1.57f;
                    NPC.velocity = Vector2.Zero;
                }
                if (++Timer < 60)
                {
                    NPC.alpha = (int)MathHelper.Lerp(NPC.alpha, 120, 0.1f);
                    NPC.friendly = true;
                    MoveTo(trueTargetPos, 10, 40);
                    NPC.localAI[0] = Owner.spriteDirection * -1;
                }
                if (Timer > 60 && Timer < 120)
                {
                    NPC.alpha = (int)MathHelper.Lerp(NPC.alpha, 0, 0.1f);
                    NPC.ai[2] = Utils.AngleLerp(NPC.ai[2], -1.57f, 0.1f);
                    NPC.Center = Vector2.Lerp(NPC.Center, trueTargetPos, 0.15f);
                }
                if (Timer > 120 && Timer < 180)
                {
                    NPC.alpha = (int)MathHelper.Lerp(NPC.alpha, 0, 0.1f);
                    NPC.ai[2] = Utils.AngleLerp(NPC.ai[2], -1.57f + NPC.localAI[0] * 1f, 0.05f);
                    NPC.Center = Vector2.Lerp(NPC.Center, trueTargetPos, 0.5f);
                }
                if (Timer > 180 && Timer < 220)
                {
                    NPC.ai[2] -= NPC.localAI[0] * 0.12f;
                    NPC.Center = Vector2.Lerp(NPC.Center, trueTargetPos, 0.5f);
                }
                if (Timer == 220)
                {
                    NPC.velocity = Main.rand.NextVector2Unit() * 10f;
                }
                if (Timer > 250)
                {
                    NPC.ai[0] = -1;
                    Timer = 0;
                }

            }
            if (NPC.ai[0] == 4)//拳
            {
                CheckOwnerActive();

                base.NPC.spriteDirection = Math.Sign(base.NPC.velocity.X);
                Vector2 trueTargetPos = Owner.Center + targetPos.RotatedBy(NPC.ai[2]);
                Player player = Main.player[Owner.target];
                if (Timer == 0)
                {
                    NPC.dontTakeDamage = true;
                    NPC.ai[2] = 0f;
                    NPC.localAI[0] = 0;
                    NPC.velocity = Vector2.Zero;
                }
                if (++Timer < 60)
                {
                    NPC.alpha += 2;
                    NPC.friendly = true;
                    MoveTo(trueTargetPos, 10, 40);
                }
                if (Timer > 60 && Timer < 120)
                {
                    NPC.alpha -= 2;
                    NPC.ai[2] = Utils.AngleLerp(NPC.ai[2], Owner.DirectionTo(player.Center).ToRotation(), 0.1f);
                    NPC.Center = Vector2.Lerp(NPC.Center, trueTargetPos, 0.1f);
                }
                if (Timer > 120 && Timer < 160)
                {
                    NPC.friendly = false;
                    Vector2 d = Owner.DirectionTo(player.Center);
                    NPC.ai[2] = Utils.AngleLerp(NPC.ai[2], d.ToRotation(), 0.2f);
                    NPC.Center = Vector2.Lerp(NPC.Center, trueTargetPos, 0.4f) + d * NPC.localAI[0];
                    NPC.localAI[0] = MathHelper.Lerp(NPC.localAI[0], -70, 0.05f);
                }
                if (Timer == 160)
                {
                    NPC.velocity = NPC.ai[2].ToRotationVector2() * 40;
                }
                if (Timer == 200)
                {
                    NPC.velocity = Main.rand.NextVector2Unit() * Main.rand.Next(2, 10);
                    Timer = 0;
                    NPC.ai[0] = 0;
                    NPC.dontTakeDamage = false;
                    NPC.netUpdate2 = true;
                }
            }

            if (NPC.ai[0] == 1 || NPC.ai[0] == 3 || NPC.ai[0] == 4)
            {
                NPC.noTileCollide = true;
            }
            else
            {
                NPC.noTileCollide = false;
            }
        }
        private void MoveTo(Vector2 targetPos, float Speed, float n)
        {
            Vector2 targetVec = Utils.SafeNormalize(targetPos - NPC.Center, Vector2.Zero) * Speed;
            NPC.velocity = (NPC.velocity * n + targetVec) / (n + 1);
        }
        public override void FindFrame(int frameHeight)
        {
            if (base.NPC.frame.Y > 48)
            {
                base.NPC.frame.Y = 0;
            }

            if (Timer % 10 == 0)
            {
                base.NPC.frame.Y += 24;
            }

            if (Timer % 3 == 0 && NPC.alpha < 20)
            {
                int index = Dust.NewDust(base.NPC.position - new Vector2(8), base.NPC.width, base.NPC.height, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.7f, 1.9f));
                Main.dust[index].velocity = base.NPC.velocity * 0.5f;
            }

        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 6; i++)
            {
                int index = Dust.NewDust(base.NPC.position - new Vector2(8), base.NPC.width, base.NPC.height, DustID.Electric, 0f, 0f, 100, Color.Blue, Main.rand.NextFloat(0.7f, 1.2f));
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(5f, 10f)).RotatedByRandom(6.283);
                Main.dust[index].noGravity = true;
            }
            base.HitEffect(hitDirection, damage);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 0) * (1 - NPC.alpha / 255f);
        }
    }
}
