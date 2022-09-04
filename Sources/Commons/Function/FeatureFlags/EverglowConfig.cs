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

        [DefaultValue(TextureReplaceMode.Terraria)]
        [Label("In-game UI Texture")] //Mods.Everglow.Config.TextureReplace.Label
        [Tooltip("You can choose to replace the Terraria UI texture with other textures\nOptions: Terraria, Eternal Resolve, Myth")] //Mods.Everglow.Config.TextureReplace.Tooltip
        [DrawTicks]
        public TextureReplaceMode TextureReplace;

		public override void OnChanged() {
            if ((int)TextureReplace >= 3) {
                TextureReplace = TextureReplaceMode.Terraria;
            }
            if (AssetReplaceModule.IsLoaded)
                AssetReplaceModule.ReplaceTextures(TextureReplace);
            base.OnChanged();
		}
	}

    public enum TextureReplaceMode
    {
        Terraria,
        EternalResolve,
        Myth
    }
}
