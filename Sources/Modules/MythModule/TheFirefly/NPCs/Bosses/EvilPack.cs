﻿using Terraria.Localization;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.NPCs.Bosses
{
    public class EvilPack : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evil Cocoon");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "魔茧");
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 80;
            NPC.height = 150;
            NPC.defense = 0;
            NPC.lifeMax = 1;
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
            NPC.boss = false;
        }
        private float omega = 0;
        public override void AI()
        {
            NPC.rotation += omega;
            omega -= NPC.rotation * 0.03f;
            omega *= 0.97f;
            if(NPC.ai[0] < 10)
            {
                NPC.frame = new Rectangle(0, 150, 80, 150);
            }
            else
            {
                if (NPC.ai[1] > 90)
                {
                    NPC.frame = new Rectangle(0, 0, 80, 150);
                    if(NPC.ai[2] == 0)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            for (int x = 0; x < 8; x++)
                            {
                                Gore.NewGore(NPC.GetSource_FromAI(), NPC.position + new Vector2(26, 106) + new Vector2(0, Main.rand.Next(40)).RotatedByRandom(6.283), new Vector2(0, Main.rand.Next(8)).RotatedByRandom(6.283) + new Vector2(-2, 2), ModContent.Find<ModGore>("Everglow/Cocoon" + x.ToString()).Type);
                            }
                        }
                        for (int x = 0; x < 72; x++)
                        {
                            Dust d = Dust.NewDustDirect(NPC.position + new Vector2(26 - 20, 106 - 30), 40, 60, ModContent.DustType<BlueGlowAppear>(), 0, 0, 0, default, Main.rand.NextFloat(0.7f, 1.7f));
                            d.velocity = new Vector2(0, Main.rand.Next(16)).RotatedByRandom(6.283) + new Vector2(-4, 4);

                            for(int k = 0;k < 3;k++)
                            {
                                Dust d2 = Dust.NewDustDirect(NPC.position + new Vector2(26 - 20, 106 - 30), 40, 60, ModContent.DustType<BlueParticleDark2>(), 0, 0, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
                                d2.velocity = new Vector2(0, Main.rand.Next(12)).RotatedByRandom(6.283) + new Vector2(-4, 4);
                                d2.alpha = (int)(d2.scale * 50);
                            }

                            Dust d3 = Dust.NewDustDirect(NPC.position + new Vector2(26 - 20, 106 - 30), 40, 60, 191, 0, 0, 0, default, Main.rand.NextFloat(0.7f, 1.7f));
                            d3.velocity = new Vector2(0, Main.rand.Next(16)).RotatedByRandom(6.283) + new Vector2(-4, 4);
                        }
                        if (!NPC.AnyNPCs(ModContent.NPCType<CorruptMoth>()))
                        {
                            Main.NewText("Corrupted Moth has awoken!", 175, 75, 255);
                        }
                        int n = NPC.NewNPC(NPC.GetSource_FromAI(),(int)NPC.position.X + 26, (int)NPC.position.Y + 106,ModContent.NPCType<CorruptMoth>());
                        Main.npc[n].velocity = new Vector2(-1, 1);
                        NPC.ai[2] += 1;
                    }
                }
                else
                {
                    omega *= 0.9f;
                    float step = 0.05f;
                    NPC.ai[1] += step;
                    if(NPC.ai[1] >= 4f && NPC.ai[1] - step < 4f)
                    {
                        omega += 0.02f;
                        NPC.ai[1] += 12;
                        SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/MothHitCocoon"), NPC.Center);
                    }
                    if (NPC.ai[1] >= 19f && NPC.ai[1] - step < 19f)
                    {
                        NPC.ai[1] += 24;
                        omega -= 0.03f;
                        SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/MothHitCocoon"), NPC.Center);
                    }
                    if (NPC.ai[1] >= 45f && NPC.ai[1] - step < 45f)
                    {
                        NPC.ai[1] += 22;
                        omega += 0.04f;
                        SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/MothHitCocoon"), NPC.Center);
                    }
                    if (NPC.ai[1] >= 70f && NPC.ai[1] - step < 70f)
                    {
                        NPC.ai[1] += 4;
                        omega -= 0.05f;
                        SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/MothHitCocoon"), NPC.Center);
                    }
                    if (NPC.ai[1] >= 76f && NPC.ai[1] - step < 76f)
                    {
                        NPC.ai[1] += 13;
                        omega += 0.02f;
                        SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/MothHitCocoon"), NPC.Center);
                    }
                    if (NPC.ai[1] >= 89.7f && NPC.ai[1] - step < 89.7f)
                    {
                        omega += 0.1f;
                        SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/MothBreakCocoon"), NPC.Center);
                    }
                }
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            //SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/MothHitCocoon"), NPC.Center);
            if (NPC.ai[0] < 10)
            {
                NPC.ai[0] += 1;
            }
            else
            {
                if(NPC.ai[1] < 90f)
                {
                    NPC.ai[1] += 0.01f;
                }
                else
                {
                    NPC.ai[1] = 91f;
                }
            }
            NPC.life = NPC.lifeMax;
            if (Math.Abs(omega) < 0.2f)
            {
                omega -= hitDirection * (float)damage / 1000f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D tg = MythContent.QuickTexture("TheFirefly/NPCs/Bosses/EvilPack");
            Color color = drawColor;
            Main.spriteBatch.Draw(tg, NPC.position + new Vector2(tg.Width / 2f, 0) - Main.screenPosition, new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(tg.Width / 2f, 0), 1f, effects, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D tg = MythContent.QuickTexture("TheFirefly/NPCs/Bosses/EvilPackGlow");
            float C = (float)Math.Sqrt(Math.Max((90 - NPC.ai[1]) / 90f, 0)) * 0.6f + Math.Abs(omega * 15);
            Color color = new Color(C, C, C, 0);
            Main.spriteBatch.Draw(tg, NPC.position + new Vector2(tg.Width / 2f, 0) - Main.screenPosition, new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(tg.Width / 2f, 0), 1f, effects, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.AnisotropicWrap,DepthStencilState.None,RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix) ;
            Vector2 CrackCenter = new Vector2(-14, 106).RotatedBy(NPC.rotation) + new Vector2(40, 0);
            if(NPC.ai[1] <= 90)
            {
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1], 0, 15), 0);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 8, 0, 15), 1);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 16, 0, 15), 2);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 24, 0, 15), 3);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 30, 0, 15), 4);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 36, 0, 15), 5);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 24, 0, 15), 6, (int)Math.Clamp((NPC.ai[1] - 36) / 2f, 1, 50));
            }
        }
        public void DrawCrack(Vector2 DrawCenter, float Radius, int type, int Power = 1)
        {
            Texture2D t0 = MythContent.QuickTexture("TheFirefly/NPCs/Bosses/Crack" + type.ToString());
            
            List<Vertex2D> vertex2Ds = new List<Vertex2D>();
            for(int a = 1;a < Power + 1;a++)
            {
                Color color = new Color(1f / (float)a, 1f / (float)a, 1f / (float)a, 0);
                float scale = 2 + (a - 1) / 8f;
                Vector2 Move = new Vector2(-1, 1) * a;
                for (int x = 0; x < 10; x++)
                {
                    Vector2 DrawPoint1 = new Vector2(0, -Radius).RotatedBy(x / 5d * Math.PI);
                    Vector2 DrawPoint2 = new Vector2(0, -Radius).RotatedBy((x + 1) / 5d * Math.PI);
                    Vector2 dp1 = DrawPoint1.RotatedBy(NPC.rotation) * scale + Move;
                    Vector2 dp2 = DrawPoint2.RotatedBy(NPC.rotation) * scale + Move;
                    vertex2Ds.Add(new Vertex2D(DrawCenter + dp1, color, new Vector3(0.5f + DrawPoint1.X / t0.Width, 0.5f + DrawPoint1.Y / t0.Height, 0)));
                    vertex2Ds.Add(new Vertex2D(DrawCenter + dp2, color, new Vector3(0.5f + DrawPoint2.X / t0.Width, 0.5f + DrawPoint2.Y / t0.Height, 0)));
                    vertex2Ds.Add(new Vertex2D(DrawCenter, color, new Vector3(0.5f, 0.5f, 0)));
                }
            }

            Main.graphics.GraphicsDevice.Textures[0] =t0;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }
    }
}
