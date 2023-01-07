using Everglow.Sources.Modules.MythModule.LanternMoon.LanternCommon;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.NPCs
{

    public class FloatLantern : ModNPC
    {
        private bool A = true;
        private int num10;
        private int num11;
        private int num12;
        private int num13;
        private int num14;
        private int num15;
        private int num16;
        private int num17;
        private int num18;
        private int num19;
        private int num20;
        private int num21;
        private int num22;
        private int num23;
        private int num24;

		public LanternMoonProgress LanternMoonProgress = ModContent.GetInstance<LanternMoonProgress>();
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lantern Ghost");
            Main.npcFrameCount[NPC.type] = 3;
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "鬼灯笼");
        }
        public override void SetDefaults()
        {
            NPC.damage = 100;
            NPC.lifeMax = 500;
            NPC.npcSlots = 14f;
            NPC.width = 62;
            NPC.height = 74;
            NPC.defense = 0;
            NPC.value = 0;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.6f;
            NPC.dontTakeDamage = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit3;
            /*NPC.BannerID = NPC.type;
            this.bannerItem = base.mod.ItemType("LanternghostBanner");*/
        }
        private int A2 = 0;
        private int num1 = 0;
        private bool initialization = true;
        public override void AI()
        {
            num1 += 1;
            if (initialization)
            {
                num1 = Main.rand.Next(-120, 0);
                num4 = Main.rand.NextFloat(0.3f, 1800f);
                initialization = false;
            }
            num4 += 0.01f;
            if (num1 > 0 && num1 <= 120)
            {
                num = num1 / 120f;
            }
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            NPC.rotation = NPC.velocity.X / 30f;
            A2 += 1;
            if (A2 % 45 < 15)
            {
                NPC.frame.Y = 0;
            }
            if (A2 % 45 >= 15 && A2 % 45 < 30)
            {
                NPC.frame.Y = 74;
            }
            if (A2 % 45 >= 30 && A2 % 45 < 45)
            {
                NPC.frame.Y = 148;
            }
            Vector2 v = player.Center + new Vector2((float)Math.Sin(A2 / 40f) * 500f, (float)Math.Sin((A2 + 200) / 40f) * 50f - 150) - NPC.Center;
            if (NPC.velocity.Length() < 9f)
            {
                NPC.velocity += v / v.Length() * 0.35f;
            }
            NPC.velocity *= 0.96f;
            /*if (Math.Abs(NPC.Center.X - player.Center.X) < 200 && A2 % 25 == 1)
            {
                Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y + 30f, NPC.velocity.X / 3f, NPC.velocity.Y * 0.25f + 1.5f, mod.ProjectileType("LanternBoomLi"), 25, 0f, Main.myPlayer, 0f, 0f);
            }*/
            if (Main.dayTime)
            {
                NPC.velocity.Y += 1;
            }
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            //MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if (NPC.life <= 0)
            {
                Vector2 vF = new Vector2(Main.rand.NextFloat(-0.4f, 0.4f), 0).RotatedByRandom(6.283) * 6f;
                Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore1").Type, 1f);
                vF = new Vector2(Main.rand.NextFloat(-0.4f, 0.4f), 0).RotatedByRandom(6.283) * 6f;
                Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore2").Type, 1f);
                for (int f = 0; f < 3; f++)
                {
                    vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                    int gra0 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore3").Type, 1f);
                    Main.gore[gra0].timeLeft = Main.rand.Next(300, 600);
                    vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                    int gra1 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore4").Type, 1f);
                    Main.gore[gra1].timeLeft = Main.rand.Next(300, 600);
                    vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                    int gra2 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore5").Type, 1f);
                    Main.gore[gra2].timeLeft = Main.rand.Next(300, 600);
                    vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                    int gra3 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore6").Type, 1f);
                    Main.gore[gra3].timeLeft = Main.rand.Next(300, 600);
                }
				LanternMoonProgress.Point += 15;
                LanternMoonProgress.WavePoint += 15;
                for (int f = 0; f < 55; f++)
                {
                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 12f)).RotatedByRandom(MathHelper.TwoPi);
                    int r = Dust.NewDust(NPC.Center - new Vector2(4, 4) - new Vector2(4, 4), 8, 8, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1.8f));
                    Main.dust[r].noGravity = true;
                    Main.dust[r].velocity = v3;
                }
            }

        }
        private float num = 0;
        private float num4 = 0;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Texture2D tg = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/FloatLanternGlow").Value;
            Texture2D tg2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/FloatLanternGlow2").Value;
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(NPC.Center.X, NPC.Center.Y);
            Vector2 vector = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)tg.Width, (float)(tg.Height / Main.npcFrameCount[NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + NPC.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(297 - NPC.alpha, 297 - NPC.alpha, 297 - NPC.alpha, 0), Color.White);
            Main.spriteBatch.Draw(tg, vector2, new Rectangle(0, NPC.frame.Y, 62, 74), new Color(200, 200, 200, 0), NPC.rotation, vector, 1f, effects, 0f);
            x += 0.01f;
            float K = (float)(Math.Sin(x + Math.Sin(x) * 6) * (0.95 + Math.Sin(x + 0.24 + Math.Sin(x))) + 3) / 30f;
            float M = (float)(Math.Sin(x + Math.Tan(x) * 6) * (0.95 + Math.Cos(x + 0.24 + Math.Sin(x))) + 3) / 30f;
            float y = M * 4.8f;
            Color colorT = new Color(1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 0.15f * num * (float)(Math.Sin(num4) + 2) / 3f);
            Main.spriteBatch.Draw(tg2, vector2, new Rectangle(0, NPC.frame.Y, 62, 74), colorT, NPC.rotation, vector, 1f, effects, 0f);
        }
        private float x = 0;
    }
}
