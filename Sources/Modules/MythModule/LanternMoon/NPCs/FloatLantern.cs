using Everglow.Sources.Modules.MythModule.LanternMoon.LanternCommon;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.NPCs
{

    public class FloatLantern : ModNPC
    {

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
            if (NPC.life <= 0)
            {
                Vector2 vF = new Vector2(Main.rand.NextFloat(-0.4f, 0.4f), 0).RotatedByRandom(6.283) * 6f;
                Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore1").Type, 1f);
                vF = new Vector2(Main.rand.NextFloat(-0.4f, 0.4f), 0).RotatedByRandom(6.283) * 6f;
                Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore2").Type, 1f);
                for (int f = 0; f < 3; f++)
                {
                    vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                    int gra0 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore3").Type, 1f);
                    Main.gore[gra0].timeLeft = Main.rand.Next(300, 600);
                    vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                    int gra1 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore4").Type, 1f);
                    Main.gore[gra1].timeLeft = Main.rand.Next(300, 600);
                    vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                    int gra2 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore5").Type, 1f);
                    Main.gore[gra2].timeLeft = Main.rand.Next(300, 600);
                    vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                    int gra3 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore6").Type, 1f);
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
            Main.spriteBatch.Draw(tg, vector2, new Rectangle(0, NPC.frame.Y, 62, 74), new Color(200, 200, 200, 0), NPC.rotation, vector, 1f, effects, 0f);
            x += 0.01f;
            Color colorT = new Color(1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 0.15f * num * (float)(Math.Sin(num4) + 2) / 3f);
            Main.spriteBatch.Draw(tg2, vector2, new Rectangle(0, NPC.frame.Y, 62, 74), colorT, NPC.rotation, vector, 1f, effects, 0f);
        }
        private float x = 0;
    }
}
