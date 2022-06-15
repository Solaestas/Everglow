using Everglow.Sources.Commons.Core.ModuleSystem;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Water
{
    public class FireFlyWaterSystem : ModSystem
    {
        private WaterDustRenderer m_waterDustRenderer;

        public override void OnModLoad()
        {
            if (!Main.dedServ)
            {
                m_waterDustRenderer = new WaterDustRenderer();
            }
            base.OnModLoad();
        }

        public override void PostDrawTiles()
        {
            m_waterDustRenderer.PresentDusts();
            base.PostDrawTiles();
        }
    }
    internal class WaterDustRenderer
    {
        private RenderTarget2D[] m_dustTargetSwap = null;
        private int m_currentDustTarget;
        private Vector2 m_lastDrawPosition;
        private Asset<Effect> m_dustLogicEffect; 
        private Asset<Effect> m_dustDrawEffect;
        private Asset<Effect> m_dustSpawnEffect;

        private RenderTarget2D CurrentDustTarget
        {
            get
            {
                return m_dustTargetSwap[m_currentDustTarget];
            }
        }

        private RenderTarget2D NextDustTarget
        {
            get
            {
                return m_dustTargetSwap[m_currentDustTarget ^ 1];
            }
        }

        private float scale
        {
            get
            {
                return 1f;
            }
        }

        public string Name => throw new NotImplementedException();

        public WaterDustRenderer()
        {
            m_dustLogicEffect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/DustLogic");
            m_dustDrawEffect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/DustDraw");
            m_dustSpawnEffect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/DustSpawn");
            m_dustTargetSwap = new RenderTarget2D[2];
            Everglow.MainThreadContext.AddTask(() =>
            {
                m_dustTargetSwap[0] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                    (int)Math.Floor(Main.screenWidth * scale), (int)Math.Floor(Main.screenHeight * scale), false, SurfaceFormat.Color,
                    DepthFormat.None);
                m_dustTargetSwap[1] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                    (int)Math.Floor(Main.screenWidth * scale), (int)Math.Floor(Main.screenHeight * scale), false, SurfaceFormat.Color,
                    DepthFormat.None);
            });

            m_currentDustTarget = 0;
            m_lastDrawPosition = Vector2.Zero;

            Main.OnResolutionChanged += Main_OnResolutionChanged;
            Main.OnPreDraw += Main_OnPreDraw;
        }



        public void PresentDusts()
        {
            if (CurrentDustTarget == null || NextDustTarget == null)
            {
                return;
            }
            var spriteBatch = Main.spriteBatch;
            var graphicsDevice = Main.graphics.GraphicsDevice;
            m_dustDrawEffect.Wait();
            var dustDraw = m_dustDrawEffect.Value.CurrentTechnique.Passes[0];

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, 
                DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            dustDraw.Apply();
            spriteBatch.Draw(CurrentDustTarget,
                new Rectangle(0, 0, (int)(CurrentDustTarget.Width / scale), (int)(CurrentDustTarget.Height / scale)), Color.White);
            spriteBatch.End();
        }

        private void EndPass()
        {
            var graphicsDevice = Main.graphics.GraphicsDevice;
            m_lastDrawPosition = Main.screenPosition;
            m_currentDustTarget = (m_currentDustTarget + 1) % 2;
            graphicsDevice.SetRenderTarget(null);
        }

        private void Main_OnPreDraw(GameTime obj)
        {
            if (CurrentDustTarget == null || NextDustTarget == null)
            {
                return;
            }

            var graphicsDevice = Main.graphics.GraphicsDevice;
            var spriteBatch = Main.spriteBatch;
            var tileBatch = Main.tileBatch;


            var motionVector = m_lastDrawPosition - Main.screenPosition;

            m_dustLogicEffect.Wait();
            m_dustSpawnEffect.Wait();
            var dustLogicShader = m_dustLogicEffect.Value.CurrentTechnique.Passes[0];
            var dustSpawnShader = m_dustSpawnEffect.Value.CurrentTechnique.Passes[0];
            graphicsDevice.SetRenderTarget(NextDustTarget);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);
            {
                dustLogicShader.Apply();
                spriteBatch.Draw(CurrentDustTarget, motionVector * scale, Color.White);
            }
            spriteBatch.End();

            var waterShader = ((WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader());
            var disortionTarget = (RenderTarget2D)typeof(WaterShaderData).GetField("_distortionTarget", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(waterShader);
            var lastDistortionDrawOffset = (Vector2)typeof(WaterShaderData).GetField("_lastDistortionDrawOffset", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(waterShader);
            Vector2 value = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
            Vector2 value2 = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) - Main.screenPosition - value;
            Vector2 offset = -(value2 * 0.25f - lastDistortionDrawOffset) / new Vector2(disortionTarget.Width, disortionTarget.Height);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);
            {
                graphicsDevice.Textures[1] = disortionTarget;
                graphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

                m_dustSpawnEffect.Value.Parameters["uZoom"].SetValue(Main.GameViewMatrix.Zoom);
                m_dustSpawnEffect.Value.Parameters["uOffset"].SetValue(offset);
                m_dustSpawnEffect.Value.Parameters["uThreashold"].SetValue(0.02f);

                dustSpawnShader.Apply();
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, NextDustTarget.Width, NextDustTarget.Height), Color.White);
            }
            spriteBatch.End();

            EndPass();
        }

        private void Main_OnResolutionChanged(Vector2 obj)
        {
            m_dustTargetSwap[0] = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)Math.Floor(Main.screenWidth * scale),
                (int)Math.Floor(Main.screenHeight * scale), false, SurfaceFormat.Color, DepthFormat.None);
            m_dustTargetSwap[1] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                (int)Math.Floor(Main.screenWidth * scale), (int)Math.Floor(Main.screenHeight * scale), false, SurfaceFormat.Color, DepthFormat.None);
        }

        public void Unload()
        {
            Main.OnPreDraw -= Main_OnPreDraw;
            Main.OnResolutionChanged -= Main_OnResolutionChanged;
        }
    }
}
