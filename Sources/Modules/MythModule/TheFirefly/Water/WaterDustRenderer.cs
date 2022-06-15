using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;

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

        public WaterDustRenderer()
        {
            m_dustLogicEffect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/DustLogic");
            m_dustDrawEffect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/DustDraw");
            m_dustTargetSwap = new RenderTarget2D[2];
            Everglow.MainThreadContext.AddTask(() =>
            {
                m_dustTargetSwap[0] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                    (int)(Main.screenWidth * 0.25f), (int)(Main.screenHeight * 0.25f), false, SurfaceFormat.Color,
                    DepthFormat.None);
                m_dustTargetSwap[1] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                    (int)(Main.screenWidth * 0.25f), (int)(Main.screenHeight * 0.25f), false, SurfaceFormat.Color,
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

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, 
                DepthStencilState.None, RasterizerState.CullNone);
            dustDraw.Apply();
            spriteBatch.Draw(CurrentDustTarget,
                new Rectangle(0, 0, CurrentDustTarget.Width * 4, CurrentDustTarget.Height * 4), Color.White);
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
            var dustLogicShader = m_dustLogicEffect.Value.CurrentTechnique.Passes[0];
            
            graphicsDevice.SetRenderTarget(NextDustTarget);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);
            {
                dustLogicShader.Apply();
                spriteBatch.Draw(CurrentDustTarget, new Rectangle((int)motionVector.X, (int)motionVector.Y,
                    CurrentDustTarget.Width, CurrentDustTarget.Height), Color.White);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(100, 100, 10, 10), Color.White);
            }
            spriteBatch.End();

            EndPass();
        }

        private void Main_OnResolutionChanged(Vector2 obj)
        {
            m_dustTargetSwap[0] = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)(Main.screenWidth * 0.25f),
                (int)(Main.screenHeight * 0.25f), false, SurfaceFormat.Color, DepthFormat.None);
            m_dustTargetSwap[1] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                (int)(Main.screenWidth * 0.25f), (int)(Main.screenHeight * 0.25f), false, SurfaceFormat.Color, DepthFormat.None);
        }

    }
}
