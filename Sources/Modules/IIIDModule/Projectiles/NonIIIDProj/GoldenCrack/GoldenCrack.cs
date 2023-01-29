using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.ID;
using Everglow.Sources.Modules.MEACModule;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using ReLogic.Content;


namespace Everglow.Sources.Modules.IIIDModule.Projectiles.NonIIIDProj.GoldenCrack
{
    public class GoldenCrack : ModProjectile ,IBloomProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 40;
            Projectile.tileCollide = false;
            Projectile.scale = 0.8f;
            Projectile.alpha = 60;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }

        public override void AI()
        {
            if (Projectile.timeLeft >= 28)
            {
                Projectile.ai[1] = 1f;
                if (Projectile.timeLeft % 4 == 0)
                {
                    Projectile.oldPos[(int)Projectile.ai[0]] = Projectile.Center;
                    Projectile obj = Projectile;
                    obj.Center = obj.Center + Utils.RotatedBy(Projectile.velocity, (double)((Projectile.ai[0] % 2f == 0f ? 1 : (-1)) * 0.4f));
                    Projectile obj2 = Projectile;
                    obj2.Center = obj2.Center + 2f * Utils.NextVector2Unit(Main.rand, 0f, (float)Math.PI * 2f * 30f);
                    Projectile.ai[0] += 1f;
                }
            }
            else
            {
                Projectile.ai[1] -= 1f / 24f;
            }
            Projectile obj3 = Projectile;
            obj3.Center = (obj3.Center - Projectile.velocity);
        }

        public override bool PreDraw(ref Color lightColor)
        {

            List<Vertex2D> vertices = new List<Vertex2D>();
            Color color = default(Color);
            color = new Color(150, 0, 200);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (!(Projectile.oldPos[i] == Vector2.Zero))
                {
                    if (i == 0 || i == 3)
                    {
                        vertices.Add(new Vertex2D(Projectile.oldPos[i] - Main.screenPosition, new Color(200, 100, 200), new Vector3(1f, 0f, 1f)));
                    }
                    if (i == 1 || i == 2)
                    {
                        Vector2 normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                        normalDir = Vector2.Normalize(new Vector2(0f - normalDir.Y, normalDir.X));
                        float width = 15f * Projectile.ai[1];
                        vertices.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width - Main.screenPosition, color, new Vector3(1f, 0f, 1f)));
                        vertices.Add(new Vertex2D(Projectile.oldPos[i] - normalDir * width - Main.screenPosition, color, new Vector3(1f, 0f, 1f)));
                    }
                }
            }
            Main.graphics.GraphicsDevice.Textures[1] = TextureAssets.MagicPixel.Value;
            if (vertices.Count > 2)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives<Vertex2D>((PrimitiveType)1, vertices.ToArray(), 0, vertices.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            bool flag = false;
            float width = 20f * Projectile.ai[1];
            for (int i = 0; i < Projectile.oldPos.Length - 1 && !(Projectile.oldPos[i + 1] == Vector2.Zero); i++)
            {
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.oldPos[i], Projectile.oldPos[i + 1], width * 2f, ref point))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public void DrawBloom()
        {
            Color color = Color.White;
            this.PreDraw(ref color);
        }
    }
    public class GoldenCrackEffect : ModSystem
    {
        RenderTarget2D render, screen;
        Effect GoldenCrack;
        public override void OnModLoad()
        {
            GoldenCrack = ModContent.Request<Effect>("Everglow/Sources/Modules/IIIDModule/Effects/Cosmic").Value;
            On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;//原版绘制场景的最后部分——滤镜。在这里运用render保证不会与原版冲突
            Main.OnResolutionChanged += Main_OnResolutionChanged;
            On.Terraria.Main.LoadWorlds += Main_OnLoadWorlds;
            base.OnModLoad();
        }

        public override void OnModUnload()
        {
            On.Terraria.Graphics.Effects.FilterManager.EndCapture -= FilterManager_EndCapture;
            Main.OnResolutionChanged -= Main_OnResolutionChanged;
            On.Terraria.Main.LoadWorlds -= Main_OnLoadWorlds;
            base.OnModUnload();
        }
        private void GetOrig(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(screen);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend);
            Main.spriteBatch.Draw((Texture2D)(object)Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
        }
        private void UseCosmic(GraphicsDevice graphicsDevice)
        {
            GoldenCrack = ModContent.Request<Effect>("Everglow/Sources/Modules/IIIDModule/Effects/Cosmic").Value;
            bool use = false;
            GetOrig(graphicsDevice);
            graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
            foreach (Projectile proj in Main.projectile)
            {
                if (!(proj).active)
                {
                    continue;
                }
                if (proj.type == ModContent.ProjectileType<GoldenCrack>())
                {
                    Color c3 = Color.White;
                    ((proj.ModProjectile as GoldenCrack)).PreDraw(ref c3);
                }
            }
            Main.spriteBatch.End();
            graphicsDevice.SetRenderTarget(Main.screenTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend);
            Main.spriteBatch.Draw((Texture2D)(object)screen, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
            GoldenCrack.CurrentTechnique.Passes[0].Apply();
            GoldenCrack.Parameters["m"].SetValue(0.1f);
            GoldenCrack.Parameters["t"].SetValue(0.1f * ((float)Math.Sin(Main.timeForVisualEffects * 0.01) * 0.5f + 0.5f));
            GoldenCrack.Parameters["tex0"].SetValue((Texture)(object)ModContent.Request<Texture2D>("Everglow/Sources/Modules/Core/IIIDModule/Projectiles/NonIIIDProj/GoldenCrack/GoldenCrack").Value);
            Main.spriteBatch.Draw((Texture2D)(object)Main.screenTargetSwap, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
        }
        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, Microsoft.Xna.Framework.Graphics.RenderTarget2D finalTexture, Microsoft.Xna.Framework.Graphics.RenderTarget2D screenTarget1, Microsoft.Xna.Framework.Graphics.RenderTarget2D screenTarget2, Microsoft.Xna.Framework.Color clearColor)
        {
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;

            if (render == null)
            {
                CreateRender();
            }
            if (gd == null)
            {
                return;
            }
            UseCosmic(gd);
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        private void Main_OnResolutionChanged(Vector2 obj)//在分辨率更改时，重建render防止某些bug
        {
            if (render != null)
            {
                CreateRender();
            }
        }
        private void Main_OnLoadWorlds(On.Terraria.Main.orig_LoadWorlds orig)
        {
            if (render != null)
            {
                CreateRender();
            }
            orig();
        }
        public void CreateRender()
        {
            render = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            screen = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
        }
    }

}

