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
        }

        public override void PostDrawTiles()
        {
            m_waterDustRenderer.PresentDusts();
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

        private int m_oldScreenWidth, m_oldScreenHeight;

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

        /// <summary>
        /// DEBUG usage
        /// </summary>
        private float Scale
        {
            get
            {
                return 1f;
            }
        }

        public WaterDustRenderer()
        {
            m_dustLogicEffect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/DustLogic");
            m_dustDrawEffect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/DustDraw");
            m_dustSpawnEffect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/DustSpawn");
            m_dustTargetSwap = new RenderTarget2D[2];
            Everglow.MainThreadContext.AddTask(() =>
            {
                var parameters = Main.instance.GraphicsDevice.PresentationParameters;
                m_dustTargetSwap[0] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                    (int)Math.Floor(parameters.BackBufferWidth * Scale),
                    (int)Math.Floor(parameters.BackBufferHeight * Scale), 
                    false, SurfaceFormat.Color,
                    DepthFormat.None);
                m_dustTargetSwap[1] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                    (int)Math.Floor(parameters.BackBufferWidth * Scale),
                    (int)Math.Floor(parameters.BackBufferHeight * Scale), 
                    false, SurfaceFormat.Color,
                    DepthFormat.None);
            });

            m_currentDustTarget = 0;
            m_lastDrawPosition = Vector2.Zero;

            Main.OnPreDraw += Main_OnPreDraw;
        }

        public void PresentDusts()
        {
            if (CurrentDustTarget == null || NextDustTarget == null)
            {
                return;
            }

            if(!m_dustDrawEffect.IsLoaded || m_dustDrawEffect.IsDisposed)
            {
                return;
            }

            if (m_oldScreenWidth != Main.screenWidth || m_oldScreenHeight != Main.screenHeight)
            {
                OnResolutionChanged();
                m_oldScreenWidth = Main.screenWidth;
                m_oldScreenHeight = Main.screenHeight;
            }
            var spriteBatch = Main.spriteBatch;
            var graphicsDevice = Main.graphics.GraphicsDevice;
            m_dustDrawEffect.Wait();
            var dustDraw = m_dustDrawEffect.Value.CurrentTechnique.Passes[0];

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            m_dustDrawEffect.Value.Parameters["uColor"].SetValue(new Vector3(1f, 1.0f, 1.0f));
            m_dustDrawEffect.Value.Parameters["uResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            m_dustDrawEffect.Value.Parameters["uResolutionInv"].SetValue(new Vector2(1f / Main.screenWidth, 1f / Main.screenHeight));
            dustDraw.Apply();
            spriteBatch.Draw(CurrentDustTarget,
                new Rectangle(0, 0, (int)(CurrentDustTarget.Width / Scale), (int)(CurrentDustTarget.Height / Scale)), Color.White);
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
            if(Lighting.Mode != Terraria.Graphics.Light.LightMode.Color)
            {
                return;
            }

            if (CurrentDustTarget == null || NextDustTarget == null
                || !m_dustLogicEffect.IsLoaded || !m_dustSpawnEffect.IsLoaded
                || m_dustDrawEffect.IsDisposed || m_dustDrawEffect.IsDisposed)
            {
                return;
            }

            var graphicsDevice = Main.graphics.GraphicsDevice;
            var spriteBatch = Main.spriteBatch;


            var motionVector = m_lastDrawPosition - Main.screenPosition;

            if (!Main.gamePaused)
            {
                var waterShader = (WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader();
                var disortionTarget = (RenderTarget2D)typeof(WaterShaderData).GetField("_distortionTarget", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(waterShader);
                var lastDistortionDrawOffset = (Vector2)typeof(WaterShaderData).GetField("_lastDistortionDrawOffset", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(waterShader);
                Vector2 value = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
                Vector2 value2 = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) - Main.screenPosition - value;
                Vector2 offset = -(value2 * 0.25f - lastDistortionDrawOffset) / new Vector2(disortionTarget.Width, disortionTarget.Height);
                Vector2 targetPos = Main.screenPosition - Main.sceneWaterPos
                    + new Vector2(Main.offScreenRange, Main.offScreenRange) + value - new Vector2(Main.offScreenRange, Main.offScreenRange);

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
                    spriteBatch.Draw(CurrentDustTarget, motionVector * Scale, Color.White);
                }
                spriteBatch.End();
                Vector2 screenSizeZoom = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp,
                    DepthStencilState.None, RasterizerState.CullNone);
                {
                    graphicsDevice.Textures[1] = disortionTarget;
                    graphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
                    graphicsDevice.Textures[2] = Main.waterTarget;
                    graphicsDevice.SamplerStates[2] = SamplerState.PointClamp;

                    m_dustSpawnEffect.Value.Parameters["uResolution"].SetValue(screenSizeZoom);
                    m_dustSpawnEffect.Value.Parameters["uTargetPos"].SetValue(targetPos);
                    m_dustSpawnEffect.Value.Parameters["uInvWaterSize"].SetValue(new Vector2(1f / Main.waterTarget.Width, 1f / Main.waterTarget.Height));
                    m_dustSpawnEffect.Value.Parameters["uZoom"].SetValue(new Vector2(1f / Main.GameViewMatrix.Zoom.X, 1f / Main.GameViewMatrix.Zoom.Y));
                    m_dustSpawnEffect.Value.Parameters["uOffset"].SetValue(offset);
                    m_dustSpawnEffect.Value.Parameters["uThreasholdMin"].SetValue(0.03f);
                    m_dustSpawnEffect.Value.Parameters["uThreasholdMax"].SetValue(0.08f);
                    m_dustSpawnEffect.Value.Parameters["uSpawnChance"].SetValue(1f);
                    m_dustSpawnEffect.Value.Parameters["uVFXTime"].SetValue((float)Main.timeForVisualEffects * 0.2f);

                    dustSpawnShader.Apply();
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, NextDustTarget.Width, NextDustTarget.Height), Color.White);
                }
                spriteBatch.End();
                EndPass();
            }


        }

        private void OnResolutionChanged()
        {
            m_dustTargetSwap[0] = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)Math.Floor(Main.screenWidth * Scale),
                (int)Math.Floor(Main.screenHeight * Scale), false, SurfaceFormat.Color, DepthFormat.None);
            m_dustTargetSwap[1] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                (int)Math.Floor(Main.screenWidth * Scale), (int)Math.Floor(Main.screenHeight * Scale), false, SurfaceFormat.Color, DepthFormat.None);
        }

        public void Unload()
        {
            Main.OnPreDraw -= Main_OnPreDraw;
        }
    }
}
