using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Sources.Modules.MythModule.Common
{
    public class MythContent
    {
        /// <summary>
        /// 对于神话模块专用的获取图片封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D QuickTexture(string path)
        {
            return ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/" + path).Value;
        }
        /// <summary>
        /// 对于神话模块专用的获取特效封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Effect QuickEffect(string path)
        {
            return ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/" + path).Value;
        }
        /// <summary>
        /// 对于神话模块专用的Glowmask获取
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
                glowMasks[glowMasks.Length - 1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Glows/" + modItem.GetType().Name + "_glow");
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
