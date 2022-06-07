using Everglow.Sources.Modules.MythModule.Common;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    internal class ShadowWingBow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 90;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
        }

        private int Ran = -1;
        private int Tokill = -1;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
        }

        private bool Release = true;
        private int PdamF = 0;
        private Vector2 oldPo = Vector2.Zero;
        private float[] ArRot = new float[5];
        private float[] ArVel = new float[5];
        private float[] SArVel = new float[5];
        private float[] ArVelφ = new float[5];
        private float[] Arcol = new float[5];
        private int addi = 0;
        public override void AI()
        {
            addi++;
            Player player = Main.player[Projectile.owner];
            if (ArRot[0] == 0)
            {
                ArRot[0] = Main.rand.NextFloat(-0.7f, -0.5f);
                for (int s = 1; s < 4; s++)
                {
                    ArRot[s] = Main.rand.NextFloat(-0.7f, 0.7f);
                }
                ArRot[4] = Main.rand.NextFloat(0.5f, 0.7f);
            }
            if (SArVel[0] == 0)
            {
                for (int s = 0; s < 5; s++)
                {
                    SArVel[s] = Main.rand.NextFloat(24f, 28f);
                }
            }
            if (ArVelφ[0] == 0)
            {
                for (int s = 0; s < 5; s++)
                {
                    ArVelφ[s] = Main.rand.NextFloat(0, 6.283f);
                }
            }
            if (Energy > 60 && Energy < 120)
            {
                for (int s = 0; s < 5; s++)
                {
                    ArRot[s] *= 0.97f;
                }
            }
            for (int s = 0; s < 5; s++)
            {
                ArVel[s] = SArVel[s] + (float)(Math.Sin(ArVelφ[s] + Main.time / 20d) * 3);
            }
            for (int s = 0; s < 5; s++)
            {
                Arcol[s] = Math.Clamp((float)((Math.Abs(s - 2.5) * 100) + (Energy - 90) * 7) / 255f, 0, 1f);
            }
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].Center;
            if (Main.mouseLeft && Release)
            {
                if (Projectile.timeLeft > 80)
                {
                    PdamF = Projectile.damage;
                }
                Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) + Math.PI * 0.25);
                Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(v0) * 22f;
                oldPo = Projectile.Center;
                Projectile.Center = oldPo;
                Projectile.velocity *= 0;
                Projectile.timeLeft = 5 + Energy;
                if (Energy <= 120)
                {
                    if (addi % 2 == 1)
                    {
                        Energy++;
                    }
                    Energy++;
                }
                else
                {
                    Energy = 120;
                }
            }
            if (!Main.mouseLeft && Release)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.friendly = true;
                Projectile.tileCollide = true;
                Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(v0) * 22f;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Normalize(v0) * 34f, (int)(Projectile.ai[0]), Projectile.damage + Energy / 5, Projectile.knockBack, player.whoAmI);
                for (int s = 0; s < 5; s++)
                {
                    if (Arcol[s] > 0)
                    {
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Normalize(v0).RotatedBy(ArRot[s]) * ArVel[s] * 0.6f, ModContent.ProjectileType<MothArrow>(), (int)((Projectile.damage + Energy / 5) * 0.47), Projectile.knockBack, player.whoAmI, 0, player.HeldItem.crit + player.GetCritChance(DamageClass.Ranged) + player.GetCritChance(DamageClass.Generic));
                    }
                }
                Release = false;
            }
            if (Projectile.ai[1] > 0)
            {
                Projectile.ai[1] -= 1f;
            }
            if (!Main.mouseLeft && !Release)
            {
                if (Projectile.ai[1] > 0)
                {
                    Projectile.ai[1] -= 1f;
                    Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(v0) * 22f;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            if (Ran == -1)
            {
                Ran = Main.rand.Next(9);
            }
        }
        private bool Nul = false;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        private int Energy = 0;
        private Vector2[] Vlaser = new Vector2[501];
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D TexString = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowString0");
            Texture2D TexMainU0 = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowU0");
            Texture2D TexMainU1 = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowU1");
            Texture2D TexMainU0G = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowU0Glow");
            Texture2D TexMainU1G = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowU1Glow");
            Texture2D TexMainU2 = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowU2");
            Texture2D TexMainD0 = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowD0");
            Texture2D TexMainD1 = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowD1");
            Texture2D TexMain = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowMain");
            Texture2D TexMainG = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowMainGlow");
            Texture2D TexArrow = TextureAssets.Projectile[(int)(Projectile.ai[0])].Value;
            Texture2D TexMothArrow = MythContent.QuickTexture("TheFirefly/Projectiles/MothArrow");
            float a0 = Energy % 60f;
            float a1 = (60 - a0) / 60f;
            float a2 = a1 * 1.5f;
            float a3 = a2 * a2;
            float b0 = Math.Clamp(Energy / 2f, 0, 60);
            float b1 = b0 / 60f;
            float b2 = b1;
            float b3 = b2 * b2;
            Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
            SpriteEffects se = SpriteEffects.None;
            if (Projectile.Center.X < player.Center.X)
            {
                se = SpriteEffects.FlipVertically;
                player.direction = -1;
            }
            else
            {
                player.direction = 1;
            }
            int StringFrame = 0;
            StringFrame = Math.Clamp((int)(Energy / 20f), 0, 5);
            TexString = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowString" + StringFrame.ToString());
            Vector2 v0 = Main.MouseWorld - player.Center;
            if (Main.mouseLeft)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v0.Y, v0.X) - Math.PI / 2d));
            }
            Vector2 vProA = Main.player[Projectile.owner].Center + Vector2.Normalize(v0) * (28f - 12f * b3);
            for (int s = 0; s < 5; s++)
            {
                Vector2 vProB = Main.player[Projectile.owner].Center + Vector2.Normalize(v0).RotatedBy(ArRot[s]) * (ArVel[s] * 1.4f - 16f * b3);
                Main.spriteBatch.Draw(TexMothArrow, vProB - Main.screenPosition, null, new Color(Arcol[s], Arcol[s], Arcol[s], 0), Projectile.rotation + ArRot[s], new Vector2(TexMothArrow.Width / 2f, TexMothArrow.Height / 2f), 1f, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(TexArrow, vProA - Main.screenPosition, new Rectangle(0, 0, TexArrow.Width, TexArrow.Height), drawColor, Projectile.rotation + (float)(Math.PI * 0.25), new Vector2(TexArrow.Width / 2f, TexArrow.Height / 2f), 1f, SpriteEffects.None, 0);
            float rotu0 = Energy / 1200f;
            float rotu1 = Energy / 750f;
            float rotu2 = Energy / 600f;
            float rotd0 = Energy / 1050f;
            float rotd1 = Energy / 720f;
            int ColS = (int)(Energy * 3 / 2f + 50f);

            Main.spriteBatch.Draw(TexMainU0, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) - rotu0 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
            Main.spriteBatch.Draw(TexMainU0G, Projectile.Center - Main.screenPosition, null, new Color(ColS, ColS, ColS, 0), Projectile.rotation - (float)(Math.PI * 0.25) - rotu0 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
            Main.spriteBatch.Draw(TexMainU1, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) - rotu1 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
            Main.spriteBatch.Draw(TexMainU1G, Projectile.Center - Main.screenPosition, null, new Color(ColS, ColS, ColS, 0), Projectile.rotation - (float)(Math.PI * 0.25) - rotu1 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
            Main.spriteBatch.Draw(TexMainU2, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) - rotu2 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
            Main.spriteBatch.Draw(TexMainD0, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) + rotd0 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
            Main.spriteBatch.Draw(TexMainD1, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) + rotd1 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
            Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25), new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
            Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition, null, new Color(ColS, ColS, ColS, 0), Projectile.rotation - (float)(Math.PI * 0.25), new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);

        }

        private bool[] HasHit = new bool[200];
        private float x = 0;
        private struct CustomVertexInfo : IVertexType
        {
            private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
            });
            public Vector2 Position;
            public Color Color;
            public Vector3 TexCoord;

            public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
            {
                this.Position = position;
                this.Color = color;
                this.TexCoord = texCoord;
            }

            public VertexDeclaration VertexDeclaration
            {
                get
                {
                    return _vertexDeclaration;
                }
            }
        }
    }
    public class StringDraw
    {
        public static void Load()
        {
            On.Terraria.Main.DrawGore += DrawString;
        }
        public static void UnLoad()
        {
            //On.Terraria.Main.DrawGore -= DrawString;
        }
        private static void DrawString(On.Terraria.Main.orig_DrawGore orig, Terraria.Main self)
        {
            for (int d = 0; d < Main.projectile.Length; d++)
            {
                if (Main.projectile[d].active)
                {
                    if (Main.projectile[d].type == ModContent.ProjectileType<ShadowWingBow>())
                    {
                        Player player = Main.player[Main.projectile[d].owner];
                        Texture2D TexString = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowString0");
                        Texture2D TrueString = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowString");
                        int TotalLength = 34;//满弦长
                        int TotalWidth = 6;//弦宽
                        int BreakLength = 10;//断点长
                        if (player.direction == -1)
                        {
                            BreakLength = 24;
                            TrueString = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowStringFlip");
                        }
                        float b0 = Math.Clamp((Main.projectile[d].timeLeft - 5) / 2f, 0, 60);
                        float b1 = b0 / 60f;
                        float b2 = b1;
                        float b3 = b2 * b2;
                        float DelX = -11;
                        float DelY = -12;
                        if (player.direction == -1)
                        {
                            DelY = -22;
                        }
                        Vector2 DragVertex0 = Main.projectile[d].Center + new Vector2(DelX, DelY).RotatedBy(Main.projectile[d].rotation - Math.PI * 0.25);
                        Vector2 DragVertex1 = Main.projectile[d].Center + new Vector2(DelX - 6 * b3, DelY + BreakLength).RotatedBy(Main.projectile[d].rotation - Math.PI * 0.25);//手拉力点
                        Vector2 DragVertex2 = Main.projectile[d].Center + new Vector2(DelX, DelY + TotalLength).RotatedBy(Main.projectile[d].rotation - Math.PI * 0.25);
                        Vector2 line0 = DragVertex1 - DragVertex0;
                        Vector2 line2 = DragVertex1 - DragVertex2;
                        Vector2 DrawPoint0 = (DragVertex1 + DragVertex0) / 2f;
                        Vector2 DrawPoint2 = (DragVertex1 + DragVertex2) / 2f;
                        float Rot0 = (float)(Math.Atan2(line0.Y, line0.X));
                        float Rot2 = (float)(Math.Atan2(line2.Y, line2.X));
                        float Len0 = line0.Length();
                        float Len2 = line2.Length();
                        SpriteEffects se0 = SpriteEffects.FlipVertically;
                        SpriteEffects se2 = SpriteEffects.None;
                        Color drawColor = Lighting.GetColor((int)Main.projectile[d].Center.X / 16, (int)(Main.projectile[d].Center.Y / 16));
                        int ColS = (int)((Main.projectile[d].timeLeft - 5) * 3 / 2f + 50f);
                        Main.spriteBatch.Draw(TrueString, DrawPoint0 - Main.screenPosition, new Rectangle(0, 0, TotalWidth, BreakLength)/*第一段长度*/, drawColor, Rot0 + (float)(Math.PI * 0.5), new Vector2(TotalWidth / 2f, BreakLength / 2f), new Vector2(1f, Len0 / BreakLength), se0, 0);
                        Main.spriteBatch.Draw(TrueString, DrawPoint2 - Main.screenPosition, new Rectangle(0, BreakLength, TotalWidth, TotalLength - BreakLength)/*第二段长度*/, drawColor, Rot2 + (float)(Math.PI * 0.5), new Vector2(TotalWidth / 2f, (TotalLength - BreakLength) / 2f), new Vector2(1f, Len2 / (TotalLength - BreakLength)), se2, 0);
                        //Main.spriteBatch.Draw(TexString, Main.projectile[d].Center - Main.screenPosition, null, drawColor, Main.projectile[d].rotation - (float)(Math.PI * 0.25), new Vector2(TexString.Width / 2f, TexString.Height / 2f), 1f, se, 0);
                        //Main.spriteBatch.Draw(TexString, Main.projectile[d].Center - Main.screenPosition, null, new Color(ColS, ColS, ColS, 0), Main.projectile[d].rotation - (float)(Math.PI * 0.25), new Vector2(TexString.Width / 2f, TexString.Height / 2f), 1f, se, 0);
                    }
                }
            }
        }
    }
}
