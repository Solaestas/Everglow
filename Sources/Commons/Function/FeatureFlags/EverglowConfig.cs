using Everglow.Sources.Modules.AssetReplaceModule;
using System.ComponentModel;

using Terraria.ModLoader.Config;

namespace Everglow.Sources.Commons.Function.FeatureFlags
{
    public class EverglowConfig : ModConfig
    {
        /// <summary>
        /// 用于整个Mod，包括各个客户端的设置
        /// </summary>
        public override ConfigScope Mode => ConfigScope.ServerSide;

        /// <summary>
        /// 是否是Debug模式，如果是那么我们可以打印很多debug信息，或者运行一些debug下才有的逻辑
        /// </summary>
        [DefaultValue(false)]
        [Label("Enable Debug Mode")]
        [Tooltip("[For developers] Enable debug mode to allow debug functions to run")]
        public bool debugMode;

        public static bool DebugMode
        {
            get
            {
                return ModContent.GetInstance<EverglowConfig>().debugMode;
            }
        }
    }

    public class EverglowClientConfig : ModConfig
    {
        /// <summary>
        /// 用于各个客户端的个性化设置
        /// </summary>
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header("$Mods.Everglow.Config.Header.TextureReplace")]
        [DefaultValue(TextureReplaceMode.Terraria)]
        [Label("$Mods.Everglow.Config.TextureReplace.Label")]
        [Tooltip("$Mods.Everglow.Config.TextureReplace.Tooltip")]
        [DrawTicks]
        public TextureReplaceMode TextureReplace;

        [DefaultValue(true)]
        [Label("$Mods.Everglow.Config.ItemPickSoundReplace.Label")]
        public bool ItemPickSoundReplace;

        public override void OnChanged() {
        [Header("$Mods.Everglow.Config.Header.AudioReplace")]

        [DefaultValue(MothAudioReplaceMode.MothFighting)]
        [Label("$Mods.Everglow.Config.MothAudioReplace.Label")]
        [Tooltip("$Mods.Everglow.Config.MothAudioReplace.Tooltip")]
        [DrawTicks]
        public MothAudioReplaceMode MothAudioReplace;

        [DefaultValue(TuskAudioReplaceMode.TuskFighting)]
        [Label("$Mods.Everglow.Config.TuskAudioReplace.Label")]
        [Tooltip("$Mods.Everglow.Config.TuskAudioReplace.Tooltip")]
        [DrawTicks]
        public TuskAudioReplaceMode TuskAudioReplace;

        public override void OnChanged() {
            if ((int)TextureReplace >= 3) {
                TextureReplace = TextureReplaceMode.Terraria;
            }
            if ((int)MothAudioReplace >= 3)
            {
                MothAudioReplace = MothAudioReplaceMode.MothFighting;
            }
            if ((int)TuskAudioReplace >= 2)
            {
                TuskAudioReplace = TuskAudioReplaceMode.TuskFighting;
            }
            if (AssetReplaceModule.IsLoaded)
                AssetReplaceModule.ReplaceTextures(TextureReplace);
            base.OnChanged();
		}
	}

    public enum TextureReplaceMode
    {
        Terraria,
        [Label("Eternal Resolve")]
        EternalResolve,
        Myth
    }
    public enum MothAudioReplaceMode
    {
        [Label("Original")]
        MothFighting,
        [Label("Alternate")]
        AltMothFighting,
        [Label("Old")]
        OldMothFighting
    }
    public enum TuskAudioReplaceMode
    {
        [Label("Original")]
        TuskFighting,
        [Label("Old")]
        OldTuskFighting
    }
}
