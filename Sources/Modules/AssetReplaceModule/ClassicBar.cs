using ReLogic.Content;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
    public class ClassicBar
    {
        public Asset<Texture2D> BlueStar;
        public Asset<Texture2D> RedHeart;
        public Asset<Texture2D> GoldHeart;

        /// <summary>
        /// 根据传入的路径读取Texture2D
        /// </summary>
        /// <param name="path">贴图组在Resources文件夹内的名字，比如UISkinMyth</param>
        public void LoadTextures(string path) {
            BlueStar = AssetReplaceModule.GetTexture($"{path}/Bars/Classic/BlueStar");
            RedHeart = AssetReplaceModule.GetTexture($"{path}/Bars/Classic/RedHeart");
            GoldHeart = AssetReplaceModule.GetTexture($"{path}/Bars/Classic/GoldHeart");
        }

        public void ReplaceTextures() {
            TextureAssets.Mana = BlueStar;
            TextureAssets.Heart = RedHeart;
            TextureAssets.Heart2 = GoldHeart;
        }

        // 非static不需要Unload
        //public void UnloadTextures() {
        //    BlueStar = null;
        //    RedHeart = null;
        //    GoldHeart = null;
        //}
    }
}
