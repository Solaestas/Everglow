using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.LanternMoon.Gores;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Melee.Hepuyuan
{
    public class Hepuyuan : ModProjectile
    {
        protected virtual float HoldoutRangeMin => 24f;
        protected virtual float HoldoutRangeMax => 150f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hepuyuan");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "和璞鸢");

        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.  
            Projectile.extraUpdates = 6;
            Projectile.width = 80;
            Projectile.height = 80;
        }
        private bool fi = true;
        int kad = 0;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            OldplCen[0] = Projectile.Center - Vector2.Normalize(Projectile.velocity) * 15f;//记录位置
            if (Main.rand.Next(5) == 0)
            {
                Vector2 v1 = new Vector2(Main.rand.NextFloat(0.8f, 10f), 0).RotatedByRandom(6.28);
                Dust.NewDust(OldplCen[0] - new Vector2(4), 1, 1, ModContent.DustType<MiscDusts.XiaoDust>(), (Projectile.velocity * 0.35f + v1).X, (Projectile.velocity * 0.35f + v1).Y, 0, default(Color), Main.rand.NextFloat(0.85f, 1.25f));
            }
            if (Main.rand.Next(3) == 0)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(0.8f, 10f), 0).RotatedByRandom(6.28);
                if (Main.rand.Next(2) == 0)
                {
                    Gore.NewGore(null, OldplCen[0], Projectile.velocity * 0.35f + v0, ModContent.GoreType<XiaoDash0>(), Main.rand.NextFloat(0.65f, 1.25f));
                }
                else
                {
                    Gore.NewGore(null, OldplCen[0], Projectile.velocity * 0.35f + v0, ModContent.GoreType<XiaoDash1>(), Main.rand.NextFloat(0.65f, 1.25f));
                }
            }
            kad++;
            if (kad % 24 == 1 && Projectile.timeLeft > 30)
            {
                Vector2 v = Vector2.Normalize(Projectile.velocity);
                int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.Hepuyuan.XiaoBlackWave>(), 0, 0, player.whoAmI, Math.Clamp(Projectile.velocity.Length() / 8f, 0f, 4f), 0);
                Main.projectile[h].rotation = (float)(Math.Atan2(v.Y, v.X) + Math.PI / 2d);
            }
            for (int f = OldplCen.Length - 1; f > 0; f--)
            {
                OldplCen[f] = OldplCen[f - 1];
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
            float progress;

            if (Projectile.timeLeft < halfDuration + 2 && !max)
            {
                max = true;
            }
            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
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
            float dx = Projectile.timeLeft / (duration);
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
        private Effect ef;
        private Effect ef2;
        private bool max = false;
        float MaxR = 0.1f;
        Vector2 FirstVel = Vector2.Zero;
        float[] wid = new float[60];
        Vector2[] OldplCen = new Vector2[60];
        int TrueL = 0;
        float[] statrP = new float[4];
        public override void PostDraw(Color lightColor)
        {
            if (FirstVel == Vector2.Zero)
            {
                FirstVel = Vector2.Normalize(Projectile.velocity);
            }
            Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);
            //Texture2D tg = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Melee/Hepuyuan/HepuyuanGlow").Value;
            //float rot = (float)(Math.Atan2(FirstVel.Y, FirstVel.X) + Math.PI * 0.75);
            //Main.spriteBatch.Draw(tg, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Vector2.Normalize(Projectile.velocity) * 85f, null, new Color(255, 255, 255, 0), rot, new Vector2(85f, 85f), Projectile.scale, SpriteEffects.None, 0);
            for (int i = 1; i < 60; ++i)
            {
                TrueL++;
                if (OldplCen[i] == Vector2.Zero)
                    break;
            }
            Player player = Main.player[Projectile.owner];

            ef2 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeCurseGreen").Value;


            for (int d = 0; d < 4; d++)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                List<Vertex2D> VxII = new List<Vertex2D>();
                List<Vertex2D> barsII = new List<Vertex2D>();
                Vector2 deltaPos = new Vector2(0, 24).RotatedBy(d / 2d * Math.PI + Main.time / 32d);
                float widk = Vector2.Dot(Vector2.Normalize(deltaPos), Vector2.Normalize(Projectile.velocity)) + 1f;
                float widV = (float)Math.Clamp(1.6 - Math.Sqrt(Projectile.velocity.Length() / 16f), 0, 1.6f);
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
                    var factor = i / 60f;
                    var factor2 = i / (float)(TrueL);
                    var w = statrP[d] - factor;
                    if (w > 1)
                    {
                        w = 2 - w;
                    }
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] + FlipVel * wid[i] * widk * widk * 0.6f, new Color(255, 255, 255, 100), new Vector3((float)Math.Sqrt(factor), 1, w)));
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] - FlipVel * wid[i] * widk * widk * 0.6f, new Color(255, 255, 255, 100), new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                if (barsII.Count > 2)
                {
                    VxII.Add(barsII[0]);
                    if (statrP[d] > 1)
                    {
                        statrP[d] = 2 - statrP[d];
                    }
                    var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(255, 255, 255, 100), new Vector3(0, 0.5f, statrP[d]));
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

                //Texture2D t0 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapShadeXiaoGreen").Value;
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                ef2.Parameters["uTransform"].SetValue(model * projection);
                ef2.Parameters["alphaValue"].SetValue(widk * widk / 36f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapWindCyan").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ShakeWave").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ShakeWave").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                ef2.CurrentTechnique.Passes[0].Apply();

                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (FirstVel == Vector2.Zero)
            {
                FirstVel = Vector2.Normalize(Projectile.velocity);
            }
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeCurseGreen").Value;
            Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);
            for (int d = 0; d < 7; d++)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                List<Vertex2D> VxII = new List<Vertex2D>();
                List<Vertex2D> barsII = new List<Vertex2D>();
                Vector2 deltaPos = new Vector2(0, 24).RotatedBy(d / 3d * Math.PI + Main.time / 7d);
                float widk = Vector2.Dot(Vector2.Normalize(deltaPos), Vector2.Normalize(Projectile.velocity)) + 1.2f;
                float widV = (float)Math.Clamp(1.6 - Math.Sqrt(Projectile.velocity.Length() / 16f), 0, 1.6f);
                Color c0 = Color.White;
                if (d == 0)
                {
                    deltaPos *= 0;
                    widk = 4f * Projectile.timeLeft / 60f;
                    c0 = new Color(255, 255, 255, 0);
                }
                for (int i = 1; i < 60; ++i)
                {
                    if (OldplCen[i] == Vector2.Zero)
                        break;
                    var factor = i / 60f;
                    var factor2 = i / (float)(TrueL);
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] + FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 1, 1 - factor2)));
                    barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV + OldplCen[i] - FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 0, 1 - factor2)));
                }
                if (barsII.Count > 2)
                {
                    VxII.Add(barsII[0]);
                    var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(255, 255, 255, 255), new Vector3(0, 0.5f, 1));
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
                Main.graphics.GraphicsDevice.Textures[0] = t0;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            }

            return true;
        }
        float Adding = 0;
        public static int CyanStrike = 0;
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            CyanStrike = 1;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.Hepuyuan.XiaoHit>(), 0, 0, Projectile.owner, 0.45f);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void Load()
        {
            On.Terraria.CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
        }

        public override void Unload()
        {
            //On.Terraria.CombatText.NewText_Rectangle_Color_string_bool_bool -= CombatText_NewText_Rectangle_Color_string_bool_bool;
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
