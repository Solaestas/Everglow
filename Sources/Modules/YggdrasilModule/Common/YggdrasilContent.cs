using Terraria.GameContent;
using Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO;

namespace Everglow.Sources.Modules.YggdrasilModule.Common
{
    public class YggdrasilContent
    {
        /// <summary>
        /// 对于天穹模块专用的获取图片封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D QuickTexture(string path)
        {
            return ModContent.Request<Texture2D>("Everglow/Sources/Modules/YggdrasilModule/" + path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }
        /// <summary>
        /// 对于天穹模块专用的获取特效封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Effect QuickEffect(string path)
        {
            return ModContent.Request<Effect>("Everglow/Sources/Modules/YggdrasilModule/" + path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }
        /// <summary>
        /// 对于天穹模块专用的获取音乐封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int QuickMusic(string path)
        {
            Mod everglow = ModLoader.GetMod("Everglow");
            if(everglow != null)
            {
                return MusicLoader.GetMusicSlot(everglow, "Sources/Modules/YggdrasilModule/Musics/" + path);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 对于天穹模块专用的Glowmask获取
        /// </summary>
        /// <param name="modItem"></param>
        /// <returns></returns>
        public static short SetStaticDefaultsGlowMask(ModItem modItem)
        {
            if (!Terraria.Main.dedServ)
            {
                ReLogic.Content.Asset<Texture2D>[] glowMasks = new ReLogic.Content.Asset<Texture2D>[TextureAssets.GlowMask.Length + 1];
                for (int i = 0; i < TextureAssets.GlowMask.Length; i++)
                {
                    glowMasks[i] = TextureAssets.GlowMask[i];
                }
                glowMasks[glowMasks.Length - 1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/YggdrasilModule/Glows/" + modItem.GetType().Name + "_glow");
                TextureAssets.GlowMask = glowMasks;
                return (short)(glowMasks.Length - 1);
            }
            else
            {
                return 0;
            }
        }
    }
}
