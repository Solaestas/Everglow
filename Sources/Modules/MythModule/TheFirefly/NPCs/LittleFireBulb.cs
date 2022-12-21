using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.NPCs
{
    public class LittleFireBulb : ModNPC
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 32;
            NPC.height = 32;
            NPC.defense = 0;
            NPC.lifeMax = 1;
            NPC.knockBackResist = 1f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.color = new Color(0, 0, 0, 0);
            NPC.alpha = 0;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;

            NPC.dontTakeDamage = true;
            NPC.aiStyle = -1;
        }

        private bool HitT = false;
        private bool Ini = false;
        private float MaxL = 0;
        private Vector2 StaCen = Vector2.Zero;

        public override void AI()
        {
            if (!Ini)
            {
                int MaxxL = 400;
                for (int Dy = 5; Dy < 400; Dy++)
                {
                    if (Collision.SolidCollision(NPC.Center + new Vector2(0 - 5, 20 + Dy - 5), 10, 10))
                    {
                        MaxxL = Dy;
                        break;
                    }
                }
                NPC.velocity = new Vector2(0, 1);
                MaxL = Main.rand.Next(4, MaxxL);
                Ini = true;
                StaCen = NPC.Center;
            }
            Vector2 TOCen = StaCen - NPC.Center;
            if (!HitT)
            {
                if (NPC.Center.Y - StaCen.Y < MaxL)
                {
                    if (Collision.SolidCollision(NPC.position - Vector2.One * 5f + NPC.velocity * 10, 10, 10))
                    {
                        NPC.velocity *= 0.96f;
                        if (NPC.velocity.Length() < 0.05f)
                        {
                            NPC.velocity *= 0;
                            MaxL = (NPC.Center.Y - StaCen.Y);
                            HitT = true;
                        }
                    }
                }
                else
                {
                    NPC.velocity *= 0.99f;
                    if (NPC.velocity.Length() < 0.05f)
                    {
                        NPC.velocity *= 0;
                        MaxL = (NPC.Center.Y - StaCen.Y);
                        HitT = true;
                    }
                }
            }
            else
            {
                NPC.noTileCollide = false;
                NPC.dontTakeDamage = false;
                float Leng = NPC.velocity.Length() * NPC.velocity.Length() / MaxL;

                NPC.velocity += TOCen / TOCen.Length() * Leng;
                NPC.velocity += new Vector2(0, 0.35f);
                NPC.velocity += TOCen / TOCen.Length() * (TOCen.Length() - MaxL) * 0.01f;
                if (NPC.velocity.Length() > 1f)
                {
                    NPC.velocity -= NPC.velocity * 0.01f;
                }
            }
            NPC.rotation = (float)(Math.Atan2(TOCen.Y, TOCen.X) + Math.PI / 2d);
            Lighting.AddLight((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16 - 1), 0, 0.1f, 0.8f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                NPC.life = 1;
                NPC.active = true;
            }
        }

        private Vector2[] vPos = new Vector2[200];

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D tx = MythContent.QuickTexture("TheFirefly/NPCs/LittleFireBulb");
            Texture2D tg = MythContent.QuickTexture("TheFirefly/NPCs/LittleFireBulb_Glow");
            Vector2 vector = new Vector2(tx.Width / 2f, tx.Height / (float)Main.npcFrameCount[NPC.type] / 2f);

            Color color0 = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
            Main.spriteBatch.Draw(tx, NPC.Center - Main.screenPosition, new Rectangle(0, 32, 32, 32), color0, NPC.rotation, vector, 1f, effects, 0f);
            Main.spriteBatch.Draw(tx, StaCen - Main.screenPosition + new Vector2(0, 24), new Rectangle(0, 0, 32, 8), color0, 0, vector, 1f, effects, 0f);
            Color color = new Color(255, 255, 255, 0);
            Main.spriteBatch.Draw(tg, NPC.Center - Main.screenPosition, new Rectangle(0, 32, 32, 32), color, NPC.rotation, vector, 1f, effects, 0f);
            vPos[0] = NPC.Center;
            for (int f = 1; f < 200; f++)
            {
                if ((StaCen - vPos[f - 1]).Length() < 24)
                {
                    break;
                }
                vPos[f] = vPos[f - 1] + (StaCen - vPos[f - 1]) / (StaCen - vPos[f - 1]).Length() * 6;
                Color color2 = Lighting.GetColor((int)(vPos[f].X / 16d), (int)(vPos[f].Y / 16d));
                Main.spriteBatch.Draw(tx, vPos[f] - Main.screenPosition, new Rectangle(0, 10, 32, 6), color2, NPC.rotation, vector, 1f, effects, 0f);
            }
            return false;
        }
    }
}