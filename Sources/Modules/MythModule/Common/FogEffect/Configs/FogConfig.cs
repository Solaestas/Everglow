using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Everglow.Sources.Modules.MythModule.Common.FogEffect.Configs
{
    /// <summary>
    /// 用于给美术调节雾效最终效果
    /// </summary>
    public class FogConfigs : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(false)]
        [Label("开启雾效")]
        [Tooltip("是否开启参与介质散射")]
        public bool EnableScattering;

        [DefaultValue(true)]
        [Label("【性能测试】开启光源数据")]
        [Tooltip("是否启用发光体数据上传（CPU到GPU）")]
        public bool EnableLightUpload;

        [DefaultValue(2)]
        [Range(0, 4)]
        [Label("模糊半径")]
        [Tooltip("光效的最大溢出半径，最终半径是以2为底的指数，越大越影响性能")]
        public int MaxBloomRadius;

        [DefaultValue(0.7f)]
        [Range(0, 5.0f)]
        [Label("光效强度")]
        [Tooltip("光效的强度，用于放大亮度")]
        public float BloomIntensity;

        [DefaultValue(4)]
        [Range(0, 40)]
        [Label("离屏物块渲染数量")]
        [Tooltip("光效需要计算超出屏幕的物块数量")]
        public int OffscreenTiles;

        [DefaultValue(0.0f)]
        [Range(0, 1.0f)]
        [Label("光源阈值")]
        [Tooltip("可以调整原版点亮物块的光源阈值")]
        public float LightLuminanceThreashold;

        [DefaultValue(true)]
        [Label("是否开启高斯卷积核")]
        [Tooltip("更柔和的效果，但是性能较差")]
        public bool GaussianKernel;

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

        [DefaultValue(false)]
        [Label("扩散率随距离增大")]
        [Tooltip("是否让散射效果随着距离增大而增大")]
        public bool FogScatterWithDistance;

        [DefaultValue(0.5f)]
        [Range(0.0f, 1.0f)]
        [Label("雾散射扩散率")]
        [Tooltip("散射光的扩散比率")]
        public float FogBloomRate;

        [DefaultValue(0.28f)]
        [Range(0.0f, 1.0f)]
        [Label("雾散射能见度影响系数")]
        [Tooltip("散射光受到能见度影响的比率，0为不受能见度影响，1完全受能见度影响")]
        public float FogBloomAbsorptionFactor;

        [DefaultValue(false)]
        [Label("开启Temporal插值")]
        [Tooltip("是否允许光照结果进行帧间插值来平滑")]
        public bool EnableTemporalInterp;
    }
}
