namespace Everglow.Sources.Modules.OceanModule.Common
{
    public class OceanContent
    {
        /// <summary>
        /// 对于海洋模块专用的获取图片封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D QuickTexture(string path)
        {
            return ModContent.Request<Texture2D>("Everglow/Sources/Modules/OceanModule/" + path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }

        /// <summary>
        /// 对于海洋模块专用的获取特效封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Effect QuickEffect(string path)
        {
            return ModContent.Request<Effect>("Everglow/Sources/Modules/OceanModule/" + path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }

        /// <summary>
        /// 对于海洋模块专用的获取音乐封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int QuickMusic(string path)
        {
            Mod everglow = ModLoader.GetMod("Everglow");
            if (everglow != null)
            {
                return MusicLoader.GetMusicSlot(everglow, "Sources/Modules/OceanModule/Musics/" + path);
            }
            else
            {
                return 0;
            }
        }
    }
}