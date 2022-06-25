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
using Terraria.Audio;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Shaders;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Sky
{
    public class FogPass
    {
        private Asset<Effect> m_boxKernelEffect;
        private Asset<Effect> m_gaussianKernelEffect;
        private Asset<Effect> m_fogScreenEffect;

        private RenderTarget2D[] m_blurRenderTargets;
        private RenderTarget2D m_renderTargetSwap;
        private RenderTarget2D m_filteredScreenTarget;

        private Color[] m_lightMap;
        private Texture2D m_lightTexture;

        private int m_bloomRadius = 2;
        private int m_offscreenTilesSize = 4;

        private int m_frameWidth, m_frameHeight;
        private int m_screenWidth, m_screenHeight;
        private int m_tileWidth, m_tileHeight;
        private bool m_shouldResetRenderTargets;
        private bool m_enableGaussian;

        private readonly int MAX_BLUR_LEVELS = 10;

        private int m_maxBlurLevel;

        private float m_bloomIntensity;
        private int m_startTileX, m_startTileY;
        private bool m_enableLightUpload;
        private bool m_resetedLightmaps;
        private Vector2 m_screenPosition;
        private const SurfaceFormat m_surfaceFormat = SurfaceFormat.Rgba1010102;

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
            get
            {
                return m_bloomIntensity;
            }
            set
            {
                m_bloomIntensity = value;
            }
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
        /// 是否让散射效果随着距离增大而增大
        /// </summary>
        public bool FogScatterWithDistance
        {
            get; set;
        }

        /// <summary>
        /// 光晕效果开启的比例
        /// </summary>
        public float BloomFactor
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

            m_blurRenderTargets = new RenderTarget2D[MAX_BLUR_LEVELS];
            m_shouldResetRenderTargets = true;
            m_resetedLightmaps = true;
            m_screenPosition = Vector2.Zero;
        }

        public void Preprocess()
        {

            UpdateParameters();
        }

        private void UpdateParameters()
        {
            var fogConfig = ModContent.GetInstance<FogConfigs>();

            BloomRadius = fogConfig.MaxBloomRadius;
            BloomIntensity = fogConfig.BloomIntensity;
            LuminanceThreashold = fogConfig.LightLuminanceThreashold;
            BloomAbsorptionRate = fogConfig.FogBloomAbsorptionFactor;
            BloomFactor = fogConfig.FogBloomRate;
            FogScatterWithDistance = fogConfig.FogScatterWithDistance;

            m_shouldResetRenderTargets |= (m_offscreenTilesSize != fogConfig.OffscreenTiles);
            m_offscreenTilesSize = fogConfig.OffscreenTiles;
            m_enableGaussian = fogConfig.GaussianKernel;
            m_enableLightUpload = fogConfig.EnableLightUpload;

            Enable = fogConfig.EnableScattering;
        }

        private void ResetLightMap()
        {
            m_tileWidth = m_screenWidth / 16 + m_offscreenTilesSize * 2 + 2;
            m_tileHeight = m_screenHeight / 16 + m_offscreenTilesSize * 2 + 2;
            m_lightMap = new Color[m_tileWidth * m_tileHeight];
            m_lightTexture = new Texture2D(Main.graphics.GraphicsDevice, m_tileWidth, m_tileHeight, false,
                SurfaceFormat.Color);
            m_resetedLightmaps = true;
        }


        public void ExtractLightMap()
        {
            m_screenPosition = Main.screenPosition;
            m_resetedLightmaps = false;

            int rows = m_lightTexture.Height;
            int cols = m_lightTexture.Width;

            //for (int i = 0; i < rows; i++)
            //{
            //    for (int j = 0; j < cols; j++)
            //    {
            //        m_lightMap[i * cols + j] = Color.Transparent;
            //    }
            //}

            Parallel.For(0, rows, i =>
            {
                for (int j = 0; j < cols; j++)
                {
                    m_lightMap[i * cols + j] = Color.Transparent;
                }
            });

            m_startTileX = Math.Max(0, (int)(m_screenPosition.X / 16) - m_offscreenTilesSize);
            int endTileX = Math.Min(Main.maxTilesX - 1,
                (int)((m_screenPosition.X + m_screenWidth) / 16) + m_offscreenTilesSize);

            if (endTileX - m_startTileX < cols)
            {
                endTileX += cols - (endTileX - m_startTileX);
            }

            m_startTileY = Math.Max(0, (int)(m_screenPosition.Y / 16) - m_offscreenTilesSize);
            int endTileY = Math.Min(Main.maxTilesY - 1,
                (int)((m_screenPosition.Y + m_screenHeight) / 16) + m_offscreenTilesSize);

            if (endTileY - m_startTileY < rows)
            {
                endTileY += rows - (endTileY - m_startTileY);
            }

            //for (int i = m_startTileY; i < endTileY; i++)
            //{
            //    for (int j = m_startTileX; j < endTileX; j++)
            //    {
            //        int x = j - m_startTileX;
            //        int y = i - m_startTileY;
            //        var color = Lighting.GetColor(j, i);

            //        var s = color.ToVector3();
            //        if ((s.X + s.Y + s.Z) * 0.333f > LuminanceThreashold)
            //        {
            //            m_lightMap[y * cols + x] = color;
            //        }
            //    }
            //}

            Parallel.For(m_startTileY, endTileY, i =>
            {
                for (int j = m_startTileX; j < endTileX; j++)
                {
                    int x = j - m_startTileX;
                    int y = i - m_startTileY;
                    var color = Lighting.GetColor(j, i);

                    var s = color.ToVector3();
                    if ((s.X + s.Y + s.Z) * 0.333f > LuminanceThreashold)
                    {
                        m_lightMap[y * cols + x] = color;
                    }
                }
            });

            // CPU bound 性能大头
            if (m_enableLightUpload)
            {
                m_lightTexture.SetData(m_lightMap);
            }
        }

        private void ResetRenderTargets()
        {
            //int sz = 256;
            //while (sz < Math.Max(Main.screenWidth, Main.screenHeight))
            //{
            //    sz *= 2;
            //}
            m_frameWidth = m_tileWidth * 16;
            m_frameHeight = m_tileHeight * 16;

            int l = 0;
            for (; l < MAX_BLUR_LEVELS; l++)
            {
                if ((m_frameWidth >> l) == 0 || (m_frameHeight >> l) == 0)
                {
                    break;
                }
                m_blurRenderTargets[l] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                        m_frameWidth >> l, m_frameWidth >> l, false,
                        m_surfaceFormat, DepthFormat.None);
            }
            m_maxBlurLevel = Math.Min(l, 8);

            for (int i = 0; i < m_maxBlurLevel; i++)
            {
                m_blurRenderTargets[i] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                        m_frameWidth >> i, m_frameHeight >> i, false,
                        m_surfaceFormat, DepthFormat.None);
            }

            m_renderTargetSwap = new RenderTarget2D(Main.graphics.GraphicsDevice,
                    m_frameWidth >> Math.Min(m_maxBlurLevel - 1, (4 + BloomRadius)), m_frameHeight >> Math.Min(m_maxBlurLevel - 1, (4 + BloomRadius)),
                    false, m_surfaceFormat, DepthFormat.None);
            m_filteredScreenTarget = new RenderTarget2D(Main.graphics.GraphicsDevice,
                    m_screenWidth, m_screenHeight,
                    false, m_surfaceFormat, DepthFormat.None);

            m_shouldResetRenderTargets = false;
        }

        public void Apply( RenderTarget2D screenTarget1, RenderTarget2D screenTarget2)
        {
            UpdateParameters();
            if (!Enable)
                return;

            if (screenTarget1 != Main.screenTarget)
            {
                return;
            }
            //else
            //{

            //}

            if (m_screenWidth != Main.screenWidth || m_screenHeight != Main.screenHeight
                || m_shouldResetRenderTargets)
            {
                m_screenWidth = Main.screenWidth;
                m_screenHeight = Main.screenHeight;
                ResetLightMap();
                ResetRenderTargets();
            }
            ExtractLightMap();

            Generate(BloomRadius, 4);

            var spriteBatch = Main.spriteBatch;
            var graphicsDevice = Main.graphics.GraphicsDevice;
            int x = m_startTileX * 16;
            int y = m_startTileY * 16;
            graphicsDevice.SetRenderTarget(m_filteredScreenTarget);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.PointClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null, Main.Transform);
            spriteBatch.Draw(m_blurRenderTargets[0], new Rectangle((int)(x - m_screenPosition.X),
                (int)(y - m_screenPosition.Y), m_blurRenderTargets[0].Width, m_blurRenderTargets[0].Height),
                Color.White);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(screenTarget2);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.PointClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
            spriteBatch.Draw(screenTarget1, Vector2.Zero,
                Color.White);
            spriteBatch.End();

            var fogEffect = m_fogScreenEffect.Value;
            graphicsDevice.SetRenderTarget(screenTarget1);
            graphicsDevice.Clear(Color.Transparent);
            fogEffect.Parameters["uImageSize0"].SetValue(new Vector2(m_screenWidth, m_screenHeight));

            var config = ModContent.GetInstance<FogConfigs>();
            var absorption = new Vector3(config.FogAbsorptionR, config.FogAbsorptionG, config.FogAbsorptionB);
            absorption *= absorption;
            fogEffect.Parameters["uAbsorption"].SetValue(absorption);
            fogEffect.Parameters["uBloomIntensity"].SetValue(BloomIntensity);
            fogEffect.Parameters["uBloomFactor"].SetValue(BloomFactor);
            fogEffect.Parameters["uBloomAbsorptionRate"].SetValue(BloomAbsorptionRate);
            fogEffect.Parameters["uFogScatterWithDistance"].SetValue(FogScatterWithDistance);

            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.Opaque,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone, null);
            {
                graphicsDevice.Textures[1] = m_filteredScreenTarget;
                graphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
                fogEffect.CurrentTechnique.Passes[0].Apply();

                spriteBatch.Draw(screenTarget2, Vector2.Zero,
                    Color.White);
            }
            spriteBatch.End();
        }

        private void Generate(int down, int up)
        {

            var spriteBatch = Main.spriteBatch;
            var graphicsDevice = Main.graphics.GraphicsDevice;

            graphicsDevice.SetRenderTarget(m_blurRenderTargets[4]);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque,
                    SamplerState.PointClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone, null);
            spriteBatch.Draw(m_lightTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            var filterBox = m_boxKernelEffect.Value;

            int downLevels = Math.Min(m_maxBlurLevel - 4, down);

            // Downsampling
            for (int i = 4; i < 4 + downLevels; i++)
            {
                int curWidth = m_frameWidth >> (i + 1);
                int curHeight = m_frameHeight >> (i + 1);
                Main.graphics.GraphicsDevice.SetRenderTarget(m_blurRenderTargets[i + 1]);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone, null);
                filterBox.Parameters["uImageSize0"].SetValue(m_blurRenderTargets[i].Size());
                filterBox.Parameters["uDelta"].SetValue(1.0f);
                filterBox.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(m_blurRenderTargets[i], new Rectangle(0, 0, curWidth, curHeight),
                    Color.White);
                spriteBatch.End();
            }

            ApplyGaussian(downLevels + 4);

            // Upsampling
            for (int i = 4 + downLevels - 1; i >= 0; i--)
            {
                int curWidth = m_frameWidth >> (i);
                int curHeight = m_frameHeight >> (i);
                graphicsDevice.SetRenderTarget(m_blurRenderTargets[i]);
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
                filterBox.Parameters["uImageSize0"].SetValue(m_blurRenderTargets[i + 1].Size());
                filterBox.Parameters["uDelta"].SetValue(1.0f);
                filterBox.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(m_blurRenderTargets[i + 1], new Rectangle(0, 0, curWidth, curHeight),
                    Color.White);
                spriteBatch.End();
            }
        }

        private void ApplyGaussian(int level)
        {
            if (m_enableGaussian)
            {
                var gaussianFilter = m_gaussianKernelEffect.Value;
                var spriteBatch = Main.spriteBatch;
                var graphicsDevice = Main.graphics.GraphicsDevice;

                var target = m_blurRenderTargets[level];

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
