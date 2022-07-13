using Everglow.Sources.Commons.Function.Vertex;
using Terraria.GameContent;
namespace Everglow.Sources.Modules.MEACModule.NonTrueMeleeProj
{
    public class StonePost : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1800;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.hide = true;
            if (Projectile.timeLeft <= 27 && Projectile.timeLeft >= 15 && Projectile.timeLeft % 3 == 0)
            {
                for (int x = 0; x < 12; x++)
                {
                    float X = (float)(Math.Sqrt(Main.rand.NextFloat(0, 0.5f)));
                    float Y = (float)(Math.Sqrt(Main.rand.NextFloat(0, 1)));
                    Vector2 v0 = RotByPro(new Vector2(X * Math.Sign(Main.rand.NextFloat(-1, 1)) * 27, -Y * 153));
                    int k = Dust.NewDust(Projectile.Center + v0 - new Vector2(4), 0, 0, DustID.GoldFlame, 0, 0, 0, default, Main.rand.NextFloat(0.8f, 2f));
                    Main.dust[k].noGravity = true;
                }
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        public void DrawDoubleLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2)
        {
            Vector2 DrawCen = Projectile.Center - Main.screenPosition;
            float Wid = 1.5f;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;
            Vector2 WidthS = Vector2.Normalize(StartPos - DrawCen).RotatedBy(Math.PI / 2d) * Wid;
            Vector2 WidthE = Vector2.Normalize(EndPos - DrawCen).RotatedBy(Math.PI / 2d) * Wid;
            List<Vertex2D> vertex2Ds = new List<Vertex2D>();

            for(int x = 0;x < 3;x++)
            {
                vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
            }

            
            Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }
        public Vector2 RotByPro(Vector2 orig)
        {
            return orig.RotatedBy(Projectile.rotation);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D BackG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/StonePostBackGround").Value;
            Texture2D Front = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/StonePost").Value;
            Texture2D FaceBackG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/StonePostFaceBackGround").Value;
            Texture2D FaceBackGGlow = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/StonePostFaceBackGroundGlow").Value;
            Texture2D Root = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/StonePostRoot").Value;

            lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16));
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            float WaveRange = 1.7f;

            float k0 = (float)Math.Sqrt(1 - (Projectile.timeLeft * 0.004) % 1) * 2;//画方波
            float k1 = 1 - k0;
            float k2 = k1 * k1;
            float k3 = (float)Math.Sqrt(k1);
            Vector2 DrawCen = Projectile.Center - Main.screenPosition;
            if (k0 < 1)
            {
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(0, -k0 * 40)) * WaveRange, DrawCen + RotByPro(new Vector2(k0 * 75, -k0 * 20)) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(k0 * 75, -k0 * 20)) * WaveRange, DrawCen + RotByPro(new Vector2(k0 * 150, 0)) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(0, -k0 * 40)) * WaveRange, DrawCen + RotByPro(new Vector2(-k0 * 75, -k0 * 20)) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(-k0 * 75, -k0 * 20)) * WaveRange, DrawCen + RotByPro(new Vector2(-k0 * 150, 0)) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
            }



            if (Projectile.timeLeft >= 10)
            {
                Main.spriteBatch.Draw(BackG, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), 1, SpriteEffects.None, 0);


                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                    Effect efS = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/StoneColorII", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                    efS.Parameters["Str"].SetValue(0.1f);
                    efS.Parameters["uTime"].SetValue((float)(-Projectile.timeLeft * 0.004));
                    efS.Parameters["tex0"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/StonePostLight", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
                    efS.CurrentTechnique.Passes[0].Apply();

                    Main.spriteBatch.Draw(BackG, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 255), Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), 1, SpriteEffects.None, 0);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }

                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                    Effect efS = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/StoneColorII", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                    efS.Parameters["Str"].SetValue(0.08f);
                    efS.Parameters["uTime"].SetValue((float)(-Projectile.timeLeft * 0.004));
                    efS.Parameters["tex0"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/StonePostLight", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
                    efS.CurrentTechnique.Passes[0].Apply();

                    Main.spriteBatch.Draw(BackG, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 255), Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), new Vector2(0.1f, 1f), SpriteEffects.None, 0);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
                Main.spriteBatch.Draw(FaceBackG, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), 1, SpriteEffects.None, 0);
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                    Effect efS = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/StoneColorII", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                    efS.Parameters["Str"].SetValue(0.1f);
                    efS.Parameters["uTime"].SetValue((float)(Projectile.timeLeft * 0.004 + 0.7));

                    efS.Parameters["tex0"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/StonePostLight", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
                    efS.CurrentTechnique.Passes[0].Apply();

                    Main.spriteBatch.Draw(FaceBackGGlow, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 255), Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), new Vector2(0.9f, 1f), SpriteEffects.None, 0);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }

                Main.spriteBatch.Draw(Front, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), 1, SpriteEffects.None, 0);

                Main.spriteBatch.Draw(Root, Projectile.Center - Main.screenPosition + RotByPro(new Vector2(0, 0)), null, lightColor, Projectile.rotation, Root.Size() / 2f, 1, SpriteEffects.None, 0);


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                if(k0 < 1)//画方波
                {
                    DrawDoubleLine(DrawCen + RotByPro(new Vector2(0, k0 * 50)) * WaveRange, DrawCen + RotByPro(new Vector2(k0 * 75, k0 * 25)) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                    DrawDoubleLine(DrawCen + RotByPro(new Vector2(k0 * 75, k0 * 25)) * WaveRange, DrawCen + RotByPro(new Vector2(k0 * 150, 0)) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                    DrawDoubleLine(DrawCen + RotByPro(new Vector2(0, k0 * 50)) * WaveRange, DrawCen + RotByPro(new Vector2(-k0 * 75, k0 * 25)) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                    DrawDoubleLine(DrawCen + RotByPro(new Vector2(-k0 * 75, k0 * 25)) * WaveRange, DrawCen + RotByPro(new Vector2(-k0 * 150, 0)) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                }

            }
            int LeftTime = 1800 - Projectile.timeLeft;
            float glowStrength = 0;
            if (LeftTime < 10)
            {
                glowStrength = (float)(-Math.Cos(LeftTime / 10d * Math.PI) + 1) / 2f;
            }
            else if(LeftTime < 40)
            {
                glowStrength = (float)(-Math.Cos((LeftTime + 20) / 30d * Math.PI) + 1) / 2f;
            }

            if (LeftTime < 40)
            {
                Player player = Main.player[Projectile.owner];
                float k = LeftTime / 40f;
                k = (float)Math.Sqrt(k);
                float Rot = (float)(-Math.PI * 0.6f) * player.direction * k;
                player.SetCompositeArmBack(true,Player.CompositeArmStretchAmount.Full, Rot);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, -Rot);
            }
            if (LeftTime >= 40 && LeftTime < 60)
            {
                Player player = Main.player[Projectile.owner];
                float k = (60 - LeftTime) / 20f;
                k = (float)Math.Sqrt(k);
                float Rot = (float)(-Math.PI * 0.6f) * player.direction * k;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Rot);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, -Rot);
            }

            if (Projectile.timeLeft < 10)
            {
                glowStrength = (float)(-Math.Cos(Projectile.timeLeft / 10d * Math.PI) + 1) / 2f;
            }
            else if (Projectile.timeLeft < 40)
            {
                glowStrength = (float)(-Math.Cos((Projectile.timeLeft + 20) / 30d * Math.PI) + 1) / 2f;
            }
            if (glowStrength > 0)//消失光效
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                Effect efS = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/StoneColor", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                efS.Parameters["Str"].SetValue(glowStrength);
                efS.Parameters["tex0"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/img_color", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
                efS.CurrentTechnique.Passes[0].Apply();

                Main.spriteBatch.Draw(BackG, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 255), Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), 1, SpriteEffects.None, 0);

                Main.spriteBatch.Draw(Root, Projectile.Center - Main.screenPosition + RotByPro(new Vector2(0, 0)), null, lightColor, Projectile.rotation, Root.Size() / 2f, 1, SpriteEffects.None, 0);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            
            return false;
        }
        public void DrawWarp()
        {
            float WaveRange = 1.7f;
            Texture2D BackG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/Black").Value;

            float k0 = (float)Math.Sqrt(1 - (Projectile.timeLeft * 0.004) % 1) * 2;//画方波
            k0 = Math.Max(k0 - 0.025f, 0);
            float k1 = 1 - k0;
            float k2 = k1 * k1;
            float k3 = (float)Math.Sqrt(k1);
            float Gdir = Main.player[Projectile.owner].gravDir;

            Vector2 DrawCen = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            if (k0 < 1)
            {
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(0, -k0 * 40)) * WaveRange, DrawCen + RotByPro(new Vector2(k0 * 75, -k0 * 20)) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(k0 * 75, -k0 * 20)) * WaveRange, DrawCen + RotByPro(new Vector2(k0 * 150, 0)) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(0, -k0 * 40)) * WaveRange, DrawCen + RotByPro(new Vector2(-k0 * 75, -k0 * 20)) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(-k0 * 75, -k0 * 20)) * WaveRange, DrawCen + RotByPro(new Vector2(-k0 * 150, 0)) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.Draw(BackG, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 255), Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), 1, SpriteEffects.None, 0);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            //Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            if (k0 < 1)
            {
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(0, k0 * 50)) * WaveRange, DrawCen + RotByPro(new Vector2(k0 * 75, k0 * 25)) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(k0 * 75, k0 * 25)) * WaveRange, DrawCen + RotByPro(new Vector2(k0 * 150, 0)) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(0, k0 * 50)) * WaveRange, DrawCen + RotByPro(new Vector2(-k0 * 75, k0 * 25)) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
                DrawDoubleLine(DrawCen + RotByPro(new Vector2(-k0 * 75, k0 * 25)) * WaveRange, DrawCen + RotByPro(new Vector2(-k0 * 150, 0)) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}