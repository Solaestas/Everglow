using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.NPCs
{
    public class GlowingFirefly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
            MothLandGlobalNPC.RegisterMothLandNPC(Type);
        }
        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 20;
            NPC.height = 20;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.knockBackResist = 1f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.color = new Color(0, 0, 0, 0);
            NPC.alpha = 0;
            NPC.aiStyle = -1;
        }
        public override void AI()
        {
            Player player = Main.player[NPC.FindClosestPlayer()];
            if(NPC.ai[1] > 0)
            {
                if(NPC.ai[0] < 4)
                {
                    NPC.ai[0] += 0.18f;
                }
                else
                {
                    NPC.ai[0] += 0.5f;
                }
                
                if (NPC.ai[0] >= 8f)
                {
                    NPC.ai[0] = 4f;
                }
                NPC.velocity.Y = 0f;
            }
            else
            {
                if ((player.Center - NPC.Center).Length() < 80f)
                {
                    NPC.ai[1] = 1f;
                }
                foreach(NPC same in Main.npc)
                {
                    if(same.type == NPC.type)
                    {
                        if((same.Center - NPC.Center).Length() < 60)
                        {
                            if(same.ai[1] > 0)
                            {
                                NPC.ai[1] = 1;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D tx = MythContent.QuickTexture("TheFirefly/NPCs/GlowingFirefly");
            Texture2D tg = MythContent.QuickTexture("TheFirefly/NPCs/GlowingFireflyGlow");
            Vector2 vector = new Vector2(tx.Width / 2f, tx.Height / (float)Main.npcFrameCount[NPC.type] / 2f);

            Color color0 = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
            Color color1 = new Color(255, 255, 255, 0);
            Main.spriteBatch.Draw(tx, NPC.Center - Main.screenPosition, new Rectangle(0, 50 * (int)NPC.ai[0], 46, 50), color0, NPC.rotation, vector, NPC.scale, effects, 0f);
            Main.spriteBatch.Draw(tg, NPC.Center - Main.screenPosition, new Rectangle(0, 50 * (int)NPC.ai[0], 46, 50), color1, NPC.rotation, vector, NPC.scale, effects, 0f);

            return false;
        }
    }
}
