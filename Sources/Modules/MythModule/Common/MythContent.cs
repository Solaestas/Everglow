using ReLogic.Content;
using Terraria.GameContent;

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
            return ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/" + path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }
        /// <summary>
        /// 对于神话模块专用的获取特效封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Effect QuickEffect(string path)
        {
            return ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/" + path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }

        /// <summary>
        /// 对于神话模块专用的获取特效封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Asset<Effect> QuickEffectAsset(string path)
        {
            return ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/" + path);
        }
        /// <summary>
        /// 对于神话模块专用的获取音乐封装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int QuickMusic(string path)
        {
            Mod everglow = ModLoader.GetMod("Everglow");
            if(everglow != null)
            {
                return MusicLoader.GetMusicSlot(everglow, "Sources/Modules/MythModule/Musics/" + path);
            }
            else
            {
                return 0;
            }
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
        /// <summary>
        /// 获取太阳位置
        /// </summary>
        public static Vector2 GetSunPos()
        {
            float HalfMaxTime = Main.dayTime ? 27000 : 16200;
            float bgTop = -Main.screenPosition.Y / (float)(Main.worldSurface * 16.0 - 600.0) * 200f;
            float value = 1 - (float)Main.time / HalfMaxTime;
            float StarX = (1 - value) * Main.screenWidth / 2f - 100 * value;
            float t = value * value;
            float StarY = bgTop + t * 250f + 180;
            if (Main.LocalPlayer != null)
                if (Main.LocalPlayer.gravDir == -1)
                {
                    return new Vector2(StarX, Main.screenHeight - StarY);
                }
            return new Vector2(StarX, StarY);
        }
    }
}
