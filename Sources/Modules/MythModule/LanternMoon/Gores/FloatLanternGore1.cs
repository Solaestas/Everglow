using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Gores
{

    public class FloatLanternGore1 : ModGore
    {
        public override void SetStaticDefaults()
        {
            //GoreID.Sets.DisappearSpeed[ModContent.GoreType<Gores.FloatLanternGore1>()] = 3;
            //GoreID.Sets.DisappearSpeed[ModContent.GoreType<Gores.FloatLanternGore2>()] = 3;
            GoreID.Sets.DisappearSpeed[ModContent.GoreType<Gores.FloatLanternGore3>()] = 3;
            GoreID.Sets.DisappearSpeed[ModContent.GoreType<Gores.FloatLanternGore4>()] = 3;
            GoreID.Sets.DisappearSpeed[ModContent.GoreType<Gores.FloatLanternGore5>()] = 3;
            GoreID.Sets.DisappearSpeed[ModContent.GoreType<Gores.FloatLanternGore6>()] = 3;
        }
        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            return base.GetAlpha(gore, new Color(0, 0, 0, 0));
        }
        public override bool Update(Gore gore)
        {
            gore.velocity.Y -= 0.06f;
            return base.Update(gore);
        }
    }
    public class ShaderLanternGore
    {
        public static void Load()
        {
            On.Terraria.Main.DrawGore += DrawShaderLantern;
        }
        public static void UnLoad()
        {
            //On.Terraria.Main.DrawGore -= DrawShaderLantern;
        }
        public static Effect ef;
        public static Effect ef2;
        public static Vector2[] OldVelo = new Vector2[601];
        private static void DrawShaderLantern(On.Terraria.Main.orig_DrawGore orig, Terraria.Main self)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore1S").Value;
            Texture2D texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore1G").Value;
            Texture2D texB = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore1B").Value;
            for (int g = 0; g < Main.gore.Length; g++)
            {
                if (Main.gore[g].type == ModContent.GoreType<Gores.FloatLanternGore1>() && Main.gore[g].active)
                {
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore1S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore1G").Value;
                    texB = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore1B").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(4) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                        if (OldVelo[g] != Vector2.Zero)
                        {
                            if ((OldVelo[g] - Main.gore[g].velocity).Length() > 6)
                            {
                                for (int f = 0; f < (OldVelo[g] - Main.gore[g].velocity).Length() * 2; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                    Main.dust[r].noGravity = true;
                                    Main.dust[r].velocity = v3;
                                }
                                for (int f = 0; f < 3; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    Gore.NewGore(null, Main.gore[g].position, v3, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore" + (Main.rand.Next(4) + 3).ToString()).Type, 1f);
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).X) > 0.05)
                                {
                                    Main.gore[g].velocity.X *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).Y) > 0.05)
                                {
                                    Main.gore[g].velocity.Y *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                            }
                        }
                    }

                    OldVelo[g] = Main.gore[g].velocity;
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    Main.spriteBatch.Draw(texB, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.FloatLanternGore2>() && Main.gore[g].active)
                {
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore2S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore2G").Value;
                    texB = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore2B").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(4) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                        if (OldVelo[g] != Vector2.Zero)
                        {
                            if ((OldVelo[g] - Main.gore[g].velocity).Length() > 6)
                            {
                                for (int f = 0; f < (OldVelo[g] - Main.gore[g].velocity).Length() * 2; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                    Main.dust[r].noGravity = true;
                                    Main.dust[r].velocity = v3;
                                }
                                for (int f = 0; f < 3; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    Gore.NewGore(null, Main.gore[g].position, v3, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore" + (Main.rand.Next(4) + 3).ToString()).Type, 1f);
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).X) > 0.05)
                                {
                                    Main.gore[g].velocity.X *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).Y) > 0.05)
                                {
                                    Main.gore[g].velocity.Y *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                            }
                        }
                    }

                    OldVelo[g] = Main.gore[g].velocity;
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    Main.spriteBatch.Draw(texB, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.FloatLanternGore3>() && Main.gore[g].active)
                {
                    if (!Collision.SolidCollision(Main.gore[g].position, (int)Main.gore[g].Width, (int)Main.gore[g].Height))
                    {
                        Main.gore[g].velocity += new Vector2(0, 0.7f + (float)(Math.Sin(g)) / 4f).RotatedBy(g + Main.gore[g].timeLeft / 60f) / 10f;
                    }
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore3S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore3G").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(7) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                    }
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.FloatLanternGore4>() && Main.gore[g].active)
                {
                    if (!Collision.SolidCollision(Main.gore[g].position, (int)Main.gore[g].Width, (int)Main.gore[g].Height))
                    {
                        Main.gore[g].velocity += new Vector2(0, 0.7f + (float)(Math.Sin(g)) / 4f).RotatedBy(g + Main.gore[g].timeLeft / 60f) / 10f;
                    }
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore4S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore4G").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(7) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                    }
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.FloatLanternGore5>() && Main.gore[g].active)
                {
                    if (!Collision.SolidCollision(Main.gore[g].position, (int)Main.gore[g].Width, (int)Main.gore[g].Height))
                    {
                        Main.gore[g].velocity += new Vector2(0, 0.7f + (float)(Math.Sin(g)) / 4f).RotatedBy(g + Main.gore[g].timeLeft / 60f) / 10f;
                    }
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore5S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore5G").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(7) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                    }
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.FloatLanternGore6>() && Main.gore[g].active)
                {
                    if (!Collision.SolidCollision(Main.gore[g].position, (int)Main.gore[g].Width, (int)Main.gore[g].Height))
                    {
                        Main.gore[g].velocity += new Vector2(0, 0.7f + (float)(Math.Sin(g)) / 4f).RotatedBy(g + Main.gore[g].timeLeft / 60f) / 10f;
                    }
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore6S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore6G").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(7) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                    }
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }

                if (Main.gore[g].type == ModContent.GoreType<Gores.LanternGhostKingGore0>() && Main.gore[g].active)
                {
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore0S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore0G").Value;
                    texB = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore0B").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(4) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                        if (OldVelo[g] != Vector2.Zero)
                        {
                            if ((OldVelo[g] - Main.gore[g].velocity).Length() > 6)
                            {
                                for (int f = 0; f < (OldVelo[g] - Main.gore[g].velocity).Length() * 2; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                    Main.dust[r].noGravity = true;
                                    Main.dust[r].velocity = v3;
                                }
                                for (int f = 0; f < 13; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    int f0 = Gore.NewGore(null, Main.gore[g].position, v3, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore" + (Main.rand.Next(4) + 3).ToString()).Type, 1f);
                                    Main.gore[f0].timeLeft = Main.rand.Next(300, 600);
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).X) > 0.05)
                                {
                                    Main.gore[g].velocity.X *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).Y) > 0.05)
                                {
                                    Main.gore[g].velocity.Y *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                            }
                        }
                    }

                    OldVelo[g] = Main.gore[g].velocity;
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    Main.spriteBatch.Draw(texB, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.LanternGhostKingGore7>() && Main.gore[g].active)
                {
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore7S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore7G").Value;
                    texB = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore7B").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(4) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                        if (OldVelo[g] != Vector2.Zero)
                        {
                            if ((OldVelo[g] - Main.gore[g].velocity).Length() > 6)
                            {
                                for (int f = 0; f < (OldVelo[g] - Main.gore[g].velocity).Length() * 2; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                    Main.dust[r].noGravity = true;
                                    Main.dust[r].velocity = v3;
                                }
                                for (int f = 0; f < 3; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    Gore.NewGore(null, Main.gore[g].position, v3, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore" + (Main.rand.Next(4) + 3).ToString()).Type, 1f);
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).X) > 0.05)
                                {
                                    Main.gore[g].velocity.X *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).Y) > 0.05)
                                {
                                    Main.gore[g].velocity.Y *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                            }
                        }
                    }

                    OldVelo[g] = Main.gore[g].velocity;
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    Main.spriteBatch.Draw(texB, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.LanternGhostKingGore8>() && Main.gore[g].active)
                {
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore8S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore8G").Value;
                    texB = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore8B").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(4) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                        if (OldVelo[g] != Vector2.Zero)
                        {
                            if ((OldVelo[g] - Main.gore[g].velocity).Length() > 6)
                            {
                                for (int f = 0; f < (OldVelo[g] - Main.gore[g].velocity).Length() * 2; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                    Main.dust[r].noGravity = true;
                                    Main.dust[r].velocity = v3;
                                }
                                for (int f = 0; f < 3; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    Gore.NewGore(null, Main.gore[g].position, v3, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore" + (Main.rand.Next(4) + 3).ToString()).Type, 1f);
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).X) > 0.05)
                                {
                                    Main.gore[g].velocity.X *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).Y) > 0.05)
                                {
                                    Main.gore[g].velocity.Y *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                            }
                        }
                    }

                    OldVelo[g] = Main.gore[g].velocity;
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    Main.spriteBatch.Draw(texB, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.LanternGhostKingGore9>() && Main.gore[g].active)
                {
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore9S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore9G").Value;
                    texB = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore9B").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(4) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                        if (OldVelo[g] != Vector2.Zero)
                        {
                            if ((OldVelo[g] - Main.gore[g].velocity).Length() > 6)
                            {
                                for (int f = 0; f < (OldVelo[g] - Main.gore[g].velocity).Length() * 2; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                    Main.dust[r].noGravity = true;
                                    Main.dust[r].velocity = v3;
                                }
                                for (int f = 0; f < 3; f++)
                                {
                                    Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, (OldVelo[g] - Main.gore[g].velocity).Length())).RotatedByRandom(MathHelper.TwoPi);
                                    Gore.NewGore(null, Main.gore[g].position, v3, ModContent.Find<ModGore>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/FloatLanternGore" + (Main.rand.Next(4) + 3).ToString()).Type, 1f);
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).X) > 0.05)
                                {
                                    Main.gore[g].velocity.X *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                                if (Math.Abs((OldVelo[g] - Main.gore[g].velocity).Y) > 0.05)
                                {
                                    Main.gore[g].velocity.Y *= -0.8f;
                                    Main.gore[g].position += Main.gore[g].velocity;
                                }
                            }
                        }
                    }

                    OldVelo[g] = Main.gore[g].velocity;
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    Main.spriteBatch.Draw(texB, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.LanternGhostKingGore6>() && Main.gore[g].active)
                {
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore6S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore6G").Value;
                    if (!Collision.SolidCollision(Main.gore[g].position, (int)Main.gore[g].Width, (int)Main.gore[g].Height))
                    {
                        Main.gore[g].velocity += new Vector2(0, 0.7f + (float)(Math.Sin(g)) / 4f).RotatedBy(g + Main.gore[g].timeLeft / 60f) / 40f;
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(7) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                    }
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.LanternGhostKingGore5>() && Main.gore[g].active)
                {
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore5S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/LanternGhostKingGore5G").Value;
                    if (!Collision.SolidCollision(Main.gore[g].position, (int)Main.gore[g].Width, (int)Main.gore[g].Height))
                    {
                        Main.gore[g].velocity += new Vector2(0, 0.7f + (float)(Math.Sin(g)) / 4f).RotatedBy(g + Main.gore[g].timeLeft / 60f) / 40f;
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/LanternGore").Value;
                    ef.Parameters["minr"].SetValue((600 - Main.gore[g].timeLeft) / 600f);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    if ((600 - Main.gore[g].timeLeft) / 600f < 0.6f)
                    {
                        float Sc = Math.Clamp((1 - ((600 - Main.gore[g].timeLeft) / 600f) / 0.6f) * 2, 0, 1);
                        if (!Main.gamePaused)
                        {
                            if (Main.rand.Next(7) == 1)
                            {
                                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
                                int r = Dust.NewDust(Main.gore[g].position - new Vector2(4, 4), (int)Main.gore[g].Width, (int)Main.gore[g].Height, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default(Color), Main.rand.NextFloat(0.6f, 1f) * Sc);
                                Main.dust[r].noGravity = true;
                                Main.dust[r].velocity = Vector2.Zero;
                            }
                        }
                    }
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.PlatformGore>() && Main.gore[g].active)
                {
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    tex = TextureAssets.Tile[19].Value;
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, new Rectangle((int)Gores.PlatformGore.PlatFrame[g].X, (int)Gores.PlatformGore.PlatFrame[g].Y, 16, 16), cg, Main.gore[g].rotation, new Vector2(8), Main.gore[g].scale, SpriteEffects.None, 0);
                }

                if (Main.gore[g].type == ModContent.GoreType<Gores.XiaoDash0>() && Main.gore[g].active)
                {
                    if (!Collision.SolidCollision(Main.gore[g].position, (int)Main.gore[g].Width, (int)Main.gore[g].Height))
                    {
                        Main.gore[g].velocity += new Vector2(0, 0.7f + (float)(Math.Sin(g)) / 4f).RotatedBy(g + Main.gore[g].timeLeft / 60f) / 10f;
                    }
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/XiaoDash0S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/XiaoDash0G").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/EffectsXiaoDash").Value;
                    ef.Parameters["minr"].SetValue(1 - Main.gore[g].scale);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
                if (Main.gore[g].type == ModContent.GoreType<Gores.XiaoDash1>() && Main.gore[g].active)
                {
                    if (!Collision.SolidCollision(Main.gore[g].position, (int)Main.gore[g].Width, (int)Main.gore[g].Height))
                    {
                        Main.gore[g].velocity += new Vector2(0, 0.7f + (float)(Math.Sin(g)) / 4f).RotatedBy(g + Main.gore[g].timeLeft / 60f) / 10f;
                    }
                    tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/XiaoDash1S").Value;
                    texG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Gores/XiaoDash1G").Value;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/EffectsXiaoDash").Value;
                    ef.Parameters["minr"].SetValue(1 - Main.gore[g].scale);
                    ef.Parameters["uImage1"].SetValue(texG);

                    ef.CurrentTechnique.Passes["Test"].Apply();
                    Color cg = Lighting.GetColor((int)(Main.gore[g].position.X / 16f), (int)(Main.gore[g].position.Y / 16f));
                    ef.Parameters["BackCol"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - Main.gore[g].alpha) / 255f);
                    float alp = (255 - Main.gore[g].alpha) / 255f;
                    cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);
                    Main.spriteBatch.Draw(tex, Main.gore[g].position + new Vector2(Main.gore[g].Width / 2f, Main.gore[g].Height / 2f) - Main.screenPosition, null, cg, Main.gore[g].rotation, tex.Size() / 2, Main.gore[g].scale, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
            orig.Invoke(self);
        }
    }
}
