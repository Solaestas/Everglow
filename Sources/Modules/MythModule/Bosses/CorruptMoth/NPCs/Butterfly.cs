namespace Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.NPCs
{
    public class Butterfly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("幻蝶");
            Main.npcFrameCount[NPC.type] = 3;
        }
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 24;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.lifeMax = 1;
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.dontTakeDamageFromHostiles = true;
        }
        NPC npc => NPC;
        int t
        {
            get =>(int) npc.ai[1];
            set => npc.ai[1] = value;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public Vector2 TargetPos;
        NPC owner => Main.npc[(int)npc.ai[3]];
        public override void AI()
        {
            if (npc.ai[0] == -2)
            {
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);
                npc.dontTakeDamage = false;
                t++;
                if (npc.alpha >0)
                    npc.alpha -= 2;
                npc.ai[2] += 0.1f;
                if (Vector2.Distance(npc.Center, owner.Center) > 80)
                    MoveTo(owner.Center + npc.ai[2].ToRotationVector2() * 200, 20, 20);
                npc.friendly = false;
            }
            if (npc.ai[0] == -1)
            {
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);
                npc.dontTakeDamage = true;
                t++;
                if (npc.alpha<120)
                    npc.alpha += 2;
                if(Vector2.Distance(npc.Center,owner.Center)>300)
                    MoveTo(owner.Center+Main.rand.NextVector2Unit()*100, 10, 20);
                npc.friendly = true;

            }//在boss附近游荡
            if (npc.ai[0] == 0)
            {
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);
                NPC.TargetClosest(false);
                Player player = Main.player[NPC.target];
                if (t < 30)
                    npc.dontTakeDamage = true;
                else
                    npc.dontTakeDamage = false;
                if (t == 0)
                {
                    npc.ai[2] = Main.rand.Next(60, 200);
                    NPC.frame.Y = Main.rand.Next(2) * 24;
                }
                if (++t > npc.ai[2] && t < npc.ai[2] + 350)//追踪玩家
                {
                    NPC.TargetClosest(false);
                    MoveTo(player.Center, 8, 80);
                }
                if (t > npc.ai[2] + 600)
                {
                    npc.scale -= 0.05f;
                    if (npc.scale < 0)
                    {
                        npc.HitEffect(1);
                        npc.active = false;
                    }
                }
            }//延迟后朝玩家运动
            if (npc.ai[0] == 1)//弓,rot:ai2
            {
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);
                Vector2 trueTargetPos = owner.Center + TargetPos.RotatedBy(npc.ai[2]);
                Player player= Main.player[owner.target];
                if (t == 0)
                {
                    npc.dontTakeDamage = true;
                    npc.ai[2] = -1.57f;
                    npc.velocity = Vector2.Zero;
                }
                if (++t < 60)
                {
                    npc.alpha += 2;
                    npc.friendly = true;
                    MoveTo(trueTargetPos, 10, 40);
                }
                if (t > 60 && t < 120)
                {
                    npc.alpha -= 2;
                    npc.ai[2] = Utils.AngleLerp(npc.ai[2], owner.DirectionTo(player.Center).ToRotation(),0.1f);
                    npc.Center = Vector2.Lerp(npc.Center, trueTargetPos, 0.1f);
                }
                if (t > 120)
                {
                    npc.friendly = false;
                    npc.ai[2] = Utils.AngleLerp(npc.ai[2], owner.DirectionTo(player.Center + player.velocity * 20).ToRotation(), 0.15f);
                    npc.Center = Vector2.Lerp(npc.Center, trueTargetPos, 0.4f);
                }

            }
            if (npc.ai[0] == 2)//箭,rot:ai2，distance:localAI0
            {
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);
                Vector2 trueTargetPos = owner.Center + TargetPos.RotatedBy(npc.ai[2]);
                Player player = Main.player[owner.target];
                if (t == 0)
                {
                    npc.dontTakeDamage = true;
                    npc.ai[2] = -1.57f;
                    npc.localAI[0] = 0;
                    npc.velocity = Vector2.Zero;
                }
                if (++t < 60)
                {
                    npc.alpha += 2;
                    npc.friendly = true;
                    MoveTo(trueTargetPos, 10, 40);
                }
                if (t > 60 && t < 120)
                {
                    npc.alpha -= 2;
                    npc.ai[2] = Utils.AngleLerp(npc.ai[2], owner.DirectionTo(player.Center).ToRotation(), 0.1f);
                    npc.Center = Vector2.Lerp(npc.Center, trueTargetPos, 0.1f);
                }
                if (t > 120&&t<160)
                {
                    npc.friendly = false;
                    Vector2 d = owner.DirectionTo(player.Center + player.velocity * 20);
                    npc.ai[2] = Utils.AngleLerp(npc.ai[2], d.ToRotation(), 0.2f);
                    npc.Center = Vector2.Lerp(npc.Center, trueTargetPos, 0.4f) + d * npc.localAI[0];
                    npc.localAI[0] = MathHelper.Lerp(npc.localAI[0],-60,0.05f);
                }
                if (t == 160)
                {
                    npc.velocity = npc.ai[2].ToRotationVector2() * 30;
                }
                if (t == 220)
                {
                    npc.velocity *= 0.5f;
                    npc.velocity += Main.rand.NextVector2Unit() * 10;
                    npc.friendly = true;
                }
                if (t > 240)
                    t = 0;

            }
            if (npc.ai[0] == 3)//剑
            {
                Vector2 trueTargetPos = owner.Center + TargetPos.RotatedBy(npc.ai[2]);
                Player player = Main.player[owner.target];
                if (t == 0)
                {
                    npc.friendly = false;
                    npc.dontTakeDamage = true;
                    npc.ai[2] = -1.57f;
                    npc.velocity = Vector2.Zero;
                }
                if (++t < 60)
                {
                    npc.alpha = (int)MathHelper.Lerp(npc.alpha, 120, 0.1f);
                    npc.friendly = true;
                    MoveTo(trueTargetPos, 10, 40);
                    npc.localAI[0] = owner.spriteDirection * -1;
                }
                if (t > 60 && t < 120)
                {
                    npc.alpha =(int)MathHelper.Lerp(npc.alpha,0,0.1f);
                    npc.ai[2] = Utils.AngleLerp(npc.ai[2],-1.57f, 0.1f);
                    npc.Center = Vector2.Lerp(npc.Center, trueTargetPos, 0.15f);
                }
                if (t > 120 && t < 180)
                {
                    npc.alpha = (int)MathHelper.Lerp(npc.alpha, 0, 0.1f);
                    npc.ai[2] = Utils.AngleLerp(npc.ai[2], -1.57f + npc.localAI[0] * 1f, 0.05f);
                    npc.Center = Vector2.Lerp(npc.Center, trueTargetPos, 0.5f);
                }
                if (t > 180 && t < 220)
                {
                    npc.ai[2] -= npc.localAI[0] * 0.12f;
                    npc.Center = Vector2.Lerp(npc.Center, trueTargetPos, 0.5f);
                }
                if (t == 220)
                {
                    npc.velocity =Main.rand.NextVector2Unit()*10f;
                }
                if (t > 250)
                {
                    npc.ai[0] = -1;
                    t = 0;
                }

            }
            if (npc.ai[0] == 4)//拳
            {
                NPC.spriteDirection = Math.Sign(NPC.velocity.X);
                Vector2 trueTargetPos = owner.Center + TargetPos.RotatedBy(npc.ai[2]);
                Player player = Main.player[owner.target];
                if (t == 0)
                {
                    npc.dontTakeDamage = true;
                    npc.ai[2] = 0f;
                    npc.localAI[0] = 0;
                    npc.velocity = Vector2.Zero;
                }
                if (++t < 60)
                {
                    npc.alpha += 2;
                    npc.friendly = true;
                    MoveTo(trueTargetPos, 10, 40);
                }
                if (t > 60 && t < 120)
                {
                    npc.alpha -= 2;
                    npc.ai[2] = Utils.AngleLerp(npc.ai[2], owner.DirectionTo(player.Center).ToRotation(), 0.1f);
                    npc.Center = Vector2.Lerp(npc.Center, trueTargetPos, 0.1f);
                }
                if (t > 120 && t < 160)
                {
                    npc.friendly = false;
                    Vector2 d = owner.DirectionTo(player.Center);
                    npc.ai[2] = Utils.AngleLerp(npc.ai[2], d.ToRotation(), 0.2f);
                    npc.Center = Vector2.Lerp(npc.Center, trueTargetPos, 0.4f) + d * npc.localAI[0];
                    npc.localAI[0] = MathHelper.Lerp(npc.localAI[0], -70, 0.05f);
                }
                if (t == 160)
                {
                    npc.velocity = npc.ai[2].ToRotationVector2() * 40;
                }
                if (t == 200)
                {
                    npc.velocity = Main.rand.NextVector2Unit() * Main.rand.Next(2, 10);
                    t = 0;
                    npc.ai[0] = 0;
                    npc.dontTakeDamage = false;
                }
            }

            if (npc.ai[0] == 1 || npc.ai[0] == 3 || npc.ai[0] == 4)
                npc.noTileCollide = true;
            else
                npc.noTileCollide = false;
        }
        public override void PostAI()
        {
            
        }
        private void MoveTo(Vector2 targetPos, float Speed, float n)
        {
            Vector2 targetVec = Utils.SafeNormalize(targetPos - npc.Center, Vector2.Zero) * Speed;
            npc.velocity = (npc.velocity * n + targetVec) / (n + 1);
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frame.Y > 48)
                NPC.frame.Y = 0;
            if (t % 10 == 0)
                NPC.frame.Y += 24;

            if (t % 3 == 0 && npc.alpha<20)
            {
                int num90 = Dust.NewDust(NPC.position - new Vector2(8), NPC.width, NPC.height, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.7f, 1.9f));
                Main.dust[num90].velocity = NPC.velocity * 0.5f;
            }
            
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 6; i++)
            {
                int num90 = Dust.NewDust(NPC.position - new Vector2(8), NPC.width, NPC.height, 226, 0f, 0f, 100, Color.Blue, Main.rand.NextFloat(0.7f, 1.2f));
                Main.dust[num90].velocity = new Vector2(0, Main.rand.NextFloat(5f, 10f)).RotatedByRandom(6.283);
                Main.dust[num90].noGravity = true;
            }
            base.HitEffect(hitDirection, damage);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 0)*(1-npc.alpha/255f);
        }
    }
}
