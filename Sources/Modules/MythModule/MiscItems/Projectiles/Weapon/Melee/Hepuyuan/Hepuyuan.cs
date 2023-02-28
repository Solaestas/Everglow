using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.LanternMoon.Gores;
using Everglow.Sources.Modules.MythModule.MiscItems.Gores;
using System.Data;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Melee.Hepuyuan
{
    public class Hepuyuan : ModProjectile, IWarpProjectile
    {
        protected virtual float HoldoutRangeMin => 24f;
        protected virtual float HoldoutRangeMax => 150f;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.  
            Projectile.extraUpdates = 6;
            Projectile.width = 80;
            Projectile.height = 80;
        }
        internal int timer = 0;
        internal bool max = false;
        internal Vector2 FirstVel = Vector2.Zero;
        internal float[] wid = new float[60];
        internal Vector2[] OldplCen = new Vector2[60];
        internal float[] statrP = new float[4];
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            OldplCen[0] = Projectile.Center - Vector2.Normalize(Projectile.velocity) * 15f;//记录位置
            for (int f = OldplCen.Length - 1; f > 0; f--)
            {
                OldplCen[f] = OldplCen[f - 1];
            }

            for (int i = 0; i < 1; i++)
            {
                Vector2 v1 = new Vector2(Main.rand.NextFloat(0.8f, 10f), 0).RotatedByRandom(6.28);
                Vector2 v2 = new Vector2(Main.rand.NextFloat(30f), 0).RotatedByRandom(6.28);
                Dust.NewDust(OldplCen[0] - new Vector2(4) + v2, 1, 1, Main.rand.NextBool(2) ? ModContent.DustType<MiscItems.Dusts.XiaoDustCyan>() : ModContent.DustType<MiscItems.Dusts.XiaoDust>(), (Projectile.velocity * 0.35f + v1).X, (Projectile.velocity * 0.35f + v1).Y, 0, default, Main.rand.NextFloat(0.85f, Main.rand.NextFloat(0.85f, 3.75f)));
            }

            if (Main.rand.NextBool(1) && Projectile.timeLeft > 15)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(0.8f, 10f), 0).RotatedByRandom(6.28);
                if (Main.rand.NextBool(2))
                {
                    Vector2 v2 = new Vector2(Main.rand.NextFloat(70f), 0).RotatedByRandom(6.28);
                    Gore g = Gore.NewGoreDirect(null, OldplCen[0] + v2, Projectile.velocity * 0.35f + v0, ModContent.GoreType<XiaoDash0>(), Main.rand.NextFloat(0.65f, Main.rand.NextFloat(2.5f, 4.75f)));
                    g.timeLeft = Main.rand.Next(250, 500);
                }
                else
                {
                    Vector2 v2 = new Vector2(Main.rand.NextFloat(70f), 0).RotatedByRandom(6.28);
                    Gore g = Gore.NewGoreDirect(null, OldplCen[0] + v2, Projectile.velocity * 0.35f + v0, ModContent.GoreType<XiaoDash1>(), Main.rand.NextFloat(0.65f, Main.rand.NextFloat(2.5f, 4.75f)));
                    g.timeLeft = Main.rand.Next(250, 500);
                }
            }

            timer++;
            if (timer % 24 == 1 && Projectile.timeLeft > 30)
            {
                Vector2 v = Vector2.Normalize(Projectile.velocity);
                int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Melee.Hepuyuan.XiaoBlackWave>(), 0, 0, player.whoAmI, Math.Clamp(Projectile.velocity.Length() / 8f, 0f, 4f), 0);
                Main.projectile[h].rotation = (float)(Math.Atan2(v.Y, v.X) + Math.PI / 2d);
            }

            wid[0] = Math.Clamp(Projectile.velocity.Length() / 6f, 0, 60);//宽度
            for (int f = wid.Length - 1; f > 0; f--)
            {
                wid[f] = wid[f - 1];
            }
            if (player.direction == -Math.Sign(Projectile.velocity.X))
            {
                player.direction *= -1;
            }
            float duration = player.itemAnimationMax * 7.2f;
            player.heldProj = Projectile.whoAmI;
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = (int)duration;
            }
            float halfDuration = duration * 0.5f;


            if (Projectile.timeLeft < halfDuration + 2 && !max)
            {
                max = true;
            }

            Projectile.velocity *= 0.995f;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                Projectile.rotation += MathHelper.ToRadians(135f);
            }

            if (Collision.SolidCollision(Projectile.Center, 1, 1))
            {
                Projectile.velocity *= 0.9f;
                Projectile.timeLeft -= 4;
                player.velocity *= 0.9f;
                if (Projectile.timeLeft % 30 == 1)
                {
                    for (int h = 0; h < 18; h++)
                    {
                        Vector2 v = Projectile.Center - Vector2.Normalize(Projectile.velocity) * 16 * h;
                        if (Collision.SolidCollision(v, 1, 1))
                        {
                            Collision.HitTiles(v, Projectile.velocity * 20, 16, 16);
                        }
                    }
                }
            }
            if (Projectile.timeLeft > 6 && !Collision.SolidCollision(Projectile.Center, 1, 1))
            {
                player.velocity = Projectile.velocity * 6;
            }
            if (Projectile.timeLeft < 6)
            {
                player.velocity *= 0.4f;
                Projectile.velocity *= 0.4f;
            }
            MythContentPlayer myplayer = player.GetModPlayer<MythContentPlayer>();
            myplayer.IMMUNE = 15;
            return false;
        }

        public void DrawWarp(VFXBatch spriteBatch)
        {
            if (FirstVel == Vector2.Zero)
            {
                FirstVel = Vector2.Normalize(Projectile.velocity);
            }
            Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);

            List<Vertex2D> VxII = new List<Vertex2D>();
            List<Vertex2D> barsII = new List<Vertex2D>();

            int TrueL = 0;
            for (int i = 1; i < 60; ++i)
            {
                if (OldplCen[i] == Vector2.Zero)
                {
                    TrueL = i - 1;
                    break;
                }
            }
            for (int i = 1; i < 60; ++i)
            {
                if (OldplCen[i] == Vector2.Zero)
                    break;
                float widk = MathF.Sqrt(i * 75);
                Vector2 DeltaV0 = -OldplCen[i] + OldplCen[i - 1];
                float d = DeltaV0.ToRotation() + 3.14f + 1.57f;
                if (d > 6.28f)
                {
                    d -= 6.28f;
                }
                float dir = d / MathHelper.TwoPi;
                var factor = i / 60f;
                float factor2 = i / (float)(TrueL);

                Color c0 = new Color(dir, MathF.Sin(factor2 * 3.14159f), 0f, 0f);
                barsII.Add(new Vertex2D(OldplCen[i] + FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 1, 1 - factor2)));
                barsII.Add(new Vertex2D(OldplCen[i] - FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 0, 1 - factor2)));
            }
            if (barsII.Count > 2)
            {
                VxII.Add(barsII[0]);
                var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 90, new Color(255, 255, 255, 255), new Vector3(0, 0.5f, 1));
                VxII.Add(barsII[1]);
                VxII.Add(vertex);
                for (int i = 0; i < barsII.Count - 2; i += 2)
                {
                    VxII.Add(barsII[i]);
                    VxII.Add(barsII[i + 2]);
                    VxII.Add(barsII[i + 1]);

                    VxII.Add(barsII[i + 1]);
                    VxII.Add(barsII[i + 2]);
                    VxII.Add(barsII[i + 3]);
                }
            }


            spriteBatch.Draw(MythContent.QuickTexture("UIimages/VisualTextures/BladeShadow"), VxII, PrimitiveType.TriangleList);
        }
        public override void PostDraw(Color lightColor)
        {
            int TrueL = 0;
            for (int i = 1; i < 60; ++i)
            {
                if (OldplCen[i] == Vector2.Zero)
                {
                    TrueL = i - 1;
                    break;
                }
            }

            if (FirstVel == Vector2.Zero)
            {
                FirstVel = Vector2.Normalize(Projectile.velocity);
            }
            Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);

            for (int d = 0; d < 4; d++)
            {
                List<Vertex2D> VxII = new List<Vertex2D>();
                List<Vertex2D> barsII = new List<Vertex2D>();
                Vector2 deltaPos = new Vector2(0, 24).RotatedBy(d / 2d * Math.PI + Main.time / 32d);
                float widk = Vector2.Dot(Vector2.Normalize(deltaPos), Vector2.Normalize(Projectile.velocity)) + 2f;
                float widV = 2;
                if (d == 0)
                {
                    deltaPos *= 0;
                    statrP[d] = 1;
                }
                else
                {
                    if (statrP[d] == 0)
                    {
                        statrP[d] = Main.rand.NextFloat(1f, 2f);
                    }
                }
                for (int i = 1; i < 60; ++i)
                {
                    if (OldplCen[i] == Vector2.Zero)
                        break;
                    var factor = i / (float)(TrueL);
                    var w = statrP[d] - factor;
                    if (w > 1)
                    {
                        w = 2 - w;
                    }
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] + FlipVel * wid[i] * widk * widk * 0.6f - Main.screenPosition, new Color(0, 1f * (1 - factor), 0.8f * (1 - factor), 0), new Vector3((float)Math.Sqrt(factor), 1, w)));
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] - FlipVel * wid[i] * widk * widk * 0.6f - Main.screenPosition, new Color(0, 1f * (1 - factor), 0.8f * (1 - factor), 0), new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                if (barsII.Count > 2)
                {
                    VxII.Add(barsII[0]);
                    if (statrP[d] > 1)
                    {
                        statrP[d] = 2 - statrP[d];
                    }
                    Vector2 Vadd = (barsII[0].position + barsII[1].position) * 0.5f - (barsII[2].position + barsII[3].position) * 0.5f;
                    var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vadd * 2.7f, new Color(0, 1f, 0.8f, 0), new Vector3(0, 0.5f, statrP[d]));
                    VxII.Add(barsII[1]);
                    VxII.Add(vertex);
                    for (int i = 0; i < barsII.Count - 2; i += 2)
                    {
                        VxII.Add(barsII[i]);
                        VxII.Add(barsII[i + 2]);
                        VxII.Add(barsII[i + 1]);

                        VxII.Add(barsII[i + 1]);
                        VxII.Add(barsII[i + 2]);
                        VxII.Add(barsII[i + 3]);
                    }
                }

                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("UIimages/VisualTextures/EShoot");


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            int TrueL = 0;
            for (int i = 1; i < 60; ++i)
            {
                if (OldplCen[i] == Vector2.Zero)
                {
                    TrueL = i - 1;
                    break;
                }
            }
            if (FirstVel == Vector2.Zero)
            {
                FirstVel = Vector2.Normalize(Projectile.velocity);
            }
            Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);
            for (int d = 0; d < 7; d++)
            {
                List<Vertex2D> VxII = new List<Vertex2D>();
                List<Vertex2D> barsII = new List<Vertex2D>();
                Vector2 deltaPos = new Vector2(0, 24).RotatedBy(d / 3d * Math.PI + Main.time / 7d);
                
                float widV = 2;
                Color c0 = Color.White;
                if (d == 0)
                {
                    deltaPos *= 0;
                    c0 = new Color(255, 255, 255, 0);
                }

                for (int i = 1; i < 60; ++i)
                {
                    float widk = Vector2.Dot(Vector2.Normalize(deltaPos), Vector2.Normalize(Projectile.velocity)) + 1.2f;

                    widk += MathF.Sqrt(60 - i) * 0.11f;
                    if (Projectile.timeLeft < 60f)
                    {
                        widk *= Projectile.timeLeft / 60f;
                    }
                    if (OldplCen[i] == Vector2.Zero)
                        break;
                    var factor = i / (float)(TrueL);
                    var factor2 = i / (float)(TrueL);
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] + FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 1, 1 - factor2)));
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] - FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 0, 1 - factor2)));
                }
                if (barsII.Count > 2)
                {
                    VxII.Add(barsII[0]);
                    Vector2 Vadd = (barsII[0].position + barsII[1].position) * 0.5f - (barsII[2].position + barsII[3].position) * 0.5f;
                    var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vadd * 2.7f, new Color(255, 255, 255, 255), new Vector3(0, 0.5f, 1));
                    VxII.Add(barsII[1]);
                    VxII.Add(vertex);
                    for (int i = 0; i < barsII.Count - 2; i += 2)
                    {
                        VxII.Add(barsII[i]);
                        VxII.Add(barsII[i + 2]);
                        VxII.Add(barsII[i + 1]);

                        VxII.Add(barsII[i + 1]);
                        VxII.Add(barsII[i + 2]);
                        VxII.Add(barsII[i + 3]);
                    }
                }

                Texture2D t0 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapShadeXiao").Value;
                if (d == 0)
                {
                    t0 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapShadeXiaoGreen").Value;
                }
                Main.graphics.GraphicsDevice.Textures[0] = t0;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);
            }
            return true;
        }

        public static int CyanStrike = 0;
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            CyanStrike = 1;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Melee.Hepuyuan.XiaoHit>(), 0, 0, Projectile.owner, 0.45f);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void Load()
        {
            On.Terraria.CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
        }
        private int CombatText_NewText_Rectangle_Color_string_bool_bool(On.Terraria.CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Rectangle location, Color color, string text, bool dramatic, bool dot)
        {
            if (CyanStrike > 0)
            {
                color = new Color(0, 255, 174);
                CyanStrike--;
            }
            return orig(location, color, text, dramatic, dot);
        }
    }
}
