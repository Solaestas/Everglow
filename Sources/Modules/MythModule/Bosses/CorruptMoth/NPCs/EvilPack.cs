using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.NPCs
{
    public class EvilPack : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evil Cocoon");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "魔茧");
        }
        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 80;
            NPC.height = 256;
            NPC.defense = 0;
            NPC.lifeMax = 600;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.color = new Color(0, 0, 0, 0);
            NPC.alpha = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;
            NPC.HitSound = SoundID.NPCHit18; //Or use NPCHit11. Whichever one sounds more realistic to the cocoon. ~Setnour6
            NPC.DeathSound = SoundID.NPCDeath11;
            NPC.aiStyle = -1;
            NPC.boss = true;
        }
        private float omega = 0;
        private float E = 0;
        public override void AI()
        {
            NPC.rotation += omega;
            omega -= NPC.rotation * 0.03f;
            omega *= 0.97f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Vector2 vector = new Vector2(NPC.position.X, NPC.position.Y);
                float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100f);
                Gore.NewGore(null, new Vector2(NPC.position.X, NPC.position.Y + 128), NPC.velocity * scaleFactor, ModContent.Find<ModGore>("MythMod/EvilPackBreak").Type, 1f);
                NPC.active = false;
            }
            if (Math.Abs(omega) < 0.2f)
            {
                omega -= hitDirection * Main.rand.NextFloat(0.2f) * (float)damage / 100f <= 0.2f * hitDirection ? 0.02f : hitDirection * Main.rand.NextFloat(0.2f) * (float)damage / 1000f;
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D tx = ModContent.Request<Texture2D>("MythMod/NPCs/CorruptMoth/EvilPack").Value;
            Texture2D tg = ModContent.Request<Texture2D>("MythMod/NPCs/CorruptMoth/EvilPackGlow").Value;
            Vector2 vector = new Vector2(tx.Width / 2f, tx.Height / (float)Main.npcFrameCount[NPC.type] / 2f);
            Color color = Utils.MultiplyRGBA(new Color(297 - NPC.alpha, 297 - NPC.alpha, 297 - NPC.alpha, 0), Color.White);
            Main.spriteBatch.Draw(tg, NPC.Center - Main.screenPosition + new Vector2(0, +4), new Rectangle?(NPC.frame), color, NPC.rotation, vector, 1f, effects, 0f);
        }
    }
}
