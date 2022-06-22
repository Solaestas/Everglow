using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.UI;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds;
using Everglow.Sources.Modules.MythModule.TheFirefly.UI;
using Everglow.Sources.Modules.MythModule.TheTusk.Configs;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Shaders;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Sky
{
    public class FogPass
    {
        private Asset<Effect> m_boxKernelEffect;
        private Asset<Effect> m_gaussianKernelEffect;
        private Asset<Effect> m_fogScreenEffect;

        private RenderTarget2D[] m_downSampleRenderTargets;
        private RenderTarget2D[] m_upSampleRenderTargets;

        private RenderTarget2D m_renderTargetSwap;
        private RenderTarget2D m_filteredScreenTarget;
        private Color[] m_lightMap;
        private Texture2D m_lightTexture;

        private int m_bloomRadius;
        private int m_offscreenTilesSize;

        private int m_frameWidth, m_frameHeight;
        private int m_screenWidth, m_screenHeight;
        private int m_tileWidth, m_tileHeight;
        private bool m_shouldResetRenderTargets;
        private bool m_enableGaussian, m_enableProgressiveUpSampling;

        private readonly int MAX_DOWNSAMPLE_LEVEL = 4;
        private readonly int MAX_UPSAMPLE_LEVEL = 4;

        private int m_maxDownsampleLevels;
        private int m_maxUpsampleLevels;

        //private Effect _bloomThreasholdFilter;
        //private Effect _luminanceFilter;
        // private float _bloomIntensity, _luminTheashold;

        // private int _adaptiveLuminanceBlockSize;

        // private float _bloomAbsorption;
        // private bool _enableAdaptiveBrightness;

        // private bool _enable;

        /// <summary>
        /// 是否开启大雾效果
        /// </summary>
        public bool Enable
        {
            get; set;
        }

        /// <summary>
        /// 光晕效果的强度，越强光晕越亮
        /// </summary>
        public float BloomIntensity
        {
            get; set;
        }

        /// <summary>
        /// 原版光照物块光强的阈值，阈值越大那么亮度比较暗的光照环境不会加入光晕计算
        /// </summary>
        public float LuminanceThreashold
        {
            get; set;
        }

        /// <summary>
        /// 单位距离的雾会吸收多少亮度，该值越大则雾浓度越高，可见性越差
        /// </summary>
        public float FogAbsorptionRate
        {
            get; set;
        }

        /// <summary>
        /// 光晕效果收到雾的浓度的影响因子，越大则能见度越低
        /// </summary>
        public float BloomAbsorptionRate
        {
            get; set;
        }

        /// <summary>
        /// 光晕效果的模糊卷积核半径，该值为2^k
        /// </summary>
        public int BloomRadius
        {
            get
            {
                return m_bloomRadius;
            }
            set
            {

                m_shouldResetRenderTargets |= (m_bloomRadius != value);
                m_bloomRadius = value;
            }
        }

        public FogPass()
        {
            m_screenWidth = 0;
            m_screenHeight = 0;

            m_boxKernelEffect = MythContent.QuickEffectAsset("Effects/BoxFilter");
            m_gaussianKernelEffect = MythContent.QuickEffectAsset("Effects/GBlur");
            m_fogScreenEffect = MythContent.QuickEffectAsset("Effects/Fog");

            m_downSampleRenderTargets = new RenderTarget2D[MAX_DOWNSAMPLE_LEVEL];
            m_upSampleRenderTargets = new RenderTarget2D[MAX_UPSAMPLE_LEVEL];
            m_shouldResetRenderTargets = true;
        }

        public void Preprocess()
        {

            UpdateParameters();
        }

        private void UpdateParameters()
        {

            var fogConfig = ModContent.GetInstance<FogConfigs>();

            BloomRadius = fogConfig.MaxBloomRadius;

            m_shouldResetRenderTargets |= (m_offscreenTilesSize != fogConfig.OffscreenTiles);
            m_offscreenTilesSize = fogConfig.OffscreenTiles;

            Enable = fogConfig.GaussianKernel;
            m_enableProgressiveUpSampling = fogConfig.EnableProgressiveUpSampling;
        }

        private void ResetLightMap()
        {
            m_tileWidth = Main.screenWidth / 16 + m_offscreenTilesSize * 2 + 2;
            m_tileHeight = Main.screenHeight / 16 + m_offscreenTilesSize * 2 + 2;
            m_lightMap = new Color[m_tileWidth * m_tileHeight];
            m_lightTexture = new Texture2D(Main.graphics.GraphicsDevice, m_tileWidth, m_tileHeight, false,
                SurfaceFormat.Rgba1010102);
        }

        private void ExtractLightMap()
        {
            int rows = m_lightTexture.Height;
            int cols = m_lightTexture.Width;

            Parallel.For(0, rows, i =>
            {
                for (int j = 0; j < cols; j++)
                {
                    m_lightMap[i * cols + j] = Color.Transparent;
                }
            });

            int startTileX = Math.Max(0, (int)(Main.screenPosition.X / 16) - m_offscreenTilesSize);
            int endTileX = Math.Min(Main.maxTilesX - 1,
                (int)((Main.screenPosition.X + Main.screenWidth) / 16) + m_offscreenTilesSize);
            int startTileY = Math.Max(0, (int)(Main.screenPosition.Y / 16) - m_offscreenTilesSize);
            int endTileY = Math.Min(Main.maxTilesY - 1,
                (int)((Main.screenPosition.Y + Main.screenHeight) / 16) + m_offscreenTilesSize);


            int offX = (int)(Main.screenPosition.X / 16);
            int offY = (int)(Main.screenPosition.Y / 16);

            Parallel.For(startTileY, endTileY, i =>
            {
                for (int j = startTileX; j <= endTileX; j++)
                {
                    int x = j - offX + m_offscreenTilesSize + 2;
                    int y = i - offY + m_offscreenTilesSize + 2;
                    var color = Lighting.GetColor(i, j);

                    var s = color.ToVector3();
                    if ((s.X + s.Y + s.Z) * 0.333f > LuminanceThreashold)
                    {
                        m_lightMap[y * cols + x] = color;
                    }
                }
            });

            m_lightTexture.SetData(m_lightMap);
        }

        public void Apply(RenderTarget2D screenTarget)
        {
            UpdateParameters();
            if (!Enable)
                return;

            if (m_screenWidth != Main.screenWidth || m_screenHeight != Main.screenHeight
                || m_shouldResetRenderTargets)
            {
                //int sz = 256;
                //while (sz < Math.Max(Main.screenWidth, Main.screenHeight))
                //{
                //    sz *= 2;
                //}
                ResetLightMap();
                m_frameWidth = m_tileWidth * 16;
                m_frameHeight = m_tileHeight * 16;

                int l = 0;
                for (; l < MAX_DOWNSAMPLE_LEVEL; l++)
                {
                    if ((m_tileWidth >> l) == 0 || (m_tileHeight >> l) == 0)
                    {
                        break;
                    }
                    m_downSampleRenderTargets[l] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                            m_tileWidth >> l, m_tileHeight >> l, false,
                            SurfaceFormat.Rgba1010102, DepthFormat.None);
                }
                m_maxDownsampleLevels = l;

                for (int i = 0; i < MAX_UPSAMPLE_LEVEL; i++)
                {
                    m_upSampleRenderTargets[l] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                            m_tileWidth << l, m_tileHeight << l, false,
                            SurfaceFormat.Rgba1010102, DepthFormat.None);
                }

                int maxKernel = m_maxDownsampleLevels - 1;
                m_renderTargetSwap = new RenderTarget2D(Main.graphics.GraphicsDevice,
                        m_tileWidth >> maxKernel, m_tileHeight >> maxKernel,
                        false, SurfaceFormat.Rgba1010102, DepthFormat.None);
                //_filteredScreenTarget = new RenderTarget2D(Main.graphics.GraphicsDevice,
                //        Main.screenWidth, Main.screenHeight,
                //        false, SurfaceFormat.Rgba1010102, DepthFormat.None);
                m_screenWidth = Main.screenWidth;
                m_screenHeight = Main.screenHeight;

                m_shouldResetRenderTargets = false;
            }

            ExtractLightMap();

            Generate(2, 4);

            int x = (int)(Main.screenPosition.X / 16) - _offscreenTilesSize - 2;
            x *= 16;
            int y = (int)(Main.screenPosition.Y / 16) - _offscreenTilesSize - 2;
            y *= 16;
            GraphicsDevice.SetRenderTarget(_filteredScreenTarget);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.PointClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
            spriteBatch.TeaNPCDraw(_samplingRenderTargets[0], new Rectangle((int)(x - Main.screenPosition.X),
                (int)(y - Main.screenPosition.Y), _samplingRenderTargets[0].Width, _samplingRenderTargets[0].Height),
                Color.White);
            spriteBatch.End();

            RenderTarget2D tmpTarget = (screenTarget == Main.screenTarget) ? Main.screenTargetSwap : Main.screenTarget;
            GraphicsDevice.SetRenderTarget(tmpTarget);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.PointClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
            spriteBatch.TeaNPCDraw(screenTarget, Vector2.Zero,
                Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(screenTarget);
            GraphicsDevice.Clear(Color.Transparent);
            _fogEffect.Parameters["uImageSize0"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            _fogEffect.Parameters["uAspectRatio"].SetValue(Main.screenWidth / (float)Main.screenHeight);
            _fogEffect.Parameters["uAbsorption"].SetValue(TeaNPC.FogConfigs.FogAbsorption * 0.05f);
            _fogEffect.Parameters["uBloomIntensity"].SetValue(BloomIntensity);
            _fogEffect.Parameters["uBloomAbsorption"].SetValue(BloomAbsorptionRate);
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.Opaque,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone, null);
            {
                GraphicsDevice.Textures[1] = _filteredScreenTarget;
                GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
                _fogEffect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.TeaNPCDraw(tmpTarget, Vector2.Zero,
                    Color.White);
            }
            spriteBatch.End();
        }

        private void Generate(int down, int up)
        {
            var spriteBatch = Main.spriteBatch;
            var graphicsDevice = Main.graphics.GraphicsDevice;

            graphicsDevice.SetRenderTarget(m_downSampleRenderTargets[0]);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque,
                    SamplerState.PointClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone, null);
            spriteBatch.Draw(m_lightTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            var filterBox = m_boxKernelEffect.Value;

            int downLevels = Math.Min(m_maxDownsampleLevels, down);

            // Downsampling
            for (int i = 1; i < downLevels; i++)
            {
                int curWidth = m_tileWidth >> i;
                int curHeight = m_tileHeight >> i;
                Main.graphics.GraphicsDevice.SetRenderTarget(m_downSampleRenderTargets[i]);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone, null);
                filterBox.Parameters["uImageSize0"].SetValue(m_downSampleRenderTargets[i - 1].Size());
                filterBox.Parameters["uDelta"].SetValue(1.0f);
                filterBox.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(m_downSampleRenderTargets[i - 1], new Rectangle(0, 0, curWidth, curHeight),
                    Color.White);
                spriteBatch.End();
            }

            ApplyGaussian();

            // Upsampling
            for (int i = downLevels; i > 0; i--)
            {
                int curWidth = m_tileWidth >> (i - 1);
                int curHeight = m_tileHeight >> (i - 1);
                Main.graphics.GraphicsDevice.SetRenderTarget(m_downSampleRenderTargets[i + 1]);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
                filterBox.Parameters["uImageSize0"].SetValue(m_downSampleRenderTargets[i].Size());
                filterBox.Parameters["uDelta"].SetValue(1.0f);
                filterBox.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(m_downSampleRenderTargets[i], new Rectangle(0, 0, curWidth, curHeight),
                    Color.White);
                spriteBatch.End();
            }
        }

        private void ApplyGaussian()
        {
            if (m_enableGaussian)
            {
                var gaussianFilter = m_gaussianKernelEffect.Value;
                var spriteBatch = Main.spriteBatch;
                var graphicsDevice = Main.graphics.GraphicsDevice;

                var target = m_downSampleRenderTargets[m_maxDownsampleLevels - 1];

                gaussianFilter.Parameters["uImageSize0"].SetValue(target.Size());
                gaussianFilter.Parameters["uDelta"].SetValue(1.0f);

                // Blur
                graphicsDevice.SetRenderTarget(m_renderTargetSwap);
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
                gaussianFilter.Parameters["uHorizontal"].SetValue(true);
                gaussianFilter.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(target, Vector2.Zero,
                    Color.White);
                spriteBatch.End();

                graphicsDevice.SetRenderTarget(target);
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
                gaussianFilter.Parameters["uHorizontal"].SetValue(false);
                gaussianFilter.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(m_renderTargetSwap, Vector2.Zero,
                    Color.White);
                spriteBatch.End();
            }
        }


    }
}
