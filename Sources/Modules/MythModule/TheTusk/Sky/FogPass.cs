using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.UI;
using Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds;
using Everglow.Sources.Modules.MythModule.TheFirefly.UI;
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
        private Asset<Effect> m_fogEffect;

        private RenderTarget2D[] m_progressiveRenderTarget;
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
        }

        public override void Preprocess()
        {
            _boxFilter = TeaNPC.ShaderManager.Get("BoxFilter");
            //_luminanceFilter = TeaNPC.ShaderManager.Get("Luminance");
            //_bloomThreasholdFilter = TeaNPC.ShaderManager.Get("Threashold");
            _gaussianBlurFilter = TeaNPC.ShaderManager.Get("GBlur");
            _fogEffect = TeaNPC.ShaderManager.Get("Fog");
            _samplingRenderTargets = new RenderTarget2D[12];
            _shouldResetRenderTargets = true;
            UpdateParameters();
        }

        private void UpdateParameters()
        {
            //_enable = TeaNPC.BloomConfigs.EnableBloom;

            //_bloomIntensity = TeaNPC.FogConfigs.BloomIntensity;

            _shouldResetRenderTargets = (_bloomRadius != TeaNPC.FogConfigs.BloomRadius);
            _bloomRadius = TeaNPC.FogConfigs.BloomRadius;

            //_shouldResetRenderTargets |= (_adaptiveLuminanceBlockSize != TeaNPC.FogConfigs.AdaptiveBrightnessSize);
            //_adaptiveLuminanceBlockSize = TeaNPC.FogConfigs.AdaptiveBrightnessSize;

            _shouldResetRenderTargets |= (_offscreenTilesSize != TeaNPC.FogConfigs.OffscreenTiles);
            _offscreenTilesSize = TeaNPC.FogConfigs.OffscreenTiles;


            _enableGaussian = TeaNPC.FogConfigs.GaussianKernel;
            _enableProgressiveUpSampling = TeaNPC.FogConfigs.EnableProgressiveUpSampling;

            //_luminTheashold = TeaNPC.FogConfigs.LightLuminanceThreashold;
        }

        private void ResetLightMap()
        {
            _tileWidth = Main.screenWidth / 16 + _offscreenTilesSize * 2 + 10;
            _tileHeight = Main.screenHeight / 16 + _offscreenTilesSize * 2 + 10;
            _lightMap = new Color[_tileWidth * _tileHeight];
            _lightTexture = new Texture2D(GraphicsDevice, _tileWidth, _tileHeight);
        }

        private void ExtractLightMap()
        {
            int rows = _lightTexture.Height;
            int cols = _lightTexture.Width;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    _lightMap[i * cols + j] = Color.Transparent;
                }
            }
            int startTileX = Math.Max(0, (int)(Main.screenPosition.X / 16) - _offscreenTilesSize);
            int endTileX = Math.Min(Main.maxTilesX - 1,
                (int)((Main.screenPosition.X + Main.screenWidth) / 16) + _offscreenTilesSize);
            int startTileY = Math.Max(0, (int)(Main.screenPosition.Y / 16) - _offscreenTilesSize);
            int endTileY = Math.Min(Main.maxTilesY - 1,
                (int)((Main.screenPosition.Y + Main.screenHeight) / 16) + _offscreenTilesSize);

            for (int j = startTileY; j <= endTileY; j++)
            {
                for (int i = startTileX; i <= endTileX; i++)
                {
                    int x = i - (int)(Main.screenPosition.X / 16) + _offscreenTilesSize + 2;
                    int y = j - (int)(Main.screenPosition.Y / 16) + _offscreenTilesSize + 2;
                    var color = Lighting.GetColor(i, j);

                    var s = color.ToVector3();
                    if ((s.X + s.Y + s.Z) * 0.333f > LuminanceThreashold)
                    {
                        _lightMap[y * cols + x] = color;
                    }
                }
            }
            _lightTexture.SetData(_lightMap);
        }

        public override void Apply(RenderTarget2D screenTarget)
        {
            UpdateParameters();
            if (!Enable)
                return;

            if (_screenWidth != Main.screenWidth || _screenHeight != Main.screenHeight
                || _shouldResetRenderTargets)
            {
                //int sz = 256;
                //while (sz < Math.Max(Main.screenWidth, Main.screenHeight))
                //{
                //    sz *= 2;
                //}
                ResetLightMap();
                _frameWidth = _tileWidth * 16;
                _frameHeight = _tileHeight * 16;
                int factor = 1;
                int maxSz = _bloomRadius;
                for (int j = 0; j <= maxSz; j++)
                {
                    _samplingRenderTargets[j] = new RenderTarget2D(Main.graphics.GraphicsDevice,
                        _frameWidth / factor, _frameHeight / factor, false,
                        SurfaceFormat.Rgba1010102, DepthFormat.None);
                    factor <<= 1;
                }
                _renderTargetSwap = new RenderTarget2D(Main.graphics.GraphicsDevice,
                        _frameWidth / (1 << _bloomRadius), _frameHeight / (1 << _bloomRadius), false, SurfaceFormat.Rgba1010102, DepthFormat.None);
                _filteredScreenTarget = new RenderTarget2D(Main.graphics.GraphicsDevice,
                        Main.screenWidth, Main.screenHeight,
                        false, SurfaceFormat.Rgba1010102, DepthFormat.None);
                _screenWidth = Main.screenWidth;
                _screenHeight = Main.screenHeight;

                _shouldResetRenderTargets = false;
            }

            ExtractLightMap();

            var spriteBatch = Main.spriteBatch;

            GraphicsDevice.SetRenderTarget(_samplingRenderTargets[0]);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.PointClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
            spriteBatch.TeaNPCDraw(_lightTexture, new Rectangle(0, 0, _frameWidth, _frameHeight),
                Color.White);
            spriteBatch.End();


            // Downsampling
            for (int i = 0; i < _bloomRadius; i++)
            {
                int block = 1 << (i + 1);
                int curWidth = _frameWidth / block;
                int curHeight = _frameHeight / block;
                Main.graphics.GraphicsDevice.SetRenderTarget(_samplingRenderTargets[i + 1]);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
                _boxFilter.Parameters["uImageSize0"].SetValue(_samplingRenderTargets[i].Size());
                _boxFilter.Parameters["uDelta"].SetValue(1.0f);
                _boxFilter.CurrentTechnique.Passes[0].Apply();
                spriteBatch.TeaNPCDraw(_samplingRenderTargets[i], new Rectangle(0, 0, curWidth, curHeight),
                    Color.White);
                spriteBatch.End();
            }

            if (_enableGaussian)
            {
                _gaussianBlurFilter.Parameters["uImageSize0"].SetValue(_samplingRenderTargets[_bloomRadius].Size());
                _gaussianBlurFilter.Parameters["uDelta"].SetValue(1.0f);
                // Blur
                Main.graphics.GraphicsDevice.SetRenderTarget(_renderTargetSwap);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
                _gaussianBlurFilter.Parameters["uHorizontal"].SetValue(true);
                _gaussianBlurFilter.CurrentTechnique.Passes[0].Apply();
                spriteBatch.TeaNPCDraw(_samplingRenderTargets[_bloomRadius], Vector2.Zero,
                    Color.White);
                spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTarget(_samplingRenderTargets[_bloomRadius]);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone, null);
                _gaussianBlurFilter.Parameters["uHorizontal"].SetValue(false);
                _gaussianBlurFilter.CurrentTechnique.Passes[0].Apply();
                spriteBatch.TeaNPCDraw(_renderTargetSwap, Vector2.Zero,
                    Color.White);
                spriteBatch.End();

            }

            // Upsampling
            if (_enableProgressiveUpSampling)
            {
                for (int i = _bloomRadius; i > 0; i--)
                {
                    int block = 1 << (i - 1);
                    int curWidth = _frameWidth / block;
                    int curHeight = _frameHeight / block;
                    Main.graphics.GraphicsDevice.SetRenderTarget(_samplingRenderTargets[i - 1]);
                    Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                    spriteBatch.Begin(SpriteSortMode.Immediate,
                        BlendState.Opaque,
                        SamplerState.AnisotropicClamp,
                        DepthStencilState.Default,
                        RasterizerState.CullNone, null);
                    _boxFilter.Parameters["uImageSize0"].SetValue(_samplingRenderTargets[i].Size());
                    _boxFilter.Parameters["uDelta"].SetValue(1.0f);
                    _boxFilter.CurrentTechnique.Passes[0].Apply();
                    spriteBatch.TeaNPCDraw(_samplingRenderTargets[i], new Rectangle(0, 0, curWidth, curHeight),
                        Color.White);
                    spriteBatch.End();
                }
            }
            else
            {
                if (_bloomRadius != 0)
                {
                    Main.graphics.GraphicsDevice.SetRenderTarget(_samplingRenderTargets[0]);
                    Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                    spriteBatch.Begin(SpriteSortMode.Immediate,
                        BlendState.Opaque,
                        SamplerState.AnisotropicClamp,
                        DepthStencilState.Default,
                        RasterizerState.CullNone, null);
                    spriteBatch.TeaNPCDraw(_samplingRenderTargets[_bloomRadius], new Rectangle(0, 0, _frameWidth, _frameHeight),
                        Color.White);
                    spriteBatch.End();
                }
            }

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

        public override bool IsActive()
        {
            return true;
        }
    }
}
