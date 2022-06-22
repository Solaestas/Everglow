using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Configs
{
    /// <summary>
    /// 用于给美术调节雾效最终效果
    /// </summary>
    public class FogConfigs : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(false)]
        [Label("Enable Participating Medium")]
        [Tooltip("是否开启参与介质散射")]
        public bool EnableScattering;

        [DefaultValue(5)]
        [Range(0, 10)]
        [Label("Bloom Radius")]
        [Tooltip("光效的最大溢出半径，最终半径是以2为底的指数，越大越影响性能")]
        public int MaxBloomRadius;

        [DefaultValue(0.7f)]
        [Range(0, 5.0f)]
        [Label("Bloom Intensity")]
        [Tooltip("光效的强度，用于放大亮度")]
        public float BloomIntensity;

        [DefaultValue(4)]
        [Range(0, 40)]
        [Label("Offscreen Tiles Count")]
        [Tooltip("光效需要计算超出屏幕的物块数量")]
        public int OffscreenTiles;

        [DefaultValue(0)]
        [Range(0, 10)]
        [Label("Adaptive Brightness Size")]
        [Tooltip("自适应亮度统计区块的大小，该值是2^k")]
        public int AdaptiveBrightnessSize;

        [DefaultValue(false)]
        [Label("Enable Adaptive Brightness")]
        [Tooltip("是否开启自适应亮度检测")]
        public bool EnableAdaptiveBrightnessDetect;

        [DefaultValue(0.0f)]
        [Range(0, 1.0f)]
        [Label("Bloom Threshold Multiplier")]
        [Tooltip("可以调整原版点亮物块的光源阈值")]
        public float LightLuminanceThreashold;

        [DefaultValue(true)]
        [Label("是否开启高斯卷积核")]
        [Tooltip("更柔和的效果，但是性能较差")]
        public bool GaussianKernel;

        [DefaultValue(true)]
        [Label("是否开启渐进式上采样")]
        [Tooltip("更柔和的效果，但是性能较差")]
        public bool EnableProgressiveUpSampling;

        [DefaultValue(0.12f)]
        [Range(0.0f, 1.0f)]
        [Label("雾吸收率（R）")]
        [Tooltip("雾吸收红色通道直接光的比率，越大则越暗淡")]
        public float FogAbsorptionR;

        [DefaultValue(0.12f)]
        [Range(0.0f, 1.0f)]
        [Label("雾吸收率（G）")]
        [Tooltip("雾吸收绿色通道直接光的比率，越大则越暗淡")]
        public float FogAbsorptionG;

        [DefaultValue(0.12f)]
        [Range(0.0f, 1.0f)]
        [Label("雾吸收率（B）")]
        [Tooltip("雾吸收蓝色通道直接光的比率，越大则越暗淡")]
        public float FogAbsorptionB;

        [DefaultValue(0.28f)]
        [Range(0.0f, 1.0f)]
        [Label("雾散射吸收率")]
        [Tooltip("散射光的吸收比率")]
        public float FogBloomAbsorption;
    }
}
